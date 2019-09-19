using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Fenit.Toolbox.Core.Enum;
using LinqKit;

namespace Fenit.Toolbox.EntityFramework
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> OrderByDynamic<T>(
            this IQueryable<T> query,
            string orderByMember,
            Order direction)
        {
            var queryElementTypeParam = Expression.Parameter(typeof(T));

            Expression memberAccess = null;

            if (orderByMember.Contains('.'))
            {
                memberAccess = queryElementTypeParam;
                foreach (var property in orderByMember.Split('.'))
                    memberAccess = Expression.PropertyOrField(memberAccess, property);
            }
            else
            {
                memberAccess = Expression.PropertyOrField(queryElementTypeParam, orderByMember);
            }

            var keySelector = Expression.Lambda(memberAccess, queryElementTypeParam);

            var orderBy = Expression.Call(
                typeof(Queryable),
                direction == Order.Asc ? "OrderBy" : "OrderByDescending",
                new[] {typeof(T), memberAccess.Type},
                query.Expression,
                Expression.Quote(keySelector));

            return query.Provider.CreateQuery<T>(orderBy);
        }

        public static IQueryable<T> Filter<T>(this IQueryable<T> query, string search)
        {
            if (string.IsNullOrEmpty(search)) return query;
            var properties = typeof(T).GetProperties().Where(p =>
                p.PropertyType == typeof(string) &&
                p.CustomAttributes.All(w => w.AttributeType != typeof(NotMappedAttribute)));

            var predicate = PredicateBuilder.False<T>();
            foreach (var property in properties) predicate = predicate.Or(CreateLike<T>(property, search));
            return query.AsExpandable().Where(predicate);
        }

        private static Expression<Func<T, bool>> CreateLike<T>(PropertyInfo prop, string value)
        {
            var parameter = Expression.Parameter(typeof(T), "f");
            var propertyAccess = Expression.MakeMemberAccess(parameter, prop);
            var like = Expression.Call(propertyAccess, "Contains", null, Expression.Constant(value, typeof(string)));

            return Expression.Lambda<Func<T, bool>>(like, parameter);
        }
    }
}