using System;

namespace DepthChart.Exceptions
{
    public class DepthOutOfRangeException : Exception
    {
        public DepthOutOfRangeException() { }

        public DepthOutOfRangeException(string message) : base(message) { }

        public DepthOutOfRangeException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}