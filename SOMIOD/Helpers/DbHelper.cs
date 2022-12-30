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
        #region Generic Methods
        private static void IsParentValid(SqlConnection db, string parentType, string parentName, string childType, string childName)
        {
            var cmd =
                new
                    SqlCommand("SELECT * FROM " + childType + " c JOIN " + parentType + " p ON (c.Parent = p.Id) WHERE p.Name=@ParentName AND c.Name=@ChildName",
                               db);
            cmd.Parameters.AddWithValue("@ParentName", parentName);
            cmd.Parameters.AddWithValue("@ChildName", childName);
            var reader = cmd.ExecuteReader();

            if (!reader.Read())
                throw new
                    ModelNotFoundException("Couldn't find " + childType.ToLower() + " '" + childName + "' in " + parentType.ToLower() + " '" + parentName + "'",
                                           false);

            reader.Close();
        }

        #endregion

        #region Application

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

        private static void ProcessSqlExceptionApplication(SqlException e)
        {
            switch (e.Number)
            {
                //Cannot insert duplicate key in object
                case 2627:
                    throw new UnprocessableEntityException("An application with that name already exists");
                default:
                    throw new UntreatedSqlException(e);
            }
        }

        public static void CreateApplication(string name)
        {
            using (var dbConn = new DbConnection()) {
                var db = dbConn.Open();

                var cmd = new SqlCommand("INSERT INTO Application (Name, CreationDate) VALUES (@Name, @CreationDate)", db);
                cmd.Parameters.AddWithValue("@Name", name);
                cmd.Parameters.AddWithValue("@CreationDate", DateTime.Now);
                try
                {
                    int rowChng = cmd.ExecuteNonQuery();
                    if (rowChng != 1)
                        throw new ModelNotFoundException("Application");
                }
                catch (SqlException e)
                {
                    ProcessSqlExceptionApplication(e);
                }
            }
        }

        public static void UpdateApplication(string name, string newName)
        {
            using (var dbConn = new DbConnection()) {
                var db = dbConn.Open();
                
                //Check if app with that name exists
                //CheckAppNameIsFree(db, newName);

                var cmd = new SqlCommand("UPDATE Application SET Name=@NewName WHERE Name=@Name", db);
                cmd.Parameters.AddWithValue("@Name", name);
                cmd.Parameters.AddWithValue("@NewName", newName);
                try
                {
                    int rowChng = cmd.ExecuteNonQuery();
                    if (rowChng != 1)
                        throw new ModelNotFoundException("Application");
                }
                catch(SqlException e)
                {
                    ProcessSqlExceptionApplication(e);
                }

            }
        }

        public static void DeleteApplication(string name)
        {
            using (var dbConn = new DbConnection()) {
                var db = dbConn.Open();

                var cmd = new SqlCommand("SELECT * FROM Module m JOIN Application a ON (m.Parent = a.Id) WHERE a.Name=@Name", db);
                cmd.Parameters.AddWithValue("@Name", name);
                var reader = cmd.ExecuteReader();

                if (reader.Read()) {
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

        #endregion Application

        #region Module

        private static void IsModuleParentValid(SqlConnection db, string appName, string moduleName)
        {
            IsParentValid(db, "Application", appName, "Module", moduleName);
        }

        public static void DeleteModule(string appName, string moduleName)
        {
            using (var dbConn = new DbConnection()) {
                var db = dbConn.Open();

                IsModuleParentValid(db, appName, moduleName);

                //Check if module has any data / subscription children
                var cmd =
                    new
                        SqlCommand("SELECT * FROM Module m JOIN Subscription s ON (m.Id = s.Parent) JOIN Data d ON (m.Id = d.Parent) WHERE m.Name=@Name",
                                   db);
                cmd.Parameters.AddWithValue("@Name", moduleName);
                var reader = cmd.ExecuteReader();

                if (reader.Read()) {
                    throw new Exception("Cannot delete a module with data or subscriptions");
                }

                reader.Close();

                cmd = new SqlCommand("DELETE FROM Module WHERE Name=@Name", db);
                cmd.Parameters.AddWithValue("@Name", moduleName);
                int rowChng = cmd.ExecuteNonQuery();

                if (rowChng != 1)
                    throw new ModelNotFoundException("Module");
            }
        }

        #endregion Module

        #region Subscription

        #endregion Subscription

        #region Data

        #endregion Data

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
