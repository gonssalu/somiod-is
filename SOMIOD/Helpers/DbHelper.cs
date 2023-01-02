using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using SOMIOD.Exceptions;
using SOMIOD.Models;
using SOMIOD.Properties;

namespace SOMIOD.Helpers
{
    public static class DbHelper
    {
        #region Generic Methods

        //Verifica se existe uma child com o nome fornecido no parent com o nome fornecido, e se o parent existe em si
        //Este método deve ser utilizado em Deletes / Updates, ou seja em situações em que a child já existe.
        //Retorna o id da child
        private static int IsParentValid(SqlConnection db, string parentType, string parentName, string childType, string childName)
        {
            var cmd =
                new
                    SqlCommand(
                    "SELECT c.Id FROM " + childType + " c JOIN " + parentType + " p ON (c.Parent = p.Id) WHERE p.Name=@ParentName AND c.Name=@ChildName",
                    db);
            cmd.Parameters.AddWithValue("@ParentName", parentName.ToLower());
            cmd.Parameters.AddWithValue("@ChildName", childName.ToLower());
            var reader = cmd.ExecuteReader();

            if (!reader.Read())
                throw new
                    ModelNotFoundException("Couldn't find " + childType.ToLower() + " '" + childName + "' in " + parentType.ToLower() + " '" + parentName + "'",
                                           false);

            int childId = reader.GetInt32(0);
            reader.Close();
            return childId;
        }

        //Procura o parent, e se existir retorna o seu id.
        //Faz logo a verificação da existência do parent
        //Este método deve ser utilizado em Creates onde a child ainda não existe e necessita do id do parent.
        private static int GetParentId(SqlConnection db, string parentType, string parentName)
        {
            var cmd = new SqlCommand("SELECT Id FROM " + parentType + " WHERE Name=@ParentName", db);
            cmd.Parameters.AddWithValue("@ParentName", parentName.ToLower());
            var reader = cmd.ExecuteReader();

            if (!reader.Read())
                throw new ModelNotFoundException("Couldn't find " + parentType.ToLower() + " '" + parentName + "'", false);

            int parentId = reader.GetInt32(0);
            reader.Close();
            return parentId;
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

                reader.Close();
            }

            return applications;
        }

        public static Application GetApplication(string name)
        {
            using (var dbConn = new DbConnection()) {
                var db = dbConn.Open();
                var cmd = new SqlCommand("SELECT * FROM Application WHERE Name=@Name", db);
                cmd.Parameters.AddWithValue("@Name", name.ToLower());
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
            switch (e.Number) {
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
                cmd.Parameters.AddWithValue("@Name", name.ToLower());
                cmd.Parameters.AddWithValue("@CreationDate", DateTime.Now);

                try {
                    int rowChng = cmd.ExecuteNonQuery();

                    if (rowChng != 1)
                        throw new UntreatedSqlException();
                }
                catch (SqlException e) {
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
                cmd.Parameters.AddWithValue("@Name", name.ToLower());
                cmd.Parameters.AddWithValue("@NewName", newName.ToLower());

                try {
                    int rowChng = cmd.ExecuteNonQuery();

                    if (rowChng != 1)
                        throw new ModelNotFoundException("Application");
                }
                catch (SqlException e) {
                    ProcessSqlExceptionApplication(e);
                }
            }
        }

        public static void DeleteApplication(string name)
        {
            using (var dbConn = new DbConnection()) {
                var db = dbConn.Open();

                var cmd = new SqlCommand("SELECT * FROM Module m JOIN Application a ON (m.Parent = a.Id) WHERE a.Name=@Name", db);
                cmd.Parameters.AddWithValue("@Name", name.ToLower());
                var reader = cmd.ExecuteReader();

                if (reader.Read()) {
                    throw new Exception("Cannot delete an application with modules");
                }

                reader.Close();

                cmd = new SqlCommand("DELETE FROM Application WHERE Name=@Name", db);
                cmd.Parameters.AddWithValue("@Name", name.ToLower());
                int rowChng = cmd.ExecuteNonQuery();

                if (rowChng != 1)
                    throw new ModelNotFoundException("Application");
            }
        }

        #endregion Application

        #region Module

        private static int IsModuleParentValid(SqlConnection db, string appName, string moduleName)
        {
            return IsParentValid(db, "Application", appName, "Module", moduleName);
        }

        private static void ProcessSqlExceptionModule(SqlException e)
        {
            switch (e.Number) {
                //Cannot insert duplicate key in object
                case 2627:
                    throw new UnprocessableEntityException("A module with that name already exists in that application");
                default:
                    throw new UntreatedSqlException(e);
            }
        }

        public static List<Module> GetModules(string appName)
        {
            var modules = new List<Module>();

            using (var dbConn = new DbConnection()) {
                var db = dbConn.Open();
                var cmd = new SqlCommand("SELECT * FROM Module m JOIN Application a ON (m.Parent = a.Id) WHERE a.Name=@AppName", db);
                cmd.Parameters.AddWithValue("@AppName", appName.ToLower());
                var reader = cmd.ExecuteReader();

                while (reader.Read()) {
                    modules.Add(new Module(reader.GetInt32(0), reader.GetString(1), reader.GetDateTime(2), reader.GetInt32(3)));
                }

                reader.Close();
            }

            return modules;
        }

        public static ModuleWithData GetModule(string appName, string moduleName)
        {
            using (var dbConn = new DbConnection()) {
                var db = dbConn.Open();

                int moduleId = IsModuleParentValid(db, appName, moduleName);

                var cmd = new SqlCommand("SELECT * FROM Module WHERE Id=@Id", db);
                cmd.Parameters.AddWithValue("@Id", moduleId);
                var reader = cmd.ExecuteReader();

                List<Data> data = GetDataResourcesForModule(moduleId);

                if (reader.Read()) {
                    return new ModuleWithData(reader.GetInt32(0), reader.GetString(1), reader.GetDateTime(2), reader.GetInt32(3), data);
                } else {
                    throw new UntreatedSqlException();
                }
            }
        }

        public static void CreateModule(string appName, string moduleName)
        {
            using (var dbConn = new DbConnection()) {
                var db = dbConn.Open();

                int parentId = GetParentId(db, "Application", appName);

                var cmd = new SqlCommand("INSERT INTO Module (Name, CreationDate, Parent) VALUES (@Name, @CreationDate, @Parent)", db);
                cmd.Parameters.AddWithValue("@Name", moduleName.ToLower());
                cmd.Parameters.AddWithValue("@CreationDate", DateTime.Now);
                cmd.Parameters.AddWithValue("@Parent", parentId);

                try {
                    int rowChng = cmd.ExecuteNonQuery();

                    if (rowChng != 1)
                        throw new UntreatedSqlException();
                }
                catch (SqlException e) {
                    ProcessSqlExceptionModule(e);
                }
            }
        }

        public static void UpdateModule(string appName, string moduleName, string newModuleName)
        {
            using (var dbConn = new DbConnection()) {
                var db = dbConn.Open();

                IsModuleParentValid(db, appName, moduleName);

                var cmd = new SqlCommand("UPDATE Module SET Name=@NewName WHERE Name=@Name", db);
                cmd.Parameters.AddWithValue("@Name", moduleName.ToLower());
                cmd.Parameters.AddWithValue("@NewName", newModuleName.ToLower());

                try {
                    int rowChng = cmd.ExecuteNonQuery();

                    if (rowChng != 1)
                        throw new ModelNotFoundException("Module");
                }
                catch (SqlException e) {
                    ProcessSqlExceptionModule(e);
                }
            }
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
                cmd.Parameters.AddWithValue("@Name", moduleName.ToLower());
                var reader = cmd.ExecuteReader();

                if (reader.Read()) {
                    throw new Exception("Cannot delete a module with data or subscriptions");
                }

                reader.Close();

                cmd = new SqlCommand("DELETE FROM Module WHERE Name=@Name", db);
                cmd.Parameters.AddWithValue("@Name", moduleName.ToLower());
                int rowChng = cmd.ExecuteNonQuery();

                if (rowChng != 1)
                    throw new UntreatedSqlException();
            }
        }

        #endregion Module

        #region Subscription

        private static void ProcessSqlExceptionSubscription(SqlException e)
        {
            switch (e.Number)
            {
                //Cannot insert duplicate key in object
                case 2627:
                    throw new UnprocessableEntityException("A subscription with that name already exists in that module");
                default:
                    throw new UntreatedSqlException(e);
            }
        }

        public static void CreateSubscription(string appName, string moduleName, Subscription subscription)
        {
            using (var dbConn = new DbConnection()) {
                var db = dbConn.Open();

                int parentId = IsModuleParentValid(db, appName, moduleName);

                var cmd = new SqlCommand(
                    "INSERT INTO Subscription (Name, CreationDate, Parent, Event, Endpoint) VALUES (@Name, @CreationDate, @Parent, @Event, @Endpoint)",
                    db);
                cmd.Parameters.AddWithValue("@Name", subscription.Name.ToLower());
                cmd.Parameters.AddWithValue("@CreationDate", DateTime.Now);
                cmd.Parameters.AddWithValue("@Parent", parentId);
                cmd.Parameters.AddWithValue("@Event", subscription.EventType.ToUpper());
                cmd.Parameters.AddWithValue("@Endpoint", subscription.Endpoint.ToLower());

                try {
                    int rowChng = cmd.ExecuteNonQuery();

                    if (rowChng != 1)
                        throw new UntreatedSqlException();
                }
                catch (SqlException e) {
                    ProcessSqlExceptionSubscription(e);
                }
            }
        }

        public static void DeleteSubscription(string appName, string moduleName, string subscriptionName)
        {
            using (var dbConn = new DbConnection()) {
                var db = dbConn.Open();

                IsModuleParentValid(db, appName, moduleName);

                int parentId = GetParentId(db, "Module", moduleName);

                var cmd = new SqlCommand("DELETE FROM Subscription WHERE Name=@Name AND Parent=@Parent", db);
                cmd.Parameters.AddWithValue("@Name", subscriptionName.ToLower());
                cmd.Parameters.AddWithValue("@Parent", parentId);
                int rowChng = cmd.ExecuteNonQuery();

                if (rowChng != 1)
                    throw new ModelNotFoundException("Subscription");
            }
        }

        #endregion Subscription

        #region Data

        public static List<Data> GetDataResourcesForModule(int parentId)
        {
            var dataRes = new List<Data>();

            using (var dbConn = new DbConnection()) {
                var db = dbConn.Open();

                var cmd = new SqlCommand("SELECT * FROM Data WHERE Parent=@Parent", db);
                cmd.Parameters.AddWithValue("@Parent", parentId);
                var reader = cmd.ExecuteReader();

                while (reader.Read()) {
                    dataRes.Add(new Data(reader.GetInt32(0), reader.GetString(1), reader.GetDateTime(2), reader.GetInt32(3)));
                }

                reader.Close();
            }

            return dataRes;
        }

        public static void CreateData(string appName, string moduleName, string dataContent)
        {
            using (var dbConn = new DbConnection()) {
                var db = dbConn.Open();

                IsModuleParentValid(db, appName, moduleName);

                int parentId = GetParentId(db, "Module", moduleName);

                var cmd = new SqlCommand("INSERT INTO Data (Content, CreationDate, Parent) VALUES (@Content, @CreationDate, @Parent)", db);
                cmd.Parameters.AddWithValue("@Content", dataContent);
                cmd.Parameters.AddWithValue("@CreationDate", DateTime.Now);
                cmd.Parameters.AddWithValue("@Parent", parentId);

                try {
                    int rowChng = cmd.ExecuteNonQuery();

                    if (rowChng != 1)
                        throw new UntreatedSqlException();

                    //TODO: Notify Create Subscriptions
                }
                catch (SqlException e) {
                    throw new UntreatedSqlException();
                }
            }
        }
        public static void DeleteData(string appName, string moduleName, int dataId)
        {
            using (var dbConn = new DbConnection()) {
                var db = dbConn.Open();

                IsModuleParentValid(db, appName, moduleName);

                var cmd =
                    new
                        SqlCommand(
                        "SELECT * FROM Module m JOIN Data d ON (d.Parent = m.Id) WHERE d.Id=@DataId AND m.Name=@ModuleName",
                        db);
                cmd.Parameters.AddWithValue("@DataId", dataId);
                cmd.Parameters.AddWithValue("@ModuleName", moduleName.ToLower());
                var reader = cmd.ExecuteReader();

                if (!reader.Read())
                    throw new
                        ModelNotFoundException("A data resource with the Id #" + dataId + " does not exist in the module " + moduleName, false);

                reader.Close();

                cmd = new SqlCommand("DELETE FROM Data WHERE Id=@Id", db);
                cmd.Parameters.AddWithValue("@Id", dataId);
                int rowChng = cmd.ExecuteNonQuery();

                if (rowChng != 1)
                    throw new UntreatedSqlException();

                // TODO: Notify Delete Subscriptions
            }
        }

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
