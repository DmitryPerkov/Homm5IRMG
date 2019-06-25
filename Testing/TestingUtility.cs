using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Text.RegularExpressions;

namespace Homm5RMG.Testing
{
    public enum TRESULT { T_OK, T_ERROR };

    public static class TestingUtility
    {
        public static void ShowErrorMessage(string msg)
        {
            System.Windows.Forms.MessageBox.Show("ERROR. " + msg);
        }

        /// <summary>
        /// Test objects areas
        /// </summary>
        public static void TestObjectsArea()
        {
            List<Pair> areas = new List<Pair>();
            areas.AddRange(test_xml("Blocks.xml"));
            areas.AddRange(test_xml("Mines.xml"));
            areas.AddRange(test_xml("Objects.xml"));
            areas.AddRange(test_xml("OtherObjects.xml"));


            dump_areas(areas);
        }

        /// <summary>
        /// Test objects areas
        /// </summary>
        private static List<Pair> test_xml(string xml_file)
        {
            XmlAreaReader reader = new XmlAreaReader();
            if (reader.Read(xml_file) == TRESULT.T_ERROR)
                return null;
            List<Pair> areas = new List<Pair>();
            areas = reader.GetAreasList();
            return areas;
        }

        /// <summary>
        /// dump areas to file
        /// </summary>
        /// <param name="areas">areas list</param>
        private static void dump_areas(List<Pair> areas)
        {
            try {
                System.IO.StreamWriter s = System.IO.File.CreateText("Areas.txt");
                for (int i = 0; i < areas.Count; ++i) {
                    s.WriteLine("name=\"" + areas[i].First + "\"; area=\"" + areas[i].Second + "\"");
                    s.WriteLine(generate_area_string_image(areas[i].Second.ToString()));
                }
                s.Flush();
                s.Close();
                s.Dispose();
            }
            catch (Exception e) {
                throw new Exception("error writing areas file", e);
            }
        }

        /// <summary>
        /// Generate string image for dump file
        /// </summary>
        /// <param name="area"></param>
        /// <returns></returns>
        private static string generate_area_string_image(string area)
        {
            int[,] matrix = area_string_to_matrix(area);
            string image = matrix_to_image(matrix);

            return image;
        }

        /// <summary>
        /// Convert area string to matrix
        /// </summary>
        /// <param name="area">area string</param>
        /// <returns>matrix</returns>
        private static int[,] area_string_to_matrix(string area)
        {
            const string pattern_ = @"(?<x>-?\d+),(?<y>-?\d+)(;|$)";
            int x_min = 0, x_max = 0, y_min = 0, y_max = 0;
            List<Pair> temp = new List<Pair>();
            try {
                Regex reg = new Regex(pattern_);
                MatchCollection mc = reg.Matches(area);
                foreach (Match m in mc) {
                    int x = Convert.ToInt32(m.Result("${x}"));
                    int y = Convert.ToInt32(m.Result("${y}"));
                    x_min = x < x_min ? x : x_min;
                    x_max = x > x_max ? x : x_max;
                    y_min = y < y_min ? y : y_min;
                    y_max = y > y_max ? y : y_max;
                    temp.Add(new Pair(x, y));
                }
            }
            catch (Exception e) {
                throw new Exception("wrong area string", e);
            }
            int[,] matrix = new int[x_max - x_min + 1, y_max - y_min + 1];

            matrix[0 - x_min, 0 - y_min] = 1; //base point
            for (int i = 0; i < temp.Count; ++i) {
                matrix[(int)temp[i].First - x_min, (int)temp[i].Second - y_min] = 1;
            }
            
            return matrix;
        }

        private static string matrix_to_image(int[,] matrix)
        {
            string image = "";
            for (int y = matrix.GetLength(1) - 1; y >= 0; --y) {
                for (int x = 0; x < matrix.GetLength(0); ++x) {
                    image += (matrix[x, y] == 1) ? "X" : " ";
                }
                image += "\r\n";
            }
            return image;
        }
    }


}
