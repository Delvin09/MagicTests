using MagicTests.CLI.Interfaces;

namespace MagicTests.CLI.Abstractions
{
    /// <summary>
    /// Represent an element with sub-elements.
    /// </summary>
    internal abstract class ConsoleContainerElement : ConsoleElement
    {
        protected List<IConsoleElement> _consoleElements = new List<IConsoleElement>();

        public override void Init()
        {
            base.Init();
            foreach (var element in _consoleElements)
                element.Init();
        }

        public override void Dispose()
        {
            base.Dispose();
            foreach (var element in _consoleElements)
                element.Dispose();
        }
    }
}
