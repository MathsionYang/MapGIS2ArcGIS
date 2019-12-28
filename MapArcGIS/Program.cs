using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MapArcGIS
{
    class Program
    {
        static void Main(string[] args)
        {
            //MapGIS test = new MapGIS("250地质图.MPJ");
            WorkSpaceWT test = new WorkSpaceWT();
            test.LoadDataFromFile(@"wt shape\G51symbol.wt");
            test.PrintFeatureTable();
            test.ConvertToShapeFileAndIndexFile();
            //test.ConvertToShapeFile();
            Console.ReadKey();
        }
    }
}
