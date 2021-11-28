﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data.SqlClient;

namespace Market365_3._0 {
    public class Produkt {
        public Int32 id { get; set; }
        public string image { get; set; }
        public string name { get; set; }
        public float price { get; set; }
        public float quantity { get; set; }
        public string unit { get; set; }
        public float ocena { get; set; }

        private List<ProductField> cechyProduktu;

        public Produkt() { }

        public Produkt(string name) {
            cechyProduktu = new List<ProductField>();
        }
        public Produkt(int id) {
            cechyProduktu = new List<ProductField>();

            string Polaczenie = ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString;
            SqlConnection sql = new SqlConnection(Polaczenie);
            try {
                sql.Open();
                SqlCommand cmd = new SqlCommand("select * from [products] WHERE ID=@login AND password=@password", sql);
                int count = Convert.ToInt32(cmd.ExecuteScalar());

                if (count == 1) {
                    //cmd = new SqlCommand("update [customers] set isActive=@isActive WHERE login='" + login.Text + "'", sql);
                    //cmd.Parameters.AddWithValue("@isActive", "true");
                    //cmd.ExecuteNonQuery();
                }
                sql.Close();
            }
            catch(Exception ex) {
                Console.WriteLine(ex.Message);
            }
            
        }
    }
}