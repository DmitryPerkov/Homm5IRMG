using System;
using System.Collections.Generic;
using System.Text;

namespace Homm5RMG.Testing
{
    public class ComboArmyTester
    {
        private int size_ = 0; //required army size
        private int power_ = 0; //required army power
        private ArmyAlignment alignment_ = ArmyAlignment.None; //required alignment
        private MonsterTraits[] traits_ = null;
        private int[] count_ = new int[7]; //armies count on size
        private int[] count2_ = new int[7]; //armies count on real size

        private int size_changed_ = 0; //number of armies with changed size
        private int power_changed_ = 0; //number of armies with changed power
        private int alignment_changed_ = 0; //number of armies with changed alignment
        private int composition_changed_ = 0; //number of armies with changed composition

        public ComboArmyTester()
        {
        }

        /// <summary>
        /// Analyze ComboArmyBuild object before building army
        /// </summary>
        public void PreBuildAnalyze(ComboArmyBuilder builder)
        {
            //save initial data
            size_ = builder.Size;
            power_ = builder.Power;
            alignment_ = builder.Alignment;
            traits_ = new MonsterTraits[builder.PlaceHolders.Count];
            int i = 0;
            builder.PlaceHolders.ForEach(
                delegate(MonsterPlaceHolder h) {
                    traits_[i++] = h.Traits;
                }
            );
        }

        /// <summary>
        /// Analyze ComboArmyBuild object after building army
        /// </summary>
        public void PostBuildAnalyze(ComboArmyBuilder builder)
        {
            //analyze changes
            //size
            if (size_ != builder.RealSize) {
                ++size_changed_;
            }
            //power
            if ((power_ - builder.RealPower) > builder.WeakestMonster.Power * 100) {
                ++power_changed_;
            }
            //alignment
            if (alignment_ != builder.CurrentAlignment) {
                ++alignment_changed_;
            }
            //composition
            for (int i = 0; i < traits_.Length; ++i) {
                if (traits_[i] != builder.PlaceHolders[i].Traits) {
                    ++composition_changed_;
                    break;
                }
            }
            ++count_[builder.Size - 1]; //count of armies
            ++count2_[builder.RealSize - 1]; //count of armies
        }

        /// <summary>
        /// Show results of all generation session
        /// </summary>
        public void ShowResults()
        {
            StringBuilder sb = new StringBuilder();
            int s = 0;
            Array.ForEach(count_, delegate(int i) { s += i; });
            sb.AppendFormat("Total armies generated: {0}\n", s);
            for (int i = 0; i < 7; ++i) {
                sb.AppendFormat("  size={0}: {1}; realsize={0}: {2}\n", i + 1, count_[i], count2_[i]);
            }
            sb.AppendFormat("size have been changed in {0} cases\n", size_changed_);
            sb.AppendFormat("power have been changed in {0} cases\n", power_changed_);
            sb.AppendFormat("alignment have been changed in {0} cases\n", alignment_changed_);
            sb.AppendFormat("composition have been changed in {0} cases\n", composition_changed_);
            System.Windows.Forms.MessageBox.Show(sb.ToString());
        }
    }
}
