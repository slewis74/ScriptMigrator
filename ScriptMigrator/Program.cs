using System;
using Fclp;
using ScriptMigrator.Data;
using ScriptMigrator.Executors;

namespace ScriptMigrator
{
    class Program
    {
        static int Main(string[] args)
        {
            var logger = new Logger();

            try
            {
                var parser = new FluentCommandLineParser();

                var deployDbConnection = string.Empty;
                parser.Setup<string>('d', "deployDbConnection").Callback(s => deployDbConnection = s).Required();

                var environment = string.Empty;
                parser.Setup<string>('e', "environment").Callback(s => environment = s).Required();

                var scriptFolder = string.Empty;
                parser.Setup<string>('f', "folder").Callback(s => scriptFolder = s).Required();

                var filenameMask = string.Empty;
                parser.Setup<string>('m', "mask").Callback(s => filenameMask = s).Required();

                var sqlConn = string.Empty;
                parser.Setup<string>('s', "sqlConn").Callback(s => sqlConn = s).SetDefault(String.Empty);

                var psFile = string.Empty;
                parser.Setup<string>('p', "psFile").Callback(s => psFile = s).SetDefault(String.Empty);

                var cmdFile = string.Empty;
                parser.Setup<string>('c', "cmdFile").Callback(s => cmdFile = s).SetDefault(String.Empty);

                var result = parser.Parse(args);
                if (result.HasErrors)
                {
                    logger.LogError(result.ErrorText);
                    return 2;
                }

                IExecutor executor;
                if (string.IsNullOrWhiteSpace(sqlConn) == false)
                {
                    executor = new SqlExecutor(sqlConn, logger);
                }
                else if (string.IsNullOrWhiteSpace(psFile) == false)
                {
                    executor = new PowerShellExecutor();
                }
                else if (string.IsNullOrWhiteSpace(cmdFile) == false)
                {
                    executor = new CommandExecutor();
                }
                else
                {
                    throw new ApplicationException("Script type cannot be determined.  Specify sqlConn, psFile or cmdFile");
                }

                var locator = new FileLocator();
                var history = new ScriptHistory(deployDbConnection);
                var processor = new ScriptProcessor(environment, scriptFolder, filenameMask, locator, history, executor);

                processor.ProcessScripts();
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                return 1;
            }

            return 0;
        }
    }
}
