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
    public class PracownikController : Controller
    {
        string connectionString = @"Data Source=.;Initial Catalog=Projekt;Integrated Security=True";
        
        [HttpGet]
        [Authorize(Roles = "admin")]
        public ActionResult Index()
        {
            DataTable dataTable = new DataTable();
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                string query = "SELECT p.imie, p.nazwisko, p.telefon, p.email, p.data_zatrudnienia, p.pesel, p.pensja, a.ulica, a.numer, a.miasto, a.kod, p.id " +
                    "FROM pracownik p INNER JOIN adres a " +
                    "ON p.adres_id = a.id";
                SqlDataAdapter sqlDa = new SqlDataAdapter(query, sqlCon);
                sqlDa.Fill(dataTable);

            }
            return View(dataTable);
        }

        // GET: Pracownik/Details/5
        [Authorize(Roles = "admin")]
        public ActionResult Details(int id)
        {
            DataTable dataTable = new DataTable();
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                string query = "SELECT k.imie + ' ' + k.nazwisko as Klient, p.imie + ' ' + p.nazwisko as Pracownik, o.opis " +
                    "FROM klient k INNER JOIN opinia o " +
                    "ON o.klient_id = k.id INNER JOIN pracownik p " +
                    "ON o.pracownik_id = p.id " +
                    "WHERE p.id = @ID";
                SqlDataAdapter sqlDa = new SqlDataAdapter(query, sqlCon);
                sqlDa.SelectCommand.Parameters.AddWithValue("@ID", id);
                sqlDa.Fill(dataTable);
            }
            return View(dataTable);
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public ActionResult Create()
        {
            return View(new PracownikModel());
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult Create(PracownikModel pracownikModel)
        {
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();

                string query = "INSERT INTO adres VALUES(@PracownikUlica, @PracownikNumer, @PracownikMiasto, @PracownikKod);";
                SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
                sqlCmd.Parameters.AddWithValue("@PracownikUlica", pracownikModel.PracownikUlica);
                sqlCmd.Parameters.AddWithValue("@PracownikNumer", pracownikModel.PracownikNumer);
                sqlCmd.Parameters.AddWithValue("@PracownikMiasto", pracownikModel.PracownikMiasto);
                sqlCmd.Parameters.AddWithValue("@PracownikKod", pracownikModel.PracownikKod);
                sqlCmd.ExecuteNonQuery();

                query = "SELECT id FROM adres" +
                    " WHERE ulica=@PracownikUlica and numer=@PracownikNumer";
                sqlCmd = new SqlCommand(query, sqlCon);
                sqlCmd.Parameters.AddWithValue("@PracownikUlica", pracownikModel.PracownikUlica);
                sqlCmd.Parameters.AddWithValue("@PracownikNumer", pracownikModel.PracownikNumer);
                int id_adres = Convert.ToInt32(sqlCmd.ExecuteScalar());

                query = "INSERT INTO KONTO VALUES(@PracownikLogin, @PracownikHaslo, 'worker');";
                sqlCmd = new SqlCommand(query, sqlCon);
                sqlCmd.Parameters.AddWithValue("@PracownikLogin", pracownikModel.PracownikLogin);
                sqlCmd.Parameters.AddWithValue("@PracownikHaslo", pracownikModel.PracownikHaslo);
                sqlCmd.ExecuteNonQuery();

                query = "SELECT id from KONTO" +
                    " WHERE login_user=@PracownikLogin and password_user=@PracownikHaslo";
                sqlCmd = new SqlCommand(query, sqlCon);
                sqlCmd.Parameters.AddWithValue("@PracownikLogin", pracownikModel.PracownikLogin);
                sqlCmd.Parameters.AddWithValue("@PracownikHaslo", pracownikModel.PracownikHaslo);
                int id_konto = Convert.ToInt32(sqlCmd.ExecuteScalar());


                query = "INSERT INTO pracownik VALUES(@id_adres, @id_konto, @PracownikImie, @PracownikNazwisko, @PracownikTelefon, @PracownikEmail," +
                    " @PracownikDataZatrudnienia, @PracownikPesel, @PracownikPensja);";
                sqlCmd = new SqlCommand(query, sqlCon);
                sqlCmd.Parameters.AddWithValue("@id_adres", id_adres);
                sqlCmd.Parameters.AddWithValue("@id_konto", id_konto);
                sqlCmd.Parameters.AddWithValue("@PracownikImie", pracownikModel.PracownikImie);
                sqlCmd.Parameters.AddWithValue("@PracownikNazwisko", pracownikModel.PracownikNazwisko);
                sqlCmd.Parameters.AddWithValue("@PracownikTelefon", pracownikModel.PracownikTelefon);
                sqlCmd.Parameters.AddWithValue("@PracownikEmail", pracownikModel.PracownikEmail);
                sqlCmd.Parameters.AddWithValue("@PracownikDataZatrudnienia", pracownikModel.PracownikDataZatrudnienia);
                sqlCmd.Parameters.AddWithValue("@PracownikPesel", pracownikModel.PracownikPesel);
                sqlCmd.Parameters.AddWithValue("@PracownikPensja", pracownikModel.PracownikPensja);
                sqlCmd.ExecuteNonQuery();
            }

            return RedirectToAction("Index");
        }

        // GET: Pracownik/Edit/5
        [Authorize(Roles = "admin")]
        public ActionResult Edit(int id)
        {
            PracownikModel pracownikModel = new PracownikModel();
            DataTable dtPracownik = new DataTable();

            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                string query = "SELECT p.imie, p.nazwisko, p.telefon, p.email, p.data_zatrudnienia, p.pesel, p.pensja, a.ulica, a.numer, a.miasto, a.kod, k.login_user, k.password_user " +
                    "FROM pracownik p INNER JOIN adres a " +
                    "ON p.adres_id = a.id " +
                    "INNER JOIN KONTO k " +
                    "ON p.konto_id = k.id " +
                    "WHERE p.id = @PracownikID;";
                SqlDataAdapter sqlDa = new SqlDataAdapter(query, sqlCon);
                sqlDa.SelectCommand.Parameters.AddWithValue("@PracownikID", id);

                sqlDa.Fill(dtPracownik);

            }

            if (dtPracownik.Rows.Count == 1)
            {
                pracownikModel.PracownikImie = dtPracownik.Rows[0][0].ToString();
                pracownikModel.PracownikNazwisko = dtPracownik.Rows[0][1].ToString();
                pracownikModel.PracownikTelefon = dtPracownik.Rows[0][2].ToString();
                pracownikModel.PracownikEmail = dtPracownik.Rows[0][3].ToString();
                pracownikModel.PracownikDataZatrudnienia = Convert.ToDateTime(dtPracownik.Rows[0][4].ToString());
                pracownikModel.PracownikPesel = dtPracownik.Rows[0][5].ToString();
                pracownikModel.PracownikPensja = Convert.ToDecimal(dtPracownik.Rows[0][6].ToString());

                pracownikModel.PracownikUlica = dtPracownik.Rows[0][7].ToString();
                pracownikModel.PracownikNumer = dtPracownik.Rows[0][8].ToString();
                pracownikModel.PracownikMiasto = dtPracownik.Rows[0][9].ToString();
                pracownikModel.PracownikKod = dtPracownik.Rows[0][10].ToString();
                pracownikModel.PracownikLogin = dtPracownik.Rows[0][11].ToString();
                pracownikModel.PracownikHaslo = dtPracownik.Rows[0][12].ToString();

                return View(pracownikModel);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        // POST: Pracownik/Edit/5
        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult Edit(int id, PracownikModel pracownikModel)
        {

            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();

                string query = "UPDATE pracownik SET imie=@PracownikImie, nazwisko=@PracownikNazwisko, telefon=@PracownikTelefon, email=@PracownikEmail, " +
                    "data_zatrudnienia=@PracownikDataZatrudnienia, pesel=@PracownikPesel, pensja=@PracownikPensja " +
                    "WHERE id=@PracownikID";
                SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
                sqlCmd.Parameters.AddWithValue("@PracownikID", id);
                sqlCmd.Parameters.AddWithValue("@PracownikImie", pracownikModel.PracownikImie);
                sqlCmd.Parameters.AddWithValue("@PracownikNazwisko", pracownikModel.PracownikNazwisko);
                sqlCmd.Parameters.AddWithValue("@PracownikTelefon", pracownikModel.PracownikTelefon);
                sqlCmd.Parameters.AddWithValue("@PracownikEmail", pracownikModel.PracownikEmail);
                sqlCmd.Parameters.AddWithValue("@PracownikDataZatrudnienia", pracownikModel.PracownikDataZatrudnienia);
                sqlCmd.Parameters.AddWithValue("@PracownikPesel", pracownikModel.PracownikPesel);
                sqlCmd.Parameters.AddWithValue("@PracownikPensja", pracownikModel.PracownikPensja);
                sqlCmd.ExecuteNonQuery();

                query = "SELECT adres_id FROM pracownik WHERE id=@PracownikID";
                sqlCmd = new SqlCommand(query, sqlCon);
                sqlCmd.Parameters.AddWithValue("@PracownikID", id);
                int id_adres = Convert.ToInt32(sqlCmd.ExecuteScalar());

                query = "UPDATE adres SET ulica=@PracownikUlica, numer=@PracownikNumer, miasto=@PracownikMiasto, kod=@PracownikKod " +
                    "WHERE id=@id_adres";
                sqlCmd = new SqlCommand(query, sqlCon);
                sqlCmd.Parameters.AddWithValue("@id_adres", id_adres);
                sqlCmd.Parameters.AddWithValue("@PracownikUlica", pracownikModel.PracownikUlica);
                sqlCmd.Parameters.AddWithValue("@PracownikNumer", pracownikModel.PracownikNumer);
                sqlCmd.Parameters.AddWithValue("@PracownikMiasto", pracownikModel.PracownikMiasto);
                sqlCmd.Parameters.AddWithValue("@PracownikKod", pracownikModel.PracownikKod);
                sqlCmd.ExecuteNonQuery();

                query = "SELECT konto_id FROM pracownik WHERE id=@PracownikID";
                sqlCmd = new SqlCommand(query, sqlCon);
                sqlCmd.Parameters.AddWithValue("@PracownikID", id);
                int id_konto = Convert.ToInt32(sqlCmd.ExecuteScalar());

                query = "UPDATE KONTO SET login_user=@PracownikLogin, password_user=@PracownikHaslo WHERE id=@id_konto";
                sqlCmd = new SqlCommand(query, sqlCon);
                sqlCmd.Parameters.AddWithValue("@id_konto", id_konto);
                sqlCmd.Parameters.AddWithValue("@PracownikLogin", pracownikModel.PracownikLogin);
                sqlCmd.Parameters.AddWithValue("@PracownikHaslo", pracownikModel.PracownikHaslo);
                sqlCmd.ExecuteNonQuery();
            }


            return RedirectToAction("Index");
        }

        // GET: Pracownik/Delete/5
        [Authorize(Roles = "admin")]
        public ActionResult Delete(int id)
        {
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();

                string query = "SELECT adres_id FROM pracownik WHERE id=@PracownikID";
                SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
                sqlCmd.Parameters.AddWithValue("@PracownikID", id);
                int id_adres = Convert.ToInt32(sqlCmd.ExecuteScalar());

                query = "SELECT konto_id FROM pracownik WHERE id=@PracownikID";
                sqlCmd = new SqlCommand(query, sqlCon);
                sqlCmd.Parameters.AddWithValue("@PracownikID", id);
                int id_konto = Convert.ToInt32(sqlCmd.ExecuteScalar());

                query = "DELETE FROM adres WHERE id=@id_adres";
                sqlCmd = new SqlCommand(query, sqlCon);
                sqlCmd.Parameters.AddWithValue("@id_adres", id_adres);
                sqlCmd.ExecuteNonQuery();

                query = "DELETE FROM KONTO WHERE id=@id_konto";
                sqlCmd = new SqlCommand(query, sqlCon);
                sqlCmd.Parameters.AddWithValue("@id_konto", id_konto);
                sqlCmd.ExecuteNonQuery();

                query = "DELETE FROM pracownik WHERE id=@PracownikID";
                sqlCmd = new SqlCommand(query, sqlCon);
                sqlCmd.Parameters.AddWithValue("@PracownikID", id);
                sqlCmd.ExecuteNonQuery();
            }
            return RedirectToAction("Index");
        }
    }
}
