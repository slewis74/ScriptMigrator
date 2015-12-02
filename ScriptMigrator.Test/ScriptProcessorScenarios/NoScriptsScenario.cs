using System.Linq;
using NSubstitute;
using NUnit.Framework;
using Shouldly;
using TestStack.BDDfy;

namespace ScriptMigrator.Test.ScriptProcessorScenarios
{
    [TestFixture]
    public class NoScriptsScenario
    {
        private IFileLocator _fileLocator;
        private bool _completed;

        public void GivenNoScripts()
        {
            _fileLocator = Substitute.For<IFileLocator>();
            _fileLocator.GetFilePaths("Scripts", "*.sql").Returns(Enumerable.Empty<string>());
        }

        public void WhenTheProcessorExecutes()
        {
            var processor = new ScriptProcessor("Dev", "Scripts", "*.sql", _fileLocator, null, null);
            processor.ProcessScripts();
            _completed = true;
        }

        public void ThenItDoesntFail()
        {
            _completed.ShouldBeTrue();
        }

        [Test]
        public void Execute()
        {
            this.BDDfy();
        }
    }
}
