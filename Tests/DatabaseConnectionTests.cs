using System.Data.SqlClient;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class DatabaseConnectionTests
    {
        //[TestCase(@"Data Source=212.83.184.206\1433;Initial Catalog=Test;Integrated Security=False;User ID=aakca;Pooling=False")]
        //[TestCase("Data Source=212.83.184.206,1433;Network Library=DBMSSOCN;Initial Catalog=Test;Trusted_Connection=True;MultipleActiveResultSets=true")]
        //[TestCase(@"Server=212.83.184.206\MSSQLLocalDB;Database=Test;Trusted_Connection=True;MultipleActiveResultSets=true")]
        public void TestDbConnection(string connectionString)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    Assert.Pass();
                }
                catch (SqlException e)
                {
                    Assert.Fail($"SqlException: {e}");
                }
                finally
                {
                    connection.Close();
                }
            }

        }
    }
}
