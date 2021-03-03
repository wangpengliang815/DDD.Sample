
using DotnetCoreInfra.DataAccess;

namespace DotnetCoreInfra.Options
{
    public class DataAccessorOptions
    {
        /// <summary>
        /// Gets or sets a value indicating whether [save immediately].
        /// </summary>
        /// <value><c>true</c> if [save immediately]; otherwise, <c>false</c>.</value>
        public bool SaveImmediately { get; set; } = false;

        /// <summary>
        /// 编辑时使用的字段
        /// </summary>
        public string[] EditionFields { get; set; } = new string[] { nameof(BaseEntity.Editor)
            , nameof(BaseEntity.Edited) };

        /// <summary>
        /// 正常操作下不允许操作的字段
        /// </summary>
        public string[] CreationFields { get; set; } = new string[] { nameof(BaseEntity.Creator)
            , nameof(BaseEntity.Created) };

        /// <summary>
        /// 逻辑删除的时候只允许更新的字段
        /// </summary>
        public string[] DeletionFields { get; set; } = new string[] { nameof(BaseEntity.IsDeleted) };
    }
}
