using System;
using System.Collections.Generic;
using System.Text;

namespace Homm5RMG
{
    public static class EnumConvert
    {
        /// <summary>
        /// Convert string to eTownType enum
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static eTownType TownTypeFromString(string str)
        {
            int i = Array.IndexOf(Enum.GetNames(typeof(eTownXMLArmyHREFType)), str);
            if (i == -1) { //not found yet
                i = Array.IndexOf(Enum.GetNames(typeof(eTownType)), str) - 1;
                if (i == -1) {
                    return eTownType.None;
                }
            }
            return (eTownType)(i + 1);
        }

        /// <summary>
        /// Convert string to eTownXMLArmyHREFType string
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string HREFTownStringFromString(string str)
        {
            string[] towns = Enum.GetNames(typeof(eTownXMLArmyHREFType));
            int i = Array.IndexOf(towns, str);
            if (i == -1) { //not found yet
                i = Array.IndexOf(Enum.GetNames(typeof(eTownType)), str) - 1;
                if (i == -1) {
                    return "";
                }
            }
            return towns[i];
        }

        /// <summary>
        /// Convert eTownType enum to RaceSide enum
        /// </summary>
        /// <param name="town"></param>
        /// <returns></returns>
        public static RaceSide RaceSideFromTownType(eTownType town)
        {
            switch (town) {
                case eTownType.Academy:
                case eTownType.Fortress:
                case eTownType.Haven:
                case eTownType.Sylvan:
                    return RaceSide.Good;
                case eTownType.Dungeon:
                case eTownType.Inferno:
                case eTownType.Necropolis:
                case eTownType.Orcs:
                    return RaceSide.Evil;
                default:
                    return RaceSide.Neutral;
            }
        }

        /// <summary>
        /// Convert string to MonsterTraits enum
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static MonsterTraits MonsterTraitsFromString(string str)
        {
            if (str == null || str.Length == 0) {
                return MonsterTraits.Any;
            }
            string[] traits = Enum.GetNames(typeof(MonsterTraits));
            int i = Array.FindIndex(traits,
                                    delegate(string s) {
                                        return s[0].ToString() == str;
                                    }
                                   );
            if (i == -1) { //not found yet
                return MonsterTraits.Any;
            }
            return (MonsterTraits)Enum.GetValues(typeof(MonsterTraits)).GetValue(i);
        }

        public static MapSize MapSizeFromString(string str)
        {
            int i = Array.IndexOf(Enum.GetNames(typeof(MapSize)), str);
            if (i == -1) {
                return MapSize.Tiny;
            }
            return (MapSize)Enum.GetValues(typeof(MapSize)).GetValue(i);
        }

        public static MapSize MapSizeFromInt(int i)
        {
            return (MapSize)Enum.GetValues(typeof(MapSize)).GetValue(i);
        }
    }
}
