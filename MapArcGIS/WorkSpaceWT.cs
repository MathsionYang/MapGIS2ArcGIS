using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
namespace MapArcGIS
{
    public class WorkSpaceWT : WorkSpace
    {
        /// <summary>
        /// 文件长度是经过压缩的，是实际长度的二分之一
        /// </summary>
        internal class PointData
        {
            public int ID;
            public short stringSum;
            public Int32 stringOffset;
            public double positionX;
            public double positionY;
            public byte pointPype;
            public byte transpresent;
            public short layer;
            public Int32 color;
        }
        internal class StringPointData : PointData
        {
            //public PointData pointData;
            public string stringData;
            public float charHeight;
            public float charWidth;
            public float charSpace;
            public float stringAngle;
            public short chinaFont;
            public short englishFont;
            public byte fontShape;
            public byte arragement;

        }
        internal class SubMapPointData : PointData
        {
            //public PointData pointData;
            public Int32 subNumber;
            public float subHeight;
            public float subWidth;
            public float subAngle;
            public float lineWidth;
            public float assColor;
        }
        internal class CriclePointData : PointData
        {
            //public PointData pointData;
            public float radius;
            public Int32 lineColor;
            public float lineWidth;
            public byte sign;//0是空心圆，1不是
        }
        internal class ArcPointData : PointData
        {
            //public PointData pointData;
            public double radius;
            public float startAngle;
            public float endAngle;
            public float lineWidth;

        }
        internal class ImagePointData : PointData
        {
            //public PointData pointData;
            public string imgaeFileName;
            public float charHeight;
            public float charWidth;
            public float stringAngle;
        }
        internal class TextPointData : PointData
        {
            //public PointData pointData;
            public string textData;
            public float charHeight;
            public float charWidth;
            public float charSpace;
            public float stringAngle;
            public short chinaFont;
            public short englishFont;
            public byte fontShape;
            public float verticalSpacing;
            public float sheetHeight;
            public float sheetWidth;
            public float sheetSpace;
            public byte arragement;
        }
        internal List<PointData> pointDatas;

        internal List<byte[]> pointFeatures;

        public override void LoadData(WorkSpaceInfo wsi)
        {
            base.LoadData(wsi);
            loadData();
        }
        private void loadData()
        {
            int pointsDataLength = dataHeads[0].dataOffset + dataHeads[0].size;
            workSpaceFile.Seek(dataHeads[0].dataOffset + 93, SeekOrigin.Begin);
            pointDatas = new List<PointData>();
            int id = 1;//mapgis里的属性表ID是从1开始的，0位置存放的为空
            while (workSpaceFile.Position <= pointsDataLength)
            {
                //if (br.ReadByte() != 1)
                //{
                //    return;
                //}

                id++;
                long temp = workSpaceFile.Position;

                workSpaceFile.Seek(30, SeekOrigin.Current);
                byte pointPype = br.ReadByte();
                workSpaceFile.Seek(temp + 1, SeekOrigin.Begin); //多个1
                long current;
                switch (pointPype)
                {
                    case 0:
                        StringPointData spd = new StringPointData();
                        spd.pointPype = pointPype;
                        spd.stringSum = br.ReadInt16();
                        spd.stringOffset = br.ReadInt32();
                        spd.positionX = br.ReadDouble();
                        spd.positionY = br.ReadDouble();
                        //byte[] pX = br.ReadBytes(8);
                        //byte[] pY = br.ReadBytes(8); 

                        //spd.positionX = BitConverter.ToDouble(pX,0);
                        //spd.positionY = BitConverter.ToDouble(pY, 0);
                        current = workSpaceFile.Position;
                        workSpaceFile.Seek(dataHeads[1].dataOffset + spd.stringOffset, SeekOrigin.Begin);
                        spd.stringData = System.Text.Encoding.Default.GetString(br.ReadBytes(spd.stringSum));
                        workSpaceFile.Seek(current, SeekOrigin.Begin);
                        spd.charHeight = br.ReadSingle();
                        spd.charWidth = br.ReadSingle();
                        spd.charSpace = br.ReadSingle();
                        spd.stringAngle = br.ReadSingle();
                        spd.chinaFont = br.ReadInt16();
                        spd.englishFont = br.ReadInt16();
                        spd.fontShape = br.ReadByte();
                        spd.arragement = br.ReadByte();
                        workSpaceFile.Seek(17, SeekOrigin.Current);
                        spd.transpresent = br.ReadByte();
                        spd.layer = br.ReadInt16();
                        spd.color = br.ReadInt32();
                        pointDatas.Add(spd);
                        workSpaceFile.Seek(temp, SeekOrigin.Begin);
                        workSpaceFile.Seek(93, SeekOrigin.Current);
                        continue;
                    case 1:
                        SubMapPointData smpd = new SubMapPointData();
                        //smpd = pointData;
                        smpd.pointPype = pointPype;
                        smpd.stringSum = br.ReadInt16();
                        smpd.stringOffset = br.ReadInt32();
                        smpd.positionX = br.ReadDouble();
                        smpd.positionY = br.ReadDouble();
                        smpd.subNumber = br.ReadInt32();
                        smpd.subHeight = br.ReadSingle();
                        smpd.subWidth = br.ReadSingle();
                        smpd.subAngle = br.ReadSingle();
                        smpd.lineWidth = br.ReadSingle();
                        smpd.assColor = br.ReadUInt32();
                        workSpaceFile.Seek(15, SeekOrigin.Current);
                        smpd.transpresent = br.ReadByte();
                        smpd.layer = br.ReadInt16();
                        smpd.color = br.ReadInt32();
                        pointDatas.Add(smpd);
                        workSpaceFile.Seek(14, SeekOrigin.Current);
                        continue;
                    case 2:
                        CriclePointData cpd = new CriclePointData();
                        cpd.pointPype = pointPype;
                        cpd.stringSum = br.ReadInt16();
                        cpd.stringOffset = br.ReadInt32();
                        cpd.positionX = br.ReadDouble();
                        cpd.positionY = br.ReadDouble();
                        cpd.radius = br.ReadSingle();
                        cpd.lineColor = br.ReadInt32();
                        cpd.lineWidth = br.ReadSingle();
                        cpd.sign = br.ReadByte();
                        workSpaceFile.Seek(24, SeekOrigin.Current);
                        cpd.transpresent = br.ReadByte();
                        cpd.layer = br.ReadInt16();
                        cpd.color = br.ReadInt32();
                        pointDatas.Add(cpd);
                        workSpaceFile.Seek(14, SeekOrigin.Current);
                        continue;
                    case 3:
                        ArcPointData apd = new ArcPointData();
                        apd.pointPype = pointPype;
                        apd.stringSum = br.ReadInt16();
                        apd.stringOffset = br.ReadInt32();
                        apd.positionX = br.ReadDouble();
                        apd.positionY = br.ReadDouble();
                        apd.radius = br.ReadDouble();
                        apd.startAngle = br.ReadSingle();
                        apd.endAngle = br.ReadSingle();
                        apd.lineWidth = br.ReadSingle();
                        workSpaceFile.Seek(21, SeekOrigin.Current);
                        apd.transpresent = br.ReadByte();
                        apd.layer = br.ReadInt16();
                        apd.color = br.ReadInt32();
                        pointDatas.Add(apd);
                        workSpaceFile.Seek(14, SeekOrigin.Current);
                        continue;
                    case 4:
                        ImagePointData ipd = new ImagePointData();
                        ipd.pointPype = pointPype;
                        ipd.stringSum = br.ReadInt16();
                        ipd.stringOffset = br.ReadInt32();
                        ipd.positionX = br.ReadDouble();
                        ipd.positionY = br.ReadDouble();
                        current = workSpaceFile.Position;
                        workSpaceFile.Seek(dataHeads[1].dataOffset + ipd.stringOffset - ipd.stringSum, SeekOrigin.Begin);
                        ipd.imgaeFileName = br.ReadString();
                        workSpaceFile.Seek(current, SeekOrigin.Begin);
                        ipd.charHeight = br.ReadSingle();
                        ipd.charWidth = br.ReadSingle();
                        ipd.stringAngle = br.ReadSingle();
                        workSpaceFile.Seek(29, SeekOrigin.Current);
                        ipd.transpresent = br.ReadByte();
                        ipd.layer = br.ReadInt16();
                        ipd.color = br.ReadInt32();
                        pointDatas.Add(ipd);
                        workSpaceFile.Seek(14, SeekOrigin.Current);

                        continue;
                    case 5:

                        TextPointData tpd = new TextPointData();
                        tpd.pointPype = pointPype;
                        tpd.stringSum = br.ReadInt16();
                        tpd.stringOffset = br.ReadInt32();
                        tpd.positionX = br.ReadDouble();
                        tpd.positionY = br.ReadDouble();
                        current = workSpaceFile.Position;
                        workSpaceFile.Seek(dataHeads[1].dataOffset + tpd.stringOffset - tpd.stringSum, SeekOrigin.Begin);
                        tpd.textData = br.ReadString();
                        workSpaceFile.Seek(current, SeekOrigin.Begin);
                        tpd.charHeight = br.ReadSingle();
                        tpd.charWidth = br.ReadSingle();
                        tpd.charSpace = br.ReadSingle();
                        tpd.stringAngle = br.ReadSingle();
                        tpd.chinaFont = br.ReadInt16();
                        tpd.englishFont = br.ReadInt16();
                        tpd.fontShape = br.ReadByte();
                        tpd.sheetSpace = br.ReadSingle();
                        tpd.sheetHeight = br.ReadSingle();
                        tpd.sheetWidth = br.ReadSingle();
                        tpd.arragement = br.ReadByte();
                        workSpaceFile.Seek(7, SeekOrigin.Current);
                        tpd.transpresent = br.ReadByte();
                        tpd.layer = br.ReadInt16();
                        tpd.color = br.ReadInt32();
                        pointDatas.Add(tpd);
                        workSpaceFile.Seek(14, SeekOrigin.Current);
                        continue;
                    default:
                        break;
                }
                pointFeatures = new List<byte[]>();
                workSpaceFile.Seek(dataHeads[2].dataOffset, SeekOrigin.Begin);
                byte[] pointFeature = br.ReadBytes(dataHeads[2].size);
                pointFeatures.Add(pointFeature);
                workSpaceFile.Seek(dataHeads[2].dataOffset + 0x0c, SeekOrigin.Begin);
                int tableItemOffset = br.ReadInt32();


                //br.Close();
            }
            workSpaceFile.Seek(dataHeads[2].dataOffset + 0x142, SeekOrigin.Begin);//140是表头文件的开始偏移位置强两位是标示符
            table = new FeatureTable();
            table.tableHeadNumber = br.ReadInt16();
            table.tableItemNumer = br.ReadInt32();
            table.tableItemLengthInBytes = br.ReadInt32();
            workSpaceFile.Seek(16, SeekOrigin.Current);
            table.tableHead = new List<FeatureTableHead>();
            for (int i = 0; i < table.tableHeadNumber; i++)
            {
                long tmp = workSpaceFile.Position;
                FeatureTableHead tableHead = new FeatureTableHead();
                //tableHead.headName = System.Text.Encoding.Default.GetString(br.ReadBytes(20)).Trim();//表单元名占20个字节
                tableHead.headNameBytes = br.ReadBytes(20);
                tableHead.headName = System.Text.Encoding.Default.GetString(tableHead.headNameBytes).Trim();//表单元名占20个字节
                //tableHead.headName = br.ReadBytes(20).Trim();//表单元名占20个字节
                tableHead.itemType = (FeatureType)br.ReadByte();
                tableHead.offset = br.ReadInt32();
                tableHead.lengthInBytes = br.ReadInt16();
                tableHead.tableItemCharLength = br.ReadInt16();
                table.tableHead.Add(tableHead);
                workSpaceFile.Seek(10, SeekOrigin.Current);
            }
            //workSpaceFile.Seek(table.tableItemLengthInBytes, SeekOrigin.Current);
            //workSpaceFile.Seek(-1, SeekOrigin.Current);
            table.tableItem = new List<byte[]>();
            for (int i = 0; i < table.tableItemNumer; i++)
            {
                //br.ReadByte();
                byte[] data = br.ReadBytes(table.tableItemLengthInBytes);
                //table.tableItem.Add(br.ReadBytes(table.tableItemLengthInBytes)); // 
                int count = 0;
                foreach (var item in table.tableHead)
                {
                    count += item.tableItemCharLength;
                }
                byte[] newData = new byte[count];
                int flag = 0;
                foreach (var item in table.tableHead)
                {
                    //table.tableItem.Add(SubArray(data,item.offset,item.tableItemCharLength));
                    AddToArray(data,ref newData, item.offset, item.tableItemCharLength,ref flag);
                }
                table.tableItem.Add(newData);
            }
            //for (int i = 1; i < table.tableItemNumer; i++)
            //{
            //    ProcessTableItem(table.tableItem[i], table.tableHead);

            //}
        }

        private void AddToArray(byte[] data,ref byte[] newData, int offset, short length,ref int flag)
        {
            for (int i = 0; i < length; i++)
            {
                newData[flag + i] = data[offset + i];
            }
            flag += length;
        }

        private byte[] SubArray(byte[] data, int offset, short length)
        {
            byte[] result = new byte[length];
            for (int i = 0; i < length; i++)
            {
                result[i] = data[offset + i];
            }
            return result;
        }

        private void ProcessTableItem(byte[] featureData, List<FeatureTableHead> head)
        {

            using (FileStream fs = new FileStream(@"data/data.dat", FileMode.Append))
            {
                BinaryWriter ws = new BinaryWriter(fs);
                foreach (var item in head)
                {
                    char[] temp = item.headName.ToCharArray();
                    ws.Write(item.headName.Trim().ToCharArray());
                    //ws.Seek(item.offset, SeekOrigin.Begin);
                    ws.Write(featureData, item.offset, item.lengthInBytes);
                }
            }
        }
        public void PrintFeatureTable()
        {
            FileStream fs = new FileStream(fileName + ".table", FileMode.OpenOrCreate);
            BinaryWriter ws = new BinaryWriter(fs);

            foreach (var item in pointFeatures)
            {
                ws.Write(item);
            }
            ws.Close();
            //FileStream fs = new FileStream(workInfo.fileName + ".txt", FileMode.OpenOrCreate);
            //BinaryWriter ws = new BinaryWriter(fs);

            //foreach (var item in table.tableItem)
            //{
            //    ws.Write(item);
            //}
            //ws.Close();

        }
        public void LoadDataFromFile(string fileName)
        {
            base.LoadData(fileName);
            this.fileName = fileName;
            loadData();
        }
        public void ConvertToShapeFileAndIndexFile()
        {
            FileStream shapeFile = new FileStream(fileName + ".shp", FileMode.OpenOrCreate);
            BinaryWriter shpWR = new BinaryWriter(shapeFile);
            FileStream idxFile = new FileStream(fileName + ".shx", FileMode.OpenOrCreate);
            BinaryWriter idxWR = new BinaryWriter(idxFile);
            FileStream dbfFile = new FileStream(fileName + ".dbf", FileMode.OpenOrCreate);
            BinaryWriter dbfWR = new BinaryWriter(dbfFile);
            ////////////////////////////shp文件的头文件信息////////////////////////////////
            shpWR.Write(170328064);
            for (int i = 0; i < 6; i++)
            {
                shpWR.Write(0);
            }
            shpWR.Write(1000);
            shpWR.Write(1);
            for (int i = 0; i < 4; i++)
            {
                shpWR.Write(this.aeraBound[i]);
            }
            for (int i = 0; i < 8; i++)
            {
                shpWR.Write(0);
            }
            ///////////////////////////shx文件的头文件信息/////////////////////////////////
            idxWR.Write(170328064);
            for (int i = 0; i < 6; i++)
            {
                idxWR.Write(0);
            }
            idxWR.Write(1000);
            idxWR.Write(1);
            for (int i = 0; i < 4; i++)
            {
                idxWR.Write(this.aeraBound[i]);
            }
            for (int i = 0; i < 8; i++)
            {
                idxWR.Write(0);
            }
            ////////////////////////////shp文件的图形信息/////////////////////////////////
            for (int i = 1; i < this.pointsNumber; i++)
            {
                idxWR.Write(ReverseArray(BitConverter.GetBytes((int)shapeFile.Position / 2)));
                idxWR.Write(ReverseArray(BitConverter.GetBytes(10)));
                shpWR.Write(ReverseArray(BitConverter.GetBytes(i)));
                shpWR.Write(ReverseArray(BitConverter.GetBytes(10)));
                shpWR.Write(1);
                shpWR.Write(this.pointDatas[i - 1].positionX);
                shpWR.Write(this.pointDatas[i - 1].positionY);
            }
            shapeFile.Seek(24, SeekOrigin.Begin);
            shpWR.Write(ReverseArray(BitConverter.GetBytes((int)shapeFile.Length / 2)));
            idxFile.Seek(24, SeekOrigin.Begin);
            idxWR.Write(ReverseArray(BitConverter.GetBytes((int)idxFile.Length / 2)));
            shpWR.Close();
            shpWR.Dispose();

            shapeFile.Close();
            shapeFile.Dispose();
            idxWR.Close();
            idxWR.Dispose();
            idxFile.Close();
            idxFile.Dispose();
            ////////////////////////dbf文件的属性信息//////////////////////////////////
            dbfWR.Write((byte)0x03); //版本号
            dbfWR.Write((byte)(DateTime.Today.Year % 2000)); //表示最近的更新日期，按照 YYMMDD 格式。
            dbfWR.Write((byte)(DateTime.Today.Month));
            dbfWR.Write((byte)(DateTime.Today.Day));
            dbfWR.Write(table.tableItemNumer - 1); //文件中的记录条数
            dbfWR.Write((short)(table.tableHeadNumber * 32 + 33));//头文件字节数
            int count = 0;
            foreach (var item in table.tableHead)
            {
                count += item.tableItemCharLength;
            }
            dbfWR.Write((short)(count+1));//一条记录中的字节长度。去掉占位符
            dbfWR.Write(0);
            dbfWR.Write(0);
            dbfWR.Write(0);
            dbfWR.Write(0);
            dbfWR.Write(0);//这儿要加东西
            //UnicodeEncoding ae = new UnicodeEncoding();
            foreach (var item in table.tableHead)
            {
                //Encoding.ASCII.

                //dbfWR.Write(ae.GetBytes(item.headName), 0, 11);
                //ae.GetString
                //string s = ASCIIEncoding.GetEncoding("GB2312").GetString(Encoding.UTF8.GetBytes(item.headName));
                //byte[] test = Encoding.Default.GetBytes(item.headName);
                dbfWR.Write(Encoding.Default.GetBytes(item.headName), 0, 11);

                char type = ConvertType(item.itemType);
                dbfWR.Write(type);

                dbfWR.Write(0);//填充0
                dbfWR.Write((byte)item.tableItemCharLength);//长度
                dbfWR.Write(GetItemDemical(item.itemType));
                dbfWR.Write(new byte[14]);//补0
            }
            dbfWR.Write((byte)0x0d);
            for (int i = 1; i < table.tableItem.Count; i++)
            {
                dbfWR.Write('0');
                dbfFile.Write(table.tableItem[i], 0, table.tableItem[i].Length);
            }
            dbfWR.Write((short)6688);
            dbfWR.Close();
            dbfWR.Dispose();
        }

        private byte GetItemDemical(FeatureType featureType)
        {
            if (featureType == FeatureType.Double)
            {
                return (byte)11;
            }
            else if (featureType == FeatureType.Float)
            {
                return (byte)6;
            }
            else
            {
                return (byte)0;
            }
        }

        private byte GetItemTypeLength(char type)
        {

            if (type == 'N')
            {
                return (byte)16;
            }
            if (type == 'C' || type == 'B' || type == 'D')
            {
                return (byte)8;
            }
            if (type == 'L')
            {
                return (byte)1;
            }
            return (byte)0;
        }

        private char ConvertType(FeatureType featureType)
        {

            if (featureType == FeatureType.String || featureType == FeatureType.Text || featureType == FeatureType.Time || featureType == FeatureType.Date)
            {
                return 'C';
            }
            if (featureType == FeatureType.Image || featureType == FeatureType.Map || featureType == FeatureType.Aminate || featureType == FeatureType.PostCode || featureType == FeatureType.Table)
            {
                return 'B';
            }
            if (featureType==FeatureType.Float || featureType==FeatureType.Double || featureType==FeatureType.Short || featureType==FeatureType.Int)
            {
                return 'N';

            }
            else
            {
                return 'L';
            }


        }
        public void ConverToDataFIle()
        {
            FileStream dbfFile = new FileStream(fileName + ".dbf", FileMode.OpenOrCreate);
            BinaryWriter dbfWR = new BinaryWriter(dbfFile);

        }
        public byte[] ReverseArray(byte[] value)
        {
            byte[] result;
            //temp = BitConverter.GetBytes(Convert.ToDouble(value));
            result = new byte[value.Length];
            for (int i = 0; i < value.Length; i++)
            {
                result[i] = value[value.Length - i - 1];
            }
            return result;
        }
        //public void 
    }
}
