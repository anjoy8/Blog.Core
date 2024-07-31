using Microsoft.Data.SqlClient;
using Xunit;
using Xunit.Abstractions;

namespace Blog.Core.Tests;

public class DbTest(ITestOutputHelper testOutputHelper)
{
    [Fact]
    public void Test_CreateDataBase()
    {
        string connectionString = "Database=master;TrustServerCertificate=true;Persist Security Info=False;Trusted_Connection=True;server=(local)";
        string connectionString2 = "Database=Blog.Core;TrustServerCertificate=true;Persist Security Info=False;Trusted_Connection=True;server=(local)";

        // 创建数据库
        using (SqlConnection masterConnection = new SqlConnection(connectionString))
        {
            masterConnection.Open();
            string createDbQuery = $"CREATE DATABASE [Blog.Core];";
            using (SqlCommand command = new SqlCommand(createDbQuery, masterConnection))
            {
                command.ExecuteNonQuery();
                testOutputHelper.WriteLine("Database created successfully.");
            }
        }

        // 连接到新创建的数据库
        using (SqlConnection newDbConnection = new SqlConnection(connectionString2))
        {
            newDbConnection.Open();
            string testQuery = "SELECT 1;";
            using (SqlCommand command = new SqlCommand(testQuery, newDbConnection))
            {
                int result = (int)command.ExecuteScalar()!;
                testOutputHelper.WriteLine("Connection to new database successful, test query result: " + result);
            }
        }
    }
}