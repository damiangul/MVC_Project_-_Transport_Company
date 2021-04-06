using Projekt.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Projekt.Controllers
{
    public class HomeController : Controller
    {
        string connectionString = @"Data Source=.;Initial Catalog=Projekt;Integrated Security=True";

        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(KONTO model, string returnUrl)
        {
            ProjektEntities db = new ProjektEntities();
            var dataItem = db.KONTOes.Where(x => x.login_user == model.login_user && x.password_user == model.password_user).First();

            if(dataItem != null)
            {
                FormsAuthentication.SetAuthCookie(dataItem.login_user, false);
                if(Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/") && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                {
                    return Redirect(returnUrl);
                } 
                else
                {
                    return RedirectToAction("Index");
                }
            }
            else
            {
                ModelState.AddModelError("", "Invalid user/pass");
                return View();
            }
        }

        [Authorize]
        public ActionResult SignOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Home");
        }

        public ActionResult RegisterAccount()
        {
            return View();
        }

        [HttpPost]
        public ActionResult RegisterAccount(KlientModel klientModel)
        {
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();

                string query = "INSERT INTO adres VALUES(@KlientUlica, @KlientNumer, @KlientMiasto, @KlientKod);";
                SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
                sqlCmd.Parameters.AddWithValue("@KlientUlica", klientModel.KlientUlica);
                sqlCmd.Parameters.AddWithValue("@KlientNumer", klientModel.KlientNumer);
                sqlCmd.Parameters.AddWithValue("@KlientMiasto", klientModel.KlientMiasto);
                sqlCmd.Parameters.AddWithValue("@KlientKod", klientModel.KlientKod);
                sqlCmd.ExecuteNonQuery();

                query = "SELECT id FROM adres" +
                    " WHERE ulica=@KlientUlica and numer=@KlientNumer";
                sqlCmd = new SqlCommand(query, sqlCon);
                sqlCmd.Parameters.AddWithValue("@KlientUlica", klientModel.KlientUlica);
                sqlCmd.Parameters.AddWithValue("@KlientNumer", klientModel.KlientNumer);
                int id_adres = Convert.ToInt32(sqlCmd.ExecuteScalar());

                query = "INSERT INTO KONTO VALUES(@KlientLogin, @KlientHaslo, 'user');";
                sqlCmd = new SqlCommand(query, sqlCon);
                sqlCmd.Parameters.AddWithValue("@KlientLogin", klientModel.KlientLogin);
                sqlCmd.Parameters.AddWithValue("@KlientHaslo", klientModel.KlientHaslo);
                sqlCmd.ExecuteNonQuery();

                query = "SELECT id from KONTO" +
                    " WHERE login_user=@KlientLogin and password_user=@KlientHaslo";
                sqlCmd = new SqlCommand(query, sqlCon);
                sqlCmd.Parameters.AddWithValue("@KlientLogin", klientModel.KlientLogin);
                sqlCmd.Parameters.AddWithValue("@KlientHaslo", klientModel.KlientHaslo);
                int id_konto = Convert.ToInt32(sqlCmd.ExecuteScalar());


                query = "INSERT INTO klient VALUES(@id_adres, @id_konto, @KlientImie, @KlientNazwisko, @KlientTelefon, @KlientEmail)";
                sqlCmd = new SqlCommand(query, sqlCon);
                sqlCmd.Parameters.AddWithValue("@id_adres", id_adres);
                sqlCmd.Parameters.AddWithValue("@id_konto", id_konto);
                sqlCmd.Parameters.AddWithValue("@KlientImie", klientModel.KlientImie);
                sqlCmd.Parameters.AddWithValue("@KlientNazwisko", klientModel.KlientNazwisko);
                sqlCmd.Parameters.AddWithValue("@KlientTelefon", klientModel.KlientTelefon);
                sqlCmd.Parameters.AddWithValue("@KlientEmail", klientModel.KlientEmail);
                sqlCmd.ExecuteNonQuery();
            }

            return RedirectToAction("Login", "Home");
        }

        //[Authorize(Roles="admin")]
        //public ActionResult About()
        //{
        //    ViewBag.Message = "Your application description page.";

        //    return View();
        //}

        //public ActionResult Contact()
        //{
        //    ViewBag.Message = "Your contact page.";

        //    return View();
        //}
    }
}