using System;
using System.Collections.Generic;

namespace BWJ.Web.Core.SqlDb
{
    public class DbQueryService : IDbQueryService
    {
        private readonly Dictionary<string, string> queryTemplates = new Dictionary<string, string>();
        private readonly object lockObj = new object();

        public string GetQuery(Type entityType, string key, string schema, Func<string> queryBuilder)
        {
            var templateKey = $"{entityType.Name}.{key}";
            string template = null;
            lock (lockObj)
            {
                var templateCached = queryTemplates.ContainsKey(templateKey);
                template = templateCached ? queryTemplates[templateKey] : queryBuilder().Trim();

                if (templateCached == false)
                {
                    queryTemplates.Add(templateKey, template);
                }
            }

            return string.Format(template, schema);
        }
    }
}
