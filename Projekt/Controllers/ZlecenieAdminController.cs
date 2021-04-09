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
    public class ZlecenieAdminController : Controller
    {
        
        string connectionString = @"Data Source=.;Initial Catalog=Projekt;Integrated Security=True";

        [Authorize(Roles = "admin")]
        public ActionResult Index()
        {
            DataTable dataTable = new DataTable();
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();

                string query = "SELECT z.opis, z.waga, z.ilosc, k.imie, k.nazwisko, k.telefon, z.id " +
                    "FROM zlecenie z INNER JOIN KONTO ko " +
                    "ON z.konto_id = ko.id INNER JOIN klient k " +
                    "ON ko.id = k.konto_id";
                SqlDataAdapter sqlDa = new SqlDataAdapter(query, sqlCon);
                sqlDa.Fill(dataTable);

            }
            return View(dataTable);
        }

        // GET: ZlecenieAdmin/Details/5
        [Authorize(Roles = "admin")]
        public ActionResult Details(int id)
        {
            DataTable dataTable = new DataTable();
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                string query = "SELECT a.ulica, a.numer, a.miasto, a.kod " +
                    "FROM adres a INNER JOIN klient k " +
                    "ON a.id = k.adres_id INNER JOIN KONTO ko " +
                    "ON ko.id = k.konto_id INNER JOIN zlecenie z " +
                    "ON z.konto_id = ko.id " +
                    "WHERE z.id = @ZlecenieID;";
                SqlDataAdapter sqlDa = new SqlDataAdapter(query, sqlCon);
                sqlDa.SelectCommand.Parameters.AddWithValue("@ZlecenieID", id);
                sqlDa.Fill(dataTable);
            }
            return View(dataTable);
        }

        [Authorize(Roles = "admin")]
        public ActionResult Edit(int id)
        {
            DodajZlecenieModel dodajZlecenieModel = new DodajZlecenieModel();
            DataTable dtZlecenia = new DataTable();

            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                string query = "SELECT k.id, z.opis, z.waga, z.ilosc " +
                    "FROM klient k INNER JOIN KONTO ko " +
                    "ON ko.id = k.konto_id INNER JOIN zlecenie z " +
                    "ON z.konto_id = ko.id " +
                    "WHERE z.id = @ZlecenieID";
                SqlDataAdapter sqlDa = new SqlDataAdapter(query, sqlCon);
                sqlDa.SelectCommand.Parameters.AddWithValue("@ZlecenieID", id);

                sqlDa.Fill(dtZlecenia);

            }

            if (dtZlecenia.Rows.Count == 1)
            {
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
                dodajZlecenieModel.KlientID = Convert.ToInt32(dtZlecenia.Rows[0][0].ToString());
                dodajZlecenieModel.ZlecenieOpis = dtZlecenia.Rows[0][1].ToString();
                dodajZlecenieModel.ZlecenieWaga = Convert.ToInt32(dtZlecenia.Rows[0][2].ToString());
                dodajZlecenieModel.ZlecenieIlosc = Convert.ToInt32(dtZlecenia.Rows[0][3].ToString());

                return View(dodajZlecenieModel);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        // POST: ZlecenieAdmin/Edit/5
        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult Edit(int id, DodajZlecenieModel dodajZlecenieModel)
        {
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();

                string query = "INSERT INTO przesyłka(pracownik_id, klient_id,samochod_id, data_przewozu, cena_netto_przewozu, dlugosc_trasy) " +
                    "VALUES(@PracownikID, @KlientID, @SamochodID, @DataPrzewozu, @CennaZaKm, @DlugoscTrasy);";
                SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
                sqlCmd.Parameters.AddWithValue("@PracownikID", dodajZlecenieModel.PracownikID);
                sqlCmd.Parameters.AddWithValue("@KlientID", dodajZlecenieModel.KlientID);
                sqlCmd.Parameters.AddWithValue("@SamochodID", dodajZlecenieModel.SamochodID);
                sqlCmd.Parameters.AddWithValue("@DataPrzewozu", dodajZlecenieModel.DataPrzewozu);
                sqlCmd.Parameters.AddWithValue("@CennaZaKm", dodajZlecenieModel.CennaZaKm);
                sqlCmd.Parameters.AddWithValue("@DlugoscTrasy", dodajZlecenieModel.DlugoscTrasy);
                sqlCmd.ExecuteNonQuery();

                query = "SELECT id FROM przesyłka " +
                    "WHERE dlugosc_trasy = @DlugoscTrasy and " +
                    "cena_netto_przewozu = @CennaZaKm and " +
                    "data_przewozu = @DataPrzewozu";
                sqlCmd = new SqlCommand(query, sqlCon);
                sqlCmd.Parameters.AddWithValue("@DataPrzewozu", dodajZlecenieModel.DataPrzewozu);
                sqlCmd.Parameters.AddWithValue("@CennaZaKm", dodajZlecenieModel.CennaZaKm);
                sqlCmd.Parameters.AddWithValue("@DlugoscTrasy", dodajZlecenieModel.DlugoscTrasy);
                int id_przesylka = Convert.ToInt32(sqlCmd.ExecuteScalar());

                query = "INSERT INTO towar_do_przewiezienia VALUES(@PrzesylkaID, @ZlecenieOpis, @ZlecenieWaga, @ZlecenieIlosc);";
                sqlCmd = new SqlCommand(query, sqlCon);
                sqlCmd.Parameters.AddWithValue("@PrzesylkaID", id_przesylka);
                sqlCmd.Parameters.AddWithValue("@ZlecenieOpis", dodajZlecenieModel.ZlecenieOpis);
                sqlCmd.Parameters.AddWithValue("@ZlecenieWaga", dodajZlecenieModel.ZlecenieWaga);
                sqlCmd.Parameters.AddWithValue("@ZlecenieIlosc", dodajZlecenieModel.ZlecenieIlosc);
                sqlCmd.ExecuteNonQuery();

                //TODO USUNIE SIE TO TERAZ Z ZLECENIE I KLIENT BEDZIE MIAL ZAKLADKE OGARNIETE ZLECENIA

                query = "DELETE FROM zlecenie WHERE id=@ZlecenieID";
                sqlCmd = new SqlCommand(query, sqlCon);
                sqlCmd.Parameters.AddWithValue("@ZlecenieID", id);
                sqlCmd.ExecuteNonQuery();

                query = "INSERT INTO aktualny_stan_przewozu VALUES(@PrzesylkaID, @StanID, @data_zmiany_statusu, @uwagi);";
                sqlCmd = new SqlCommand(query, sqlCon);
                sqlCmd.Parameters.AddWithValue("@PrzesylkaID", id_przesylka);
                sqlCmd.Parameters.AddWithValue("@StanID", 1);
                sqlCmd.Parameters.AddWithValue("@data_zmiany_statusu", dodajZlecenieModel.DataPrzewozu);
                sqlCmd.Parameters.AddWithValue("@uwagi", "Brak");
                sqlCmd.ExecuteNonQuery();
            }
            return RedirectToAction("Index");
        }
    }
}
