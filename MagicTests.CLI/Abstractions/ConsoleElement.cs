using MagicTests.CLI.Interfaces;
using System.Drawing;

namespace MagicTests.CLI.Abstractions
{
    /// <summary>
    /// Simple single console element.
    /// </summary>
    internal abstract class ConsoleElement : IConsoleElement
    {
        protected Point _startPosition;
        protected Point _endPosition;

        public virtual void UpdateElement()
        {
            Clear();
            InnerUpdate();
        }

        public virtual void Init()
        {
            _startPosition = new(Console.CursorLeft, Console.CursorTop);

            Update();

            Console.WriteLine();
        }

        public virtual void Dispose() { }

        /// <summary>
        /// Update this element on the screen and correct end position.
        /// </summary>
        protected void Update()
        {
            InnerUpdate();

            _endPosition = new(Console.CursorLeft, Console.CursorTop);
        }

        /// <summary>
        /// Update this element on the screen.
        /// </summary>
        protected abstract void InnerUpdate();

        /// <summary>
        /// Clear part of the screen where the element is placed.
        /// </summary>
        protected virtual void Clear()
        {
            var y = _startPosition.Y;

            while (y <= _endPosition.Y)
            {
                Console.SetCursorPosition(_startPosition.X, y);
                var emptyLine = y != _endPosition.Y
                    ? new string(' ', Console.WindowWidth)
                    : new string(' ', _endPosition.X);

                Console.Write(emptyLine);
                y++;
            }
            Console.SetCursorPosition(_startPosition.X, _startPosition.Y);
        }
    }
}
