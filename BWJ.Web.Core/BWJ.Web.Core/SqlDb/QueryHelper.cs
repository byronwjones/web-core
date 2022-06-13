using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace BWJ.Web.Core.SqlDb
{
    internal class QueryHelper<TEntity>
        where TEntity : class
    {
        public QueryHelper(string alias = null)
        {
            EntityType = typeof(TEntity);
            TableAlias = alias;
        }

        public string Table(bool includeAlias = true)
        {
            if (_Table is null)
            {
                var table = EntityType.GetCustomAttribute<TableAttribute>()?.Name ?? $"{EntityType.Name}s";
                _Table = "[{0}].[" + table + "]";
            }

            return includeAlias && !string.IsNullOrWhiteSpace(TableAlias) ? $"{_Table} {TableAlias}" :  _Table;
        }
        private string _Table = null;

        public string TableAlias { get; }

        public string StarSelect
        {
            get => string.IsNullOrWhiteSpace(TableAlias) ? $"[{TableAlias}].*" : $"*";
        }

        public PropertyInfo PrimaryKey
        {
            get
            {
                if(SearchedForPrimaryKey == false)
                {
                    _PrimaryKey = DatabaseColumns.FirstOrDefault(p => p.IsKey());
                    if(_PrimaryKey is null)
                    {
                        _PrimaryKey = DatabaseColumns
                            .FirstOrDefault(p => p.Name.Equals("id", StringComparison.OrdinalIgnoreCase));
                    }

                    SearchedForPrimaryKey = true;
                }

                return _PrimaryKey;
            }
        }
        private PropertyInfo _PrimaryKey;
        private bool SearchedForPrimaryKey = false;

        public List<PropertyInfo> DatabaseColumns
        {
            get
            {
                if(_DatabaseColumns is null)
                {
                    _DatabaseColumns = EntityType.GetProperties()
                        .Where(p => p.IsDatabaseColumn())
                        .ToList();
                }

                return _DatabaseColumns;
            }
        }
        private List<PropertyInfo> _DatabaseColumns;

        public string ColumnEqualsParameter<TProperty>(Expression<Func<TEntity, TProperty>> columnAndParameterProperty) =>
            CompareColumnToParameter(SqlOperator.Equal, columnAndParameterProperty);

        public string ColumnInParameter<TProperty>(Expression<Func<TEntity, TProperty>> columnAndParameterProperty) =>
            CompareColumnToParameter(SqlOperator.In, columnAndParameterProperty);

        /// <summary>
        /// Generates an SQL script snippet where a column is compared to a parameter of the same name
        /// </summary>
        /// <param name="op">Comparison operator</param>
        public string CompareColumnToParameter<TProperty>(SqlOperator op, Expression<Func<TEntity, TProperty>> columnAndParameterProperty) =>
            $"{Column(columnAndParameterProperty)} {op.ToOperator()} {Parameter(columnAndParameterProperty)}";

        public string Column<TProperty>(Expression<Func<TEntity, TProperty>> columnProperty)
        {
            var col = GetPropertyFromLambda(columnProperty);
            return Column(col);
        }

        public string Column(PropertyInfo columnProperty)
        {
            var col = GetDatabaseColumnProperty(columnProperty)
                .ToColumnString();

            return string.IsNullOrWhiteSpace(TableAlias) ? col : $"[{TableAlias}].{col}";
        }

        public string Parameter<TProperty>(Expression<Func<TEntity, TProperty>> parameterProperty)
        {
            return GetDatabaseColumnProperty(parameterProperty)
                .ToParameterString();
        }

        public string Parameter(PropertyInfo parameterProperty)
        {
            return GetDatabaseColumnProperty(parameterProperty)
                .ToParameterString();
        }

        private PropertyInfo GetDatabaseColumnProperty<TProperty>(Expression<Func<TEntity, TProperty>> columnProperty)
        {
            var prop = GetPropertyFromLambda(columnProperty);
            return GetDatabaseColumnProperty(prop);
        }

        private PropertyInfo GetDatabaseColumnProperty(PropertyInfo columnProperty)
        {
            if (columnProperty.IsDatabaseColumn() == false)
            {
                throw new ArgumentException("Property is not a database column");
            }

            return columnProperty;
        }

        // see https://stackoverflow.com/questions/671968/retrieving-property-name-from-lambda-expression
        private PropertyInfo GetPropertyFromLambda<TProperty>(Expression<Func<TEntity, TProperty>> propertyLambda)
        {
            Type type = typeof(TEntity);

            MemberExpression member = propertyLambda.Body as MemberExpression;
            if (member == null)
            {
                throw new ArgumentException($"Expression '{propertyLambda}' refers to a method, not a property.");
            }

            PropertyInfo property = member.Member as PropertyInfo;
            if (property == null)
            {
                throw new ArgumentException($"Expression '{propertyLambda}' refers to a field, not a property.");
            }

            return property;
        }

        private readonly Type EntityType;
    }
}
