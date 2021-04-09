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
    public class PracownikZmianaStatusuController : Controller
    {
        string connectionString = @"Data Source=.;Initial Catalog=Projekt;Integrated Security=True";
        // GET: PracownikZmianaStatusu
        [Authorize(Roles = "worker")]
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

                query = "SELECT k.imie + ' ' + k.nazwisko, t.opis, t.waga, t.ilosc, s.marka + ' ' + s.model, pr.id " +
                    "FROM klient k INNER JOIN przesyłka pr " +
                    "ON k.id = pr.klient_id INNER JOIN towar_do_przewiezienia t " +
                    "ON t.przesyłka_id = pr.id INNER JOIN samochod s " +
                    "ON s.id = pr.samochod_id INNER JOIN pracownik p " +
                    "ON p.id = pr.pracownik_id INNER JOIN KONTO ko " +
                    "ON ko.id = p.konto_id " +
                    "WHERE p.konto_id = @KontoID;";
                SqlDataAdapter sqlDa = new SqlDataAdapter(query, sqlCon);
                sqlDa.SelectCommand.Parameters.AddWithValue("@KontoID", idCurrentUser);
                sqlDa.Fill(dataTable);

            }
            return View(dataTable);
        }

        // GET: PracownikZmianaStatusu/Details/5
        [Authorize(Roles = "worker")]
        public ActionResult Details(int id)
        {
            DataTable dataTable = new DataTable();
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();

                string query = "SELECT a.ulica, a.numer, a.miasto, a.kod, s.nazwa, s.opis, asp.data_zmiany_statusu, asp.uwagi " +
                "FROM pracownik p INNER JOIN przesyłka pr " +
                "ON p.id = pr.pracownik_id INNER JOIN klient k " +
                "ON k.id = pr.klient_id INNER JOIN adres a " +
                "ON k.adres_id = a.id INNER JOIN aktualny_stan_przewozu asp " +
                "ON pr.id = asp.przesyłka_id INNER JOIN stan s " +
                "ON asp.stan_id = s.id " +
                "WHERE pr.id = @PrzesylkaID " +
                "ORDER BY asp.data_zmiany_statusu ASC";

                SqlDataAdapter sqlDa = new SqlDataAdapter(query, sqlCon);
                sqlDa.SelectCommand.Parameters.AddWithValue("@PrzesylkaID", id);
                sqlDa.Fill(dataTable);
            }

            return View(dataTable);
        }

        // GET: PracownikZmianaStatusu/Edit/5
        [Authorize(Roles = "worker")]
        public ActionResult Edit(int id)
        {
            List<StatusPaczki> statusPaczki = new List<StatusPaczki>();
            string query = "select id, nazwa + '. ' + opis + '.' as opis from stan;";

            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                using (var cmd = new SqlCommand(query, sqlCon))
                {
                    sqlCon.Open();
                    using (var rdr = cmd.ExecuteReader())
                    {
                        if (rdr.HasRows)
                        {
                            while (rdr.Read())
                            {
                                var m = new StatusPaczki();
                                m.id = rdr.GetInt32(rdr.GetOrdinal("id"));
                                m.opis = rdr.GetString(rdr.GetOrdinal("opis"));
                                statusPaczki.Add(m);
                            }
                        }
                    }
                }
            }

            DodanieStatusuPracownik dodanieStatusuPracownik = new DodanieStatusuPracownik();
            dodanieStatusuPracownik.PrzesylkaID = id;
            dodanieStatusuPracownik.Statusy = statusPaczki;

            return View(dodanieStatusuPracownik);
        }

        // POST: PracownikZmianaStatusu/Edit/5
        [HttpPost]
        [Authorize(Roles = "worker")]
        public ActionResult Edit(int id, DodanieStatusuPracownik dodanieStatusuPracownik)
        {
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();

                string query = "INSERT INTO aktualny_stan_przewozu VALUES(@PrzesylkaID, @StanID, @DataZmianyStatusu, @Uwagi);";
                SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
                sqlCmd.Parameters.AddWithValue("@PrzesylkaID", id);
                sqlCmd.Parameters.AddWithValue("@StanID", dodanieStatusuPracownik.SelectedStatus);
                sqlCmd.Parameters.AddWithValue("@DataZmianyStatusu", dodanieStatusuPracownik.ZmianaStanuPrzesylki);
                sqlCmd.Parameters.AddWithValue("@Uwagi", dodanieStatusuPracownik.Uwagi);
                sqlCmd.ExecuteNonQuery();
            }

            return RedirectToAction("Index");
        }
    }
}
