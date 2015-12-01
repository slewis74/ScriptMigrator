namespace ScriptMigrator
{
    public interface ILogger
    {
        void LogInfo(string message);
        void LogInfo(string messageFormat, params object[] messageParams);

        void LogError(string message);
        void LogError(string messageFormat, params object[] messageParams);
    }
}