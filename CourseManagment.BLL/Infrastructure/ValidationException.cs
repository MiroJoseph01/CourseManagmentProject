using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseManagment.BLL.Infrastructure
{
    /// <summary>
    /// Class ValidationException
    /// extend class Exception to implement Validation Exceptions
    /// </summary>
    public class ValidationException : Exception
    {
        public string Property { get; protected set; }
        /// <summary>
        /// Constructor to create Validation Exception
        /// </summary>
        /// <param name="message">message</param>
        /// <param name="prop">property</param>
        public ValidationException(string message, string prop) : base(message)
        {
            Property = prop;
        }
    }
}
