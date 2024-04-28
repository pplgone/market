using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.SqlClient;

namespace Eval
{
    class Connector
    {
        public SqlConnection getConn()
        {
            SqlConnection Con = new SqlConnection();

            Con.ConnectionString = "Data Source = Arkan; initial catalog = Eval; integrated security = true";

            return Con;
        }
    }
}
