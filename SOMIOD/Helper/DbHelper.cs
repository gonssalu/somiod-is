using SOMIOD.Exceptions;
using SOMIOD.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace SOMIOD.Helper
{
    public class DbHelper
    {

        //private static void HasChildren(Application app)
        //{
        //    using (var db = new SomiodContext())
        //    {
        //        if (db.Modules.Any(m => m.Parent.Id == app.Id))
        //        {
        //            throw new Exception("Application has children");
        //        }
        //    }
        //}
        public static List<Application> GetApplications()
        {
            var applications = new List<Application>();

            using (var dbcon = new DbConnection()) {
                var db = dbcon.Open();
                var cmd = new SqlCommand("SELECT * FROM Application", db);
                var reader = cmd.ExecuteReader();

                while (reader.Read()) {
                    applications.Add(new Application(reader.GetInt32(0), reader.GetString(1), reader.GetDateTime(2)));
                }
            }

            return applications;
        }

        public static Application GetApplication(string name)
        {
            var applications = new List<Application>();

            using (var dbcon = new DbConnection()) {
                var db = dbcon.Open();
                var cmd = new SqlCommand("SELECT * FROM Application WHERE Name=@Name", db);
                cmd.Parameters.AddWithValue("@Name", name);
                var reader = cmd.ExecuteReader();

                if (reader.Read()) {
                    return new Application(reader.GetInt32(0), reader.GetString(1), reader.GetDateTime(2));
                } else {
                    throw new ModelNotFoundException("Application");
                }
            }
        }

        public static void CreateApplication(string name)
        {
            using (var dbcon = new DbConnection()) {
                var db = dbcon.Open();
                var cmd = new SqlCommand("INSERT INTO Application (Name, CreationDate) VALUES (@Name, @CreationDate)", db);
                cmd.Parameters.AddWithValue("@Name", name);
                cmd.Parameters.AddWithValue("@CreationDate", DateTime.Now);
                var rowChng = cmd.ExecuteNonQuery();

                if (rowChng != 1)
                    throw new Exception("An unkown error as occurred");
            }
        }

        public static void DeleteApplication(string name)
        {
            using (var dbcon = new DbConnection()) {
                var db = dbcon.Open();
                var cmd = new SqlCommand("DELETE FROM Application WHERE Name=@Name", db);
                cmd.Parameters.AddWithValue("@Name", name);
                var rowChng = cmd.ExecuteNonQuery();

                if (rowChng != 1)
                    throw new ModelNotFoundException("Application");
            }
        }

        private class DbConnection : IDisposable
        {
            private string _connStr = Properties.Settings.Default.connStr;

            private SqlConnection conn = null;

            public DbConnection()
            {
                conn = new SqlConnection(_connStr);
            }

            public SqlConnection Open()
            {
                conn.Open();
                return conn;
            }

            public void Dispose()
            {
                conn.Close();
            }
        }
    }
}
