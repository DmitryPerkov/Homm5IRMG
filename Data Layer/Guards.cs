using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Homm5RMG.BL;
using System.Reflection;
using Homm5RMG.Testing;
using Homm5RMG.Properties;

namespace Homm5RMG
{

    enum eGuardTypes
    {
        GuardsInsideGate = 0,
        GuardsAndRandomHero ,
        NormalValueMode ,
        Custom
    }

    enum eCourage
    {
        MONSTER_COURAGE_ALWAYS_JOIN = 0 ,
        MONSTER_COURAGE_ALWAYS_FIGHT ,
        MONSTER_COURAGE_CAN_FLEE_JOIN
    }

    enum eMood
    {
        MONSTER_MOOD_FRIENDLY = 0,
        MONSTER_MOOD_AGGRESSIVE ,
        MONSTER_MOOD_HOSTILE ,
        MONSTER_MOOD_WILD
    }

    class Guards
    {
        private string GUARDSSOURCEFILENAME = Assembly.GetExecutingAssembly().CodeBase.Substring(0,Assembly.GetExecutingAssembly().CodeBase.Length - 12).Substring(8) + "Data/Monsters.xml";
        public XmlDocument xdocGuards;
        public static string strAligment;

        private ComboArmyParser ca_parser_ = null; //xml ComboArmyInfo section parser
        public ComboArmyParser CAParser { get { return ca_parser_; } }

        private TownSpecParser ts_parser_ = null; //xml ComboArmyInfo section parser
        public TownSpecParser TSParser { get { return ts_parser_; } }
#if (DEBUG) // {
        private ComboArmyTester ca_tester_ = new ComboArmyTester();
        public void ShowComboArmyResults()
        {
            ca_tester_.ShowResults();
        }
#endif // DEBUG }

        private MonstersParser m_parser_ = null; //xml section MonstersInfo parser
        public MonstersParser MParser { get { return m_parser_; } }

        private TownGarrisonParser tg_parser_ = null; //xml section TownGarrison parser
        public TownGarrisonParser TGParser { get { return tg_parser_; } }

        public Guards()
        {
            xdocGuards = new XmlDocument();
            xdocGuards.Load(GUARDSSOURCEFILENAME);
            ca_parser_ = new ComboArmyParser(xdocGuards.DocumentElement);
            m_parser_ = new MonstersParser(xdocGuards.DocumentElement);
            tg_parser_ = new TownGarrisonParser(xdocGuards.DocumentElement,
                                                EnumConvert.MapSizeFromInt(Settings.Default.SelectedSizeIndex));
            ts_parser_ = new TownSpecParser();
        }


        /// <summary>
        /// get Guards node
        /// </summary>
        /// <returns></returns>
        public XmlElement GetGuardsData()
        {
            return xdocGuards.DocumentElement;
        }

        const int IMAXJOININGVALUE = 300000;
        const double KOEFF_GROW = 1.60; /* dwp коеффициент соотвествующий 4-х недельному приросту охраны ~~ 1,13 ^ 4*/

        readonly int ARMY_POWER_GAP = BaseLevelValue[0] * 4;

        public XmlNode GetGarrisonGuards(int iValue, bool GuardContainsRandomResources, eMonsterStrength Strength, XmlNode xndGuard)
        {
            //such low values are rediculus its a fix for someone that doesn't use the object set right
            iValue = Math.Max(iValue, 2500);
            iValue = (int)(iValue * KOEFF_GROW); /* увеличиваем охрану на 4х недельный прирост*/

            int iStrengthFactor = (int)Strength;
            //dwp. в гарнизоне будет приоритет на 3 и 4 стека в охране
            int iGuardNumber = GetRandomWeightedGuardNumber(IGUARDNUMBERWEIGHT2);
            double[] dblarrLevelWeightList;
            // Random Randomizer.rnd = new Random();
            int iCurrMin = 0;
            int iMaxValue = iValue;
            int iGuardLevel = 0;
            int iRandomSplitValue = iValue;
            int iRandomSplitDifference;

            ComboArmyBuilder ca_builder = new ComboArmyBuilder(ca_parser_, m_parser_, iGuardNumber);
            ca_builder.Power = iValue;
            //define alignment:
            //mixed army:                   15%
            //allied creatures:             50%
            //all creatures from one town:  35%

            //dwp использовать в охране только одну фракцию?
            if (Settings.Default.SingleFactionGuards) // ДА
            {
                ca_builder.Alignment = ArmyAlignment.Town;
            }
            else // НЕТ, то есть по старому
            {
                ca_builder.Alignment = (ArmyAlignment)Randomizer.Choice(new double[] { 15, 50, 35 }, 100);
            }

            #region First Assaign first monster allocation , then add other monsters as additional guards
            // dwp. еще улучшаем алгоритм генерации охраны состоящей из нескольких стеков
            dblarrLevelWeightList = GetLevelWeightList(iValue); // взвешиваем левелы по всей силе охраны
            iGuardLevel = DrawRandomGuardLevel(dblarrLevelWeightList, iValue);

            if (iGuardNumber > 1)
            {
                //split it , from min value to max value
                //dwp fixed bug
                //iRandomSplitValue = Randomizer.rnd.Next(BaseLevelValue[iGuardLevel] * 4, iMaxValue); 
                //dwp  делаем разбитие на куски по силе зависимой от количества стеков
                iRandomSplitValue = Randomizer.rnd.Next(iMaxValue / iGuardNumber, iMaxValue / (iGuardNumber - 1)); 
                
            }

            iGuardLevel = AddLevelWithCap(iGuardLevel, iStrengthFactor);

            //add first random monster to army
            ca_builder.AddRandomMonster(iGuardLevel + 1, iRandomSplitValue, false);
            iCurrMin = iRandomSplitValue;

            #endregion

            //on 75% chance all monsters will be of the same alignments
            if (Randomizer.rnd.NextDouble() < 0.75)
            {
                //here pick a same alignment of guards
                //go over all guards (the number was randomly drawn - can be 1 only ) the last one is the reminder
                for (int i = 1; i < iGuardNumber - 1; i++)
                {
                    if (iCurrMin + ARMY_POWER_GAP < iMaxValue - ARMY_POWER_GAP)
                    {
                        iRandomSplitValue = Randomizer.rnd.Next(iCurrMin + ARMY_POWER_GAP, iMaxValue);
                    }
                    else break; //not enough to keep spliting
                    iRandomSplitDifference = iRandomSplitValue - iCurrMin;
                    dblarrLevelWeightList = GetLevelWeightList(iRandomSplitDifference); //gets a list of weight for each level according to the value
                    iGuardLevel = DrawRandomGuardLevel(dblarrLevelWeightList, iRandomSplitDifference);//returns random g lvl
                    iGuardLevel = AddLevelWithCap(iGuardLevel, iStrengthFactor);

                    ca_builder.AddRandomMonster(iGuardLevel + 1, iRandomSplitDifference, false);

                    ///AddNewAdditionalGuard(xndAdditionalStacks, iGuardLevel + 1, iRandomSplitDifference,true);
                    iCurrMin = iRandomSplitValue;
                }
                //add reminder 
                iRandomSplitDifference = iValue - iCurrMin;
                //if reminder is some guards 
                if (iRandomSplitDifference > BaseLevelValue[0])
                {
                    dblarrLevelWeightList = GetLevelWeightList(iRandomSplitDifference); //gets a list of weight for each level according to the value
                    iGuardLevel = DrawRandomGuardLevel(dblarrLevelWeightList, iRandomSplitDifference);//returns random g lvl
                    iGuardLevel = AddLevelWithCap(iGuardLevel, iStrengthFactor);
                    ///AddNewAdditionalGuard(xndAdditionalStacks, iGuardLevel + 1, iRandomSplitDifference,true);
                    ca_builder.AddRandomMonster(iGuardLevel + 1, iRandomSplitDifference, false);
                }
            }
            else
            {
                //here pick random alignment 
                //go over all guards (the number was randomly drawn - can be 1 only ) the last one is the reminder
                for (int i = 1; i < iGuardNumber - 1; i++)
                {
                    if (iCurrMin + ARMY_POWER_GAP < iMaxValue - ARMY_POWER_GAP)
                    {
                        iRandomSplitValue = Randomizer.rnd.Next(iCurrMin + ARMY_POWER_GAP, iMaxValue);
                    }
                    else
                        break; //not enough to keep spliting
                    iRandomSplitDifference = iRandomSplitValue - iCurrMin;
                    dblarrLevelWeightList = GetLevelWeightList(iRandomSplitDifference); //gets a list of weight for each level according to the value
                    iGuardLevel = DrawRandomGuardLevel(dblarrLevelWeightList, iRandomSplitDifference);//returns random g lvl
                    iGuardLevel = AddLevelWithCap(iGuardLevel, iStrengthFactor);
                    ca_builder.AddRandomMonster(iGuardLevel + 1, iRandomSplitDifference, false);
                    ///AddNewAdditionalGuard(xndAdditionalStacks, iGuardLevel + 1, iRandomSplitDifference);
                    iCurrMin = iRandomSplitValue;
                }
                //add reminder 
                iRandomSplitDifference = iValue - iCurrMin;
                //if reminder is some guards 
                if (iRandomSplitDifference > BaseLevelValue[0])
                {
                    dblarrLevelWeightList = GetLevelWeightList(iRandomSplitDifference); //gets a list of weight for each level according to the value
                    iGuardLevel = DrawRandomGuardLevel(dblarrLevelWeightList, iRandomSplitDifference);//returns random g lvl
                    iGuardLevel = AddLevelWithCap(iGuardLevel, iStrengthFactor);
                    ///AddNewAdditionalGuard(xndAdditionalStacks, iGuardLevel + 1, iRandomSplitDifference);
                    ca_builder.AddRandomMonster(iGuardLevel + 1, iRandomSplitDifference, false);
                }
            }
            ca_builder.SaveGarrison(xndGuard); //save garrison army
            Guards.strAligment = null;
            return xndGuard;
        }


        //todo: check if there is different levels of destribution when guard is mixed , if yes algorithem ok , if no need to check when difference in strangth is too big need to remove insagnificant guards
        //pos and id can be set when using the set object with object writer.
        public XmlNode GetRandomGuards(int iValue, bool GuardContainsRandomResources , eMonsterStrength Strength)
        {
            //such low values are rediculus its a fix for someone that doesn't use the object set right
            bool fl_no_grow = false;
            if (iValue < 0) /* охрана не должна расти на карте */
            {
                iValue = (int) (-iValue * KOEFF_GROW); /* увеличиваем охрану на 4х недельный прирост*/
                fl_no_grow = true;
            }
            
            iValue = Math.Max(iValue, 2500);

            int iStrengthFactor = (int)Strength;
            int iGuardNumber = GetRandomWeightedGuardNumber(IGUARDNUMBERWEIGHT);
            double[] dblarrLevelWeightList;
            // Random Randomizer.rnd = new Random();
            int iCurrMin = 0;
            int iMaxValue = iValue;

            XmlNode xndRandomGuard = GetGeneralRandomXML().Clone();
            int iGuardLevel = 0;
            int iRandomSplitValue = iValue;
            int iRandomSplitDifference;

            ComboArmyBuilder ca_builder = new ComboArmyBuilder(ca_parser_, m_parser_, iGuardNumber);
            ca_builder.Power = iValue;
            //define alignment:
            //mixed army:                   15%
            //allied creatures:             50%
            //all creatures from one town:  35%

            //dwp использовать в охране только одну фракцию?
            if (Settings.Default.SingleFactionGuards) // ДА
            {
                ca_builder.Alignment = ArmyAlignment.Town;
            }
            else // НЕТ, то есть по старому
            {
                ca_builder.Alignment = (ArmyAlignment)Randomizer.Choice(new double[] { 15, 50, 35 }, 100);
            }

            #region First Assaign first monster allocation , then add other monsters as additional guards
            // dwp. улучшаем алгоритм разбивание охраны на стеки
            // теперь для первого стека охраны выбирается адекватная 1я часть, в соответсвии с общей силой охраны

            dblarrLevelWeightList = GetLevelWeightList(iValue); // взвешиваем левелы по всей силе охраны
            iGuardLevel = DrawRandomGuardLevel(dblarrLevelWeightList, iValue);

            if (iGuardNumber > 1)
            {
                //split it , from min value to max value
                //dwp fixed bug
                //iRandomSplitValue = Randomizer.rnd.Next(BaseLevelValue[iGuardLevel] * 4, iMaxValue);
                //dwp  делаем разбитие на куски по силе зависимой от количества стеков
                iRandomSplitValue = Randomizer.rnd.Next(iMaxValue / iGuardNumber, iMaxValue / (iGuardNumber - 1)); 

            }

            iGuardLevel = AddLevelWithCap(iGuardLevel, iStrengthFactor);

#if (DEBUG) // {
            ca_tester_.PreBuildAnalyze(ca_builder);
#endif //DEBUG }
            //add first random monster to army
            ca_builder.AddRandomMonster(iGuardLevel + 1, iRandomSplitValue, false);

            //dmitrik: first monster in not random as well as additionals
            ///XmlNode xndAdditionalStacks = AddPrimaryGuard(xndRandomGuard, iGuardLevel + 1, iRandomSplitValue);

            //XmlNode xndAdditionalStacks = SetPrimaryRandomLevel(xndRandomGuard , iGuardLevel+1 , iRandomSplitValue / BaseLevelValue[iGuardLevel]);
            iCurrMin = iRandomSplitValue;
            
            #endregion

            //on 75% chance all monsters will be of the same alignments
            if (Randomizer.rnd.NextDouble() < 0.75)
            {
                //here pick a same alignment of guards
                //go over all guards (the number was randomly drawn - can be 1 only ) the last one is the reminder
                for (int i = 1; i < iGuardNumber - 1; i++)
                {
                    if (iCurrMin + ARMY_POWER_GAP < iMaxValue - ARMY_POWER_GAP)
                    {
                        iRandomSplitValue = Randomizer.rnd.Next(iCurrMin + ARMY_POWER_GAP, iMaxValue);
                    }
                    else break; //not enough to keep spliting
                    iRandomSplitDifference = iRandomSplitValue - iCurrMin;
                    dblarrLevelWeightList = GetLevelWeightList(iRandomSplitDifference); //gets a list of weight for each level according to the value
                    iGuardLevel = DrawRandomGuardLevel(dblarrLevelWeightList, iRandomSplitDifference);//returns random g lvl
                    iGuardLevel = AddLevelWithCap(iGuardLevel, iStrengthFactor);

                    ca_builder.AddRandomMonster(iGuardLevel + 1, iRandomSplitDifference, false);

                    ///AddNewAdditionalGuard(xndAdditionalStacks, iGuardLevel + 1, iRandomSplitDifference,true);
                    iCurrMin = iRandomSplitValue;
                }
                //add reminder 
                iRandomSplitDifference = iValue - iCurrMin;
                //if reminder is some guards 
                if (iRandomSplitDifference > BaseLevelValue[0])
                {
                    dblarrLevelWeightList = GetLevelWeightList(iRandomSplitDifference); //gets a list of weight for each level according to the value
                    iGuardLevel = DrawRandomGuardLevel(dblarrLevelWeightList, iRandomSplitDifference);//returns random g lvl
                    iGuardLevel = AddLevelWithCap(iGuardLevel, iStrengthFactor);
                    ///AddNewAdditionalGuard(xndAdditionalStacks, iGuardLevel + 1, iRandomSplitDifference,true);
                    ca_builder.AddRandomMonster(iGuardLevel + 1, iRandomSplitDifference, false);
                }
            }
            else
            {
                //here pick random alignment 
                //go over all guards (the number was randomly drawn - can be 1 only ) the last one is the reminder
                for (int i = 1; i < iGuardNumber - 1; i++)
                {
                    if (iCurrMin + ARMY_POWER_GAP < iMaxValue - ARMY_POWER_GAP)
                    {
                        iRandomSplitValue = Randomizer.rnd.Next(iCurrMin + ARMY_POWER_GAP, iMaxValue);
                    }
                    else
                        break; //not enough to keep spliting
                    iRandomSplitDifference = iRandomSplitValue - iCurrMin;
                    dblarrLevelWeightList = GetLevelWeightList(iRandomSplitDifference); //gets a list of weight for each level according to the value
                    iGuardLevel = DrawRandomGuardLevel(dblarrLevelWeightList, iRandomSplitDifference);//returns random g lvl
                    iGuardLevel = AddLevelWithCap(iGuardLevel, iStrengthFactor);
                    ca_builder.AddRandomMonster(iGuardLevel + 1, iRandomSplitDifference, false);
                    ///AddNewAdditionalGuard(xndAdditionalStacks, iGuardLevel + 1, iRandomSplitDifference);
                    iCurrMin = iRandomSplitValue;
                }
                //add reminder 
                iRandomSplitDifference = iValue - iCurrMin;
                //if reminder is some guards 
                if (iRandomSplitDifference > BaseLevelValue[0])
                {
                    dblarrLevelWeightList = GetLevelWeightList(iRandomSplitDifference); //gets a list of weight for each level according to the value
                    iGuardLevel = DrawRandomGuardLevel(dblarrLevelWeightList, iRandomSplitDifference);//returns random g lvl
                    iGuardLevel = AddLevelWithCap(iGuardLevel, iStrengthFactor);
                    ///AddNewAdditionalGuard(xndAdditionalStacks, iGuardLevel + 1, iRandomSplitDifference);
                    ca_builder.AddRandomMonster(iGuardLevel + 1, iRandomSplitDifference, false);
                }
            }
            //remove first dummy additional stack
            ///xndAdditionalStacks.RemoveChild(xndAdditionalStacks.ChildNodes[0]);
            ca_builder.Save(xndRandomGuard); //save army
#if (DEBUG) // {
            ca_tester_.PostBuildAnalyze(ca_builder);
#endif //DEBUG }
            //set a random mood
            int iRandomMood = Randomizer.rnd.Next(4);
            xndRandomGuard.SelectSingleNode(".//Mood").InnerText = ((eMood)iRandomMood).ToString();

            //dmitrik: no joining at all :)
            xndRandomGuard.SelectSingleNode(".//Courage").InnerText = eCourage.MONSTER_COURAGE_ALWAYS_FIGHT.ToString();

            if (fl_no_grow) /* выключаем прирост на карте */
            {
                xndRandomGuard.SelectSingleNode(".//DoesNotGrow").InnerText = "true";
                xndRandomGuard.SelectSingleNode(".//DoesNotDependOnDifficulty").InnerText = "true";

            }
            //on rare occations set a random courge
            //double dRandomCourageChange = Randomizer.rnd.NextDouble();
            //if (dRandomCourageChange < 0.05)
            //{
            //    //set random courage
            //    int iRandomCourage = Randomizer.rnd.Next(3);
            //    if ( iValue < IMAXJOININGVALUE)
            //        xndRandomGuard.SelectSingleNode(".//Courage").InnerText = ((eCourage)iRandomCourage).ToString();
            //}

            Guards.strAligment = null;
            return xndRandomGuard;
        }

        private int AddLevelWithCap(int iGuardLevel, int iStrengthFactor)
        {
            if (iGuardLevel + iStrengthFactor < 0)
                return 0;
            if (iGuardLevel + iStrengthFactor > 6)
                return 6;
            return ( iGuardLevel + iStrengthFactor );
        }

        public const int MAX_UNIT_AMOUNT = 100;
        public const int MIN_UNIT_AMOUNT = 7;

        /// <summary>
        /// adds a guard to a template xml with a random guard and 1 additional dummy guard (which will be removed eventually)
        /// </summary>
        /// <param name="xndAdditionalStacks"></param>
        /// <param name="iGuardLevel"></param>
        /// <param name="iValue"></param>
        private void AddNewAdditionalGuard(XmlNode xndAdditionalStacks, int iGuardLevel, int iValue)
        {
            XmlNode xndSingleAdditionalGuard;
            XmlNode xndRandomGuard;
            int iMonsterPower;
            int iAmount;
            bool bMaxLevelNotReached = true;

            do
            {
                xndRandomGuard = GetRandomGuardAtLevel(iGuardLevel);
                if (iGuardLevel == 6)
                    bMaxLevelNotReached = false;

                iMonsterPower = int.Parse(xndRandomGuard.Attributes["Power"].Value);
                iAmount = iValue / iMonsterPower;
                if (iAmount == 0)
                    iAmount = 1;
                if (iAmount > MAX_UNIT_AMOUNT)//110
                {
                    iGuardLevel = AddLevelWithCap(iGuardLevel, 1);
                }

            } while (iAmount > MAX_UNIT_AMOUNT && bMaxLevelNotReached); //110

            xndAdditionalStacks.ChildNodes[0]["Creature"].InnerText =xndRandomGuard.Attributes["Name"].Value;
            xndAdditionalStacks.ChildNodes[0]["Amount"].InnerText = iAmount.ToString();
           // xndSingleAdditionalGuard
            xndAdditionalStacks.AppendChild(  xndAdditionalStacks.ChildNodes[0].Clone());

            //return null;
        }


        /// <summary>
        /// adds a primary guard to a template xml
        /// </summary>
        /// <param name="iGuardLevel"></param>
        /// <param name="iValue"></param>
        private XmlNode AddPrimaryGuard(XmlNode xndRandomXml, int iGuardLevel, int iValue)
        {
            XmlNode xndRandomGuard;
            int iMonsterPower;
            int iAmount;
            bool bMaxLevelNotReached = true;

            do {
                if (iGuardLevel == 6)
                    bMaxLevelNotReached = false;

                xndRandomGuard = GetRandomGuardAtLevel(iGuardLevel);

                iMonsterPower = int.Parse(xndRandomGuard.Attributes["Power"].Value);
                iAmount = iValue / iMonsterPower;
                if (iAmount == 0)
                    iAmount = 1;
                if (iAmount > MAX_UNIT_AMOUNT)//110
                {
                    iGuardLevel = AddLevelWithCap(iGuardLevel, 1);
                }

            } while (iAmount > MAX_UNIT_AMOUNT && bMaxLevelNotReached); //110   

            string href = GenMonsterHREF(xndRandomGuard, iGuardLevel);
            XmlNode xndPath = ((XmlElement)xndRandomXml).GetElementsByTagName("Shared")[0];
            ((XmlElement)xndPath).SetAttribute("href", xndPath.Attributes["href"].Value.Replace("Random/Random-Monster-L3.(AdvMapMonsterShared).xdb", href));
            XmlNode xndAmount = ((XmlElement)xndRandomXml).GetElementsByTagName("Amount")[0];
            xndAmount.InnerText = iAmount.ToString();

            return ((XmlElement)xndRandomXml).GetElementsByTagName("AdditionalStacks")[0];
        }

        public string GenMonsterHREF(XmlNode guard_node, int guard_lvl)
        {
            StringBuilder href = new StringBuilder();
            string town = xmltown_to_hreftown(guard_node.Attributes["Town"].Value);
            string monster = guard_node.Attributes["CreatureName"].Value;
            if (monster == "") { //random moster
                href.AppendFormat("Random/Random-Monster-L{0}.(AdvMapMonsterShared).xdb", guard_lvl);
            }
            else { //defined moster
                string ext = monster.Contains(".xdb") ? "" : ".(AdvMapMonsterShared).xdb";
                href.AppendFormat("{0}/{1}{2}", town, monster, ext);
            }
            return href.ToString();
        }

        /// <summary>
        /// Convert town name from xml to href
        /// </summary>
        /// <param name="xmltown">xml name</param>
        /// <returns>href name</returns>
        private string xmltown_to_hreftown(string xmltown)
        {
            string hreftown = EnumConvert.HREFTownStringFromString(xmltown);
            if (hreftown.Length == 0) {
                throw new Exception("Critical error: Wrong moster description");
            }
            return hreftown;
        }

        /// <summary>
        /// adds a guard to a template xml with a random guard and 1 additional dummy guard (which will be removed eventually)
        /// </summary>
        /// <param name="xndAdditionalStacks"></param>
        /// <param name="iGuardLevel"></param>
        /// <param name="iValue"></param>
        private void AddNewAdditionalGuard(XmlNode xndAdditionalStacks, int iGuardLevel, int iValue , bool bToGetSameAlignmentGuards)
        {



            XmlNode xndSingleAdditionalGuard;
            XmlNode xndRandomGuard;
            int iMonsterPower;
            int iAmount;
            bool bMaxLevelNotReached = true;

            do
            {
                if (iGuardLevel == 6)
                    bMaxLevelNotReached = false;

                if (bToGetSameAlignmentGuards)
                    xndRandomGuard = GetRandomGuardAtLevelWithAligment(iGuardLevel, strAligment);
                else
                    xndRandomGuard = GetRandomGuardAtLevel(iGuardLevel);

                
                iMonsterPower = int.Parse(xndRandomGuard.Attributes["Power"].Value);
                iAmount = iValue / iMonsterPower;
                if (iAmount == 0)
                    iAmount = 1;
                if (iAmount > MAX_UNIT_AMOUNT)//110
                {
                    iGuardLevel = AddLevelWithCap(iGuardLevel, 1);
                }

            } while (iAmount > MAX_UNIT_AMOUNT && bMaxLevelNotReached); //110   

            xndAdditionalStacks.ChildNodes[0]["Creature"].InnerText = xndRandomGuard.Attributes["Name"].Value;
            xndAdditionalStacks.ChildNodes[0]["Amount"].InnerText = iAmount.ToString();
            // xndSingleAdditionalGuard
            xndAdditionalStacks.AppendChild(xndAdditionalStacks.ChildNodes[0].Clone());

            //return null;
        }

        private XmlNode GetRandomGuardAtLevel(int iGuardLevel)
        {
            int iLevelCount =  GetGuardsData() .ChildNodes[1].ChildNodes[iGuardLevel -1].ChildNodes.Count;

            int iRandomMonster = Randomizer.rnd.Next(iLevelCount);

            return GetGuardsData().ChildNodes[1].ChildNodes[iGuardLevel -1].ChildNodes[iRandomMonster];
        }


        private XmlNode GetRandomGuardAtLevelWithAligment(int iGuardLevel,string strAligment)
        {
            XmlNodeList xndGuardsAtLevelFromAlignment;



            do
            {

                if (strAligment == null)
                {
                    XmlNode xndRandomGuard = GetRandomGuardAtLevel(iGuardLevel);
                    Guards.strAligment = xndRandomGuard.Attributes["Town"].Value;
                    return xndRandomGuard;
                }

                xndGuardsAtLevelFromAlignment = GetGuardsData().ChildNodes[1].ChildNodes[iGuardLevel - 1].SelectNodes(".//Monster[@Town='" + strAligment + "']");

                if (strAligment == "Neutral")

                    iGuardLevel = AddLevelWithCap(iGuardLevel, 1);

            } while (xndGuardsAtLevelFromAlignment.Count==0);
            
            int iLevelCount = xndGuardsAtLevelFromAlignment.Count;
            int iRandomMonster = Randomizer.rnd.Next(iLevelCount);

            return xndGuardsAtLevelFromAlignment[iRandomMonster];
        }
        /// <summary>
        /// will calculate avarage power - for one time use only 
        /// </summary>
        /// <param name="iGuardLevel"></param>
        /// <returns></returns>
        private int GetAvaragePowerAtLevel(int iGuardLevel)
        {

            int iSum = 0;
            int iCount = 0;
            foreach (XmlNode xndMonster in GetGuardsData().ChildNodes[1].ChildNodes[iGuardLevel])
            {
                iSum += int.Parse ( xndMonster.Attributes["Power"].Value );
                iCount++;
            }

            return iSum/iCount;
        }

        /// <summary>
        /// returns xml representing a guard in the template xml format
        /// </summary>
        /// <param name="iValue">value of strength for guard</param>
        /// <returns>xml template guard</returns>
        public XmlElement GetTemplateGuardXML(int iValue)
        {
            XmlElement xTemplateGuardXML = xdocGuards.CreateElement("Guards");
            xTemplateGuardXML.SetAttribute("Type", eGuardTypes.NormalValueMode.ToString());

            xTemplateGuardXML.SetAttribute ( "Value" , iValue.ToString() ) ;

            return xTemplateGuardXML;
        }


        /// <summary>
        /// set random level in an xml random element
        /// </summary>
        /// <param name="xndRandomXml"></param>
        /// <param name="iGuardLevel"></param>
        /// <returns> additional stacks element</returns>
        private XmlNode SetPrimaryRandomLevel(XmlNode xndRandomXml ,int iGuardLevel , int iAmount)
        {
            XmlNode xndPath = ((XmlElement)xndRandomXml).GetElementsByTagName("Shared")[0];
            ((XmlElement)xndPath).SetAttribute( "href" ,  xndPath.Attributes["href"].Value.Replace("Random-Monster-L3" , "Random-Monster-L" + iGuardLevel.ToString()) );

            XmlNode xndAmount = ((XmlElement)xndRandomXml).GetElementsByTagName("Amount")[0];
            xndAmount.InnerText = iAmount.ToString();
 

            return ((XmlElement)xndRandomXml).GetElementsByTagName("AdditionalStacks")[0];
        }


        /// <summary>
        /// returns a blank format of a random xml element
        /// </summary>
        /// <returns></returns>
        private XmlNode GetGeneralRandomXML()
        {
            XmlElement xGuards = GetGuardsData();
            return xGuards.ChildNodes[0].ChildNodes[0];
        }


        /// <summary>
        /// draws a random level based on the weight llist
        /// </summary>
        /// <param name="iarrLevelWeightList">weight list for all levels </param>
        /// <returns>random level</returns>
        private int DrawRandomGuardLevel(double[] iarrLevelWeightList, int iValue)
        {

            //Random Randomizer.rnd = new Random();
            double dRandom = Randomizer.rnd.NextDouble();// draw a double number between 0 and 1
            double dSum = 0;

            //sum all chances until we get to randomly drawn number - that will be our randomly drawn level 
            for (int i = 0; i < ILEVELSNUMBER; i++)
            {
                dSum += iarrLevelWeightList[i];
                //dwp или сила минимального стека следующего левела превысит силу всего отряда
                if (dSum >= dRandom || ((i < ILEVELSNUMBER) && ((BaseLevelValue[i + 1] * 4) >= iValue)))
                    return i;
            }

            return ILEVELSNUMBER / 2; 
        }


        public static readonly int[] BaseLevelValue = { // there are currently 7 levels
            112 , //level 1 *75 = 8400
            201 , // level 2 *75 = 15075
            335 , // level 3 *75 = 25125
            651 , // level 4 *75 = 48825
            1221 , // level 5 *75 = 91575
            2346 , // level 6 *75 =175950
            5551 //level 7 *75 =416325
        };

        public const int ILEVELSNUMBER = 7; //number of monster levels
        public const int IBESTFITNUMBER = 65; // best fit number is 75 ( avarage of horde 50-99 )

        /// <summary>
        /// returns a list of weight for each level according to the value , Most weight will be gained by the guards who best fit the value in up to horde (50-99) numbers
        /// </summary>
        /// <param name="iValue"></param>
        /// <returns></returns>
        public double[] GetLevelWeightList(int iValue)
        {
            //for example if value is 22,000 then based on these values :
            //10 , //level 1 will be a number of 22,000/10 = 2200 very big deviation from number of 50-99
            //50 , // level 2 will be a number of 22,000/50 = 440 big deviation from number of 50-99
            //100 , // level 3 will be a number of 22,000/100 = 220 big deviation from number of 50-99
            //200 , // level 4 will be a number of 22,000/200 = 110 small deviation from number of 50-99
            //320 , // level 5 will be a number of 22,000/320 = 68.75>>69 perfect match from number of 50-99
            //740 , // level 6 will be a number of 22,000/740 = 29.7>>30 small deviation from number of 50-99
            //1400 //level 7 will be a number of 22,000/1400 = 15.7>>16 big deviation from number of 50-99
            //2605
            //6,1200,1399
            int[] iarrLevelsDevation = new int[ILEVELSNUMBER];
            int iMaxDevation = int.MinValue;
            int iMinDevation = int.MaxValue;

            int iMaxLevel = ILEVELSNUMBER;
            //calculate fit for value then calculate devation from 75
            for (int i = ILEVELSNUMBER - 1; i >= 0; i--)
            {
                if (iValue / BaseLevelValue[i] > 0)
                {
                    iarrLevelsDevation[i] = Math.Abs(iValue / BaseLevelValue[i] - IBESTFITNUMBER);//first div gets number of guards for value in each level , then cal divation
                    if (iMaxDevation < iarrLevelsDevation[i])
                        iMaxDevation = iarrLevelsDevation[i];

                    if (iMinDevation > iarrLevelsDevation[i])
                        iMinDevation = iarrLevelsDevation[i];
                }
                else
                {
                    //if 1 creature amount can't be in level there no chance for that level to appear
                    iarrLevelsDevation[i] = 0;
                    iMaxLevel = i;
                }

            }


            int iSum = 0;

            //Reverse devation so that closer to 0 have more weight
            for (int i = 0; i < iMaxLevel; i++)
            {
                //base value added to prevent 0 precent at maximum divation
                iarrLevelsDevation[i] = Math.Abs( iMaxDevation - iarrLevelsDevation[i]  + iMinDevation);
                iSum += iarrLevelsDevation[i];
            }

            //now create the weight list
            double[] dblarrLevelsWeightList = new double[ILEVELSNUMBER];



            //Reverse devation so that closer to 0 have more weight
            for (int i = 0; i < iMaxLevel; i++)
            {
                dblarrLevelsWeightList[i] = (double) iarrLevelsDevation[i] / iSum;
            }
            //todo:check for missing precentages and add them somewhere to complete 100%


            return dblarrLevelsWeightList;



        }

        //dwp веса по количеству стеков для охраны в поле (охрана умеет делится сама на стеки - приоритет 1 стек)
        public static readonly int[] IGUARDNUMBERWEIGHT = {
            30, 0 , 35 , 20 , 5 , 5 , 5
        };

        //dwp веса по количеству стеков для охраны гарнизона(там сама армия не делится - приоритет 3 или 4 стека)
        public static readonly int[] IGUARDNUMBERWEIGHT2 = {
            0,0, 40, 30, 20,5,5
        };


        /// <summary>
        /// returns a random guard number weighted by above list (Precentages )
        /// </summary>
        /// <returns>random guard number(1 guard is prefered)</returns>
        public int GetRandomWeightedGuardNumber(int[] IGNW)
        {

            //Random Randomizer.rnd = new Random();

            int iRandom = Randomizer.rnd.Next(100);
            int iSum = 0;
            int i = 0;

            do
            {
                iSum += IGNW[i];
                i++;
            } while (iSum < iRandom);

            return i;
        }

        /// <summary>
        /// returns a random combo guard number weighted by above list (Precentages )
        /// </summary>
        /// <returns>random combo guard number(1 guard is impossible)</returns>
        public int GetRandomWeightedComboGuardNumber()
        {
            int iRandom = Randomizer.rnd.Next(50);
            int iSum = 0;
            int i = 1;

            do {
                iSum += IGUARDNUMBERWEIGHT[i];
                i++;
            } while (iSum < iRandom);

            return i;
        }

        /// <summary>
        /// returns a list that contains all type of guards 
        /// </summary>
        /// <returns>returns a list that contains all type of guards</returns>
        internal System.Collections.ArrayList GetGuardsNameList()
        {
            XmlElement xGuards =  GetGuardsData();
            System.Collections.ArrayList GuardList = new System.Collections.ArrayList();

            foreach (XmlNode xndGuardObject in xGuards.ChildNodes)
            {
                

                XmlNode xndPath = ((XmlElement)xndGuardObject).GetElementsByTagName("Shared")[0];
                string strShared = xndPath.Attributes["href"].Value;

                int iFirstPointIndex = strShared.IndexOf('.');
                int iLastInitialCloserIndex = strShared.Substring(0, iFirstPointIndex).LastIndexOf('/');
                GuardList.Add(strShared.Substring(iLastInitialCloserIndex + 1 , iFirstPointIndex - iLastInitialCloserIndex - 1 ));
            }

            return GuardList;
        }
    }
}
