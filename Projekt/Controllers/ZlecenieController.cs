using Projekt.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace Projekt.Controllers
{
    public class ZlecenieController : Controller
    {
        string connectionString = @"Data Source=.;Initial Catalog=Projekt;Integrated Security=True";
        int idCurrentUser;
        // GET: Zlecenie
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
                idCurrentUser = Convert.ToInt32(sqlCmd.ExecuteScalar());

                query = "SELECT opis, waga, ilosc, id FROM zlecenie WHERE konto_id=@currentUser;";
                SqlDataAdapter sqlDa = new SqlDataAdapter(query, sqlCon);
                sqlDa.SelectCommand.Parameters.AddWithValue("@currentUser", idCurrentUser);
                sqlDa.Fill(dataTable);

            }
            return View(dataTable);
        }

        // GET: Zlecenie/Details/5
        //DODAJ ABY MOGL ZOBACZYC CO Z PACZKĄ
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Zlecenie/Create
        public ActionResult Create()
        {
            return View(new ZlecenieModel());
        }

        // POST: Zlecenie/Create
        [HttpPost]
        public ActionResult Create(ZlecenieModel zlecenieModel)
        {
            string nameUser = User.Identity.Name;

            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();

                string query = "SELECT id FROM KONTO WHERE login_user=@UserName";
                SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
                sqlCmd.Parameters.AddWithValue("@UserName", nameUser);
                idCurrentUser = Convert.ToInt32(sqlCmd.ExecuteScalar());

                query = "INSERT INTO zlecenie VALUES(@UserID, @ZlecenieOpis, @ZlecenieWaga, @ZlecenieIlosc);";
                sqlCmd = new SqlCommand(query, sqlCon);
                sqlCmd.Parameters.AddWithValue("@UserID", idCurrentUser);
                sqlCmd.Parameters.AddWithValue("@ZlecenieOpis", zlecenieModel.ZlecenieOpis);
                sqlCmd.Parameters.AddWithValue("@ZlecenieWaga", zlecenieModel.ZlecenieWaga);
                sqlCmd.Parameters.AddWithValue("@ZlecenieIlosc", zlecenieModel.ZlecenieIlosc);
                sqlCmd.ExecuteNonQuery();
            }

            return RedirectToAction("Index");
        }

        // GET: Zlecenie/Edit/5
        public ActionResult Edit(int id)
        {
            ZlecenieModel zlecenieModel = new ZlecenieModel();
            DataTable dtZlecenie = new DataTable();

            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                string query = "SELECT opis, waga, ilosc FROM zlecenie WHERE id=@ZlecenieID";
                SqlDataAdapter sqlDa = new SqlDataAdapter(query, sqlCon);
                sqlDa.SelectCommand.Parameters.AddWithValue("@ZlecenieID", id);

                sqlDa.Fill(dtZlecenie);

            }

            if (dtZlecenie.Rows.Count == 1)
            {
                zlecenieModel.ZlecenieOpis = dtZlecenie.Rows[0][0].ToString();
                zlecenieModel.ZlecenieWaga = dtZlecenie.Rows[0][1].ToString();
                zlecenieModel.ZlecenieIlosc = dtZlecenie.Rows[0][2].ToString();

                return View(zlecenieModel);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        // POST: Zlecenie/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, ZlecenieModel zlecenieModel)
        {
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();

                string query = "UPDATE zlecenie SET opis=@ZlecenieOpis, waga=@ZlecenieWaga, ilosc=@ZlecenieIlosc " +
                    "WHERE id=@ZlecenieID";
                SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
                sqlCmd.Parameters.AddWithValue("@ZlecenieID", id);
                sqlCmd.Parameters.AddWithValue("@ZlecenieOpis", zlecenieModel.ZlecenieOpis);
                sqlCmd.Parameters.AddWithValue("@ZlecenieWaga", zlecenieModel.ZlecenieWaga);
                sqlCmd.Parameters.AddWithValue("@ZlecenieIlosc", zlecenieModel.ZlecenieIlosc);
                sqlCmd.ExecuteNonQuery();
            }


            return RedirectToAction("Index");
        }

        // GET: Zlecenie/Delete/5
        public ActionResult Delete(int id)
        {
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();

                string query = "DELETE FROM zlecenie WHERE id=@ZlecenieID";
                SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
                sqlCmd.Parameters.AddWithValue("@ZlecenieID", id);
                sqlCmd.ExecuteNonQuery();
            }
            return RedirectToAction("Index");
        }
    }
}
