using Gym2.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Net;

namespace Gym2.Repositories
{
    public class UserRepository : RepositoryBase, IUserRepository
    {
        public bool AuthenticateUser(NetworkCredential credential)
        {
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "SELECT * FROM [User] WHERE Username=@username AND [Password]=@password";

                command.Parameters.Add("@username", SqlDbType.NVarChar).Value = credential.UserName;
                command.Parameters.Add("@password", SqlDbType.NVarChar).Value = credential.Password;

                using (var reader = command.ExecuteReader())
                {
                    return reader.HasRows;
                }
            }
        }

        public void Add(UserModel userModel)
        {
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;

                // Do not insert explicit value into identity column. Let the database generate it.
                command.CommandText =
                "INSERT INTO [User] (Username, Password, Name, LastName, Email) " +
                "VALUES(@Username, @Password, @Name, @LastName, @Email); " +
                "SELECT SCOPE_IDENTITY();";

                command.Parameters.AddWithValue("@Username", userModel.Username);
                command.Parameters.AddWithValue("@Password", userModel.Password);
                command.Parameters.AddWithValue("@Name", userModel.Name);
                command.Parameters.AddWithValue("@LastName", userModel.LastName);
                command.Parameters.AddWithValue("@Email", userModel.Email);

                var result = command.ExecuteScalar();
                if (result != null && result != DBNull.Value)
                {
                    // SCOPE_IDENTITY() can return decimal; store as string in the model
                    userModel.Id = result.ToString();
                }
            }
        }

        public void Delete(UserModel userModel)
        {
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;

                command.CommandText = "DELETE FROM [User] WHERE Id=@Id";
                command.Parameters.AddWithValue("@Id", userModel.Id);

                command.ExecuteNonQuery();
            }
        }

        public void Update(UserModel userModel)
        {
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;

                command.CommandText =
                    "UPDATE [User] SET Username=@Username, Name=@Name, LastName=@LastName, Email=@Email WHERE Id=@Id";

                command.Parameters.AddWithValue("@Id", userModel.Id);
                command.Parameters.AddWithValue("@Username", userModel.Username);
                command.Parameters.AddWithValue("@Name", userModel.Name);
                command.Parameters.AddWithValue("@LastName", userModel.LastName);
                command.Parameters.AddWithValue("@Email", userModel.Email);

                command.ExecuteNonQuery();
            }
        }

        public UserModel GetByUsername(string username)
        {
            UserModel user = null;

            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;

                command.CommandText = "SELECT * FROM [User] WHERE Username=@username";
                command.Parameters.Add("@username", SqlDbType.NVarChar).Value = username;

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        user = new UserModel()
                        {
                            Id = reader["Id"].ToString(),
                            Username = reader["Username"].ToString(),
                            Password = string.Empty,
                            Name = reader["Name"].ToString(),
                            LastName = reader["LastName"].ToString(),
                            Email = reader["Email"].ToString()
                        };
                    }
                }
            }

            return user;
        }

        public IEnumerable<UserModel> GetAllUsers()
        {
            List<UserModel> users = new List<UserModel>();

            using (var connection = GetConnection())
            using (var command = new SqlCommand("SELECT * FROM [User]", connection))
            {
                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        users.Add(new UserModel
                        {
                            Id = reader["Id"].ToString(),
                            Username = reader["Username"].ToString(),
                            Password = string.Empty,
                            Name = reader["Name"].ToString(),
                            LastName = reader["LastName"].ToString(),
                            Email = reader["Email"].ToString()
                        });
                    }
                }
            }

            return users;
        }


        public void AddAdmin(int userId)
        {
            using (var connection = GetConnection())
            using (var command = new SqlCommand("INSERT INTO [Admin] (UserId) VALUES (@UserId)", connection))
            {
                connection.Open();
                command.Parameters.AddWithValue("@UserId", userId);
                command.ExecuteNonQuery();
            }
        }

        public void RemoveAdmin(int userId)
        {
            using (var connection = GetConnection())
            using (var command = new SqlCommand("DELETE FROM [Admin] WHERE UserId = @UserId", connection))
            {
                connection.Open();
                command.Parameters.AddWithValue("@UserId", userId);
                command.ExecuteNonQuery();
            }
        }

        public IEnumerable<AdminModel> GetAllAdmins()
        {
            var adminList = new List<AdminModel>();
            using (var connection = GetConnection())
            using (var command = new SqlCommand(
                @"SELECT a.UserId, u.Username, u.Name 
                  FROM [Admin] a 
                  INNER JOIN [User] u ON a.UserId = u.Id", connection))
            {
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        adminList.Add(new AdminModel
                        {
                            UserId = (int)reader["UserId"],
                            Username = reader["Username"].ToString()
                        });
                    }
                }
            }
            return adminList;
        }

        public IEnumerable<ClienteModel> GetAllClientes()
        {
            List<ClienteModel> clientes = new List<ClienteModel>();
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                // Hacemos el JOIN para traer el nombre de usuario de la tabla User
                command.CommandText = "SELECT c.UserId, u.Username, c.MembresiaId FROM Cliente c INNER JOIN [User] u ON c.UserId = u.Id";

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ClienteModel cliente = new ClienteModel();
                        cliente.UserId = reader.GetInt32(0);
                        cliente.Username = reader.GetString(1);
                        // Verificamos si MembresiaId es NULL en la base de datos
                        cliente.MembresiaId = reader.IsDBNull(2) ? (int?)null : reader.GetInt32(2);

                        clientes.Add(cliente);
                    }
                }
            }
            return clientes;
        }

        public void AddCliente(int userId)
        {
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                // Insertamos el nuevo cliente. MembresiaId lo dejamos nulo por defecto
                command.CommandText = "INSERT INTO Cliente (UserId, MembresiaId) VALUES (@UserId, NULL)";
                command.Parameters.Add("@UserId", SqlDbType.Int).Value = userId;

                command.ExecuteNonQuery();
            }
        }

        public void RemoveCliente(int userId)
        {
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "DELETE FROM Cliente WHERE UserId = @UserId";
                command.Parameters.Add("@UserId", SqlDbType.Int).Value = userId;

                command.ExecuteNonQuery();
            }
        }
    }
    
}
