using Dapper.Contrib.Extensions;
using System.Reflection;

namespace BWJ.Web.Core.SqlDb
{
    public static class DbEntityPropertyInfoExtensions
    {
        public static string ToColumnString(this PropertyInfo property) => $"[{property.Name}]";

        public static string ToParameterString(this PropertyInfo property) => $"@{property.Name}";

        public static bool IsExplicitKey(this PropertyInfo property) =>
            property.GetCustomAttribute<ExplicitKeyAttribute>() != null;

        public static bool IsGeneratedKey(this PropertyInfo property) =>
            property.GetCustomAttribute<KeyAttribute>() != null;

        public static bool IsKey(this PropertyInfo property) =>
            IsGeneratedKey(property) || IsExplicitKey(property);

        public static bool IsDatabaseColumn(this PropertyInfo property) =>
            property.GetCustomAttribute<WriteAttribute>()?.Write ?? true;
    }
}
