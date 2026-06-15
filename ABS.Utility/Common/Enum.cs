using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTGroup.Utility.Common
{
    public enum responseMessage
    {
        Success = 1,
        Error = 0,
        Undefined = -1
    }

    public enum userType
    {
        Employee = 1,
        Buyer = 2,
        Supplier = 3,
        BuyerRef = 4,
        CTGroupUSER = 5,
        SuppContctPerson = 6
    }
}
