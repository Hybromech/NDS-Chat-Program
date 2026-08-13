using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Windows_Forms_Chat;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

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
                    string sql1 = "CREATE TABLE if not exists Users (id INTEGER PRIMARY KEY, username TEXT NOT NULL, password TEXT, wins INT, losses INT, draws INT)";
                    SQLiteCommand sql_createTable = new SQLiteCommand(sql1, cnn);
                    var rowCount = sql_createTable.ExecuteNonQuery();
                    if (rowCount > 0) // If the row count is zero than the table already exists due to CREATE if not exist
                    {
                        Console.WriteLine("Table already exists");      
                    }          
                }
            }
        }

        public static bool Login(string username, string password)
        {
            string sqlc = "SELECT 1 FROM [Users] WHERE username = @username AND password = @password LIMIT 1;";

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
                    var result = sql_queryTable.ExecuteScalar(); //!!!
                    // ExecuteScalar returns null if no rows matched
                    return result != null;
                }
            }
        }

        public static bool DoesUserExist(string username, string password)
        {
            string sqlc = "SELECT 1 FROM [Users] WHERE username = @username";

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
                    //sql_queryTable.Parameters.AddWithValue("@password", password);

                    // Execute query and check result
                    var result = sql_queryTable.ExecuteScalar(); //!!!
                    // ExecuteScalar returns null if no rows matched
                    return result != null;
                }
            }
        }
        public static void AddUser(string username, string password)
        {
            // 1. Updated SQL statement to include password
            string sql = "INSERT INTO Users (username, password) VALUES (@username, @password);";

            using (SQLiteConnection cnn = new SQLiteConnection(DATABASE_ADDRESS))
            {
                cnn.Open();

                // 2. Wrapped SQLiteCommand in a using block
                using (SQLiteCommand sql_insertTable = new SQLiteCommand(sql, cnn))
                {
                    // 3. Bind both parameters
                    sql_insertTable.Parameters.AddWithValue("@username", username);
                    sql_insertTable.Parameters.AddWithValue("@password", password);

                    sql_insertTable.ExecuteNonQuery();
                }
                // Connection and Command automatically close/dispose here
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

            string sql = "UPDATE Users SET wins = COALESCE(wins, 0) + 1 WHERE username = @username"; // Add 1 to wins if its null then set it to zero.

            using (SQLiteConnection cnn = new SQLiteConnection(DATABASE_ADDRESS))
            {
                cnn.Open();
                
                // 2. Wrapped SQLiteCommand in a using block
                using (SQLiteCommand sql_insertTable = new SQLiteCommand(sql, cnn))
                {
                    sql_insertTable.Parameters.AddWithValue("@username", username);
                    sql_insertTable.ExecuteNonQuery();
                }
                // Connection and Command automatically close/dispose here
            }

        }
        public static void UserLost(string username)
        {
            // 1. Open Database
            // 2. Increment Wins of Username

            string sql = "UPDATE Users SET losses = COALESCE(losses, 0) + 1 WHERE username = @username"; // Add 1 to wins if its null then set it to zero.

            using (SQLiteConnection cnn = new SQLiteConnection(DATABASE_ADDRESS))
            {
                cnn.Open();

                // 2. Wrapped SQLiteCommand in a using block
                using (SQLiteCommand sql_insertTable = new SQLiteCommand(sql, cnn))
                {
                    sql_insertTable.Parameters.AddWithValue("@username", username);
                    sql_insertTable.ExecuteNonQuery();
                }
                // Connection and Command automatically close/dispose here
            }

        }
        public static void UsersDraw(string username1, string username2)
        {
            // 1. Open Database
            // 2. Increment Draws of Usernames

            string sql = "UPDATE Users SET draws = COALESCE(draws, 0) + 1 WHERE username = @username1 OR username = @username2"; // Add 1 to draws if its null then set it to zero.

            using (SQLiteConnection cnn = new SQLiteConnection(DATABASE_ADDRESS))
            {
                cnn.Open();

                // 2. Wrapped SQLiteCommand in a using block
                using (SQLiteCommand sql_insertTable = new SQLiteCommand(sql, cnn))
                {
                    sql_insertTable.Parameters.AddWithValue("@username", username1);
                    sql_insertTable.Parameters.AddWithValue("@username", username2);
                    sql_insertTable.ExecuteNonQuery();
                }
                // Connection and Command automatically close/dispose here
            }

        }
        public static int getEntryLength()
        {
            // 1. The SQL query to get the row count
            string sql = "SELECT COUNT(*) FROM Users;";

            using (SQLiteConnection cnn = new SQLiteConnection(DATABASE_ADDRESS))
            {
                cnn.Open();

                using (SQLiteCommand cmd = new SQLiteCommand(sql, cnn))
                {
                    // 2. ExecuteScalar returns an 'object', so we cast it to an int
                    // Convert.ToInt32 handles potential nulls safely
                    object result = cmd.ExecuteScalar();
                    return Convert.ToInt32(result);
                }
            }
        }
        public static List<string> GetScoreInfo()
        {
            // Create a list to store the formatted leaderboard lines
            List<string> scoreboard = new List<string>();

            // SQL query to select username and wins, sorted by wins from highest to lowest
            string sql = "SELECT username, COALESCE(wins, 0) AS win_count FROM Users ORDER BY win_count DESC;";

            using (SQLiteConnection cnn = new SQLiteConnection(DATABASE_ADDRESS))
            {
                cnn.Open();

                using (SQLiteCommand cmd = new SQLiteCommand(sql, cnn))
                {
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        // Loop through every row returned by the database
                        while (reader.Read())
                        {
                            string username = reader["username"].ToString();
                            int wins = Convert.ToInt32(reader["win_count"]);

                            // Format string example: "Tom - 4 wins."
                            string entry = $"{username} - {wins} wins.";
                            scoreboard.Add(entry);
                        }
                    }
                }
            }

            return scoreboard;
        }
    }
}
