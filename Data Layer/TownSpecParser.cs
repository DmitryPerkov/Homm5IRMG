using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Reflection;

namespace Homm5RMG
{
    public class TownSpecializations : IHomm5XMLParser
    {
        private bool parse_ok_ = true; //parse result
        public bool IsParseOK { get { return parse_ok_; } }

        private eTownType town_ = eTownType.None;
        private List<string> specializations_ = new List<string>();

        public eTownType Town { get { return town_; } }
        public List<string> Specializations { get { return specializations_; } }

        public TownSpecializations(XmlNode node)
        {
            try {
                parse(node);
            }
            catch (Exception e) {
                parse_ok_ = false; //parse error
            }
        }

        /// <summary>
        /// initialization
        /// </summary>
        private void parse(XmlNode node)
        {
            town_ = EnumConvert.TownTypeFromString(node.Name);
            foreach (XmlNode n in node.ChildNodes) {
                specializations_.Add(n.InnerText);
            }
        }
    }

    /// <summary>
    /// xml section TownSpecialization parser
    /// </summary>
    public class TownSpecParser : IHomm5XMLParser
    {
        private List<TownSpecializations> towns_ = new List<TownSpecializations>();
        public List<TownSpecializations> Towns { get { return towns_; } }

        private bool parse_ok_ = true; //parse result
        public bool IsParseOK { get { return parse_ok_; } }

        public TownSpecParser()
        {
            try {
                XmlDocument xdoc = new XmlDocument();
                xdoc.Load(Assembly.GetExecutingAssembly().CodeBase.Substring(0,Assembly.GetExecutingAssembly().CodeBase.Length - 12).Substring(8) + "Data/TownSpec.xml");
                parse(xdoc.DocumentElement);
            }
            catch (Exception e) {
                parse_ok_ = false; //parse error
            }
        }

        /// <summary>
        /// initialization
        /// </summary>
        private void parse(XmlElement xml_data)
        {
            if (xml_data == null || xml_data.ChildNodes.Count == 0) {
                throw new Exception("TownSpecialization section not found");
            }
            else {
                foreach (XmlNode node in xml_data.ChildNodes) {
                    TownSpecializations ts = new TownSpecializations(node);
                    if (ts.IsParseOK) {
                        towns_.Add(ts);
                    }
                }
            }
        }
    }
}
