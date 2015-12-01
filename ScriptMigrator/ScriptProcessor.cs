using System.Collections;
using System.IO;
using System.Linq;
using ScriptMigrator.Data;
using ScriptMigrator.Executors;

namespace ScriptMigrator
{
    public class ScriptProcessor
    {
        private readonly string _environment;
        private readonly string _baseScriptFolder;
        private readonly string _fileMatchPattern;
        private readonly IFileLocator _fileLocator;
        private readonly IScriptHistory _scriptHistory;
        private readonly IExecutor _executor;

        public ScriptProcessor(
            string environment,
            string baseScriptFolder,
            string fileMatchPattern,
            IFileLocator fileLocator, 
            IScriptHistory scriptHistory,
            IExecutor executor)
        {
            _environment = environment;
            _baseScriptFolder = baseScriptFolder;
            _fileMatchPattern = fileMatchPattern;
            _fileLocator = fileLocator;
            _scriptHistory = scriptHistory;
            _executor = executor;
        }

        public void ProcessScripts()
        {
            var filePaths = _fileLocator.GetFilePaths(_baseScriptFolder, _fileMatchPattern);
            foreach (var filePath in filePaths.OrderBy(x => x))
            {
                var filename = Path.GetFileName(filePath);

                ProcessScript(filePath, filename);
            }
        }

        private void ProcessScript(string filePath, string filename)
        {
            if (_scriptHistory.HasScriptBeenRun(_environment, filename) == false)
            {
                _executor.Execute(filePath);

                _scriptHistory.SetScriptHasBeenRun(_environment, filename);
            }
        }
    }
}