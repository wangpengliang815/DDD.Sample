namespace DotnetCoreInfra.DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;

    using DotnetCoreInfra.DataAccessInterface;
    using DotnetCoreInfra.Exceptions;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.ChangeTracking;
    using Microsoft.Extensions.Options;

    /// <summary>
    /// 实体数据读写访问器
    /// </summary>
    /// <typeparam name="TContext">The type of the context.</typeparam>
    /// <seealso cref="RedPI.Data.DataAccess.IEntityDataAccessor" />
    public partial class EntityDataAccessor<TContext> : BaseDataAccessor<TContext>
        , IEntityDataAccessor
        where TContext : DbContext
    {
        public async Task<TEntity> FindAsync<TEntity>(Guid id
            , bool includeDeleted = false)
            where TEntity : BaseEntity
        {
            TEntity result = await DbContext.FindAsync<TEntity>(id).ConfigureAwait(false);

            if (result == null)
            {
                return null;
            }
            if (includeDeleted)
            {
                return result;
            }
            if (result.IsDeleted != null && !result.IsDeleted.Value)
            {
                return result;
            }
            return null;
        }

        public async Task<TEntity> InsertAsync<TEntity>(TEntity entity
            , CancellationToken cancellationToken = default)
            where TEntity : BaseEntity
        {
            if (entity.GetId() == null)
            {
                throw new RequireIdException(typeof(TEntity));
            }

            entity.Creator = UserProvider.CurrentUser;
            entity.Created = UserProvider.Now;
            entity.Editor = UserProvider.CurrentUser;
            entity.Edited = UserProvider.Now;
            entity.IsDeleted = false;

            DbContext.Add(entity);
            if (AccessorOptions.SaveImmediately)
            {
                await DbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
                // 取消新插入对象的跟踪状态
                DbContext.Entry(entity).State = EntityState.Detached;
            }
            return entity;
        }


        /// <summary>Gets the list asynchronously.</summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task<List<TEntity>> GetListAsync<TEntity>(bool includeDeleted = false, CancellationToken cancellationToken = default) where TEntity : BaseEntity
        {
            IQueryable<TEntity> query = DbContext.Set<TEntity>()
                        .AsNoTracking();
            if (!includeDeleted)
            {
                query = query.Where(p => p.IsDeleted == false);
            }
            return await query.ToListAsync(cancellationToken).ConfigureAwait(false);
        }

        /// <summary>Gets the list asynchronously.</summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="predicate">The predicate.</param>
        /// <param name="skip">The skip.</param>
        /// <param name="take">The take.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task<List<TEntity>> GetListAsync<TEntity>(
            Expression<Func<TEntity, bool>> predicate
            , int skip
            , int take
            , bool includeDeleted = false
            , CancellationToken cancellationToken = default) where TEntity : BaseEntity
        {
            IQueryable<TEntity> query = DbContext.Set<TEntity>().AsNoTracking()
                            .Where<TEntity>(predicate);
            if (!includeDeleted)
            {
                query = query.Where(p => p.IsDeleted == false);
            }

            return await query.Skip(skip).Take(take)
                  .ToListAsync(cancellationToken).ConfigureAwait(false);
        }

        /// <summary>Updates the asynchronously.</summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="entity">The entity.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">entity</exception>
        /// <exception cref="ArgumentException">Fault: Entity.GetId() is null. - entity</exception>
        /// <remarks>
        ///    Cannot change CREATION and DELETION fields
        /// </remarks>
        public async Task<TEntity> UpdateAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default) where TEntity : BaseEntity
        {
            ArgumentHelper.CheckNull(entity, nameof(entity));

            if (entity.GetId() == null)
            {
                throw new ArgumentException("Fault: Entity.GetId() is null.", nameof(entity));
            }
            List<string> propertiesToExclude =
                AccessorOptions.CreationFields.Concat(AccessorOptions.DeletionFields).ToList();

            return await InnerUpdatePartiallyAsync(entity, null, propertiesToExclude, cancellationToken)
                                 .ConfigureAwait(false);

        }



        /// <summary>Updates the partially asynchronously.</summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="entity">The entity.</param>
        /// <param name="propertiesToInclude">The properties to update.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Fault: Entity.GetId() is null. - entity
        /// or
        /// Fault: propertiesToUpdate must contains at least one property name. - propertiesToUpdate</exception>
        /// <exception cref="ArgumentNullException">entity</exception>
        /// <remarks>Creation fields will be kept unchanged!</remarks>
        public async Task<TEntity> UpdatePartiallyAsync<TEntity>(
              TEntity entity
            , List<string> propertiesToInclude
            , CancellationToken cancellationToken = default)
            where TEntity : BaseEntity
        {
            ArgumentHelper.CheckNull(entity, nameof(entity));

            if (entity.GetId() == null)
            {
                throw new ArgumentException("Fault: Entity.GetId() is null.", nameof(entity));
            }

            if (propertiesToInclude == null || propertiesToInclude.Count == 0)
            {
                throw new ArgumentException("Fault: propertiesToUpdate must contains at least one property name.", nameof(propertiesToInclude));

            }
            return await InnerUpdatePartiallyAsync(entity
                        , propertiesToInclude.Concat(AccessorOptions.EditionFields)                         // 附加 编辑标记字段
                                            .Except(AccessorOptions.CreationFields)
                                            .Except(AccessorOptions.DeletionFields)  // 排除 创建和删除标记字段
                                            .ToList()
                        , null
                        , cancellationToken)
                 .ConfigureAwait(false);
        }


        /// <summary>
        /// 逻辑删除
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        /// <remarks>
        ///   只做逻辑删除， 不包含其他任何业务字段的更新
        /// </remarks>
        public async Task<TEntity> LogicDeleteAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default) where TEntity : BaseEntity
        {
            ArgumentHelper.CheckNull(entity, nameof(entity));

            return await LogicDeleteAsync<TEntity>(entity.GetId(), cancellationToken)
                                .ConfigureAwait(false);
        }

        /// <summary>
        /// 逻辑删除
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="id"></param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        /// <remarks>
        ///   只做逻辑删除， 不包含其他任何业务字段的更新
        /// </remarks>
        public async Task<TEntity> LogicDeleteAsync<TEntity>(object id, CancellationToken cancellationToken = default) where TEntity : BaseEntity
        {
            TEntity foundOne = await FindAsync<TEntity>(id).ConfigureAwait(false);

            if (foundOne == null)
            {
                return null;
            }

            foundOne.IsDeleted = true;

            await InnerUpdatePartiallyAsync(foundOne
                    , AccessorOptions.EditionFields
                            .Concat(AccessorOptions.DeletionFields).ToList()
                    , null
                    , cancellationToken)
                        .ConfigureAwait(false);

            return foundOne;
        }

        /// <summary>物理删除数据（注意：无法找回）</summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="entity">The entity.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>数据Id</returns>
        public async Task<object> DeletePhysicallyAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default) where TEntity : BaseEntity
        {
            ArgumentHelper.CheckNull(entity, nameof(entity));

            return await DeletePhysicallyAsync<TEntity>(entity.GetId(), cancellationToken)
                                .ConfigureAwait(false);
        }

        /// <summary>物理删除数据（注意：无法找回）</summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="id">The identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>数据Id</returns>
        public async Task<object> DeletePhysicallyAsync<TEntity>(object id, CancellationToken cancellationToken = default) where TEntity : BaseEntity
        {

            if (AccessorOptions.ForbidPhysicalDeletion)
            {
                throw new NotSupportedException($"Delete data {typeof(TEntity)} physically is not allowed!");
            }

            TEntity foundOne = await FindAsync<TEntity>(id).ConfigureAwait(false);

            if (foundOne == null)
            {
                return null;
            }

            DbContext.Set<TEntity>().Remove(foundOne);

            return id;
        }



        /// <summary>Inners the update partially asynchronous.</summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="entity">The entity.</param>
        /// <param name="propertiesToInclude">The properties to include.</param>
        /// <param name="propertiesToExclude">or The properties to exclude.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Arguments propertiesToInclude and propertiesToExclude cannot be null simultaneously</exception>
        private async Task<TEntity> InnerUpdatePartiallyAsync<TEntity>(
              TEntity entity
            , List<string> propertiesToInclude
            , List<string> propertiesToExclude
            , CancellationToken cancellationToken = default)
            where TEntity : BaseEntity
        {
            DetachLocal(entity);  // 

            entity.Editor = UserProvider.CurrentUser;
            entity.Edited = UserProvider.Now;

            EntityEntry<TEntity> entry = DbContext.Entry<TEntity>(entity);

            if (propertiesToInclude != null)
            {
                foreach (string item in propertiesToInclude)
                {
                    entry.Property(item).IsModified = true; // 仅修改部分字段
                }
            }
            else if (propertiesToExclude != null)
            {
                entry.State = EntityState.Modified;
                foreach (string item in propertiesToExclude)
                {
                    entry.Property(item).IsModified = false; // 仅排除部分字段修改
                }
            }
            else
            {
                throw new ArgumentException("Arguments propertiesToInclude and propertiesToExclude cannot be null simultaneously");
            }


            if (AccessorOptions.SaveImmediately)
            {
                await DbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            }
            return entity;
        }






    }
}
