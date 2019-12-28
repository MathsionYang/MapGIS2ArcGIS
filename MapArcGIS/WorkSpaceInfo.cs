using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MapArcGIS
{
    public struct WorkSpaceInfo
    {
        public byte fileType;
        public byte fileState;
        public string fileName;
        public string fileDes;
        public double[] aeraBound;
        public byte userType;
        public byte groupCode;
        public string internetDataSource;
        public double minDisplayLevel;
        public double maxDisplayLevel;
        public byte dynamicSign;
        public string signName;
        public float signHeight;
        public short signColor;
        public byte signFont;
        public byte caseInfo;
    }
}
