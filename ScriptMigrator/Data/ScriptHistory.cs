using System.Data;
using System.Data.SqlClient;

namespace ScriptMigrator.Data
{
    public class ScriptHistory : IScriptHistory
    {
        private readonly string _connectionString;

        public ScriptHistory(string connectionString)
        {
            _connectionString = connectionString;
        }

        public bool HasScriptBeenRun(string environment, string scriptName)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var cmd = new SqlCommand("SELECT 1 FROM dbo.ScriptMigrations WHERE Environment = @environment AND Name = @name", connection))
                {
                    var envParam = cmd.Parameters.Add("environment", SqlDbType.NVarChar);
                    envParam.Value = environment;

                    var nameParam = cmd.Parameters.Add("name", SqlDbType.NVarChar);
                    nameParam.Value = scriptName;

                    using (var reader = cmd.ExecuteReader())
                    {
                        return reader.HasRows;
                    }
                }
            }
        }

        public void SetScriptHasBeenRun(string environment, string scriptName)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var cmd = new SqlCommand("INSERT INTO dbo.ScriptMigrations (Environment, Name, ExecutedOn) VALUES (@environment, @name, @executedOn)", connection))
                {
                    var envParam = cmd.Parameters.Add("environment", SqlDbType.NVarChar);
                    envParam.Value = environment;

                    var nameParam = cmd.Parameters.Add("name", SqlDbType.NVarChar);
                    nameParam.Value = scriptName;

                    var execParam = cmd.Parameters.Add("executedOn", SqlDbType.DateTime2);
                    execParam.Value = scriptName;

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void EnsureScriptMigrationsTableExists()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                if (CheckIfScriptMigrationsTableExists(connection) == false)
                {
                    CreateScriptMigrationsTable(connection);
                }
            }
        }

        private static bool CheckIfScriptMigrationsTableExists(SqlConnection connection)
        {
            using (var cmd = new SqlCommand("SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'ScriptMigrations'", connection))
            {
                using (var reader = cmd.ExecuteReader())
                {
                    return reader.HasRows;
                }
            }
        }

        private static void CreateScriptMigrationsTable(SqlConnection connection)
        {
            using (var cmd = new SqlCommand("CREATE TABLE dbo.ScriptMigrations (" +
                                            " Environment NVARCHAR(500) NOT NULL," +
                                            " Name NVARCHAR(500) NOT NULL," +
                                            " ExecutedOn DATETIME2 NOT NULL" +
                                            " CONSTRAINT PK_ScriptMigrations PRIMARY KEY CLUSTERED (Environment, Name))",
                connection))
            {
                cmd.ExecuteNonQuery();
            }
        }
    }
}