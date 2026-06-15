using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTGroup.Utility
{
    public class Enum
    {
    }

    public enum ItemType
    {
        FinishGood = 1,
        RawMaterial = 2,
        Yarn = 3,
        FixedAsset = 4,
        Chemical = 5
    }
    
    public enum ResponseMessage
    {
        Success = 1,
        Error = 0,
        Invalid = -1,
        Exception =-2,
    }
    public enum AdresType
    {
        Loading = 1,
        Discharge = 2,
        Current = 3
    }

    public enum ShipDocStatus 
    {
        InOrder = 1,
        Descripent = 2,        
    }
}
