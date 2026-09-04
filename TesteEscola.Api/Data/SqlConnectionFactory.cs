using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace TesteEscola.Api.Data
{
    public class SqlConnectionFactory : IDbConnectionFactory
    {
        public IDbConnection Create()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["TesteEscola"].ConnectionString;
            return new SqlConnection(connectionString);
        }
    }
}
