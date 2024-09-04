using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarAuctionManagement.Application.Exceptions
{
    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException(string? entityName)
            : base($"The specified {entityName} was not found.") { }
        public EntityNotFoundException(string? entityName, string? paramName, object? paramValue)
            : base($"{entityName} with {paramName} '{paramValue}' was not found.") { }
        public EntityNotFoundException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
