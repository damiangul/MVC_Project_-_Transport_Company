using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using Projekt.Models;

namespace Projekt.Controllers
{
    public class SamochodController : Controller
    {

        string connectionString = @"Data Source=.;Initial Catalog=Projekt;Integrated Security=True";

        [HttpGet]
        public ActionResult Index()
        {
            DataTable dataTable = new DataTable();
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                SqlDataAdapter sqlDa = new SqlDataAdapter("SELECT * FROM samochod", sqlCon);
                sqlDa.Fill(dataTable);

            }
            return View(dataTable);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View(new SamochodModel());
        }

        // POST: Samochod/Create
        [HttpPost]
        public ActionResult Create(SamochodModel samochodModel)
        {
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                string query = "INSERT INTO samochod VALUES(@SamochodMarka, @SamochodModelPojazdu, @SamochodDataProdukcji, " +
                    "@SamochodPojemnosc, @SamochodPrzebieg, @SamochodPrzeglad)";
                SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
                sqlCmd.Parameters.AddWithValue("@SamochodMarka", samochodModel.SamochodMarka);
                sqlCmd.Parameters.AddWithValue("@SamochodModelPojazdu", samochodModel.SamochodModelPojazdu);
                sqlCmd.Parameters.AddWithValue("@SamochodDataProdukcji", samochodModel.SamochodDataProdukcji);
                sqlCmd.Parameters.AddWithValue("@SamochodPojemnosc", samochodModel.SamochodPojemnosc);
                sqlCmd.Parameters.AddWithValue("@SamochodPrzebieg", samochodModel.SamochodPrzebieg);
                sqlCmd.Parameters.AddWithValue("@SamochodPrzeglad", samochodModel.SamochodPrzeglad);

                sqlCmd.ExecuteNonQuery();
            }

            return RedirectToAction("Index");
        }

        // GET: Samochod/Edit/5
        public ActionResult Edit(int id)
        {
            SamochodModel samochodModel = new SamochodModel();
            DataTable dtSamochod = new DataTable();

            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                string query = "SELECT * FROM samochod WHERE id=@SamochodID";
                SqlDataAdapter sqlDa = new SqlDataAdapter(query, sqlCon);
                sqlDa.SelectCommand.Parameters.AddWithValue("@SamochodID", id);

                sqlDa.Fill(dtSamochod);

            }

            if(dtSamochod.Rows.Count == 1)
            {
                samochodModel.SamochodMarka = dtSamochod.Rows[0][1].ToString();
                samochodModel.SamochodModelPojazdu = dtSamochod.Rows[0][2].ToString();
                samochodModel.SamochodDataProdukcji = Convert.ToDateTime(dtSamochod.Rows[0][3].ToString());
                samochodModel.SamochodPojemnosc = Convert.ToInt32(dtSamochod.Rows[0][4].ToString());
                samochodModel.SamochodPrzebieg = Convert.ToInt32(dtSamochod.Rows[0][5].ToString());
                samochodModel.SamochodPrzeglad = Convert.ToDateTime(dtSamochod.Rows[0][6].ToString());

                return View(samochodModel);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        // POST: Samochod/Edit/5
        [HttpPost]
        public ActionResult Edit(SamochodModel samochodModel, int id)
        {
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                string query = "UPDATE samochod SET marka=@SamochodMarka, model=@SamochodModelPojazdu, data_produkcji=@SamochodDataProdukcji, " +
                    "pojemnosc=@SamochodPojemnosc, przebieg=@SamochodPrzebieg, przeglad=@SamochodPrzeglad WHERE id=@SamochodID";
                SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
                sqlCmd.Parameters.AddWithValue("@SamochodID", id);
                sqlCmd.Parameters.AddWithValue("@SamochodMarka", samochodModel.SamochodMarka);
                sqlCmd.Parameters.AddWithValue("@SamochodModelPojazdu", samochodModel.SamochodModelPojazdu);
                sqlCmd.Parameters.AddWithValue("@SamochodDataProdukcji", samochodModel.SamochodDataProdukcji);
                sqlCmd.Parameters.AddWithValue("@SamochodPojemnosc", samochodModel.SamochodPojemnosc);
                sqlCmd.Parameters.AddWithValue("@SamochodPrzebieg", samochodModel.SamochodPrzebieg);
                sqlCmd.Parameters.AddWithValue("@SamochodPrzeglad", samochodModel.SamochodPrzeglad);

                sqlCmd.ExecuteNonQuery();
            }

            return RedirectToAction("Index");
        }

        // GET: Samochod/Delete/5
        public ActionResult Delete(int id)
        {
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                string query = "DELETE FROM samochod WHERE id=@SamochodID";

                SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
                sqlCmd.Parameters.AddWithValue("@SamochodID", id);
                sqlCmd.ExecuteNonQuery();
            }
            return RedirectToAction("Index");
        }
    }
}
