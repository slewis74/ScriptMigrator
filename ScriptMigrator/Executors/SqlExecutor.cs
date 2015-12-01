using System;
using System.Data.SqlClient;
using System.IO;

namespace ScriptMigrator.Executors
{
    public class SqlExecutor : IExecutor
    {
        private readonly string _connectionString;
        private readonly ILogger _logger;

        public SqlExecutor(string connectionString,
            ILogger logger)
        {
            _connectionString = connectionString;
            _logger = logger;
        }

        public void Execute(string filePath)
        {
            _logger.LogInfo("Executing script {0}", filePath);

            var commandText = File.ReadAllText(filePath);

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                using (var cmd = new SqlCommand(commandText, connection))
                {
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }

            _logger.LogInfo("Executed script {0}", filePath);
        }
    }
}