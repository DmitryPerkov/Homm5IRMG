using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Homm5RMG
{
    public enum YesNo
    {
        Yes = 1,
        No
    }

    /// <summary>
    /// Monster class
    /// </summary>
    public class Monster : IHomm5XMLParser
    {
        private string name_ = ""; //additional stack name
        private int power_ = 0; //power
        private string town_ = ""; //town
        private string creature_name_ = ""; //primary stack name
        private bool shooter_ = false; //shooter?
        private bool caster_ = false; //caster?
        private int level_ = 0; //level
        private bool parse_ok_ = true; //parse result

        public string Name { get { return name_; } set { name_ = value; } }
        public int Power { get { return power_; } set { power_ = value; } }
        public string Town { get { return town_; } }
        public eTownType TownType { get { return EnumConvert.TownTypeFromString(town_); } }
        public RaceSide RaceSide { get { return EnumConvert.RaceSideFromTownType(TownType); } }

        public string CreatureName { get { return creature_name_; } }
        public bool IsShooter { get { return shooter_; } }
        public bool IsCaster { get { return caster_; } }
        public int Level { get { return level_; } }
        public bool IsParseOK { get { return parse_ok_; } }
        public MonsterTraits Traits {
            get {
                if (shooter_ && caster_) {
                    return MonsterTraits.Shooter | MonsterTraits.Caster;
                }
                if (shooter_) {
                    return MonsterTraits.Shooter;
                }
                if (caster_) {
                    return MonsterTraits.Caster;
                }
                return MonsterTraits.Melee;
            }
        }

        public Monster()
        {
        }

        public Monster(XmlNode node)
        {
            try {
                parse(node);
            }
            catch (Exception e) {
                parse_ok_ = false;
            }
        }

        private void parse(XmlNode node)
        {
            name_ = node.Attributes["Name"].Value;
            power_ = int.Parse(node.Attributes["Power"].Value);
            town_ = node.Attributes["Town"].Value;
            creature_name_ = node.Attributes["CreatureName"].Value;
            shooter_ = (YesNo)Enum.Parse(typeof(YesNo), node.Attributes["Shooter"].Value) == YesNo.Yes;
            caster_ = (YesNo)Enum.Parse(typeof(YesNo), node.Attributes["Caster"].Value) == YesNo.Yes;
            level_ = int.Parse(node.ParentNode.Name.Substring(5)); //0-6
        }
    }

    /// <summary>
    /// xml section MonstersInfo parser
    /// </summary>
    public class MonstersParser
    {
        private bool parse_ok_ = true; //parse result
        public bool IsParseOK { get { return parse_ok_; } }

        private List<Monster> monsters_ = new List<Monster>();
        public List<Monster> Monsters { get { return monsters_; } }

        public MonstersParser(XmlElement xml_data)
        {
            try {
                parse(xml_data);
            }
            catch (Exception e) {
                parse_ok_ = false; //parse error
            }
        }

        private void parse(XmlElement xml_data)
        {
            if (xml_data == null ||
                xml_data.ChildNodes.Count < 2 ||
                xml_data.ChildNodes[1].ChildNodes.Count == 0) {
                throw new Exception("MonstersInfo section not found");
            }
            else {
                foreach (XmlNode lvl_node in xml_data.ChildNodes[1].ChildNodes) {
                    foreach (XmlNode monster_node in lvl_node.ChildNodes) {
                        Monster m = new Monster(monster_node);
                        if (m.IsParseOK) {
                            monsters_.Add(m);
                        }
                    }
                }
            }
        }
    }
}
