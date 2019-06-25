using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Homm5RMG
{
    /// <summary>
    /// Town garrison class
    /// </summary>
    public class TownGarrison : IHomm5XMLParser
    {
        private bool parse_ok_ = true; //parse result
        public bool IsParseOK { get { return parse_ok_; } }

        private XmlNode node_ = null;
        public XmlNode Node { get { return node_; } }

        private List<MonsterPlaceHolder> monsters_ = new List<MonsterPlaceHolder>();
        public List<MonsterPlaceHolder> Monsters { get { return monsters_; } }

        public TownGarrison(XmlNode node)
        {
            try {
                parse(node);
            }
            catch (Exception e) {
                parse_ok_ = false; //parse error
            }
        }

        private void parse(XmlNode node)
        {
            XmlNodeList items = node.SelectNodes(".//Item");
            foreach (XmlNode n in items) {
                XmlNode name = n.SelectSingleNode(".//Creature");
                XmlNode count = n.SelectSingleNode(".//Count");
                MonsterPlaceHolder h = new MonsterPlaceHolder(MonsterTraits.Any);
                h.Monster = new Monster();
                h.Monster.Name = name.InnerText;
                h.Amount = int.Parse(count.InnerText);
                monsters_.Add(h);
            }
            node_ = node;
        }
    }

    /// <summary>
    /// xml section TownGarrison parser
    /// </summary>
    public class TownGarrisonParser : IHomm5XMLParser
    {
        private MapSize map_size_ = MapSize.Tiny;
        private bool parse_ok_ = true; //parse result
        public bool IsParseOK { get { return parse_ok_; } }

        private List<TownGarrison> variants_ = new List<TownGarrison>();
        public List<TownGarrison> TownGarrisonVariants { get { return variants_; } }

        public TownGarrisonParser(XmlElement xml_data, MapSize map_size)
        {
            try {
                map_size_ = map_size;
                parse(xml_data);
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
            if (xml_data == null ||
                xml_data.ChildNodes.Count < 4 ||
                xml_data.ChildNodes[3].ChildNodes.Count == 0) {
                throw new Exception("TownGarrison section not found");
            }
            else {
                foreach (XmlNode node in xml_data.ChildNodes[3].ChildNodes) {
                    if (EnumConvert.MapSizeFromString(node.Attributes["Size"].Value) == map_size_) {
                        foreach (XmlNode n in node.ChildNodes) {
                            variants_.Add(new TownGarrison(n));
                        }
                        break;
                    }
                }
            }
        }

    }
}
