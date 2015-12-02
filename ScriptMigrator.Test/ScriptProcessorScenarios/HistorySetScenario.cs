using NSubstitute;
using NUnit.Framework;
using ScriptMigrator.Data;
using ScriptMigrator.Executors;
using TestStack.BDDfy;

namespace ScriptMigrator.Test.ScriptProcessorScenarios
{
    [TestFixture]
    public class HistorySetScenario
    {
        private IFileLocator _fileLocator;
        private IScriptHistory _history;
        private IExecutor _executor;

        public void GivenNoScripts()
        {
            var scripts = new[] {"Scripts/test1.sql", "Scripts/test2.sql"};

            _fileLocator = Substitute.For<IFileLocator>();
            _fileLocator.GetFilePaths("Scripts", "*.sql").Returns(scripts);
        }

        public void AndGivenTheScriptsHaveNoHistory()
        {
            _history = Substitute.For<IScriptHistory>();
            _history.HasScriptBeenRun("Dev", Arg.Any<string>()).Returns(false);
        }

        public void AndGivenAnExecutor()
        {
            _executor = Substitute.For<IExecutor>();
        }

        public void WhenTheProcessorExecutes()
        {
            var processor = new ScriptProcessor("Dev", "Scripts", "*.sql", _fileLocator, _history, _executor);
            processor.ProcessScripts();
        }

        public void ThenTheHistoryGetsCheckedForTheFirstScript()
        {
            _history.Received(1).SetScriptHasBeenRun("Dev", "test1.sql");
        }

        public void AndThenTheHistoryGetsCheckedForTheSecondScript()
        {
            _history.Received(1).SetScriptHasBeenRun("Dev", "test2.sql");
        }

        [Test]
        public void Execute()
        {
            this.BDDfy();
        }
    }
}