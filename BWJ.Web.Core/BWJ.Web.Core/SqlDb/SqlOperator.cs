using System;

namespace BWJ.Web.Core.SqlDb
{
    internal enum SqlOperator
    {
        Equal,
        LessThan,
        LessThanOrEqual,
        GreaterThan,
        GreaterThanOrEqual,
        NotEqual,
        In
    }

    internal static class SqlOperatorExtensions
    {
        public static string ToOperator(this SqlOperator op)
        {
            switch (op)
            {
                case SqlOperator.Equal:
                    return "=";
                case SqlOperator.LessThan:
                    return "<";
                case SqlOperator.LessThanOrEqual:
                    return "<=";
                case SqlOperator.GreaterThan:
                    return ">";
                case SqlOperator.GreaterThanOrEqual:
                    return ">=";
                case SqlOperator.NotEqual:
                    return "<>";
                case SqlOperator.In:
                    return "IN";
                default:
                    throw new ArgumentOutOfRangeException(nameof(op));
            }
        }
    }
}
