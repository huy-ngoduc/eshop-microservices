using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation.Results;

namespace Ordering.Application.Exceptions
{
    public class ValidationException : ApplicationException
    {
        public ValidationException(IEnumerable<ValidationFailure> failures) 
            : this()
        {
            Errors = failures.GroupBy(x => x.PropertyName, e => e.ErrorMessage)
                .ToDictionary(failureGroup => failureGroup.Key, failureGroup => failureGroup.ToArray());
        }

        public ValidationException()
            : base("One or more validation failures have occurred")
        {
            Errors = new Dictionary<string, string[]>();
        }

        public IDictionary<string, string[]> Errors { get; set; }
    }
}
