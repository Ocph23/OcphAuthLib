using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OcphAuthBlazorView
{
  
    [Serializable]
    public class OcphAuthClientException : Exception
    {
        [NonSerialized]
        private  ErrorResult _errorResult;

        /// <summary>
        ///     Initializes a new instance of the <see cref="OcphAuthClientException" /> class.
        /// </summary>
        public OcphAuthClientException()
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="OcphAuthClientException" /> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public OcphAuthClientException(string message)
            : base(message)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="OcphAuthClientException" /> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        public OcphAuthClientException(string message, Exception? innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="OcphAuthClientException" /> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="entries">The entries that were involved in the error.</param>
        public OcphAuthClientException( string message, ErrorResult errorResult)
            : this( message)
        {
            this._errorResult = errorResult;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="OcphAuthClientException" /> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        /// <param name="entries">The entries that were involved in the error.</param>
       
        /// <summary>
        ///     Initializes a new instance of the <see cref="OcphAuthClientException" /> class from a serialized form.
        /// </summary>
        /// <param name="info">The serialization info.</param>
        /// <param name="context">The streaming context being used.</param>
        public OcphAuthClientException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        /// <summary>
        ///     Gets the entries that were involved in the error. Typically this is a single entry, but in some cases it
        ///     may be zero or multiple entries.
        /// </summary>
        public virtual ErrorResult Errors => _errorResult ??= new ErrorResult();
    }
}
