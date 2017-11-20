using System;
using System.Net;

namespace Lassie
{
    /// <summary>
    /// Exception thrown by the client class if the returned status is different from 2xx.
    /// </summary>
    public class LassieException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Lassie.LassieException"/> class.
        /// </summary>
        /// <param name="status">Status.</param>
        /// <param name="msg">Message.</param>
        public LassieException(HttpStatusCode status, string msg) : base(String.Format("{0}: {1}", status.ToString(), msg))
        {
            StatusCode = status;
            ErrorMessage = msg;
        }

        /// <summary>
        /// The status code returned from the server.
        /// </summary>
        /// <value>The status code.</value>
        public HttpStatusCode StatusCode { get; private set; } 

        /// <summary>
        /// The error message returned by the server.
        /// </summary>
        /// <value>The error message.</value>
        public string ErrorMessage { get; private set; }
    }
}
