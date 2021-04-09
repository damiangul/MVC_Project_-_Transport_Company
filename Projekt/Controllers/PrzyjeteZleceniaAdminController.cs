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
    public class PrzyjeteZleceniaAdminController : Controller
    {
        string connectionString = @"Data Source=.;Initial Catalog=Projekt;Integrated Security=True";
        // GET: PrzyjeteZleceniaAdmin
        [Authorize(Roles = "admin")]
        public ActionResult Index()
        {
            DataTable dataTable = new DataTable();
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();

                string query = "select p.imie + ' ' + p.nazwisko as Pracownik, k.imie + ' ' + k.nazwisko as Klient, s.marka + ' ' + s.model as Samochod, " +
                    "pr.data_przewozu, pr.cena_netto_przewozu, pr.dlugosc_trasy, t.opis, t.waga, t.ilosc, pr.id " +
                    "FROM pracownik p INNER JOIN przesyłka pr " +
                    "ON p.id = pr.pracownik_id INNER JOIN klient k " +
                    "ON k.id = pr.klient_id INNER JOIN samochod s " +
                    "ON s.id = pr.samochod_id INNER JOIN towar_do_przewiezienia t " +
                    "ON t.przesyłka_id = pr.id";
                SqlDataAdapter sqlDa = new SqlDataAdapter(query, sqlCon);
                sqlDa.Fill(dataTable);

            }
            return View(dataTable);
        }

        // GET: PrzyjeteZleceniaAdmin/Details/5
        [Authorize(Roles = "admin")]
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

        // GET: PrzyjeteZleceniaAdmin/Edit/5
        [Authorize(Roles = "admin")]
        public ActionResult Edit(int id)
        {
            DodajZlecenieModel dodajZlecenieModel = new DodajZlecenieModel();
            DataTable dtZlecenia = new DataTable();

            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                string query = "SELECT p.id, k.id, s.id, pr.data_przewozu, pr.cena_netto_przewozu, pr.dlugosc_trasy, t.opis, t.waga, t.ilosc " +
                    "FROM klient k INNER JOIN przesyłka pr " +
                    "ON k.id = pr.klient_id INNER JOIN pracownik p " +
                    "ON p.id = pr.pracownik_id INNER JOIN towar_do_przewiezienia t " +
                    "ON t.przesyłka_id = pr.id INNER JOIN samochod s " +
                    "ON s.id = pr.samochod_id " +
                    "WHERE pr.id = @PrzesylkaID";
                SqlDataAdapter sqlDa = new SqlDataAdapter(query, sqlCon);
                sqlDa.SelectCommand.Parameters.AddWithValue("@PrzesylkaID", id);

                sqlDa.Fill(dtZlecenia);
            }

            if (dtZlecenia.Rows.Count == 1)
            {
                dodajZlecenieModel.PracownikID = Convert.ToInt32(dtZlecenia.Rows[0][0].ToString());
                dodajZlecenieModel.KlientID = Convert.ToInt32(dtZlecenia.Rows[0][1].ToString());
                dodajZlecenieModel.SamochodID = Convert.ToInt32(dtZlecenia.Rows[0][2].ToString());
                dodajZlecenieModel.DataPrzewozu = Convert.ToDateTime(dtZlecenia.Rows[0][3].ToString());
                dodajZlecenieModel.CennaZaKm = Convert.ToDecimal(dtZlecenia.Rows[0][4].ToString());
                dodajZlecenieModel.DlugoscTrasy = Convert.ToInt32(dtZlecenia.Rows[0][5].ToString());
                dodajZlecenieModel.ZlecenieOpis = dtZlecenia.Rows[0][6].ToString();
                dodajZlecenieModel.ZlecenieWaga = Convert.ToInt32(dtZlecenia.Rows[0][7].ToString());
                dodajZlecenieModel.ZlecenieIlosc = Convert.ToInt32(dtZlecenia.Rows[0][8].ToString());

                List<PracownikLista> pracownicy = new List<PracownikLista>();
                List<SamochodLista> samochody = new List<SamochodLista>();

                using (SqlConnection sqlCon = new SqlConnection(connectionString))
                {
                    string query = "select id, imie + ' ' + nazwisko as pracownik from pracownik;";
                    using (var cmd = new SqlCommand(query, sqlCon))
                    {
                        sqlCon.Open();
                        using (var rdr = cmd.ExecuteReader())
                        {
                            if (rdr.HasRows)
                            {
                                while (rdr.Read())
                                {
                                    var m = new PracownikLista();
                                    m.id = rdr.GetInt32(rdr.GetOrdinal("id"));
                                    m.Pracownik = rdr.GetString(rdr.GetOrdinal("pracownik"));
                                    pracownicy.Add(m);
                                }
                            }
                        }
                    }

                    query = "select id, model + ' ' + marka as samochod from samochod;";
                    using (var cmd = new SqlCommand(query, sqlCon))
                    {
                        using (var rdr = cmd.ExecuteReader())
                        {
                            if (rdr.HasRows)
                            {
                                while (rdr.Read())
                                {
                                    var m = new SamochodLista();
                                    m.id = rdr.GetInt32(rdr.GetOrdinal("id"));
                                    m.Samochod = rdr.GetString(rdr.GetOrdinal("samochod"));
                                    samochody.Add(m);
                                }
                            }
                        }
                    }
                }

                dodajZlecenieModel.Pracownicy = pracownicy;
                dodajZlecenieModel.Samochody = samochody;

                return View(dodajZlecenieModel);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        // POST: PrzyjeteZleceniaAdmin/Edit/5
        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult Edit(int id, DodajZlecenieModel dodajZlecenieModel)
        {
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                string query = "UPDATE przesyłka SET pracownik_id=@PracownikID, klient_id=@KlientID, samochod_id=@SamochodID, data_przewozu=@DataPrzewozu, " +
                    "cena_netto_przewozu=@CenaZaKm, dlugosc_trasy=@DlugoscTrasy " +
                    "WHERE id=@PrzesylkaID";
                SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
                sqlCmd.Parameters.AddWithValue("@PrzesylkaID", id);
                sqlCmd.Parameters.AddWithValue("@PracownikID", dodajZlecenieModel.PracownikID);
                sqlCmd.Parameters.AddWithValue("@KlientID", dodajZlecenieModel.KlientID);
                sqlCmd.Parameters.AddWithValue("@SamochodID", dodajZlecenieModel.SamochodID);
                sqlCmd.Parameters.AddWithValue("@DataPrzewozu", dodajZlecenieModel.DataPrzewozu);
                sqlCmd.Parameters.AddWithValue("@CenaZaKm", dodajZlecenieModel.CennaZaKm);
                sqlCmd.Parameters.AddWithValue("@DlugoscTrasy", dodajZlecenieModel.DlugoscTrasy);
                sqlCmd.ExecuteNonQuery();

                query = "UPDATE towar_do_przewiezienia SET opis=@Opis, waga=@Waga, ilosc=@Ilosc " +
                    "WHERE przesyłka_id=@PrzesylkaID";
                sqlCmd = new SqlCommand(query, sqlCon);
                sqlCmd.Parameters.AddWithValue("@PrzesylkaID", id);
                sqlCmd.Parameters.AddWithValue("@Opis", dodajZlecenieModel.ZlecenieOpis);
                sqlCmd.Parameters.AddWithValue("@Waga", dodajZlecenieModel.ZlecenieWaga);
                sqlCmd.Parameters.AddWithValue("@Ilosc", dodajZlecenieModel.ZlecenieIlosc);
                sqlCmd.ExecuteNonQuery();
            }
            return RedirectToAction("Index");
        }

        // GET: PrzyjeteZleceniaAdmin/Delete/5
        [Authorize(Roles = "admin")]
        public ActionResult Delete(int id)
        {
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();

                string query = "DELETE FROM aktualny_stan_przewozu WHERE przesyłka_id=@ID";
                SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
                sqlCmd.Parameters.AddWithValue("@ID", id);
                sqlCmd.ExecuteNonQuery();

                query = "DELETE FROM towar_do_przewiezienia WHERE przesyłka_id=@ID";
                sqlCmd = new SqlCommand(query, sqlCon);
                sqlCmd.Parameters.AddWithValue("@ID", id);
                sqlCmd.ExecuteNonQuery();

                query = "DELETE FROM przesyłka WHERE id=@ID";
                sqlCmd = new SqlCommand(query, sqlCon);
                sqlCmd.Parameters.AddWithValue("@ID", id);
                sqlCmd.ExecuteNonQuery();
            }
            return RedirectToAction("Index");
        }
    }
}
