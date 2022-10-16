using Common.Interfaces;
using Common.Models;
using System.Configuration;
using System.Data.SqlClient;

namespace DatabaseStorage
{
    public class DbStorage : IDataStorage
    {
        private string connectionString;

        public DbStorage(string connectionString)
        {
            this.connectionString = connectionString;
            testConnection();
            ensureItemsTableCreated();
        }

        public DbStorage() : this(getConnectionStringFromConfig())
        {
        }

        private void testConnection()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                }
                catch (SqlException e)
                {
                    throw new Exception("Db connection test fail", e);
                }
            }
        }

        private static string getConnectionStringFromConfig()
        {
            var con = ConfigurationManager.ConnectionStrings["MSSQL"];
            if (con == null) throw new Exception("Null connection string \"MSSQL\" in ConfigurationManager.ConnectionStrings");
            return con.ConnectionString;
        }

        private void ensureItemsTableCreated()
        {
            string tableName = "Items";
            string sqlExpression = @$"
            IF (NOT EXISTS (SELECT *
               FROM INFORMATION_SCHEMA.TABLES
               WHERE TABLE_SCHEMA = 'dbo'
               AND TABLE_NAME = '{tableName}'))
               BEGIN
                    CREATE TABLE [dbo].[{tableName}] (
                        [Id]          INT          NOT NULL,
                        [Name]        VARCHAR (50) NOT NULL,
                        [Description] VARCHAR (50) NULL,
                        [Category]    VARCHAR (50) NULL,
                        [Cost]        FLOAT (53)   NULL,
                        PRIMARY KEY CLUSTERED ([Id])
                    );
               END;
            ";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand command = new SqlCommand(sqlExpression, connection);
                command.ExecuteNonQuery();
            }
        }

        public bool deleteItem(int Id)
        {
            throw new NotImplementedException();
        }

        public Item? getItemById(int Id)
        {
            throw new NotImplementedException();
        }

        public List<Item> getItems()
        {
            List<Item> items = new List<Item>();

            string sqlExpression = "SELECT * FROM Items";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand command = new SqlCommand(sqlExpression, connection);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows) // если есть данные
                    {
                        while (reader.Read()) // построчно считываем данные
                        {
                            var res = Parser.ConvertToObject<Item>(reader);
                            if (res != null) items.Add(res);
                        }
                    }
                }
            }

            return items;
        }

        public bool postItem(Item item)
        {
            throw new NotImplementedException();
        }
    }
}