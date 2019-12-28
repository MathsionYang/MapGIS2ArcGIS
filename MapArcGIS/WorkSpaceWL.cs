                                                                                                using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MapArcGIS
{
    public class WorkSpaceWL : WorkSpace
    {   
        private struct LineData
        {
            public int lineSum;
            public int positionOffset;
            public short lineType;
            public byte assLineType;
            public byte overrideType;
            public int lineColorNumber;
            public float lineWidth;
            public byte lineKind;
            public float argX;
            public float argY;
            public int assColor;
            public int layer;
        }
        private struct LinePosition
        {
            public float X;
            public float Y;
        }
        private struct NodeData
        {

        }
        public override void LoadData(WorkSpaceInfo wsi)
        {
            base.LoadData(wsi);

        }
    }
}
