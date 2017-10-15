using System;
using System.Collections.Generic;
using refactor_me.Model.Contract;
using refactor_me.Models;
using System.Data.SqlClient;
using System.Data;

namespace refactor_me.Repository
{
    public class ProductOptionsRepository : BaseRepository, IProductOptionsRepository
    {
        public IEnumerable<Guid> All(Guid productId)
        {
            var ids = new List<Guid>();
            var cmd = IdSqlCommand("select id from productoption where productid", productId);

            var rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                var id = Guid.Parse(rdr["id"].ToString());
                ids.Add(id);
            }
            return ids;
        }

        public IProductOption Get(Guid id)
        {            
            var cmd = IdSqlCommand("select * from productoption where id", id);

            var rdr = cmd.ExecuteReader();
            if (!rdr.Read())
                return null;

            return new ProductOption
            {
                Id = Guid.Parse(rdr["Id"].ToString()),
                Name = rdr["Name"].ToString(),
                Description = (DBNull.Value == rdr["Description"]) ? null : rdr["Description"].ToString(),
            };
        }

        public void Remove(Guid id)
        {
            var cmd = IdSqlCommand("delete from productoption where id", id);
            cmd.ExecuteNonQuery();
        }

        public void Save(IProductOption option, bool exists)
        {
            var idParam = "@Id";
            var prodIdParam = "@ProdId";
            var nameParam = "@Name";
            var descrParam = "@Descr";
            SqlCommand cmd;

            if (exists)
            {
                cmd = IdSqlCommand($"update productoption set name = {nameParam}, description = {descrParam} where id", option.Id);
            }
            else
            {
                var conn = Helpers.NewConnection();
                conn.Open();
                cmd = new SqlCommand($"insert into productoption (id, productid, name, description) values ({idParam}, {prodIdParam}, {nameParam}, {descrParam})", conn);

                cmd.Parameters.Add(idParam, SqlDbType.UniqueIdentifier);
                cmd.Parameters.Add(prodIdParam, SqlDbType.UniqueIdentifier);

                cmd.Parameters[idParam].Value = option.Id;
                cmd.Parameters[prodIdParam].Value = option.ProductId;
            }

            cmd.Parameters.Add(nameParam, SqlDbType.NVarChar);
            cmd.Parameters.Add(descrParam, SqlDbType.NVarChar);

            cmd.Parameters[nameParam].Value = option.Name;
            cmd.Parameters[descrParam].Value = option.Description;

            cmd.ExecuteNonQuery();
        }

        private class ProductOption : IProductOption
        {
            public Guid Id { get; set; }

            public Guid ProductId { get; set; }

            public string Name { get; set; }

            public string Description { get; set; }
        }
    }
}