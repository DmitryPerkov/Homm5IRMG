using System;
using System.Collections.Generic;
using System.Text;

namespace Homm5RMG.BL
{
    public class Randomizer
    {
        public static Random rnd = new Random();

        public static void SetSeed (int iSeed)
        {
            rnd = new Random (iSeed );
        }

        public static int Choice(double[] chances, double total_chances)
        {
            int choice = 0;
            double s = 0; 
            int r = Randomizer.rnd.Next(Convert.ToInt16(total_chances));
            for (int i = chances.Length - 1 ; i >= 0; --i)
            {
                s += chances[i];
                if (r <= s && chances[i] != 0)
                {
                    choice = i;
                    break;
                }
            }
            return choice;
        }

        public static void ShiftChance(double[] chances, int index, double k)
        {
            if (index >= 0 && index < chances.Length) {
                double corr = (chances[index] - (chances[index] * k)) / (chances.Length - 1);
                for (int i = 0; i < chances.Length; ++i) {
                    if (i == index) {
                        chances[i] *= k;
                    }
                        /* dwp отменяю корректировку щансов. так как теперь передается общее количество шансов
                           при генерации двеллинга 
                    else {
                        chances[i] += corr;
                    } */
                }
            }
        }

        public static void SwapChances(double[] chances, int index1, int index2)
        {
            if (index1 >= 0 && index1 < chances.Length &&
                index2 >= 0 && index2 < chances.Length) {

                double t = chances[index1];
                chances[index1] = chances[index2];
                chances[index2] = t;
            }
        }

        public static int GenTimeSeed()
        {
            Random r = new Random();
            return r.Next(1, 100) * (DateTime.Now.Second * 1000 + DateTime.Now.Millisecond);
        }
    }
}
