using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Mvc;
using Twits2.Models;

namespace Twits2.Controllers
{
    public class TwitController : Controller
    {
        // GET: Twit
        public ActionResult Index()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["TwitDatabase"].ConnectionString;
            List<Twit> twits = new List<Twit>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = @"
                        SELECT Id, Text, CreatedOn
                        FROM Twits
                        ORDER BY CreatedOn DESC
                        OFFSET 0 ROWS
                        FETCH NEXT 100 ROWS ONLY
                    ";
                SqlCommand command = new SqlCommand(sql, connection);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Twit twit = new Twit();
                    twit.Id = reader.GetInt32(0);
                    twit.Text = reader.GetString(1);
                    twit.CreatedOn = reader.GetDateTimeOffset(2);
                    twits.Add(twit);
                }
            }

            return View(twits);
        }

        // GET: Twit/Create
        public ActionResult Create()
        {
            Twit twit = new Twit();
            return View(twit);
        }

        // POST: Twit/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            Twit twit = new Twit();
            twit.Text = collection.Get("Text");
            twit.CreatedOn = DateTimeOffset.Now;
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["TwitDatabase"].ConnectionString;

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = @"
                            INSERT INTO Twits(Text, CreatedOn)
                            VALUES (@Text, @CreatedOn)
                        ";
                    SqlCommand command = new SqlCommand(sql, connection);
                    command.Parameters.AddWithValue("@Text", twit.Text);
                    command.Parameters.AddWithValue("@CreatedOn", twit.CreatedOn);
                    command.ExecuteNonQuery();
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return View(twit);
            }
        }

        // GET: Twit/Edit/5
        public ActionResult Edit(int id)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["TwitDatabase"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = @"
                    SELECT Id, Text, CreatedOn
                    FROM Twits
                    WHERE Id = @Id
                ";
                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@Id", id);
                SqlDataReader reader = command.ExecuteReader();
                Twit twit = new Twit();

                while (reader.Read())
                {
                    twit.Id = reader.GetInt32(0);
                    twit.Text = reader.GetString(1);
                    twit.CreatedOn = reader.GetDateTimeOffset(2);
                }

                return View(twit);
            }
        }

        // POST: Twit/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            Twit twit = new Twit();
            twit.Id = id;
            twit.Text = collection.Get("Text");

            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["TwitDatabase"].ConnectionString;

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = @"
                        UPDATE Twits
                        SET Text = @Text
                        WHERE Id = @Id
                    ";
                    SqlCommand command = new SqlCommand(sql, connection);
                    command.Parameters.AddWithValue("@Text", twit.Text);
                    command.Parameters.AddWithValue("@Id", id);
                    command.ExecuteNonQuery();
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return View(twit);
            }
        }

        // GET: Twit/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        // POST: Twit/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["TwitDatabase"].ConnectionString;

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = @"
                        DELETE Twits
                        WHERE Id = @Id
                    ";
                    SqlCommand command = new SqlCommand(sql, connection);
                    command.Parameters.AddWithValue("@Id", id);
                    command.ExecuteNonQuery();
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }
    }
}
