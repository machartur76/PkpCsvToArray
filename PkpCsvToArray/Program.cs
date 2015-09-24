using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PkpCsvToArray
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Point> points = new List<Point>();

            using (var sw = new StreamWriter(@"d:\aaa.sql",false, Encoding.UTF8))
            {
                sw.AutoFlush = true;

                using (var sr = new StreamReader(@"d:\aaa.csv", Encoding.UTF8))
                {
                    string buffer = sr.ReadLine();
                    while (buffer != null)
                    {
                        var columns = buffer.Split(',');

                        var point = new Point();

                        for (int i = 0; i < columns.Length; i++)
                        {
                            switch (i)
                            {
                                case 0:
                                    point.Lon = Convert.ToDecimal(columns[i], CultureInfo.InvariantCulture);
                                    break;
                                case 1:
                                    point.Lat = Convert.ToDecimal(columns[i], CultureInfo.InvariantCulture);
                                    break;
                                case 2:
                                    point.Index = Convert.ToInt32(columns[i]);
                                    break;
                                case 3:
                                    point.FromStation = columns[i];
                                    break;
                                case 4:
                                    point.ToStation = columns[i];
                                    break;
                            }
                        }

                        points.Add(point);
                        buffer = sr.ReadLine();
                    }


                }

                var groupIndex = points.GroupBy(t => t.Index);

                int index = 1;
                foreach (var gr in groupIndex)
                {
                    int lp = 1;
                    foreach (var pt in gr)
                    {
                        pt.Lp = lp++;
                    }

                    string row = ToRow(gr.ToList());
                    sw.WriteLine("insert into trasy (index,from_station,to_station,point_count,point_array) VALUES ({0},'{1}','{2}',{3},'{4}'", index++, gr.First().FromStation, gr.First().ToStation, gr.Count(), row);

                    row = ToRow(gr.OrderByDescending(t => t.Lp).ToList());
                    sw.WriteLine("insert into trasy (index,from_station,to_station,point_count,point_array) VALUES ({0},'{1}','{2}',{3},'{4}'", index++, gr.First().ToStation, gr.First().FromStation, gr.Count(), row);
                }

            }

        }

        private static string ToRow(List<Point> gr)
        {
            List<string> pointList = new List<string>();
            gr.ForEach(t => pointList.Add(string.Format("[{1},{0}]", t.Lon.ToString("0.000000",CultureInfo.InvariantCulture), t.Lat.ToString("0.000000",CultureInfo.InvariantCulture))));

            return string.Join(",", pointList);
        }
    }
}
