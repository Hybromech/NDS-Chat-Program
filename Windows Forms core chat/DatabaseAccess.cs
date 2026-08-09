using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Data;
using Windows_Forms_Chat;
using System.Windows.Forms;
using System.IO;

namespace Windows_Forms_CORE_CHAT_UGH
{
    public static class DatabaseAccess
    {
        public static readonly string DATABASE_ADDRESS = "Data Source=.\\DemoDB.db;Version=3;";

        public static void Database_Connect()
        {
            if (DATABASE_ADDRESS != null) // if the file exists
            {
                using (SQLiteConnection cnn = new SQLiteConnection(DATABASE_ADDRESS))
                {
                    cnn.Open();
    
                    // SQL: Drop a Table
                    /*string sql = "DROP TABLE if exists Users";
                    SQLiteCommand sql_dropTable = new SQLiteCommand(sql, cnn);
                    sql_dropTable.ExecuteNonQuery();*/

                    //SQL: Create a Table if not exists
                    string sql1 = "CREATE TABLE if not exists Users (id INTEGER PRIMARY KEY, username, TEXT NOT NULL, password TEXT NOT NULL, wins INT, losses INT, draws INT)";
                    SQLiteCommand sql_createTable = new SQLiteCommand(sql1, cnn);
                    var rowCount = sql_createTable.ExecuteNonQuery();
                    if (rowCount > 0) // If the row count is zero than the table already exists due to CREATE if not exist
                    {
                        Console.WriteLine("Table already exists");      
                    }          
                }
            }
        }

        public static bool DoesUserExist(string username, string password)
        {
            string sqlc = "SELECT 1 FROM [Users] WHERE username = @username AND password = @password";

            using (SQLiteConnection cnn = new SQLiteConnection(DATABASE_ADDRESS)) // Connect to Database
            {
                // Open the Connection
                cnn.Open();
                // Use SQL to query if User Table has (username, password)
                // Wrap command in a using block to ensure it is properly disposed
                using (SQLiteCommand sql_queryTable = new SQLiteCommand(sqlc, cnn))
                {
                    // Add the parameters
                    sql_queryTable.Parameters.AddWithValue("@username", username);
                    sql_queryTable.Parameters.AddWithValue("@password", password);

                    // Execute query and check result
                    var result = sql_queryTable.ExecuteScalar();
                    // ExecuteScalar returns null if no rows matched
                    return result != null;
                }
            }
        }
        public static void AddUser(string username, string password)
        {
            // Add username and password to database
            // SQL: Populate table
            using (SQLiteConnection cnn = new SQLiteConnection(DATABASE_ADDRESS)) // Connect to Database
            {
                string sql = "INSERT INTO Users (username) VALUES (@_username)";
                SQLiteCommand sql_insertTable = new SQLiteCommand(sql, cnn);
                string _username = username;
                sql_insertTable.Parameters.AddWithValue("@username", _username);
                sql_insertTable.ExecuteNonQuery();

                cnn.Close();
            }
        }

        public static void RemoveUser(string username)
        {

        }

        public static void ChangeUsername(string old_username, string new_username)
        {

        }

        public static void UserWon(string username)
        {
            // 1. Open Database
            // 2. Increment Wins of Username
        }
    }
}
