using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Malshinon_09_06
{
    internal abstract class Dal
    {
        protected string ConnStr { get; } = "server=localhost;user=root;password=;database=Malshinon";

        protected MySqlConnection Conn { get; set; }

        abstract protected MySqlConnection OpenConnection();
        abstract protected void CloseConnection();
    }
}
