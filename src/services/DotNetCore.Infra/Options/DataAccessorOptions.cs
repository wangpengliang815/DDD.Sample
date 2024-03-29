﻿namespace DotNetCore.Infra.Options
{

    using DotNetCore.Infra.Abstractions;

    public class DataAccessorOptions
    {
        /// <summary>
        /// Gets or sets a value indicating whether [save immediately].
        /// </summary>
        /// <value><c>true</c> if [save immediately]; otherwise, <c>false</c>.</value>
        public bool SaveImmediately { get; set; } = false;

        /// <summary>一次批量操作的对象数量.如果超过这个数量，则增加批次</summary>
        /// <value>The size of the batch.</value>
        public int BatchSize { get; set; } = 1000;

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
