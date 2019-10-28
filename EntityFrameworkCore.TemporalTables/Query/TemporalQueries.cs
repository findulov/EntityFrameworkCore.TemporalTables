using System;
using System.Linq;

namespace EntityFrameworkCore.TemporalTables.Query
{
    public static class TemporalQueries
    {
        public static IQueryable<T> ForSystemTimeAsOf<T>(this IQueryable<T> query, DateTime dateTime)
        {
            //return query.Provider.CreateQuery<T>(new ForSystemTimeExpression());
            return query;
        }
    }
}
