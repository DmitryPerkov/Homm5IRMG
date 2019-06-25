using System;
using System.Collections.Generic;
using System.Text;
using Homm5RMG.BL;
using System.Xml;
using Homm5RMG.Properties;

namespace Homm5RMG
{
    public enum RaceSide
    {
        Neutral = 0,
        Good,
        Evil
    }

    public enum ArmyAlignment
    {
        None = 0,
        Side,
        Town
    }

    /// <summary>
    /// Monster place holder class - defines reqirements for monster in army
    /// </summary>
    public class MonsterPlaceHolder
    {
        private Monster monster_ = null; //monster info
        private int amount_ = 0; //number of monsters
        private MonsterTraits traits_ = MonsterTraits.Any; //type of monster on this place in army

        public Monster Monster { get { return monster_; } set { monster_ = value; } }
        public int Amount { get { return amount_; } set { amount_ = value; } }
        public MonsterTraits Traits { get { return traits_; } set { traits_ = value; } }

        public MonsterPlaceHolder(MonsterTraits traits)
        {
            traits_ = traits;
        }
    }

    /// <summary>
    /// Army builder class
    /// </summary>
    public class ComboArmyBuilder
    {
        private ComboArmyParser ca_parser_ = null;
        private MonstersParser m_parser_ = null;
        private List<MonsterPlaceHolder> placeholders_ = new List<MonsterPlaceHolder>();
        private int size_ = 0; //army size
        private int power_ = 0; //army power
        //alignment data
        private eTownType current_town_ = eTownType.None;
        private RaceSide current_side_ = RaceSide.Neutral;
        private ArmyAlignment current_alignment_ = ArmyAlignment.None;
        private ArmyAlignment alignment_ = ArmyAlignment.None;
        private Monster weakest_monster_ = new Monster();

        private bool empty_ = true; //empty flag

        public int Size { get { return size_; } }
        public int Power { get { return power_; } set { power_ = value; } }
        public ArmyAlignment Alignment { get { return alignment_; } set { alignment_ = value; } }
        public ArmyAlignment CurrentAlignment { get { return current_alignment_; } }
        public Monster WeakestMonster { get { return weakest_monster_; } }
        public List<MonsterPlaceHolder> PlaceHolders { get { return placeholders_; } }
        public int RealSize {
            get {
                return placeholders_.FindAll(
                    delegate(MonsterPlaceHolder h) {
                        return h.Monster != null;
                    }
                ).Count;
            }
        }
        public int RealPower
        {
            get {
                int p = 0;
                placeholders_.ForEach(
                    delegate(MonsterPlaceHolder h) {
                        if (h.Monster != null) {
                            p += h.Monster.Power * h.Amount;
                        }
                    }
                );
                return p;
            }
        }

        public ComboArmyBuilder(ComboArmyParser ca_parser, MonstersParser m_parser, int size)
        {
            ca_parser_ = ca_parser;
            m_parser_ = m_parser;
            size_ = size;
            
            //define weakest monster
            weakest_monster_.Power = int.MaxValue;
            m_parser_.Monsters.ForEach(
                delegate(Monster m) {
                    if (m.Level == 1 && m.Power < weakest_monster_.Power ) {
                        weakest_monster_ = m;
                    }
                }
            );

            gen_placeholders(null); //generate placeholders for monsters in army
        }

        /// <summary>
        /// Add random monster to army
        /// </summary>
        public void AddRandomMonster(int base_level, int strength, bool last_attempt)
        {
            int rnd,amount = 0;
            bool fl_get_new_list_monsters;
            Monster monster = null;
            List<Monster> list_monsters = null;

            if (strength < weakest_monster_.Power * Guards.MIN_UNIT_AMOUNT) {
                return; //impossible to find so weak creature
            }

            foreach (MonsterPlaceHolder holder in placeholders_) {
                if (holder.Monster == null) {
                   //find random monster according to alignment and composition
                   fl_get_new_list_monsters = true;
                   do
                   {
                       // dwp. было зацикливание при выборе охраны на остатке силы. 
                       // так как уже достаточно велико минимальное ограничение количества юнитов.
                       // защитил алгоритм от зацикливания

                       if(fl_get_new_list_monsters) list_monsters = get_ALL_monster(base_level, holder);
                       if (list_monsters != null && list_monsters.Count > 0)
                        {
                            rnd = Randomizer.rnd.Next(list_monsters.Count);
                            monster = list_monsters[rnd];
                            amount = Math.Max(strength / monster.Power, 1);

                            if (amount > Guards.MAX_UNIT_AMOUNT) { //max limit
                                if (base_level++ < 7) {
                                    fl_get_new_list_monsters = true;
                                    continue; //too many monsters, level up
                                }
                                if (holder.Traits == MonsterTraits.Any)
                                {
                                    base_level = 7;
                                    list_monsters.RemoveAt(rnd);
                                    monster = null;
                                    fl_get_new_list_monsters = false;
                                    continue; //too many level 7 monsters, try other level 7 ones
                                }
                            }
                            if (amount < Guards.MIN_UNIT_AMOUNT) { //min limit
                                if (base_level-- > 1) {
                                    fl_get_new_list_monsters = true;
                                    continue; //too few monsters, level down
                                }
                                if (holder.Traits == MonsterTraits.Any) {
                                    base_level = 1;
                                    list_monsters.RemoveAt(rnd);
                                    monster = null;
                                    fl_get_new_list_monsters = false;
                                    continue; //too few level 1 monsters, try other level 1 ones
                                }
                            }
                        }
                        break;
                    } while (true);
                    if (monster != null) { //save monster on its position
                        holder.Monster = monster;
                        holder.Amount = amount;
                        if (empty_) { //save alignment info
                            current_town_ = monster.TownType;
                            current_side_ = monster.RaceSide;
                            //dwp использовать в охране только одну фракцию?
                            if (Settings.Default.SingleFactionGuards) current_alignment_ = ArmyAlignment.Town; //да
                            else 
                            current_alignment_ = alignment_; // нет
                            empty_ = false;
                        }
                        break;
                    }
                }
            }
            if (monster == null) { //monster not found -> change search data 
              //dwp. используется одна фракция и менять её нельзя
                if (!Settings.Default.SingleFactionGuards)
                {
                    if (current_alignment_ == ArmyAlignment.None)
                    {
                        int n = 0;
                        foreach (MonsterPlaceHolder h in placeholders_)
                        { //remove monster requirements
                            if (h.Monster == null && h.Traits != MonsterTraits.Any)
                            {
                                h.Traits = MonsterTraits.Any;
                                ++n;
                            }
                        }
                        if (n == 0)
                        { //thats bad(
                            throw new Exception("Critical error: impossible to generate army");
                        }
                        else
                        {
                            current_alignment_ = alignment_; //restore alignment
                            AddRandomMonster(base_level, strength, false); //and try again
                        }
                    }
                    else
                    {
                        current_alignment_--; //weaken alignment
                        AddRandomMonster(base_level, strength, false); //next try
                    }
                }
                //dwp. фракцию нельзя менять меняем ограничение по максимуму, чтобы получить нужную охрану.
                else if(!last_attempt)AddRandomMonster(base_level, strength, true);
            }
        }

        public void SaveGarrison(XmlNode xml_node)
        {
            foreach (MonsterPlaceHolder holder in placeholders_)
            {
                if (holder.Monster != null)
                {
                    xml_node.ChildNodes[0]["Creature"].InnerText = holder.Monster.Name;
                    xml_node.ChildNodes[0]["Count"].InnerText = holder.Amount.ToString();

                    // add next node
                    xml_node.AppendChild(xml_node.ChildNodes[0].Clone());
                }
            }
            if (xml_node != null) xml_node.RemoveChild(xml_node.ChildNodes[0]);
           
        }

        /// <summary>
        /// Save army
        /// </summary>
        public void Save(XmlNode xml_node)
        {
#if (DEBUG) // {
            if (RealPower <= weakest_monster_.Power * Guards.MIN_UNIT_AMOUNT) {
                throw new Exception("Critical error: strange army generated");
            }
#endif //DEBUG }
            XmlNode xml_additionals = null;
            foreach (MonsterPlaceHolder holder in placeholders_) {
                if (holder.Monster != null) {
                    if (xml_additionals == null) { //save first monster
                        xml_additionals = save_primary_monster(xml_node, holder);
                    }
                    else { //save additional monster
                        save_additional_monster(xml_additionals, holder);
                    }
                }
            }
            if (xml_additionals != null) {
                xml_additionals.RemoveChild(xml_additionals.ChildNodes[0]);
            }
        }

        /// <summary>
        /// Save first monster in an army
        /// </summary>
        /// <param name="xml_node"></param>
        /// <param name="holder"></param>
        /// <returns></returns>
        private XmlNode save_primary_monster(XmlNode xml_node, MonsterPlaceHolder holder)
        {
            StringBuilder href = new StringBuilder();
            if (holder.Monster.CreatureName == "") { //random monster
                href.AppendFormat("Random/Random-Monster-L{0}.(AdvMapMonsterShared).xdb", holder.Monster.Level);
            }
            else { //defined moster
                string ext = holder.Monster.CreatureName.Contains(".xdb") ? "" : ".(AdvMapMonsterShared).xdb";
                href.AppendFormat("{0}/{1}{2}", EnumConvert.HREFTownStringFromString(holder.Monster.Town), holder.Monster.CreatureName, ext);
            }
            XmlNode shared = ((XmlElement)xml_node).GetElementsByTagName("Shared")[0];
            ((XmlElement)shared).SetAttribute("href", shared.Attributes["href"].Value.Replace("Random/Random-Monster-L3.(AdvMapMonsterShared).xdb", href.ToString()));
            XmlNode amount = ((XmlElement)xml_node).GetElementsByTagName("Amount")[0];
            amount.InnerText = holder.Amount.ToString();
            return ((XmlElement)xml_node).GetElementsByTagName("AdditionalStacks")[0];
        }

        /// <summary>
        /// Save additional monster in an army
        /// </summary>
        private void save_additional_monster(XmlNode xml_additionals, MonsterPlaceHolder holder)
        {
            xml_additionals.ChildNodes[0]["Creature"].InnerText = holder.Monster.Name;
            xml_additionals.ChildNodes[0]["Amount"].InnerText = holder.Amount.ToString();
            // add next node
            xml_additionals.AppendChild(xml_additionals.ChildNodes[0].Clone());
        }

        /// <summary>
        /// Generate place holders for monsters in army
        /// </summary>
        private void gen_placeholders(ComboArmyComposition composition)
        {
            ComboArmyCompositionVariant v = get_random_composition_variant(composition);
            if (v != null) {
                foreach (string r in v.References) { //add traits for each reference
                    ComboArmyComposition comp = ca_parser_.Compositions.Find(
                        delegate(ComboArmyComposition c)
                        {
                            return c.Name == r;
                        }
                    );
                    gen_placeholders(comp);
                }
                foreach (MonsterTraits mt in v.Traits) { //add traits for variant
                    placeholders_.Add(new MonsterPlaceHolder(mt));
                }
            }
            if (composition == null && placeholders_.Count == 0) {
                placeholders_.Add(new MonsterPlaceHolder(MonsterTraits.Any));
            }
        }

        /// <summary>
        /// Get random composition variant
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        private ComboArmyCompositionVariant get_random_composition_variant(ComboArmyComposition composition)
        {
            if (composition == null) {
                composition = ca_parser_.Compositions.Find(
                    delegate(ComboArmyComposition c)
                    {
                        return c.Size == size_;
                    }
                );
            }
            if (composition == null || composition.Variants.Count == 0) {
                return null;
            }
            double[] chances = new double[composition.Variants.Count];
            int i = 0;
            foreach (ComboArmyCompositionVariant v in composition.Variants) {
                chances[i++] = v.Chance;
            }
            int rnd = Randomizer.Choice(chances, 100);
            return composition.Variants[rnd];
        }

        /// <summary>
        /// Get random monster
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        private Monster get_random_monster(int level, MonsterPlaceHolder holder)
        {
            List<Monster> list = m_parser_.Monsters.FindAll(
                delegate(Monster m) {
                    return m.Level == level && //level filter
                           (current_alignment_ != ArmyAlignment.Town ||
                            current_alignment_ == ArmyAlignment.Town && m.TownType == current_town_) && //race filter
                           (current_alignment_ != ArmyAlignment.Side ||
                            current_alignment_ == ArmyAlignment.Side && m.RaceSide == current_side_) && //side filter
                           (placeholders_.Count == 0 ||
                            holder.Traits == MonsterTraits.Any ||
                            (m.Traits & holder.Traits) != MonsterTraits.Any); //monster type filter
                }
            );
            if (list.Count == 0) {
                return null;
            }
            int rnd = Randomizer.rnd.Next(list.Count);
            return list[rnd];
        }
        // dwp. против зацикливание при выборе охраны на остатке силы. 
        // так как уже достаточно велико минимальное ограничение количества юнитов.
        private List<Monster> get_ALL_monster(int level, MonsterPlaceHolder holder)
        {
            List<Monster> list = m_parser_.Monsters.FindAll(
                delegate(Monster m)
                {
                    return m.Level == level && //level filter
                           (current_alignment_ != ArmyAlignment.Town ||
                            current_alignment_ == ArmyAlignment.Town && m.TownType == current_town_) && //race filter
                           (current_alignment_ != ArmyAlignment.Side ||
                            current_alignment_ == ArmyAlignment.Side && m.RaceSide == current_side_) && //side filter
                           (placeholders_.Count == 0 ||
                            holder.Traits == MonsterTraits.Any ||
                            (m.Traits & holder.Traits) != MonsterTraits.Any)
                            &&
                        //dwp !!! ограничение парсера по выбранным городам
                        (!Settings.Default.SelectedFactionGuards || (Settings.Default.SelectedFactionGuards && (
                          m.TownType == (eTownType)Settings.Default.FactionRed || m.TownType == (eTownType)Settings.Default.FactionBlue ||
                         (m.TownType == (eTownType)Settings.Default.FactionGreen && !Program.frmMainMenu.RadioButton1.Checked) ||
                         (m.TownType == (eTownType)Settings.Default.FactionYellow && (Program.frmMainMenu.RadioButton3.Checked || Program.frmMainMenu.RadioButton4.Checked))
                        ))) ; //monster type filter
                }
            );
            if (list.Count == 0)
            {
                return null;
            }
            return list;
        }


        /// <summary>
        /// Check data for errors
        /// </summary>
        private void check_data()
        {
            if (!ca_parser_.IsParseOK) {
                throw new Exception("ComboArmyParser initialization error");
            }
            if (!m_parser_.IsParseOK) {
                throw new Exception("MonstersParser initialization error");
            }
        }
    }
}
