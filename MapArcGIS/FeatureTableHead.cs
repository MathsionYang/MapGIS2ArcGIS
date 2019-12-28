using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MapArcGIS
{
    internal struct FeatureTableHead
    {
        internal string headName;
        internal byte[] headNameBytes;
        internal FeatureType itemType;
        internal int offset;
        internal short lengthInBytes;
        internal short tableItemCharLength;
    }
    internal enum FeatureType
    {
        String,
        Byte,
        Short,
        Int,
        Float,
        Double,
        Date,
        Time,
        Bool,
        Text,
        Image,
        Map,
        Aminate,
        PostCode,
        Binary,
        Table,
    }
}
