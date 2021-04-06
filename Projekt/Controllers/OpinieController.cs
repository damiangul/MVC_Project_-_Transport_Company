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
        public ActionResult Index()
        {
            DataTable dataTable = new DataTable();
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                string query = "SELECT imie, nazwisko, id FROM pracownik";
                SqlDataAdapter sqlDa = new SqlDataAdapter(query, sqlCon);
                sqlDa.Fill(dataTable);
            }
            return View(dataTable);
        }

        // GET: Opinie/Details/5
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

        //// GET: Opinie/Create
        //public ActionResult Create()
        //{
        //    return View(new OpiniaModel());
        //}

        //// POST: Opinie/Create
        //[HttpPost]
        //public ActionResult Create(FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add insert logic here

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        // GET: Opinie/Edit/5
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

                query = "SELECT opis FROM opinia WHERE pracownik_id=@PracownikID AND klient_id=@KlientID";
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
        public ActionResult Edit(int id, OpiniaModel opiniaModel)
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

                query = "SELECT opis FROM opinia WHERE pracownik_id=@PracownikID AND klient_id=@KlientID";
                SqlDataAdapter sqlDa = new SqlDataAdapter(query, sqlCon);
                sqlDa.SelectCommand.Parameters.AddWithValue("@PracownikID", id);
                sqlDa.SelectCommand.Parameters.AddWithValue("@KlientID", idCurrentUser);

                sqlDa.Fill(dtOpinia);

                if (dtOpinia.Rows.Count == 1)
                {
                    query = "UPDATE opinia SET opis=@Opis " +
                    "WHERE pracownik_id=@PracownikID AND klient_id=@KlientID";
                    sqlCmd = new SqlCommand(query, sqlCon);
                    sqlCmd.Parameters.AddWithValue("@PracownikID", id);
                    sqlCmd.Parameters.AddWithValue("@KlientID", idCurrentUser);
                    sqlCmd.Parameters.AddWithValue("@Opis", opiniaModel.Opis);
                    sqlCmd.ExecuteNonQuery();
                }
                else if (dtOpinia.Rows.Count == 0)
                {
                    query = "INSERT INTO opinia VALUES(@KlientID, @PracownikID, @Opis)";
                    sqlCmd = new SqlCommand(query, sqlCon);
                    sqlCmd.Parameters.AddWithValue("@KlientID", idCurrentUser);
                    sqlCmd.Parameters.AddWithValue("@PracownikID", id);
                    sqlCmd.Parameters.AddWithValue("@Opis", opiniaModel.Opis);
                    sqlCmd.ExecuteNonQuery();
                }
            }
            return RedirectToAction("Index");
        }

        //// GET: Opinie/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        //// POST: Opinie/Delete/5
        //[HttpPost]
        //public ActionResult Delete(int id, FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add delete logic here

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}
    }
}
