using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Wp.Services
{
    public class ExpressionHelper
    {
        public static Expression<Func<T, object>> GetSortProperty<T>(string field)
        {
            var param = Expression.Parameter(typeof(T), "x");
            var property = GetProperty(field, param);
            // Check sort on datetime...then convert
            if (property.Type == typeof(DateTime))
            {
                property = Expression.Convert(property, typeof(object));
            }
            return Expression.Lambda<Func<T, object>>(property, param);
        }

        private static Expression GetProperty(string field, ParameterExpression param)
        {
            // Split for nested props like ChangedBy.Name
            var parts = field.Split('.');

            Expression parent = param;

            foreach (var part in parts)
            {
                parent = Expression.Property(parent, part);
            }
            return parent;
        }
    }
}
