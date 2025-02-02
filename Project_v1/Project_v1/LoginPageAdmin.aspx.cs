﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Web.Configuration;

namespace Project_v1
{
    public partial class WebForm3 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["Adminid"] = null;

                Session.Clear();
                Session.RemoveAll();
            }

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            string connString = WebConfigurationManager.ConnectionStrings["database"].ConnectionString;
            SqlConnection myConn = new SqlConnection(connString);
            try
            {
                myConn.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = myConn;
                command.CommandText = "SELECT * FROM AdminLogin WHERE Adminid=@Username";
                command.Parameters.AddWithValue("@Username", TextBox1.Text);
                SqlDataReader myReader;
                myReader = command.ExecuteReader();

                if (myReader.HasRows)
                {
                    myReader.Read();
                    if (myReader["AdminPassword"].ToString() != TextBox2.Text)
                    {
                        Label2.Text = "Invalid Password!";
                    }
                    else
                    {
                        Session["Adminid"] = myReader["Adminid"].ToString();
                        HttpCookie c1 = Request.Cookies["AdminCookie"];
                        if (c1 == null)
                            c1 = new HttpCookie("AdminCookie");
                         c1["ID"] = myReader["Adminid"].ToString();
                         Response.Cookies.Add(c1);
                         Response.Redirect("~/AdminHome.aspx");
                        //Label2.Text = "SUCCESS";
                    }
                }
                else
                    Label2.Text = "Admin not found!";
            }
            catch { }
            finally { myConn.Close(); }
        }
    }
}