using System;
using System.Collections.Generic;
using refactor_me.Model.Contract;
using refactor_me.Models;
using System.Data.SqlClient;
using System.Data;

namespace refactor_me.Repository
{
    public class ProductsRepository : IProductsRepository
    {
        public IEnumerable<Guid> All()
        {
            var ids = new List<Guid>();
            var conn = Helpers.NewConnection();
            var cmd = new SqlCommand("select id from product", conn);
            conn.Open();

            var rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                var id = Guid.Parse(rdr["id"].ToString());
                ids.Add(id);
            }
            return ids;
        }

        public IEnumerable<Guid> ByName(string name)
        {
            var ids = new List<Guid>();
            var conn = Helpers.NewConnection();
            var nameParam = "@name";
            var cmd = new SqlCommand($"select id from product where lower(name) like {nameParam}", conn);
            cmd.Parameters.Add(nameParam, SqlDbType.NVarChar);
            cmd.Parameters[nameParam].Value = "%" + name.ToLower() + "%";
            conn.Open();

            var rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                var id = Guid.Parse(rdr["id"].ToString());
                ids.Add(id);
            }
            return ids;
        }

        public IProduct Get(Guid id)
        {
            var conn = Helpers.NewConnection();
            var cmd = new SqlCommand($"select * from product where id = '{id}'", conn);
            conn.Open();

            var rdr = cmd.ExecuteReader();
            if (!rdr.Read())
                return null;

            return new Product
            {
                Id = Guid.Parse(rdr["Id"].ToString()),
                Name = rdr["Name"].ToString(),
                Description = (DBNull.Value == rdr["Description"]) ? null : rdr["Description"].ToString(),
                Price = decimal.Parse(rdr["Price"].ToString()),
                DeliveryPrice = decimal.Parse(rdr["DeliveryPrice"].ToString())
            };
        }

        private class Product : IProduct
        {
            public decimal DeliveryPrice { get; set; }

            public string Description { get; set; }

            public Guid Id { get; set; }

            public string Name { get; set; }

            public decimal Price { get; set; }        
        }
    }
}