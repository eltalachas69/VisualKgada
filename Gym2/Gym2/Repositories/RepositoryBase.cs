using System.Data.SqlClient;

namespace Gym2.Repositories
{
    public abstract class RepositoryBase
    {
        private readonly string connectionString;

        public RepositoryBase()
        {
        
            string connectionString = "Server=.\SQLEXPRESS; Database=GymDB; Integrated Security=True;";
        }

        protected SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }
    }
}