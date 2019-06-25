using System;
using System.Collections.Generic;
using System.Text;

namespace Homm5RMG.Testing
{
    public static class TownGarrisonTester
    {
        public static void ShowAllTownGarrisonsPower(TownGarrisonParser tg_parser, MonstersParser m_parser)
        {
            StringBuilder sb = new StringBuilder();
            int i = 0;
            foreach (TownGarrison tg in tg_parser.TownGarrisonVariants) {
                sb.AppendFormat("Garrison N{0}\n", ++i);
                int g_power = 0;
                foreach (MonsterPlaceHolder h in tg.Monsters) {
                    int power = m_parser.Monsters.Find(delegate(Monster m) { return m.Name == h.Monster.Name; }).Power * h.Amount;
                    g_power += power;
                    sb.AppendFormat("  {0}({1}) -> {2}\n", h.Monster.Name, h.Amount, power);
                }
                sb.AppendFormat("  power: {0}\n", g_power);
            }
            System.Windows.Forms.MessageBox.Show(sb.ToString());
        }
    }
}
