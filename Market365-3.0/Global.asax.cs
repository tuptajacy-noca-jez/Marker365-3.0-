﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace Market365_3._0
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            
        }

        protected void Session_Start(object sender, EventArgs e)
        {
            //Session.Lock();
            Session["user"] = new User();
            Session["order"] = new Order();
            Session["cart"] = new Cart();
            Session["orderProductIds"] = new List<int>();
            Session["orderProductquantity"] = new List<float>();
            Session["totalProductValue"] = new List<double>();
            Session["cartValue"] = 0.00;
            Session["reloadFlag"] = 0;
            //Session.UnLock();
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            string b = sender.GetType().Name;
            _ = b;
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }

        protected static string ReturnEncodedBase64UTF8(object rawImg)
        {
            string img = "data:image/jpg;base64,{0}";
            byte[] toEncodeAsBytes = (byte[])rawImg;
            string returnValue = System.Convert.ToBase64String(toEncodeAsBytes);
            return String.Format(img, returnValue);
        }
    }


}