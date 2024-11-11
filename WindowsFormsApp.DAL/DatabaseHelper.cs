using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using WindowsFormsApp.DAL.Models;

namespace WindowsFormsApp.DAL
{
    public class DatabaseHelper
    {
        private static string connectionString = ConfigurationManager.ConnectionStrings["OrderManagementDB"].ConnectionString;

        #region User Methods
        public static bool ValidateUser(string username, string password)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT COUNT(*) FROM [User] WHERE Username = @Username AND Password = @Password";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@Password", password);

                conn.Open();
                int count = (int)cmd.ExecuteScalar();
                return count > 0;
            }
        }
        #endregion

        #region Item Methods
        public static DataTable GetAllItems()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"
            SELECT 
                ItemID,
                ItemName,
                Size,
                Price,
                ISNULL((
                    SELECT SUM(Quantity) 
                    FROM OrderDetail 
                    WHERE ItemID = I.ItemID
                ), 0) as TotalSold
            FROM Item I
            ORDER BY ItemName";

                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                return dt;
            }
        }
        public static List<Item> GetAllItemsList()
        {
            List<Item> items = new List<Item>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT ItemID, ItemName, Size, Price FROM Item ORDER BY ItemName";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        items.Add(new Item
                        {
                            ItemID = (int)reader["ItemID"],
                            ItemName = reader["ItemName"].ToString(),
                            Size = reader["Size"].ToString(),
                            Price = (decimal)reader["Price"]
                        });
                    }
                }
            }
            return items;
        }
        public static void AddItem(Item item)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO Item (ItemName, Size, Price) VALUES (@ItemName, @Size, @Price)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ItemName", item.ItemName);
                cmd.Parameters.AddWithValue("@Size", item.Size);
                cmd.Parameters.AddWithValue("@Price", item.Price);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public static void UpdateItem(Item item)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "UPDATE Item SET ItemName = @ItemName, Size = @Size, Price = @Price WHERE ItemID = @ItemID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ItemID", item.ItemID);
                cmd.Parameters.AddWithValue("@ItemName", item.ItemName);
                cmd.Parameters.AddWithValue("@Size", item.Size);
                cmd.Parameters.AddWithValue("@Price", item.Price);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
        #endregion

        public static DataTable GetAllOrdersDetailed()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"
            SELECT 
                O.OrderID,
                O.OrderDate,
                A.AgentName,
                I.ItemName,
                OD.Quantity,
                OD.UnitAmount,
                (OD.Quantity * OD.UnitAmount) as TotalAmount
            FROM [Order] O
            JOIN Agent A ON O.AgentID = A.AgentID
            JOIN OrderDetail OD ON O.OrderID = OD.OrderID
            JOIN Item I ON OD.ItemID = I.ItemID
            ORDER BY O.OrderDate DESC, O.OrderID, I.ItemName";

                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                return dt;
            }
        }

        #region Agent Methods
        public static DataTable GetAllAgents()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"
            SELECT 
                AgentID,
                AgentName,
                Address,
                (
                    SELECT COUNT(DISTINCT OrderID) 
                    FROM [Order] 
                    WHERE AgentID = A.AgentID
                ) as TotalOrders
            FROM Agent A
            ORDER BY AgentName";

                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                return dt;
            }
        }
        public static void AddAgent(Agent agent)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO Agent (AgentName, Address) VALUES (@AgentName, @Address)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@AgentName", agent.AgentName);
                cmd.Parameters.AddWithValue("@Address", agent.Address);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
        #endregion

        #region Order Methods
        public static int CreateOrder(Order order)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        // Insert Order
                        string orderQuery = "INSERT INTO [Order] (OrderDate, AgentID) VALUES (@OrderDate, @AgentID); SELECT SCOPE_IDENTITY();";
                        SqlCommand orderCmd = new SqlCommand(orderQuery, conn, transaction);
                        orderCmd.Parameters.AddWithValue("@OrderDate", order.OrderDate);
                        orderCmd.Parameters.AddWithValue("@AgentID", order.AgentID);

                        int orderId = Convert.ToInt32(orderCmd.ExecuteScalar());

                        // Insert Order Details
                        foreach (var detail in order.OrderDetails)
                        {
                            string detailQuery = "INSERT INTO OrderDetail (OrderID, ItemID, Quantity, UnitAmount) VALUES (@OrderID, @ItemID, @Quantity, @UnitAmount)";
                            SqlCommand detailCmd = new SqlCommand(detailQuery, conn, transaction);
                            detailCmd.Parameters.AddWithValue("@OrderID", orderId);
                            detailCmd.Parameters.AddWithValue("@ItemID", detail.ItemID);
                            detailCmd.Parameters.AddWithValue("@Quantity", detail.Quantity);
                            detailCmd.Parameters.AddWithValue("@UnitAmount", detail.UnitAmount);
                            detailCmd.ExecuteNonQuery();
                        }

                        transaction.Commit();
                        return orderId;
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }
        #endregion

        public static DataTable GetBestSellingItems()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"
            SELECT 
                I.ItemName,
                SUM(OD.Quantity) as TotalQuantitySold,
                SUM(OD.Quantity * OD.UnitAmount) as TotalRevenue
            FROM Item I
            LEFT JOIN OrderDetail OD ON I.ItemID = OD.ItemID
            GROUP BY I.ItemName
            ORDER BY TotalQuantitySold DESC";

                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                return dt;
            }
        }
        public static DataTable GetAgentPurchaseHistory(int agentId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"
            SELECT 
                O.OrderDate,
                I.ItemName,
                OD.Quantity,
                OD.UnitAmount,
                (OD.Quantity * OD.UnitAmount) as TotalAmount
            FROM [Order] O
            JOIN OrderDetail OD ON O.OrderID = OD.OrderID
            JOIN Item I ON OD.ItemID = I.ItemID
            WHERE O.AgentID = @AgentID
            ORDER BY O.OrderDate DESC";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@AgentID", agentId);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                return dt;
            }
        }

        public static DataTable GetOrderDetails(int orderId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"
            SELECT 
                O.OrderID,
                O.OrderDate,
                A.AgentName,
                I.ItemName,
                OD.Quantity,
                OD.UnitAmount,
                (OD.Quantity * OD.UnitAmount) as TotalAmount
            FROM [Order] O
            JOIN Agent A ON O.AgentID = A.AgentID
            JOIN OrderDetail OD ON O.OrderID = OD.OrderID
            JOIN Item I ON OD.ItemID = I.ItemID
            WHERE O.OrderID = @OrderID";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@OrderID", orderId);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                return dt;
            }
        }
    }
}