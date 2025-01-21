namespace MagicTests.CLI.Interfaces
{
    /// <summary>
    /// Representing ui elements for the Console screen.
    /// </summary>
    internal interface IConsoleElement : IDisposable
    {
        /// <summary>
        /// Update element on the screen, but don't touch sub-elements.
        /// </summary>
        void UpdateElement();

        /// <summary>
        /// First show element on the screen with the sub-elements.
        /// </summary>
        void Init();
    }
}
