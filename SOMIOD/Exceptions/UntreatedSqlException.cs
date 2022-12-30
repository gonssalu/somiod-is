using System;
using System.Data.SqlClient;

namespace SOMIOD.Exceptions
{
    public class MySqlException : Exception
    {
        public MySqlException() : base("An unknown database error has happened")
        {
        }
        public MySqlException(SqlException e) : base("An untreated database error (#" + e.Number + ") has happened")
        {
        }
    }
}
