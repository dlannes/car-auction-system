using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarAuctionManagement.Core.Common;

namespace CarAuctionManagement.Core.Exceptions
{
    public class PropertyValidationException : DomainException
    {
        public PropertyValidationException(string messageFormat, params object[] args)
            : base(string.Format(messageFormat, args)) {}

        public PropertyValidationException(string message) : base(message) {}
    }
}
