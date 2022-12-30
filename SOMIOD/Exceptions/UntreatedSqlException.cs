using System;
using System.Data.SqlClient;

namespace SOMIOD.Exceptions
{
    public class UntreatedSqlException : Exception
    {
        public UntreatedSqlException() : base("An unknown database error has happened")
        {
        }
        public UntreatedSqlException(SqlException e) : base("An untreated database error (#" + e.Number + ") has happened")
        {
        }
    }
}
