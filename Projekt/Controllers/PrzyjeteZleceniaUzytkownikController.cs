using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Projekt.Controllers
{
    public class PrzyjeteZleceniaUzytkownikController : Controller
    {
        string connectionString = @"Data Source=.;Initial Catalog=Projekt;Integrated Security=True";
        // GET: PrzyjeteZleceniaUzytkownik
        public ActionResult Index()
        {
            string nameUser = User.Identity.Name;
            DataTable dataTable = new DataTable();
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();

                string query = "SELECT id FROM KONTO WHERE login_user=@UserName";
                SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
                sqlCmd.Parameters.AddWithValue("@UserName", nameUser);
                int idCurrentUser = Convert.ToInt32(sqlCmd.ExecuteScalar());

                query = "select p.imie + ' ' + p.nazwisko as Pracownik, k.imie + ' ' + k.nazwisko as Klient, s.marka + ' ' + s.model as Samochod, " +
                    "pr.data_przewozu, pr.cena_netto_przewozu, pr.dlugosc_trasy, t.opis, t.waga, t.ilosc, pr.id " +
                    "FROM pracownik p INNER JOIN przesyłka pr " +
                    "ON p.id = pr.pracownik_id INNER JOIN klient k " +
                    "ON k.id = pr.klient_id INNER JOIN samochod s " +
                    "ON s.id = pr.samochod_id INNER JOIN towar_do_przewiezienia t " +
                    "ON t.przesyłka_id = pr.id " +
                    "WHERE pr.klient_id=@UserID";
                SqlDataAdapter sqlDa = new SqlDataAdapter(query, sqlCon);
                sqlDa.SelectCommand.Parameters.AddWithValue("@UserID", idCurrentUser);
                sqlDa.Fill(dataTable);

            }
            return View(dataTable);
        }

        // GET: PrzyjeteZleceniaUzytkownik/Details/5
        public ActionResult Details(int id)
        {
            DataTable dataTable = new DataTable();
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();

                string query = "select s.nazwa, s.opis, a.data_zmiany_statusu, a.uwagi " +
                    "FROM aktualny_stan_przewozu a INNER JOIN stan s " +
                    "ON a.stan_id = s.id " +
                    "WHERE a.przesyłka_id = @PrzesyłkaID;";
                SqlDataAdapter sqlDa = new SqlDataAdapter(query, sqlCon);
                sqlDa.SelectCommand.Parameters.AddWithValue("@PrzesyłkaID", id);
                sqlDa.Fill(dataTable);
            }
            return View(dataTable);
        }
    }
}
