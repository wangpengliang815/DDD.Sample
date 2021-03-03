using System;

namespace DotnetCoreInfra.DataAccessInterface
{
    /// <summary>
    /// 可逻辑删除的对象
    /// </summary>
    public interface IDeletable
    {
        Guid GetId();

        bool? IsDeleted { get; set; }
    }
}
