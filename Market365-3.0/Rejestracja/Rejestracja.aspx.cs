﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data.SqlClient;
using Market365_3._0;

namespace projekt
{

    public partial class Rejestracja : System.Web.UI.Page
    {
        string Polaczenie;
        User currentUser;
        protected void Page_Load(object sender, EventArgs e)
        {
            ValidationSettings.UnobtrusiveValidationMode = UnobtrusiveValidationMode.None;
            loginValidator.Validate();
            passwordValidator.Validate();
            nameValidator.Validate();
            surnameValidator.Validate();
            zipCodeValidator.Validate();
            cityValidator.Validate();
            streetValidator.Validate();
            houseNumberValidator.Validate();
            phoneNumberValidator.Validate();
            emailValidator.Validate();
            passwordConfirmValidator.Validate();
            CompareValidator1.Validate();
            CompareValidator2.Validate();
            loginValidator.ErrorMessage = "Podany login jest za krótki";
            string Password = password.Text;
            password.Attributes.Add("value", Password);
            string PasswordConfirm = passwordConfirm.Text;
            passwordConfirm.Attributes.Add("value", PasswordConfirm);
        }

        protected void zarejestruj_Click(object sender, EventArgs e)
        {
            if(checkEmpty())
            { 
            if (loginValidator.IsValid == true && CompareValidator1.IsValid == true && CompareValidator2.IsValid == true && passwordValidator.IsValid == true && passwordConfirmValidator.IsValid == true && nameValidator.IsValid == true && surnameValidator.IsValid == true && streetValidator.IsValid == true && houseNumberValidator.IsValid == true && zipCodeValidator.IsValid == true && cityValidator.IsValid == true && phoneNumberValidator.IsValid == true && emailValidator.IsValid == true)
            {
                if (checkLogin(login.Text))
                {
                        try
                        {
                            Polaczenie = ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString;
                            SqlConnection sql = new SqlConnection(Polaczenie);
                            sql.Open();
                            SqlCommand cmd = new SqlCommand("INSERT INTO [customers] VALUES (@login,@password,@name,@surname,@street,@houseNumber,@zipCode,@city,@phoneNumber,@email,@isActive)", sql);
                            cmd.Parameters.AddWithValue("@login", login.Text);
                            cmd.Parameters.AddWithValue("@password", password.Text);
                            cmd.Parameters.AddWithValue("@name", name.Text);
                            cmd.Parameters.AddWithValue("@surname", surname.Text);
                            cmd.Parameters.AddWithValue("@street", street.Text);
                            cmd.Parameters.AddWithValue("@houseNumber", houseNumber.Text);
                            cmd.Parameters.AddWithValue("@zipCode", zipCode.Text);
                            cmd.Parameters.AddWithValue("@city", city.Text);
                            cmd.Parameters.AddWithValue("@phoneNumber", phoneNumber.Text);
                            cmd.Parameters.AddWithValue("@email", email.Text);
                            cmd.Parameters.AddWithValue("@isActive", true);
                            cmd.ExecuteNonQuery();
                            sql.Close();
                            currentUser = new User();
                            currentUser.Login = login.Text;
                            currentUser.Password = password.Text;
                            currentUser.Name = name.Text;
                            currentUser.Surname = name.Text;
                            currentUser.City = city.Text;
                            currentUser.ZipCode = zipCode.Text;
                            currentUser.HouseNumber = houseNumber.Text;
                            currentUser.Street = street.Text;
                            currentUser.PhoneNumber = phoneNumber.Text;
                            currentUser.Email = email.Text;
                            currentUser.IsActive = true;
                        }
                        catch { }

                    Session["user"] = currentUser;
                    //tworzenie koszyka dla uzytkownika
                    /*sql.Open();
                    cmd = new SqlCommand("INSERT INTO [cart] VALUES (@Id)", sql);
                    cmd.Parameters.AddWithValue("@Id", login.Text);
                    cmd.ExecuteNonQuery();
                    sql.Close();
                    */
                    Response.Redirect("/StronaGlowna/StronaGlowna.aspx");
                }
                else
                {
                    loginValidator.IsValid = false;
                    loginValidator.ErrorMessage = "Podany login jest już zajęty";
                }
            }
            }
        }

        bool checkEmpty()
        {
            loginRequiredValidator.Validate();
            passwordRequiredValidator.Validate();
            passwordConfirmRequiredValidator.Validate();
            nameRequiredValidator.Validate();
            surnameRequiredValidator.Validate();
            zipCodeRequiredValidator.Validate();
            cityRequiredValidator.Validate();
            streetRequiredValidator.Validate();
            houseNumberRequiredValidator.Validate();
            if (loginRequiredValidator.IsValid == true && passwordRequiredValidator.IsValid == true && passwordConfirmRequiredValidator.IsValid == true && nameRequiredValidator.IsValid == true && surnameRequiredValidator.IsValid == true && zipCodeRequiredValidator.IsValid == true && cityRequiredValidator.IsValid == true && streetRequiredValidator.IsValid == true && houseNumberRequiredValidator.IsValid == true)
                return true;
            else
                return false;
                    }

        /// <summary>
        /// check the availability of the login 
        /// </summary>
        /// <param name="login">user login</param>
        /// <returns>true when login is available </returns>
        bool checkLogin(string login)
        {
            try
            {
                Polaczenie = ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString;
                SqlConnection sql = new SqlConnection(Polaczenie);
                sql.Open();
                SqlCommand cmd = new SqlCommand("select [login] from [customers]");
                cmd.Connection = sql;
                SqlDataReader rd = cmd.ExecuteReader();
                while (rd.Read())
                {
                    if (rd[0].ToString() == login)
                        return false;
                }
                sql.Close();
            }
            catch { }
            return true;
        }

        protected void Login_TextChanged(object sender, EventArgs e)
        {
            if (!checkLogin(login.Text))
            {
                loginValidator.IsValid = false;
                loginValidator.ErrorMessage = "Podany login jest już zajęty";
            }
        }
    }
}