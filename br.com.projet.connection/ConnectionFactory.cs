using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace general_software_model.br.com.projet.connection
{
    internal class ConnectionFactory
    {
        public MySqlConnection GetConnection()
        {
            string Connection = ConfigurationManager.ConnectionStrings["lccomponentesgeralmodel"].ConnectionString;
            return new MySqlConnection(Connection);
        }
    }
}
