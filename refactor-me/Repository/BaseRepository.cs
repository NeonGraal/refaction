using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;

namespace refactor_me.Repository
{
    public class BaseRepository
    {
        protected SqlCommand IdSqlCommand(string sql, Guid id)
        {
            var conn = Helpers.NewConnection();
            var idParam = "@Id";
            var cmd = new SqlCommand($"{sql} = {idParam}", conn);

            cmd.Parameters.AddWithValue(idParam, id);

            conn.Open();

            return cmd;
        }

        protected IEnumerable<Guid> ToGuidList(SqlDataReader reader)
        {
            var idIndex = reader.GetOrdinal("id");
            var ids = reader.Cast<IDataRecord>()
                .Select(d => d.GetGuid(idIndex))
                .ToList();
            return ids;
        }
    }
}