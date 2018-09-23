using System.Collections.Generic;
using System.Text;

namespace EntityFrameworkCore.TemporalTables.SqlTemplates.Helpers
{
    internal static class SqlTemplateProcessor
    {
        /// <summary>
        /// Processes the specified SQL query by replacing placeholder values with the actual ones, provided in the passed dictionary.
        /// </summary>
        internal static string Process(string sqlQuery, IReadOnlyDictionary<string, string> valuesToReplace)
        {
            StringBuilder sbSqlQuery = new StringBuilder(sqlQuery);

            foreach (var kvp in valuesToReplace)
            {
                sbSqlQuery = sbSqlQuery.Replace(kvp.Key, kvp.Value);
            }

            return sbSqlQuery.ToString();
        }
    }
}
