using System;
using System.Collections.Generic;
using refactor_me.Model.Contract;
using System.Data.SqlClient;

namespace refactor_me.Repository
{
    public class ProductsRepository : BaseRepository, IProductsRepository
    {
        public IEnumerable<Guid> All()
        {
            var conn = Helpers.NewConnection();
            var cmd = new SqlCommand("select id from product", conn);

            conn.Open();

            var rdr = cmd.ExecuteReader();
            var ids = ToGuidList(rdr);
            return ids;
        }

        public IEnumerable<Guid> ByName(string name)
        {
            var conn = Helpers.NewConnection();
            var nameParam = "@name";
            var cmd = new SqlCommand($"select id from product where lower(name) like {nameParam}", conn);

            cmd.Parameters.AddWithValue(nameParam, $"%{name.ToLower()}%");

            conn.Open();

            var rdr = cmd.ExecuteReader();
            var ids = ToGuidList(rdr);
            return ids;
        }

        public IProduct Get(Guid id)
        {
            var cmd = IdSqlCommand("select * from product where id", id);

            var rdr = cmd.ExecuteReader();
            if (!rdr.Read())
                return null;

            var idIndex = rdr.GetOrdinal("id");
            var nameIndex = rdr.GetOrdinal("name");
            var descrIndex = rdr.GetOrdinal("description");
            var priceIndex = rdr.GetOrdinal("price");
            var deliveryIndex = rdr.GetOrdinal("deliveryprice");

            var result = new Product
            {
                Id = rdr.GetGuid(idIndex),
                Name = rdr.GetString(nameIndex),
                Description = rdr.IsDBNull(descrIndex) ? null : rdr.GetString(descrIndex),
                Price = rdr.GetDecimal(priceIndex),
                DeliveryPrice = rdr.GetDecimal(deliveryIndex)
            };

            return result;
        }

        public void Remove(Guid id)
        {
            var cmd = IdSqlCommand("delete from productoption where productid", id);
            cmd.ExecuteNonQuery();

            cmd = IdSqlCommand("delete from product where id", id);
            cmd.ExecuteNonQuery();
        }

        public void Save(IProduct product, bool exists)
        {
            var idParam = "@Id";
            var nameParam = "@Name";
            var descrParam = "@Descr";
            var priceParam = "@Price";
            var deliveryParam = "@Delivery";

            SqlCommand cmd;

            if (exists)
            {
                cmd = IdSqlCommand($"update product set name = {nameParam}, description = {descrParam}, price = {priceParam}, deliveryprice = {descrParam} where id", product.Id);
            }
            else
            {
                var conn = Helpers.NewConnection();

                conn.Open();

                cmd = new SqlCommand($"insert into product (id, name, description, price, deliveryprice) values ({idParam}, {nameParam}, {descrParam}, {priceParam}, {deliveryParam})", conn);

                cmd.Parameters.AddWithValue(idParam, product.Id);
            }

            cmd.Parameters.AddWithValue(nameParam, product.Name);
            cmd.Parameters.AddWithValue(descrParam, product.Description);
            cmd.Parameters.AddWithValue(priceParam, product.Price);
            cmd.Parameters.AddWithValue(deliveryParam, product.DeliveryPrice);

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