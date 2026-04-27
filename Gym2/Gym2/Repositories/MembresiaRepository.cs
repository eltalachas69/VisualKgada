using Gym2.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gym2.Repositories
{
    public class MembresiaRepository : RepositoryBase, IMembresiaRepository
    {
        public void Add(MembresiaModel membresiaModel)
        {
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "INSERT INTO Membresia (Nombre) VALUES (@nombre)";
                command.Parameters.Add("@nombre", SqlDbType.NVarChar).Value = membresiaModel.Nombre;
                command.ExecuteNonQuery();
            }
        }

        public void Delete(MembresiaModel membresiaModel)
        {
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;

                command.CommandText = "DELETE FROM [Membresia] WHERE Id=@Id";
                command.Parameters.AddWithValue("@Id", membresiaModel.Id);

                command.ExecuteNonQuery();
            }
        }

        public IEnumerable<MembresiaModel> GetAllMembresias()
        {
            List<MembresiaModel> membresias = new List<MembresiaModel>();

            using (var connection = GetConnection())
            using (var command = new SqlCommand("SELECT * FROM [Membresia]", connection))
            {
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        membresias.Add(new MembresiaModel
                        {
                            Id = reader["Id"].ToString(),
                            Nombre = reader["Nombre"].ToString(),
                        });
                    }
                }
            }
            return membresias;
        }

        public MembresiaModel GetById(string id)
        {
            MembresiaModel membresia = null;

            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "SELECT * FROM [Membresia] WHERE Id=@Id";
                command.Parameters.AddWithValue("@Id", id);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        membresia = new MembresiaModel
                        {
                            Id = reader["Id"].ToString(),
                            Nombre = reader["Nombre"].ToString()
                        };
                    }
                }
            }
            return membresia;
        }

        public void Update(MembresiaModel membresiaModel)
        {
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = @"UPDATE [Membresia] 
                                SET Nombre=@Nombre
                                WHERE Id=@Id";

                command.Parameters.AddWithValue("@Nombre", membresiaModel.Nombre);
                command.Parameters.AddWithValue("@Id", membresiaModel.Id);

                command.ExecuteNonQuery();
            }
        }
    }
}
