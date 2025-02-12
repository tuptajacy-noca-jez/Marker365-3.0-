﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Market365_3._0.Zamowienie
{
    public partial class Zamowienie : System.Web.UI.Page
    {
        User currentUser;
        Order newOrder;
        List<int> ids;
        List<double> quantities;
        List<double> total;
        string Polaczenie;
        double sum;
        double rabat;
        protected void Page_Load(object sender, EventArgs e)
        {
            
            ValidationSettings.UnobtrusiveValidationMode = UnobtrusiveValidationMode.None;
            nameValidator.Validate();
            surnameValidator.Validate();
            zipCodeValidator.Validate();
            cityValidator.Validate();
            streetValidator.Validate();
            houseNumberValidator.Validate();
            phoneNumberValidator.Validate();
            currentUser = (User)Session["user"];
            newOrder =  (Order) Session["order"];
            ids = (List<Int32>) Session["orderProductIds"];
            sum = (double)Session["cartValue"];
            quantities = (List<double>)Session["orderProductquantity"];
            total = (List<double>)Session["totalProductValue"];

            rabat = 1;
            
            if (discount.Text =="alerabat2137")
            {
                rabat = 0.9;
                value.Visible = false;
                discountValue.Visible = true;
                newOrder.Value = Math.Round(sum*rabat,2);   
            }
            else
            {
                newOrder.Value = Math.Round(sum,2);
            }
            value.Text = "Wartość koszyka: " + Math.Round(sum, 2) + "zł";
            discountValue.Text = "Wartość koszyka po rabacie: " + Math.Round(sum*rabat, 2) + "zł";
            if (zipCode.Text == "" && city.Text == "" && street.Text == "" && houseNumber.Text == "")
                ZaladujDane();
            
        }
        /// <summary>
        /// create new order
        /// </summary>
        public void CreateOrder()
        {
            newOrder.ProductsId = ids;
            newOrder.ProductsQuantity = quantities;
            newOrder.Login = currentUser.Login;
            newOrder.OrderId = currentUser.Login + DateTime.Now.Ticks.ToString();
                newOrder.City=city.Text;
                newOrder.ZipCode=zipCode.Text;
                newOrder.Street=street.Text;
                newOrder.HouseNumber=houseNumber.Text;
                newOrder.PhoneNumber=phoneNumber.Text;
                newOrder.Email=email.Text;
            
                Session["order"] = newOrder;
            AddOrderToDatabase();
        }
      
        public void AddOrderToDatabase()
        {
            try
            {
                Polaczenie = ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString;
                SqlConnection sql = new SqlConnection(Polaczenie);
                sql.Open();
                SqlCommand cmd = new SqlCommand("INSERT INTO [orders] VALUES (@Id,@userlogin,@value,@name,@surname,@zipCode,@city,@street,@houseNumber,@phoneNumber,@email,@status)", sql);
                cmd.Parameters.AddWithValue("@Id", newOrder.OrderId);
                cmd.Parameters.AddWithValue("@userlogin", currentUser.Login);
                cmd.Parameters.AddWithValue("@name", name.Text);
                cmd.Parameters.AddWithValue("@surname", surname.Text);
                cmd.Parameters.AddWithValue("@street", street.Text);
                cmd.Parameters.AddWithValue("@houseNumber", houseNumber.Text);
                cmd.Parameters.AddWithValue("@zipCode", zipCode.Text);
                cmd.Parameters.AddWithValue("@city", city.Text);
                cmd.Parameters.AddWithValue("@phoneNumber", phoneNumber.Text);
                cmd.Parameters.AddWithValue("@email", email.Text);
                cmd.Parameters.AddWithValue("@value", newOrder.Value);
                cmd.Parameters.AddWithValue("@status", "Oczekujace na platnosc");
                cmd.ExecuteNonQuery();
                sql.Close();

                sql.Open();

                //TODO: dodac parametr
                for (int i = 0; i < ids.Count(); i++)
                {
                    cmd = new SqlCommand("INSERT INTO [orderPosition] VALUES (@IdOrder,@IdProduct,@quantity,@totalProductValue)", sql);
                    cmd.Parameters.AddWithValue("@IdOrder", newOrder.OrderId);
                    cmd.Parameters.AddWithValue("@IdProduct", ids[i]);
                    cmd.Parameters.AddWithValue("@quantity", quantities[i]);
                    cmd.Parameters.AddWithValue("@totalProductValue", total[i]);
                    cmd.ExecuteNonQuery();

                }
                sql.Close();
            }
            catch { }

        }
        public void ZaladujDane()
        {
            name.Text= currentUser.Name;
            surname.Text = currentUser.Surname;
            city.Text = currentUser.City;
            zipCode.Text = currentUser.ZipCode;
            street.Text = currentUser.Street;
            houseNumber.Text = currentUser.HouseNumber;
            phoneNumber.Text = currentUser.PhoneNumber;
            email.Text = currentUser.Email;
        }

        protected void cancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Koszyk/Koszyk.aspx");
        }

        protected void order_Click(object sender, EventArgs e)
        {
        if (streetValidator.IsValid == true && houseNumberValidator.IsValid == true && zipCodeValidator.IsValid == true && cityValidator.IsValid == true && phoneNumberValidator.IsValid == true && emailValidator.IsValid == true)
        {
                CreateOrder();
                Session["cart"] = null;
                UsunKoszyk();
            Response.Redirect("/FinalizacjaZamowienia/FinalizacjaZamowienia.aspx");
        }
        }
        /// <summary>
        /// delete cart after order
        /// </summary>
        public void UsunKoszyk()
        {
            try
            {
                Polaczenie = ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString;
                SqlConnection sql = new SqlConnection(Polaczenie);
                sql.Open();
                SqlCommand cmd = new SqlCommand("DELETE from [cartPosition] WHERE IdCard='" + currentUser.Login + "'", sql);
                cmd.ExecuteNonQuery();
                sql.Close();
            }
            catch { }
        }
    }
}