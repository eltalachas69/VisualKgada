using Gym2.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gym2.Repositories
{
    public class EjerciciosRepository : RepositoryBase, IEjerciciosRepository
    {
        public void Add(EjerciciosModel ejerciciosModel)
        {
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;

                // Do not insert explicit value into identity column. Let the database generate it.
                command.CommandText =
                "INSERT INTO [Ejercicios] (Nombre, MusculosTrabajados, Dificultad, Imagen) " +
                "VALUES(@Nombre, @MusculosTrabajados, @Dificultad, @Imagen); " +
                "SELECT SCOPE_IDENTITY();";

                command.Parameters.AddWithValue("@Nombre", ejerciciosModel.Nombre);
                command.Parameters.AddWithValue("@MusculosTrabajados", ejerciciosModel.MusculosTrabajados);
                command.Parameters.AddWithValue("@Dificultad", ejerciciosModel.Dificultad);
                command.Parameters.Add("@Imagen", System.Data.SqlDbType.VarBinary).Value =
                (object)ejerciciosModel.Imagen ?? DBNull.Value;
                var result = command.ExecuteScalar();
                if (result != null && result != DBNull.Value)
                {
                    // SCOPE_IDENTITY() can return decimal; store as string in the model
                    ejerciciosModel.Id = result.ToString();
                }
            }



        }

        public void Delete(EjerciciosModel ejerciciosModel)
        {
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;

                command.CommandText = "DELETE FROM [Ejercicios] WHERE Id=@Id";
                command.Parameters.AddWithValue("@Id", ejerciciosModel.Id);

                command.ExecuteNonQuery();
            }
        }

        public IEnumerable<EjerciciosModel> GetAllEjercicios()
        {
            List<EjerciciosModel> ejercicios = new List<EjerciciosModel>();

            using (var connection = GetConnection())
            using (var command = new SqlCommand("SELECT * FROM [Ejercicios]", connection))
            {
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ejercicios.Add(new EjerciciosModel
                        {
                            Id = reader["Id"].ToString(),
                            Nombre = reader["Nombre"].ToString(),
                            MusculosTrabajados = reader["MusculosTrabajados"].ToString(),
                            // Conversión explícita a int para coincidir con el modelo
                            Dificultad = reader["Dificultad"] != DBNull.Value ? Convert.ToInt32(reader["Dificultad"]) : 0,
                            // Cast correcto para el array de bytes
                            Imagen = reader["Imagen"] as byte[]
                        });
                    }
                }
            }
            return ejercicios;
        }

        public EjerciciosModel GetById(string id)
        {
            EjerciciosModel ejercicio = null;

            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "SELECT * FROM [Ejercicios] WHERE Id=@Id";
                command.Parameters.AddWithValue("@Id", id);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        ejercicio = new EjerciciosModel
                        {
                            Id = reader["Id"].ToString(),
                            Nombre = reader["Nombre"].ToString(),
                            MusculosTrabajados = reader["MusculosTrabajados"].ToString(),
                            Dificultad = Convert.ToInt32(reader["Dificultad"]),
                            Imagen = reader["Imagen"] as byte[]
                        };
                    }
                }
            }
            return ejercicio;
        }

        public void Update(EjerciciosModel ejerciciosModel)
        {
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = @"UPDATE [Ejercicios] 
                                SET Nombre=@Nombre, 
                                    MusculosTrabajados=@MusculosTrabajados, 
                                    Dificultad=@Dificultad, 
                                    Imagen=@Imagen 
                                WHERE Id=@Id";

                command.Parameters.AddWithValue("@Nombre", ejerciciosModel.Nombre);
                command.Parameters.AddWithValue("@MusculosTrabajados", ejerciciosModel.MusculosTrabajados);
                command.Parameters.AddWithValue("@Dificultad", ejerciciosModel.Dificultad);
                command.Parameters.Add("@Imagen", System.Data.SqlDbType.VarBinary).Value = (object)ejerciciosModel.Imagen ?? DBNull.Value;
                command.Parameters.AddWithValue("@Id", ejerciciosModel.Id);

                command.ExecuteNonQuery();
            }
        }
    }
}
