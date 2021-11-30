﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Market365_3._0.Koszyk
{
    public partial class Koszyk : System.Web.UI.Page
    {
        Cart kosz;
        User currentUser;
        double value = 0.0;
        protected void Page_Load(object sender, EventArgs e)
        {
            currentUser=(User)Application["user"];
            kosz = new Cart(currentUser.Login);
            ListView1.DataSource = kosz.dt;

            foreach (var item in kosz.produkts)
            {
                value += item.price * item.quantity;
            }
            cenaSuma.Text = "Do Zapłaty: " +Math.Round(value,2)+" zł";

            ListView1.DataBind();

           
        }

        protected void anulujButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/stronaGlowna/stronaGlowna.aspx");
        }

        protected void zamowButton_Click(object sender, EventArgs e)
        {
            kosz = new Cart(currentUser.Login); //odświeżenie danych tak na wszelki wypadek
            List<int> ids = new List<int>();
            List<double> quantities = new List<double>();
            List<double> totalProductValue = new List<double>();
            double cartValue = 0.0;
            foreach (var item in kosz.produkts)
            {
                ids.Add(item.id);
                quantities.Add(item.quantity);
                totalProductValue.Add(item.price * item.quantity);
                cartValue += item.price * item.quantity;
            }
            Application.Lock();
            Application["orderProductIds"] = ids;
            Application["orderProductquantity"] = quantities;
            Application["totalProductValue"] = totalProductValue;
            Application["cartValue"] = cartValue;
            Application.UnLock();

            Response.Redirect("~/Zamowienie/Zamowienie.aspx");
        }   

        protected void iloscProduktu_TextChanged(object sender, EventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            int textboxId = Int32.Parse(textBox.ToolTip);
            string tekst = textBox.Text;

            String Polaczenie;
            Polaczenie = ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString;
            SqlConnection sql = new SqlConnection(Polaczenie);

            SqlCommand cmd = new SqlCommand("UPDATE cartPosition SET quantity="+ tekst +" WHERE cartPosition.IdProduct ="+ textboxId +" AND cartPosition.IdCard = '"+ currentUser.Login + "'" ,sql);
            sql.Open();
            cmd.ExecuteNonQuery();
            sql.Close();

            ListView1.DataSource = null;
            ListView1.DataBind();

            value = 0.0;
            currentUser = (User)Application["user"];
            Cart kosz = new Cart(currentUser.Login);
            ListView1.DataSource = kosz.dt;

            foreach (var item in kosz.produkts)
            {
                value += item.price * item.quantity;
            }
            cenaSuma.Text = "Do Zapłaty: " + Math.Round(value, 2) + " zł";

            ListView1.DataBind();

        }

        protected void usunProdukt_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            int buttonId = Int32.Parse(button.ToolTip);

            String Polaczenie;
            Polaczenie = ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString;
            SqlConnection sql = new SqlConnection(Polaczenie);

            SqlCommand cmd = new SqlCommand("DELETE FROM [cartPosition] WHERE IdCard='"+ currentUser.Login + "' AND IdProduct='"+buttonId+"'", sql);
            sql.Open();
            cmd.ExecuteNonQuery();
            sql.Close();
            ListView1.DataSource = null;
            ListView1.DataBind();

            value = 0.0;
            currentUser = (User)Application["user"];
            Cart kosz = new Cart(currentUser.Login);
            ListView1.DataSource = kosz.dt;

            foreach (var item in kosz.produkts)
            {
                value += item.price * item.quantity;
            }
            cenaSuma.Text = "Do Zapłaty: " + Math.Round(value, 2) + " zł";

            ListView1.DataBind();

        }

        protected void iloscproduktu_TextChanged1(object sender, EventArgs e)
        {
            DropDownList textBox = (DropDownList)sender;
            int textboxId = Int32.Parse(textBox.ToolTip);
            string tekst = textBox.Text;

            String Polaczenie;
            Polaczenie = ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString;
            SqlConnection sql = new SqlConnection(Polaczenie);

            SqlCommand cmd = new SqlCommand("UPDATE cartPosition SET quantity=" + tekst + " WHERE cartPosition.IdProduct =" + textboxId + " AND cartPosition.IdCard = '" + currentUser.Login + "'", sql);
            sql.Open();
            cmd.ExecuteNonQuery();
            sql.Close();

            ListView1.DataSource = null;
            ListView1.DataBind();

            value = 0.0;
            currentUser = (User)Application["user"];
            Cart kosz = new Cart(currentUser.Login);
            ListView1.DataSource = kosz.dt;

            foreach (var item in kosz.produkts)
            {
                value += item.price * item.quantity;
            }
            cenaSuma.Text = "Do Zapłaty: " + Math.Round(value, 2) + " zł";

            ListView1.DataBind();
        }
    }
}