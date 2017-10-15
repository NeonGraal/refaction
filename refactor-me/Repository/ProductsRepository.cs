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

        public void Save(IProduct product, bool exists)
        {
            var conn = Helpers.NewConnection();
            var idParam = "@Id";
            var nameParam = "@Name";
            var descrParam = "@Descr";
            var priceParam = "@Price";
            var deliveryParam = "@Delivery";
            var cmdSql = exists ?
                $"insert into product (id, name, description, price, deliveryprice) values ({idParam}, {nameParam}, {descrParam}, {priceParam}, {deliveryParam})" :
                $"update product set name = {nameParam}, description = {descrParam}, price = {priceParam}, deliveryprice = {descrParam} where id = {idParam}";
            var cmd = new SqlCommand(cmdSql, conn);

            cmd.Parameters.Add(idParam, SqlDbType.UniqueIdentifier);
            cmd.Parameters.Add(nameParam, SqlDbType.NVarChar);
            cmd.Parameters.Add(descrParam, SqlDbType.NVarChar);
            cmd.Parameters.Add(priceParam, SqlDbType.Decimal);
            cmd.Parameters.Add(deliveryParam, SqlDbType.Decimal);

            cmd.Parameters[idParam].Value = product.Id;
            cmd.Parameters[nameParam].Value = product.Name;
            cmd.Parameters[descrParam].Value = product.Description;
            cmd.Parameters[priceParam].Value = product.Price;
            cmd.Parameters[deliveryParam].Value = product.DeliveryPrice;

            conn.Open();
            cmd.ExecuteNonQuery();
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