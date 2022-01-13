using Dapper;
using System.Collections.Generic;
using System.Linq;

namespace ClevarWeb
{
    public static class ClevarDbContextExtensions
    {
        public static List<IDictionary<string, object>> DapperQuery(this ClevarWeb.Data.ClevarDbContext db, string query, object parameters = null)
        {
            var result = db.Connection.Query(query, parameters).ToList();
            return result.Select(x => (IDictionary<string, object>)x).ToList();
        }
    }
}
