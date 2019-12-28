using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
namespace MapArcGIS
{
    public class MapGIS
    {
        string mapGisProjectFilePath;
        FileStream mapGISFile;
        Int32 numberOfFile;
        double[] aeraBound = new double[4];
        string fileTitle;
        int layerDicPosition;
        Int32 firstFileOffset;
        double[] args = new double[5];//位移X,位移Y,比例X,比例Y,旋转角度
        double width;
        double height;
        Int32 prjType;
        List<WorkSpace> workSpaces;
        List<WorkSpaceInfo> workInfos;
        public MapGIS(string filePath)
        {
            CreateWorkInfos(filePath);
            //workInfos = new List<WorkSpaceInfo>();
            CreateWorkSpaces();
        }

        private void CreateWorkSpaces()
        {
            workSpaces = new List<WorkSpace>();
            foreach (var workInfo in workInfos)
            {
                string fileFlag = workInfo.fileName.Split('.')[1];
                switch (fileFlag)
                {
                    case "WT":
                        WorkSpaceWT wt = new WorkSpaceWT();
                        wt.LoadData(workInfo);
                        
                        workSpaces.Add(wt);
                        wt.PrintFeatureTable();
                        break;
                    case "WL":
                        WorkSpace wl = new WorkSpaceWL();
                        break;
                    case "WP":
                        break;
                    default:
                        break;
                }
            }
        }

        private void CreateWorkInfos(string filePath)
        {
            mapGisProjectFilePath = filePath;
            mapGISFile = new FileStream(filePath, FileMode.Open);
            BinaryReader br = new BinaryReader(mapGISFile);

            mapGISFile.Seek(12, SeekOrigin.Begin);
            numberOfFile = br.ReadInt16();
            for (Int32 i = 0; i < 4; i++)
            {
                aeraBound[i] = br.ReadDouble();
            }
            mapGISFile.Seek(686, SeekOrigin.Begin);
            fileTitle = System.Text.Encoding.Default.GetString(br.ReadBytes(60));
            layerDicPosition = br.ReadInt32();
            firstFileOffset = br.ReadInt32();
            for (Int32 i = 0; i < 5; i++)
            {
                args[i] = br.ReadDouble();
            }
            width = br.ReadDouble();
            height = br.ReadDouble();
            prjType = br.ReadInt16();
            mapGISFile.Seek(1113, SeekOrigin.Begin);
            workInfos = new List<WorkSpaceInfo>();
            for (Int32 i = 0; i < numberOfFile; i++)
            {
                WorkSpaceInfo wsi = new WorkSpaceInfo();
                string temp1;
                wsi.fileType = br.ReadByte();
                wsi.fileState = br.ReadByte();

                temp1 = Encoding.GetEncoding("gb2312").GetString(br.ReadBytes(128)).Trim();
                Int32 end = temp1.LastIndexOf('.');
                Int32 start = temp1.IndexOf('\\');
                //Int32 star = temp1.
                //StringBuilder sb = new StringBuilder(temp1);
                temp1 = temp1.Substring(2, end + 4);
                //wsi.fileName = temp1.Substring(0,end+4);
                wsi.fileName = temp1.Split('\0')[0];
                //wsi.fileName = Encoding.GetEncoding("gb2312").GetString(br.ReadBytes(128)).Substring();// br.ReadBytes(128);
                //Console.WriteLine(wsi.fileName);
                wsi.fileDes = Encoding.GetEncoding("gb2312").GetString(br.ReadBytes(128)).Trim();
                //wsi.fileDes = Encoding.GetEncoding("gb2312").GetString(br.ReadBytes(128)).Trim();// br.ReadBytes(128);
                //string stringName = 
                wsi.aeraBound = new double[4];
                for (Int32 j = 0; j < 4; j++)
                {
                    wsi.aeraBound[j] = br.ReadDouble();
                }
                wsi.userType = br.ReadByte();
                wsi.groupCode = br.ReadByte();
                wsi.internetDataSource = Encoding.GetEncoding("gb2312").GetString(br.ReadBytes(32)).Trim();

                //wsi.Int32ernetDataSource = Encoding.GetEncoding("gb2312").GetString(br.ReadBytes(32)).Trim();
                wsi.minDisplayLevel = br.ReadDouble();
                wsi.maxDisplayLevel = br.ReadDouble();
                wsi.dynamicSign = br.ReadByte();
                wsi.signName = Encoding.GetEncoding("gb2312").GetString(br.ReadBytes(21)).Trim();
                // = Encoding.GetEncoding("gb2312").GetString(br.ReadBytes(21)).Trim();
                wsi.signHeight = br.ReadSingle();
                wsi.signFont = br.ReadByte();
                wsi.caseInfo = br.ReadByte();
                mapGISFile.Seek(32, SeekOrigin.Current);
                workInfos.Add(wsi);
            }
        }
    }
}
