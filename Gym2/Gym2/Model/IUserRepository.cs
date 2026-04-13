using Gym2.Model;
using ProyectoEjemploEsco.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoEjemploEsco.Repositories
{
    public class UserRepository : RepositoryBase, IUserRepository
    {
        public bool AuthenticateUser(NetworkCredential credential)
        {
            bool validUser;
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "select * from [User] where username = @username and [password]=@password";
                command.Parameters.Add("@username", SqlDbType.NVarChar).Value = credential.UserName;
                command.Parameters.Add("@password", SqlDbType.NVarChar).Value = credential.Password;
                validUser = command.ExecuteScalar() == null ? false : true;
            }
            return validUser;
        }

        public void Add(UserModel userModel)
        {
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText =
                    "INSERT INTO [User] VALUES(@Id, @Username, @Password, @Name, @LastName, @Email)";
                command.Parameters.AddWithValue("@Id", userModel.Id);
                command.Parameters.AddWithValue("@Username", userModel.Username);
                command.Parameters.AddWithValue("@Password", userModel.Password);
                command.Parameters.AddWithValue("@Name", userModel.Name);
                command.Parameters.AddWithValue("@LastName", userModel.LastName);
                command.Parameters.AddWithValue("@Email", userModel.Email);
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        public void Delete(UserModel userModel)
        {
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "DELETE FROM [User] WHERE Id = @Id";
                command.Parameters.AddWithValue("@Id", userModel.Id);
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        public void Update(UserModel userModel)
        {
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "UPDATE [User] SET Username=@Username, Name=@Name, " +
                                      "LastName=@LastName, Email=@Email WHERE Id = @Id";
                command.Parameters.AddWithValue("@Id", userModel.Id);
                command.Parameters.AddWithValue("@Username", userModel.Username);
                command.Parameters.AddWithValue("@Name", userModel.Name);
                // command.Parameters.AddWithValue("@Password", userModel.Password);
                command.Parameters.AddWithValue("@LastName", userModel.LastName);
                command.Parameters.AddWithValue("@Email", userModel.Email);
                command.ExecuteNonQuery();
                connection.Close();
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
                command.CommandText = "select * from [User] where username=@username";
                command.Parameters.Add("@username", SqlDbType.NVarChar).Value = username;

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        user = new UserModel()
                        {
                            Id = reader[0].ToString(),
                            Username = reader[1].ToString(),
                            Password = string.Empty,
                            Name = reader[3].ToString(),
                            LastName = reader[4].ToString(),
                            Email = reader[5].ToString()
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
                            Id = reader[0].ToString(),
                            Username = reader[1].ToString(),
                            Password = string.Empty,
                            Name = reader[3].ToString(),
                            LastName = reader[4].ToString(),
                            Email = reader[5].ToString()
                        });
                    }
                }
            }
            return users;
        }
    }
}