using System;

namespace BWJ.Web.Core.SqlDb
{
    public interface IDbQueryService
    {
        string GetQuery(Type entityType, string key, string schema, Func<string> queryBuilder);
    }
}