using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace sqlQuery
{
    public static class LibSQLConnection
    {
        static string sqlConnectionString = @"data source=mssql6.gear.host;initial catalog=homebudgetapp;user id=homebudgetapp;password=Xy2DSX8_UL5!";
        
        public static void AddNewTransaction(double cost, string categoryGuid, string userGuid, string descrption)
        {
            string query = "INSERT INTO dbo.Transactions VALUES (@newid, @cost, @catid, @userid, @date, @description)";

            using(SqlConnection con = new SqlConnection(sqlConnectionString))
            {
                con.Open();

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.Add("@newid", System.Data.SqlDbType.UniqueIdentifier).Value = System.Guid.NewGuid();
                    cmd.Parameters.Add("@cost", System.Data.SqlDbType.Float).Value = cost;
                    cmd.Parameters.Add("@catid", System.Data.SqlDbType.UniqueIdentifier).Value = new Guid(categoryGuid);
                    cmd.Parameters.Add("@userid", System.Data.SqlDbType.UniqueIdentifier).Value = new Guid(userGuid);
                    cmd.Parameters.Add("@date", System.Data.SqlDbType.DateTime).Value = DateTime.Today;
                    cmd.Parameters.Add("@description", System.Data.SqlDbType.VarChar, 255).Value = descrption;
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static void AddNewTransactionWithDate(double cost, string categoryGuid, string userGuid, DateTime date, string descrption)
        {
            string query = "INSERT INTO dbo.Transactions VALUES (@newid, @cost, @catid, @userid, @date, @description)";

            using (SqlConnection con = new SqlConnection(sqlConnectionString))
            {
                con.Open();

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.Add("@newid", System.Data.SqlDbType.UniqueIdentifier).Value = System.Guid.NewGuid();
                    cmd.Parameters.Add("@cost", System.Data.SqlDbType.Float).Value = cost;
                    cmd.Parameters.Add("@catid", System.Data.SqlDbType.UniqueIdentifier).Value = new Guid(categoryGuid);
                    cmd.Parameters.Add("@userid", System.Data.SqlDbType.UniqueIdentifier).Value = new Guid(userGuid);
                    cmd.Parameters.Add("@date", System.Data.SqlDbType.DateTime).Value = date.Date;
                    cmd.Parameters.Add("@description", System.Data.SqlDbType.VarChar, 255).Value = descrption;
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static List<UserTransaction> GetTransactionSum(DateTime day)
        {
            string query = "Select Transactions.Cost, Users.Name, Transactions.Date FROM Transactions INNER JOIN Users ON Transactions.UserId = Users.Id WHERE Transactions.Date = @date AND CategoryId != 'C041805D-5CEA-4043-B349-554ABB638EA4'";
            List<UserTransaction> userList = new List<UserTransaction>() 
            {
                new UserTransaction("Andrzej", 0),
                new UserTransaction("Klaudia", 0)
            };
            using (SqlConnection con = new SqlConnection(sqlConnectionString))
            {
                con.Open();

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.Add("@date", System.Data.SqlDbType.DateTime).Value = day;
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (reader.GetString(1).Equals(Users.Andrzej.UName))
                            {
                                userList[0].Sum += reader.GetDouble(0);
                            }
                            else
                            {
                                userList[1].Sum += reader.GetDouble(0);
                            }
                        }
                    }
                }
            }
            return userList;
        }

        public static double GetDailyLimit()
        {
            string query = "Select * from Settings";
            string queryToSpent = "SELECT SUM(Cost) FROM Transactions WHERE Date >= @dateFrom AND Date <= @dateTo AND CategoryId != 'C041805D-5CEA-4043-B349-554ABB638EA4'";
            string queryUpdate = "UPDATE Settings SET DailyLimit = @limit";
            double limit = 0;
            double amount = 0;
            DateTime date = DateTime.Today;
            using (SqlConnection con = new SqlConnection(sqlConnectionString))
            {
                con.Open();

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            limit = reader.GetDouble(1);
                            date = reader.GetDateTime(4);
                            amount = reader.GetDouble(5);
                        }
                    }
                }

                if (date.Day != DateTime.Today.Day)
                {
                    double amountSpent = 0;
                    int daysLeft = DateTime.DaysInMonth(DateTime.Today.Year, DateTime.Today.Month) - DateTime.Today.Day;
                    using (SqlCommand cmd2 = new SqlCommand(queryToSpent, con))
                    {
                        DateTime dateFrom = new DateTime(DateTime.Today.Year,DateTime.Today.Month, 1);
                        DateTime dateTo = new DateTime(DateTime.Today.Year,DateTime.Today.Month, DateTime.DaysInMonth(DateTime.Today.Year, DateTime.Today.Month));
                        cmd2.Parameters.Add("@dateFrom", System.Data.SqlDbType.DateTime).Value = dateFrom;
                        cmd2.Parameters.Add("@dateTo", System.Data.SqlDbType.DateTime).Value = dateTo;
                        amountSpent = (double)cmd2.ExecuteScalar();
                    }
                    limit = (amount - amountSpent) / daysLeft;
                    limit = limit <= 0 ? 0 : limit;
                    using (SqlCommand cmd3 = new SqlCommand(queryUpdate, con))
                    {
                        cmd3.Parameters.Add("@limit", System.Data.SqlDbType.Float).Value = limit;
                        cmd3.ExecuteNonQuery();
                    }
                }               
            }
            return limit;
        }

        public static double GetAdditionalPool()
        {
            string query = "SELECT SUM(Cost) FROM Transactions WHERE Date >= @dateFrom AND Date <= @dateTo AND CategoryId = 'C041805D-5CEA-4043-B349-554ABB638EA4'";
            double additionalPool = 0;

            using (SqlConnection con = new SqlConnection(sqlConnectionString))
            {
                con.Open();

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    DateTime dateFrom = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                    DateTime dateTo = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.DaysInMonth(DateTime.Today.Year, DateTime.Today.Month));
                    cmd.Parameters.Add("@dateFrom", System.Data.SqlDbType.DateTime).Value = dateFrom;
                    cmd.Parameters.Add("@dateTo", System.Data.SqlDbType.DateTime).Value = dateTo;
                    additionalPool = (double)cmd.ExecuteScalar();
                }
            }
            return additionalPool;
        }

        /// <summary>
        /// Returns array of 7 last days without the todays. Order from the oldest to yesterday.
        /// </summary>
        /// <returns></returns>
        public static double[] GetLastWeekTransactionsSum()
        {
            string query = "select CAST(Date AS DATE) as 'Date', SUM(Cost) as 'Sum' from dbo.Transactions Where Date >= @dateFrom AND Date <= @dateTo group by CAST(Date AS DATE) order by Date";
            double[] returnArray = new double[7];

            using (SqlConnection con = new SqlConnection(sqlConnectionString))
            {
                con.Open();

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.Add("@dateFrom", System.Data.SqlDbType.DateTime).Value = DateTime.Today.AddDays(-7);
                    cmd.Parameters.Add("@dateTo", System.Data.SqlDbType.DateTime).Value = DateTime.Today.AddDays(-1);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        int i = 0;
                        while (reader.Read())
                        {
                            returnArray[i] = reader.GetDouble(1);
                            i++;
                        }
                    }
                }
            }
            return returnArray;
        }

        public static bool HasSeenMessage(Guid userGuid)
        {
            string query = "Select HasSeenMessage From Users Where Id = @userId";
            bool result = false;

            using (SqlConnection con = new SqlConnection(sqlConnectionString))
            {
                con.Open();

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.Add("@userId", System.Data.SqlDbType.UniqueIdentifier).Value = userGuid;                    
                    result = (bool)cmd.ExecuteScalar();
                }
            }
            return result;
        }

        public static MessageRecord GetMessageData()
        {
            string query = "select * from dbo.Settings";
            MessageRecord mr = new MessageRecord();

            using (SqlConnection con = new SqlConnection(sqlConnectionString))
            {
                con.Open();

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            mr.Message = reader.GetString(2);
                            if (reader.GetGuid(3).ToString().ToUpper().Equals(Users.Andrzej.UGuid))
                            {
                                mr.Creator = Users.Andrzej.UName;
                            }
                            else if (reader.GetGuid(3).ToString().ToUpper().Equals(Users.Klaudia.UGuid))
                            {
                                mr.Creator = Users.Klaudia.UName;
                            }                            
                        }
                    }
                }
            }
            return mr;
        }

        public static List<TransactionRecord> GetTransactionsHistory()
        {
            string query = "Select Top 25 Date, Cost, UserId, CategoryId, Description from dbo.Transactions Order by Date Desc";
            List<TransactionRecord> returnList = new List<TransactionRecord>();

            using (SqlConnection con = new SqlConnection(sqlConnectionString))
            {
                con.Open();

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var tr = new TransactionRecord();
                            tr.Date = reader.GetDateTime(0);
                            tr.Cost = reader.GetDouble(1);
                            tr.Name = Users.GetUserName(reader.GetGuid(2));
                            tr.Category = Categories.GetCategoryName(reader.GetGuid(3));
                            try
                            {
                                tr.Description = reader.GetString(4);
                            }
                            catch (System.Data.SqlTypes.SqlNullValueException)
                            {
                                tr.Description = string.Empty;
                            }
                            returnList.Add(tr);
                        }
                    }
                }
            }
            return returnList;
        }

        public static void ModifyAmountToSpend(double newAmount)
        {
            string query = "UPDATE Settings SET AmountToSpend = @limit";

            using (SqlConnection con = new SqlConnection(sqlConnectionString))
            {
                con.Open();

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    using (SqlCommand cmd3 = new SqlCommand(query, con))
                    {
                        cmd3.Parameters.Add("@limit", System.Data.SqlDbType.Float).Value = newAmount;
                        cmd3.ExecuteNonQuery();
                    }
                }
            }
        }
    }
}
