using System.Data.SqlClient;

namespace Gym2.Repositories
{
    public abstract class RepositoryBase
    {
        private readonly string connectionString;

        public RepositoryBase()
        {
         
            connectionString = "Server=.;Database=GymDB;Trusted_Connection=True;";
        }

        protected SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }
    }
}