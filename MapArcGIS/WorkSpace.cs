using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
namespace MapArcGIS
{
    public abstract class WorkSpace
    {
        internal struct Head
        {
            public Int32 dataOffset;
            public Int32 size;
        }

        internal WorkSpaceInfo workInfo;
        internal FileStream workSpaceFile;
        internal Int32 headOffset;
        internal Int32 pointsNumber, linesNumber, areasNumber;
        internal double[] aeraBound;
        internal List<Head> dataHeads;
        internal BinaryReader br;
        internal FeatureTable table;
        internal string fileName;
        public virtual void LoadData(WorkSpaceInfo wsi)
        {
            this.workInfo = wsi;
            gloLoadData(wsi.fileName);
        }

        private void gloLoadData(string fileName)
        {
            workSpaceFile = new FileStream(fileName, FileMode.Open);
            br = new BinaryReader(workSpaceFile);
            workSpaceFile.Seek(12, SeekOrigin.Begin);
            headOffset = br.ReadInt32();
            workSpaceFile.Seek(260, SeekOrigin.Begin);
            linesNumber = br.ReadInt32();
            pointsNumber = br.ReadInt32();
            areasNumber = br.ReadInt32();
            workSpaceFile.Seek(304, SeekOrigin.Begin);
            aeraBound = new double[4];//XMin,YMin,XMax,YMax
            for (Int32 i = 0; i < 4; i++)
            {
                aeraBound[i] = br.ReadDouble();
            }
            workSpaceFile.Seek(headOffset, SeekOrigin.Begin);
            dataHeads = new List<Head>();
            for (int i = 0; i < 11; i++)
            {
                Head head = new Head();
                head.dataOffset = br.ReadInt32();
                head.size = br.ReadInt32();
                br.ReadInt16();
                dataHeads.Add(head);
            }
            
        }
        public virtual void LoadData(string filePath)
        {
            gloLoadData(filePath);
        }
    }
}
