using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Homm5RMG.BL;

namespace Homm5RMG
{
    /// <summary>
    /// Town garrison builder class
    /// </summary>
    public class TownGarrisonBuilder
    {
        private TownGarrisonParser tg_parser_ = null;
        public TownGarrisonParser TGParser { get { return tg_parser_; } }

        public TownGarrisonBuilder(TownGarrisonParser tg_parser)
        {
            tg_parser_ = tg_parser;
        }

        public TownGarrison GetRandomTownGarrison()
        {
            if (tg_parser_.TownGarrisonVariants.Count == 0) {
                return null;
            }
            int i = Randomizer.rnd.Next(tg_parser_.TownGarrisonVariants.Count);
            return tg_parser_.TownGarrisonVariants[i];
        }
    }
}
