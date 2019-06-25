using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Web.UI;
using System.Text.RegularExpressions;
using System.Reflection;

namespace Homm5RMG.Testing
{
    public class XmlAreaReader
    {
        public XmlAreaReader()
        {
        }

        private const string pattern_ = "[^\\<]*\\<Object\\s+[^\\>]*Name=\"(?<name>[^\"]*)[^\\>]*Area=\"(?<area>[^\"]*)";
        private string data_ = ""; //xml content
        private string file_ = ""; //xml file name

        /// <summary>
        /// Read xml file
        /// </summary>
        /// <param name="file">file name</param>
        public TRESULT Read(string file)
        {
            if (file.Contains("\\")) {
                file_ = file;
            }
            else {
                file_ = Assembly.GetExecutingAssembly().CodeBase.Substring(0, Assembly.GetExecutingAssembly().CodeBase.Length - 12).Substring(8) + "Data/" + file;
            }
            if (!File.Exists(file_)) {
                return TRESULT.T_ERROR;
            }

            try {
                using (StreamReader reader = new StreamReader(file_, Encoding.GetEncoding("windows-1251"))) {
                    data_ = reader.ReadToEnd();
                }
            }
            catch (Exception e) {
                throw new Exception("xml reading error", e);
            }

            return TRESULT.T_OK;
        }

        /// <summary>
        /// Get list of areas
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="ado_params"></param>
        /// <returns>areas list</returns>
        public List<Pair> GetAreasList()
        {
            List<Pair> l = new List<Pair>();
            Regex reg = new Regex(pattern_);
            MatchCollection mc = reg.Matches(data_);
            foreach (Match m in mc) {
                l.Add(new Pair(m.Result("${name}"), m.Result("${area}")));
            }

            return l;
        }
    }
}