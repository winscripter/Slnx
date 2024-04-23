namespace Slnx
{
    /// <summary>
    /// Represents an error related to SLNX syntax or its infrastructure.
    /// </summary>
    internal class SolutionException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SolutionException" /> class.
        /// </summary> 
        /// <param name="message">Exception message.</param>
        public SolutionException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SolutionException" /> class.
        /// </summary> 
        /// <param name="message">Exception message.</param>
        /// <param name="innerException">An inner exception.</param>
        public SolutionException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
