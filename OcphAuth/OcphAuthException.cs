using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Update;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OcphAuthServer
{
    [Serializable]
    public class OcphAuthException : Exception
    {
        [NonSerialized]
        private IReadOnlyList<string>? _errors;

        public virtual IReadOnlyList<string> Entries => _errors ?? (_errors = new List<string>());

        public OcphAuthException()
        {
        }

        public OcphAuthException(string message)
            : base(message)
        {
        }

        public OcphAuthException(string message, Exception? innerException)
            : base(message, innerException)
        {
        }

        public OcphAuthException(string message, IReadOnlyList<string> entries)
            : this(message, null, entries)
        {
        }

        public OcphAuthException(string message, Exception? innerException, IReadOnlyList<string> entries)
            : base(message, innerException)
        {
            _errors = entries.ToList();
        }

        public OcphAuthException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
