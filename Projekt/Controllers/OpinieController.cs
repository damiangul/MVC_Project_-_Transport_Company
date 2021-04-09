using Projekt.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Projekt.Controllers
{
    public class OpinieController : Controller
    {
        string connectionString = @"Data Source=.;Initial Catalog=Projekt;Integrated Security=True";
        
        [Authorize(Roles = "user")]
        public ActionResult Index()
        {
            DataTable dataTable = new DataTable();
            string nameUser = User.Identity.Name;

            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();

                string query = "SELECT id FROM KONTO WHERE login_user=@UserName";
                SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
                sqlCmd.Parameters.AddWithValue("@UserName", nameUser);
                int idCurrentUser = Convert.ToInt32(sqlCmd.ExecuteScalar());

                query = "SELECT p.imie, p.nazwisko, p.id " +
                    "FROM pracownik p INNER JOIN przesyłka pr " +
                    "ON p.id = pr.pracownik_id INNER JOIN klient k " +
                    "ON k.id = pr.klient_id " +
                    "WHERE k.konto_id = @UserID;";
                SqlDataAdapter sqlDa = new SqlDataAdapter(query, sqlCon);
                sqlDa.SelectCommand.Parameters.AddWithValue("@UserID", idCurrentUser);
                sqlDa.Fill(dataTable);
            }
            return View(dataTable);
        }

        // GET: Opinie/Details/5
        [Authorize(Roles = "user")]
        public ActionResult Details(int id)
        {
            DataTable dataTable = new DataTable();
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                string query = "SELECT opis " +
                    "FROM opinia " +
                    "WHERE pracownik_id = @ID";
                SqlDataAdapter sqlDa = new SqlDataAdapter(query, sqlCon);
                sqlDa.SelectCommand.Parameters.AddWithValue("@ID", id);
                sqlDa.Fill(dataTable);
            }
            return View(dataTable);
        }

        // GET: Opinie/Edit/5
        [Authorize(Roles = "user")]
        public ActionResult Edit(int id)
        {
            OpiniaModel opiniaModel = new OpiniaModel();
            DataTable dtOpinia = new DataTable();
            string nameUser = User.Identity.Name;

            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();

                string query = "SELECT id FROM KONTO WHERE login_user=@UserName";
                SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
                sqlCmd.Parameters.AddWithValue("@UserName", nameUser);
                int idCurrentUser = Convert.ToInt32(sqlCmd.ExecuteScalar());

                query = "select opis from opinia p INNER JOIN klient k " +
                    "ON p.klient_id = k.id INNER JOIN KONTO ko " +
                    "ON ko.id = k.konto_id WHERE ko.id = @KlientID and p.pracownik_id=@PracownikID;";
                SqlDataAdapter sqlDa = new SqlDataAdapter(query, sqlCon);
                sqlDa.SelectCommand.Parameters.AddWithValue("@PracownikID", id);
                sqlDa.SelectCommand.Parameters.AddWithValue("@KlientID", idCurrentUser);

                sqlDa.Fill(dtOpinia);

            }

            if (dtOpinia.Rows.Count == 1)
            {
                opiniaModel.Opis = dtOpinia.Rows[0][0].ToString();

                return View(opiniaModel);
            }
            else if(dtOpinia.Rows.Count == 0)
            {
                opiniaModel.Opis = "Dodaj opinie...";

                return View(opiniaModel);
            } 
            else
            {
                return RedirectToAction("Index");
            }
        }

        // POST: Opinie/Edit/5
        [HttpPost]
        [Authorize(Roles = "user")]
        public ActionResult Edit(int id, OpiniaModel opiniaModel)
        {
            if(ModelState.IsValid)
            {
                DataTable dtOpinia = new DataTable();
                string nameUser = User.Identity.Name;

                using (SqlConnection sqlCon = new SqlConnection(connectionString))
                {
                    sqlCon.Open();

                    string query = "SELECT id FROM KONTO WHERE login_user=@UserName";
                    SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
                    sqlCmd.Parameters.AddWithValue("@UserName", nameUser);
                    int idCurrentUser = Convert.ToInt32(sqlCmd.ExecuteScalar());

                    query = "select opis from opinia p INNER JOIN klient k " +
                        "ON p.klient_id = k.id INNER JOIN KONTO ko " +
                        "ON ko.id = k.konto_id WHERE ko.id = @KlientID and p.pracownik_id=@PracownikID;";
                    SqlDataAdapter sqlDa = new SqlDataAdapter(query, sqlCon);
                    sqlDa.SelectCommand.Parameters.AddWithValue("@PracownikID", id);
                    sqlDa.SelectCommand.Parameters.AddWithValue("@KlientID", idCurrentUser);

                    sqlDa.Fill(dtOpinia);

                    query = "SELECT id FROM klient WHERE konto_id=@UserName";
                    sqlCmd = new SqlCommand(query, sqlCon);
                    sqlCmd.Parameters.AddWithValue("@UserName", idCurrentUser);
                    int id_klient = Convert.ToInt32(sqlCmd.ExecuteScalar());

                    if (dtOpinia.Rows.Count == 1)
                    {
                        query = "UPDATE opinia SET opis=@Opis " +
                        "WHERE pracownik_id=@PracownikID AND klient_id=@KlientID";
                        sqlCmd = new SqlCommand(query, sqlCon);
                        sqlCmd.Parameters.AddWithValue("@PracownikID", id);
                        sqlCmd.Parameters.AddWithValue("@KlientID", id_klient);
                        sqlCmd.Parameters.AddWithValue("@Opis", opiniaModel.Opis);
                        sqlCmd.ExecuteNonQuery();
                    }
                    else if (dtOpinia.Rows.Count == 0)
                    {
                        query = "INSERT INTO opinia VALUES(@KlientID, @PracownikID, @Opis)";
                        sqlCmd = new SqlCommand(query, sqlCon);
                        sqlCmd.Parameters.AddWithValue("@KlientID", id_klient);
                        sqlCmd.Parameters.AddWithValue("@PracownikID", id);
                        sqlCmd.Parameters.AddWithValue("@Opis", opiniaModel.Opis);
                        sqlCmd.ExecuteNonQuery();
                    }
                }
                return RedirectToAction("Index");
            }
            else
            {
                return View(opiniaModel);
            }
        }
    }
}
