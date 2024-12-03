using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using cuoiki_LTWNC.Models;
using System.Data.SqlClient;
namespace cuoiki_LTWNC.Controllers
{
    public class AccountController : Controller
    {
        SqlConnection con = new SqlConnection();
        SqlCommand cmd = new SqlCommand();
        SqlDataReader dr;
        // GET: Account
        [HttpGet]
        
        public ActionResult Login()
        {
            return View();
        }
        public ActionResult Finish()
        {
            return View();
        }
        public ActionResult Error()
        {
            return View();
        }
        public ActionResult Index()
        {
            return View();
        }
        void connectionString()
        {
            con.ConnectionString = "Data Source=BINHTRAN\\BINHVAN;Initial Catalog=Login;Integrated Security=True;Encrypt=False";
        }
        [HttpPost]
        public ActionResult Verify(Account acc)
        {
            connectionString();
            con.Open();

            cmd.Connection = con;
            cmd.CommandText = "select * from Login where username='" + acc.MSV + "' and password='" + acc.Password + "'";
            dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                con.Close();
                return View("Finish");
            }
            else
            {
                con.Close();
                return View("Error");
            }

        }



        //public ActionResult Login()
        //{
        //    return View();
        //}
        //[HttpPost]
        //public ActionResult Login(Account acc)
        //{
        //    var appDbContext = new AppDbContext();
        //}
    }
}