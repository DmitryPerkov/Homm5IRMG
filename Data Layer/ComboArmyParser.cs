using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Text.RegularExpressions;

namespace Homm5RMG
{
    public interface IHomm5XMLParser
    {
        bool IsParseOK { get; }
    }

    public enum MonsterTraits
    {
        Any = 0,
        Melee = 0x01,
        Shooter = 0x02,
        Caster = 0x04
    };


    /// <summary>
    /// combo army composition variant
    /// </summary>
    public class ComboArmyCompositionVariant : IHomm5XMLParser
    {
        const string pattern_ = @"(?<Type>[MSCA]){1}\s*\+|(?<Type>[MSCA]){1}\s*\((?<Chance>[^\)]+)\s*\)|^\((?<Chance>[^\)]+)\)|\s*(?<Name>[^\+MSCA\s]+)";

        private List<MonsterTraits> monsters_traits_ = new List<MonsterTraits>(); //monster types list
        private List<string> refs_ = new List<string>(); //composition references
        private int chance_ = 0; //chance to appear
        private bool parse_ok_ = true; //parse result

        public bool IsParseOK { get { return parse_ok_; } }
        public int Chance { get { return chance_; } }
        public List<MonsterTraits> Traits { get { return monsters_traits_; } }
        public List<string> References { get { return refs_; } }

        public ComboArmyCompositionVariant(string data)
        {
            try {
                parse(data);
            }
            catch (Exception e) {
                parse_ok_ = false;
            }
        }

        private void parse(string data)
        {
            Regex reg = new Regex(pattern_);
            MatchCollection mc = reg.Matches(data);
            foreach (Match m in mc) {
                if (m.Result("${Type}").Length > 0) { //monster traits
                    monsters_traits_.Add(EnumConvert.MonsterTraitsFromString(m.Result("${Type}")));
                }
                if (m.Result("${Chance}").Length > 0) { //chance for composition
                    try {
                        chance_ = int.Parse(m.Result("${Chance}"));
                    }
                    catch (Exception) {
                    }
                }
                if (m.Result("${Name}").Length > 0) { //composition reference
                    refs_.Add(m.Result("${Name}"));
                }
            }
        }

        
    }
    
    /// <summary>
    /// combo army composition
    /// </summary>
    public class ComboArmyComposition : IHomm5XMLParser
    {
        private List<ComboArmyCompositionVariant> variants_ = new List<ComboArmyCompositionVariant>(); //variants
        private int size_ = 0; //army size
        private string name_ = ""; //army name
        private bool parse_ok_ = true; //parse result

        public bool IsParseOK { get { return parse_ok_; } }
        public List<ComboArmyCompositionVariant> Variants { get { return variants_; } }
        public int Size { get { return size_; } }
        public string Name { get { return name_; } }
        
        public ComboArmyComposition(XmlNode node)
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
            size_ = int.Parse(node.Attributes["Size"].Value);
            name_ = node.Attributes["Name"].Value;
            foreach (string s in node.Attributes["Composition"].Value.Split(new char[] { ';' })) {
                ComboArmyCompositionVariant v = new ComboArmyCompositionVariant(s);
                if (v.IsParseOK) {
                    variants_.Add(v);
                }
            }
        }
    }

    /// <summary>
    /// xml section ComboArmyInfo parser
    /// </summary>
    public class ComboArmyParser : IHomm5XMLParser
    {
        private bool parse_ok_ = true; //parse result
        public bool IsParseOK { get { return parse_ok_; } }

        private List<ComboArmyComposition> compositions_ = new List<ComboArmyComposition>();
        public List<ComboArmyComposition> Compositions { get { return compositions_; } }

        public ComboArmyParser(XmlElement xml_data)
        {
            try {
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
                xml_data.ChildNodes.Count < 3 ||
                xml_data.ChildNodes[2].ChildNodes.Count == 0) {
                throw new Exception("ComboArmyInfo section not found");
            }
            else {
                foreach (XmlNode node in xml_data.ChildNodes[2].ChildNodes) {
                    ComboArmyComposition c = new ComboArmyComposition(node);
                    if (c.IsParseOK) {
                        compositions_.Add(c);
                    }
                }
            }
        }
    }
}
