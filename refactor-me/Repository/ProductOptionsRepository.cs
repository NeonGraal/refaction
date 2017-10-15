using System;
using System.Collections.Generic;
using refactor_me.Model.Contract;
using System.Data.SqlClient;

namespace refactor_me.Repository
{
    public class ProductOptionsRepository : BaseRepository, IProductOptionsRepository
    {
        public IEnumerable<Guid> All(Guid productId)
        {
            var cmd = IdSqlCommand("select id from productoption where productid", productId);

            var rdr = cmd.ExecuteReader();
            var ids = ToGuidList(rdr);
            return ids;
        }

        public IProductOption Get(Guid id)
        {
            var cmd = IdSqlCommand("select * from productoption where id", id);

            var rdr = cmd.ExecuteReader();
            if (!rdr.Read())
                return null;

            var idIndex = rdr.GetOrdinal("id");
            var prodIdIndex = rdr.GetOrdinal("productid");
            var nameIndex = rdr.GetOrdinal("name");
            var descrIndex = rdr.GetOrdinal("description");

            var result = new ProductOption
            {
                Id = rdr.GetGuid(idIndex),
                ProductId = rdr.GetGuid(prodIdIndex),
                Name = rdr.GetString(nameIndex),
                Description = rdr.IsDBNull(descrIndex) ? null : rdr.GetString(descrIndex),
            };

            return result;
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

                cmd.Parameters.AddWithValue(idParam, option.Id);
                cmd.Parameters.AddWithValue(prodIdParam, option.ProductId);
            }

            cmd.Parameters.AddWithValue(nameParam, option.Name);
            cmd.Parameters.AddWithValue(descrParam, option.Description);

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