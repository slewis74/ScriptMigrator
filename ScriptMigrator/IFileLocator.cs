using System.Collections.Generic;

namespace ScriptMigrator
{
    public interface IFileLocator
    {
        IEnumerable<string> GetFilePaths(string baseFolder, string filePattern);
    }
}