using System;
using System.Collections.Generic;
using System.Text;
using Homm5RMG.BL;

namespace Homm5RMG
{
    public class TownSpecBuilder
    {
        private TownSpecParser ts_parser_ = null;
        public TownSpecParser TSParser { get { return ts_parser_; } }

        public TownSpecBuilder(TownSpecParser ts_parser)
        {
            ts_parser_ = ts_parser;
        }

        public string GetRandomSpecialization(eTownType town)
        {
            TownSpecializations ts = ts_parser_.Towns.Find(delegate(TownSpecializations s) { return s.Town == town; });
            if (ts == null || ts.Specializations.Count == 0) {
                return null;
            }
            int i = Randomizer.rnd.Next(ts.Specializations.Count);
            return ts.Specializations[i];
        }
    }
}
