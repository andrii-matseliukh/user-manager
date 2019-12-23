using System;
using System.Collections.Generic;
using System.Text;

namespace UserManager.Common.Exceptions
{
    public class SaveChangesDbException : Exception
    {
        public SaveChangesDbException()
        {
        }

        public SaveChangesDbException(string message)
            : base(message)
        {
        }

        public SaveChangesDbException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
