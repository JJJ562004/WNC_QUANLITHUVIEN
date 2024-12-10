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
        Account db = new Account();
        [HttpGet]
        public ActionResult DangKy()
        {
            return View();
        }

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
            con.ConnectionString = "Data Source=DESKTOP-7FI0AQQ;Initial Catalog=WNC_QUANLYTHUVIEN_REAL;Integrated Security=True;Encrypt=False";
        }
        [HttpPost]
        public ActionResult DangKy(Account acc)
        {
            try
            {
                connectionString();
                con.Open();

                cmd.Connection = con;
                cmd.CommandText = @"INSERT INTO Student 
    (FirstName, LastName, Email, PhoneNumber, StudentAddress, EnrollmentDate) 
    VALUES 
    (@FirstName, @LastName, @Email, @PhoneNumber, @StudentAddress, @EnrollmentDate)";

                // Thêm các tham số phù hợp với các cột trong bảng
                cmd.Parameters.AddWithValue("@FirstName", acc.Ten);
                cmd.Parameters.AddWithValue("@LastName", acc.Ho);
                cmd.Parameters.AddWithValue("@Email", acc.Email);
                cmd.Parameters.AddWithValue("@PhoneNumber", acc.Password);
                cmd.Parameters.AddWithValue("@StudentAddress", acc.DiaChi);
                cmd.Parameters.AddWithValue("@EnrollmentDate", DateTime.Now); // Hoặc acc.EnrollmentDate nếu được truyền từ View
                cmd.Parameters.AddWithValue("@StudentID ", acc.MSV); 
               


                cmd.ExecuteNonQuery();
                con.Close();

                return Redirect("~/Account/Login");
            }
            catch (Exception ex)
            {
                con.Close();
                return View("Error");
            }
        }

        public ActionResult Verify(Account acc)
        {
            connectionString();
            con.Open();

            cmd.Connection = con;
            cmd.CommandText = "select * from Student where StudentID='" + acc.MSV + "' and PhoneNumber='" + acc.Password + "'";
            dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                con.Close();
                return Redirect("~/LibraryBook/index");
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