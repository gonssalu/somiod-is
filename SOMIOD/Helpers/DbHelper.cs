using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Xml.Linq;
using SOMIOD.Exceptions;
using SOMIOD.Models;
using SOMIOD.Properties;

namespace SOMIOD.Helpers
{
    public static class DbHelper
    {   
        public static List<Application> GetApplications()
        {
            var applications = new List<Application>();

            using (var dbConn = new DbConnection()) {
                var db = dbConn.Open();
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

            using (var dbConn = new DbConnection()) {
                var db = dbConn.Open();
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
            using (var dbConn = new DbConnection()) {
                var db = dbConn.Open();

                //Check if app with that name exists
                var cmd = new SqlCommand("SELECT * FROM Application WHERE Name=@Name", db);
                cmd.Parameters.AddWithValue("@Name", name);
                var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    throw new UnprocessableEntityException("An application with that name already exists");
                }
                reader.Close();
                
                cmd = new SqlCommand("INSERT INTO Application (Name, CreationDate) VALUES (@Name, @CreationDate)", db);
                cmd.Parameters.AddWithValue("@Name", name);
                cmd.Parameters.AddWithValue("@CreationDate", DateTime.Now);
                int rowChng = cmd.ExecuteNonQuery();

                if (rowChng != 1)
                    throw new Exception("An unknown error as occurred");
            }
        }

        public static void UpdateApplication(string name, string newName)
        {
            using (var dbConn = new DbConnection()) {
                var db = dbConn.Open();
                var cmd = new SqlCommand("UPDATE Application SET Name=@NewName WHERE Name=@Name", db);
                cmd.Parameters.AddWithValue("@Name", name);
                cmd.Parameters.AddWithValue("@NewName", newName);
                int rowChng = cmd.ExecuteNonQuery();

                if (rowChng != 1)
                    throw new ModelNotFoundException("Application");
            }
        }

        public static void DeleteApplication(string name)
        {
            using (var dbConn = new DbConnection()) {
                var db = dbConn.Open();

                var cmd = new SqlCommand("SELECT * FROM Module m JOIN Application a ON (m.Parent = a.Id) WHERE a.Name=@Name", db);
                cmd.Parameters.AddWithValue("@Name", name);
                var reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    throw new Exception("Cannot delete an application with modules");
                }

                reader.Close();

                cmd = new SqlCommand("DELETE FROM Application WHERE Name=@Name", db);
                cmd.Parameters.AddWithValue("@Name", name);
                int rowChng = cmd.ExecuteNonQuery();

                if (rowChng != 1)
                    throw new ModelNotFoundException("Application");
            }
        }

        private class DbConnection : IDisposable
        {
            private readonly string _connStr = Settings.Default.connStr;
            private readonly SqlConnection _conn;

            public DbConnection()
            {
                _conn = new SqlConnection(_connStr);
            }

            public SqlConnection Open()
            {
                _conn.Open();
                return _conn;
            }

            public void Dispose()
            {
                _conn.Close();
            }
        }
    }
}
