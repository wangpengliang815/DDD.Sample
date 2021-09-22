namespace DotNetCore.Infra.Extension
{
    using System.Collections.Generic;
    using System.Linq;

    using DotNetCore.Infra.Options;

    public static class DataAccessorOptionsExtension
    {
        public static List<string> GetExcludingFieldsWhenEditing(this DataAccessorOptions options)
        {
            return options.CreationFields.Concat(options.DeletionFields).ToList();
        }

        public static List<string> GetIncludingFieldsWhenDeleting(this DataAccessorOptions options)
        {
            return options.EditionFields.Concat(options.DeletionFields).ToList();
        }
    }
}
