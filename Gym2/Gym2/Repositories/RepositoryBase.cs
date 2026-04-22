using System.Data.SqlClient;

namespace Gym2.Repositories
{
    public abstract class RepositoryBase
    {
        private readonly string _connectionString;


        public RepositoryBase()
        {
            _connectionString =
                @"Server=laptop-noscfotb\vsgestion; " +
                "Database=GestionGym2; " +
                "Integrated Security=true; "+
                "TrustServerCertificate=true;";
        }
        protected SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}