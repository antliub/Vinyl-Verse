using System;
using System.Data;
using MySql.Data.MySqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using static VinylVerse.App;

namespace VinylVerse
{
    public class DatabaseHelper
    {
        public static string connectionString = "Server=185.139.230.196;Database=VynilVerse;Uid=root;Pwd=((As1234567890l));";
        public bool IsUserExists(string name)
        {
            try
            {
                using (var connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT COUNT(*) FROM Users WHERE name = @Name";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Name", name);

                        int userCount = Convert.ToInt32(command.ExecuteScalar());
                        return userCount > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return false;
            }
        }

        public bool RegisterUser(string name, string email, string password)
        {
            try
            {
                using (var connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    // if there is user with rank 1
                    string checkQuery = "SELECT COUNT(*) FROM Users WHERE user_rank = 1";
                    bool isAdminExists = false;

                    using (var checkCommand = new MySqlCommand(checkQuery, connection))
                    {
                        int count = Convert.ToInt32(checkCommand.ExecuteScalar());
                        isAdminExists = count > 0; // true, if rank 1 exsists
                    }

                    // Add new user
                    string query = "INSERT INTO Users (name, email, user_rank, password, registration_date) " +
                                   "VALUES (@Name, @Email, @UserRank, @Password, NOW())";

                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Name", name);
                        command.Parameters.AddWithValue("@Email", email);

                        int userRank = isAdminExists ? 0 : 1;
                        command.Parameters.AddWithValue("@UserRank", userRank);

                        string hashedPassword = HashPassword(password);
                        command.Parameters.AddWithValue("@Password", hashedPassword);

                        command.ExecuteNonQuery();
                    }

                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return false;
            }
        }

        public App.User LoginUser(string identifier, string password)
        {
            try
            {
                using (var connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string hashedPassword = HashPassword(password);

                    string query = "SELECT id, name, email, user_rank, profile_picture, payment_method, language, registration_date " +
                                   "FROM Users WHERE (email = @Identifier OR name = @Identifier) AND password = @Password";

                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Identifier", identifier);
                        command.Parameters.AddWithValue("@Password", hashedPassword);

                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new App.User
                                {
                                    Id = reader.GetInt32("id"),
                                    Name = reader.GetString("name"),
                                    Email = reader.GetString("email"),
                                    UserRank = reader.GetInt32("user_rank"),
                                    ProfilePicture = reader.IsDBNull("profile_picture") ? null : reader.GetString("profile_picture"),
                                    PaymentMethod = reader.IsDBNull("payment_method") ? null : reader.GetString("payment_method"),
                                    Language = reader.IsDBNull("language") ? null : reader.GetString("language"),
                                    RegistrationDate = reader.GetDateTime("registration_date")
                                };
                            }
                            else
                            {
                                return null;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return null;
            }
        }

        public string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }
    }
}
