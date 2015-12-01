using System.Collections.Generic;
using System.IO;

namespace ScriptMigrator
{
    public class FileLocator : IFileLocator
    {
        public IEnumerable<string> GetFilePaths(string baseFolder, string filePattern)
        {
            var files = Directory.GetFiles(baseFolder, filePattern, SearchOption.AllDirectories);
            return files;
        }
    }
}