#nullable enable

using System;


namespace Avidity
{
    public static partial class Bases
    {
        /// <summary> Base class for application exceptions that should be displayed to the screen. </summary>
        public class DisplayedException : Exception
        {
            /// <summary> Optionally, a title to display above the error. Use this for a single type of error that may fail in different ways. </summary>
            public string? title;

            /// <summary> The displayed message. Should be a single sentence with no trailing puncutation. </summary>
            public string message;


            public DisplayedException(string message)
            {
                this.message = message;
            }
            
            public DisplayedException(string title, string message)
            {
                this.title = title;
                this.message = message;
            }
        }
    }
}
