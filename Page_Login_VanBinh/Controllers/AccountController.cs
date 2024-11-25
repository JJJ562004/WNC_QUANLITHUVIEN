using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Page_Login_VanBinh.Models;
using System.Data.SqlClient;
namespace Page_Login_VanBinh.Controllers
{
    public class AccountController : Controller
    {
        SqlConnection con =new SqlConnection();
        SqlCommand cmd = new SqlCommand();
        SqlDataReader dr;
        // GET: Account
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        void connectionString()
        {
            con.ConnectionString = "Data Source=BINHTRAN/BINHVAN;Initial Catalog=Login;Integrated Security=True;Encrypt=False";
        }
        [HttpPost]
        public ActionResult Verify(Account acc)
        {
            connectionString();
            con.Open();
            
            cmd.Connection = con;
            cmd.CommandText = "select * from Login where username='"+acc.Name+"' and password='"+acc.Password+"'";
            dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                con.Close();
                return View("Create");
            }
            else
            {
                con.Close();
                return View("Error");
            }
              
        }
    }
}