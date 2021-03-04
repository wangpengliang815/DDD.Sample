using System.Collections.Generic;
using System.Linq;

using DotnetCoreInfra.Options;

namespace DotnetCoreInfra.Extension
{
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
