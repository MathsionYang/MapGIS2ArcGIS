using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MapArcGIS
{
    internal struct FeatureTable
    {
        internal List<FeatureTableHead> tableHead;
        internal List<byte[]> tableItem;
        internal int tableHeadNumber;
        internal int tableItemNumer;
        internal int tableItemLengthInBytes;
        
    }
}
