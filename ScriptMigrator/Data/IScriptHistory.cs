namespace ScriptMigrator.Data
{
    public interface IScriptHistory
    {
        bool HasScriptBeenRun(string environment, string scriptName);
        void SetScriptHasBeenRun(string environment, string scriptName);
        void EnsureScriptMigrationsTableExists();
    }
}