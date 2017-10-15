using refactor_me.Models;
using System;
using System.Data;
using System.Data.SqlClient;

namespace refactor_me.Repository
{
    public class BaseRepository
    {
        protected SqlCommand IdSqlCommand(string sql, Guid id)
        {
            var conn = Helpers.NewConnection();
            conn.Open();

            var idParam = "@Id";
            var cmd = new SqlCommand($"{sql} = {idParam}", conn);
            cmd.Parameters.Add(idParam, SqlDbType.UniqueIdentifier);
            cmd.Parameters[idParam].Value = id;

            return cmd;
        }
    }
}