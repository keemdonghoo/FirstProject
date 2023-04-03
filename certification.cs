using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace TeamProject
{
    public class certification
    {
        private readonly string _connectionString;

        public certification(string connectionString)
        {
            _connectionString = connectionString;
        }

        public SqlCommand GetSqlCommand()
        {
            using SqlConnection conn = new SqlConnection(_connectionString);
            conn.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            return cmd;
        }
    }
}