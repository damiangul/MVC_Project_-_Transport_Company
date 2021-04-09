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
    public class KlientController : Controller
    {
        string connectionString = @"Data Source=.;Initial Catalog=Projekt;Integrated Security=True";
        // GET: Klient
        [Authorize(Roles = "admin")]
        public ActionResult Index()
        {
            DataTable dataTable = new DataTable();
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                string query = "SELECT k.imie, k.nazwisko, k.telefon, k.email, a.ulica, a.numer, a.miasto, a.kod, k.id " +
                    "FROM klient k INNER JOIN adres a " +
                    "ON k.adres_id = a.id";
                SqlDataAdapter sqlDa = new SqlDataAdapter(query, sqlCon);
                sqlDa.Fill(dataTable);

            }
            return View(dataTable);
        }

        // GET: Klient/Create
        [Authorize(Roles = "admin")]
        public ActionResult Create()
        {
            return View(new KlientModel());
        }

        // POST: Klient/Create
        
        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult Create(KlientModel klientModel)
        {
            if(ModelState.IsValid)
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

                return RedirectToAction("Index");
            } 
            else
            {
                return View(klientModel);
            }
        }

        // GET: Klient/Edit/5
        [Authorize(Roles = "admin")]
        public ActionResult Edit(int id)
        {
            KlientModel klientModel = new KlientModel();
            DataTable dtKlient = new DataTable();

            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                string query = "SELECT k.imie, k.nazwisko, k.telefon, k.email, a.ulica, a.numer, a.miasto, a.kod, ko.login_user, ko.password_user " +
                    "FROM klient k INNER JOIN adres a " +
                    "ON k.adres_id = a.id " +
                    "INNER JOIN KONTO ko " +
                    "ON k.konto_id = ko.id " +
                    "WHERE k.id = @KlientID;";
                SqlDataAdapter sqlDa = new SqlDataAdapter(query, sqlCon);
                sqlDa.SelectCommand.Parameters.AddWithValue("@KlientID", id);

                sqlDa.Fill(dtKlient);

            }

            if (dtKlient.Rows.Count == 1)
            {
                klientModel.KlientImie = dtKlient.Rows[0][0].ToString();
                klientModel.KlientNazwisko = dtKlient.Rows[0][1].ToString();
                klientModel.KlientTelefon = dtKlient.Rows[0][2].ToString();
                klientModel.KlientEmail = dtKlient.Rows[0][3].ToString();

                klientModel.KlientUlica = dtKlient.Rows[0][4].ToString();
                klientModel.KlientNumer = dtKlient.Rows[0][5].ToString();
                klientModel.KlientMiasto = dtKlient.Rows[0][6].ToString();
                klientModel.KlientKod = dtKlient.Rows[0][7].ToString();

                klientModel.KlientLogin = dtKlient.Rows[0][8].ToString();
                klientModel.KlientHaslo = dtKlient.Rows[0][9].ToString();

                return View(klientModel);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        // POST: Klient/Edit/5
        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult Edit(int id, KlientModel klientModel)
        {
            if(ModelState.IsValid)
            {
                using (SqlConnection sqlCon = new SqlConnection(connectionString))
                {
                    sqlCon.Open();

                    string query = "UPDATE klient SET imie=@KlientImie, nazwisko=@KlientNazwisko, telefon=@KlientTelefon, email=@KlientEmail " +
                        "WHERE id=@KlientID";
                    SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
                    sqlCmd.Parameters.AddWithValue("@KlientID", id);
                    sqlCmd.Parameters.AddWithValue("@KlientImie", klientModel.KlientImie);
                    sqlCmd.Parameters.AddWithValue("@KlientNazwisko", klientModel.KlientNazwisko);
                    sqlCmd.Parameters.AddWithValue("@KlientTelefon", klientModel.KlientTelefon);
                    sqlCmd.Parameters.AddWithValue("@KlientEmail", klientModel.KlientEmail);
                    sqlCmd.ExecuteNonQuery();

                    query = "SELECT adres_id FROM klient WHERE id=@KlientID";
                    sqlCmd = new SqlCommand(query, sqlCon);
                    sqlCmd.Parameters.AddWithValue("@KlientID", id);
                    int id_adres = Convert.ToInt32(sqlCmd.ExecuteScalar());

                    query = "UPDATE adres SET ulica=@KlientUlica, numer=@KlientNumer, miasto=@KlientMiasto, kod=@KlientKod " +
                        "WHERE id=@id_adres";
                    sqlCmd = new SqlCommand(query, sqlCon);
                    sqlCmd.Parameters.AddWithValue("@id_adres", id_adres);
                    sqlCmd.Parameters.AddWithValue("@KlientUlica", klientModel.KlientUlica);
                    sqlCmd.Parameters.AddWithValue("@KlientNumer", klientModel.KlientNumer);
                    sqlCmd.Parameters.AddWithValue("@KlientMiasto", klientModel.KlientMiasto);
                    sqlCmd.Parameters.AddWithValue("@KlientKod", klientModel.KlientKod);
                    sqlCmd.ExecuteNonQuery();

                    query = "SELECT konto_id FROM klient WHERE id=@KlientID";
                    sqlCmd = new SqlCommand(query, sqlCon);
                    sqlCmd.Parameters.AddWithValue("@KlientID", id);
                    int id_konto = Convert.ToInt32(sqlCmd.ExecuteScalar());

                    query = "UPDATE KONTO SET login_user=@KlientLogin, password_user=@KlientHaslo WHERE id=@id_konto";
                    sqlCmd = new SqlCommand(query, sqlCon);
                    sqlCmd.Parameters.AddWithValue("@id_konto", id_konto);
                    sqlCmd.Parameters.AddWithValue("@KlientLogin", klientModel.KlientLogin);
                    sqlCmd.Parameters.AddWithValue("@KlientHaslo", klientModel.KlientHaslo);
                    sqlCmd.ExecuteNonQuery();
                }


                return RedirectToAction("Index");
            }
            else
            {
                return View(klientModel);
            }
        }

        // GET: Klient/Delete/5
        [Authorize(Roles = "admin")]
        public ActionResult Delete(int id)
        {
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();

                string query = "SELECT adres_id FROM klient WHERE id=@KlientID";
                SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
                sqlCmd.Parameters.AddWithValue("@KlientID", id);
                int id_adres = Convert.ToInt32(sqlCmd.ExecuteScalar());

                query = "SELECT konto_id FROM klient WHERE id=@KlientID";
                sqlCmd = new SqlCommand(query, sqlCon);
                sqlCmd.Parameters.AddWithValue("@KlientID", id);
                int id_konto = Convert.ToInt32(sqlCmd.ExecuteScalar());

                query = "DELETE FROM adres WHERE id=@id_adres";
                sqlCmd = new SqlCommand(query, sqlCon);
                sqlCmd.Parameters.AddWithValue("@id_adres", id_adres);
                sqlCmd.ExecuteNonQuery();

                query = "DELETE FROM KONTO WHERE id=@id_konto";
                sqlCmd = new SqlCommand(query, sqlCon);
                sqlCmd.Parameters.AddWithValue("@id_konto", id_konto);
                sqlCmd.ExecuteNonQuery();

                query = "DELETE FROM klient WHERE id=@KlientID";
                sqlCmd = new SqlCommand(query, sqlCon);
                sqlCmd.Parameters.AddWithValue("@KlientID", id);
                sqlCmd.ExecuteNonQuery();
            }
            return RedirectToAction("Index");
        }
    }
}
