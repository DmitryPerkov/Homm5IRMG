using System;
using System.Collections.Generic;
using System.Text;

namespace Homm5RMG
{
    public class Voronoi
    {
        public  System.Collections.ArrayList VoronoiPoints = new System.Collections.ArrayList();
        int iNextAreaCode = 0;

        private int VoronoiFarestPointIndex1 = 0;
        private int VoronoiFarestPointIndex2 = 1;
        private int VoronoiFarestPointIndex3 = 2;
        private int VoronoiFarestPointIndex4 = 3;

        public VoronoiPoint FarestPoint(int num)
        {
            switch(num) {
                   case 1: return (VoronoiPoint)VoronoiPoints[VoronoiFarestPointIndex1]; 
                   case 2: return (VoronoiPoint)VoronoiPoints[VoronoiFarestPointIndex2]; 
                   case 3: return (VoronoiPoint)VoronoiPoints[VoronoiFarestPointIndex3]; 
                   case 4: return (VoronoiPoint)VoronoiPoints[VoronoiFarestPointIndex4]; 
                   default: return null;
           }
        }

        public void AddVPoint(VoronoiPoint Vp)
        {
            Vp.iAreaCode = iNextAreaCode;            
            if (VoronoiPoints.Contains(Vp))
            {                
                throw new VoronoiPointExistException("Already Exist");
            }
            else
            {
                VoronoiPoints.Add(Vp);
                iNextAreaCode++;
            }
            
        }

        public int AreaCode(MapPoint mpPoint)
        {
            int iAreaCodeForMinDis = 0;
            double dCurrMinDis = double.MaxValue;
            double dDistance;
            foreach (VoronoiPoint vpItem in VoronoiPoints)
            {
                dDistance = mpPoint - vpItem;//Distance(mpPoint, vpItem);
                //finds minimum distance between points and return area code for  it
                if (dCurrMinDis > dDistance)
                {
                    iAreaCodeForMinDis = vpItem.iAreaCode;
                    dCurrMinDis = dDistance;
                }
            }

            return iAreaCodeForMinDis;
        }

        public double Distance(MapPoint mpSource, VoronoiPoint vpDest)
        {            
            return (mpSource - vpDest );
        }


        public void ComputeFarestPoints()
        {
            double dCurrMaxDis, dCurrMaxDis2, dDistance1, dDistance2, dDistance3;

            dCurrMaxDis = -1;
            int iIndex1=0, iIndex2=0, iIndex3=0, iIndex4=0;
            for (int i = 0; i < VoronoiPoints.Count; i++)
            {
                for (int j = 0; j < VoronoiPoints.Count; j++)
                {
                    dDistance1 = (VoronoiPoint)VoronoiPoints[i] - (VoronoiPoint)VoronoiPoints[j];
                    if (dCurrMaxDis < dDistance1)
                    {
                        dCurrMaxDis = dDistance1;
                        iIndex1 = i;
                        iIndex2 = j;
                    }
                }
            }
            this.VoronoiFarestPointIndex1 = iIndex1;
            this.VoronoiFarestPointIndex2 = iIndex2;
            if (Program.frmMainMenu.RadioButton2.Checked || 
                Program.frmMainMenu.RadioButton3.Checked ||
                Program.frmMainMenu.RadioButton4.Checked)
            {
                dCurrMaxDis = double.MaxValue;
                dCurrMaxDis2 = -1;

                for (int i = 0; i < VoronoiPoints.Count; i++)
                {
                    if (i != iIndex1 && i != iIndex2)
                    {
                        dDistance1 = (VoronoiPoint)VoronoiPoints[iIndex1] - (VoronoiPoint)VoronoiPoints[i];
                        dDistance2 = (VoronoiPoint)VoronoiPoints[iIndex2] - (VoronoiPoint)VoronoiPoints[i];
                        if ((Math.Abs(dDistance1 - dDistance2) < dCurrMaxDis) &&
                             ((dDistance1 + dDistance2) > dCurrMaxDis2))
                        {
                            dCurrMaxDis = Math.Abs(dDistance1 - dDistance2);
                            dCurrMaxDis2 = dDistance1 + dDistance2;
                            iIndex3 = i;
                        }
                    }
                }
                this.VoronoiFarestPointIndex3 = iIndex3;
            }
            if (Program.frmMainMenu.RadioButton3.Checked ||
                Program.frmMainMenu.RadioButton4.Checked)
            {
                dCurrMaxDis = -1;
                for (int i = 0; i < VoronoiPoints.Count; i++)
                {
                    if (i != iIndex1 && i != iIndex2 && i != iIndex3)
                    {
                        dDistance1 = (VoronoiPoint)VoronoiPoints[iIndex1] - (VoronoiPoint)VoronoiPoints[i];
                        dDistance2 = (VoronoiPoint)VoronoiPoints[iIndex2] - (VoronoiPoint)VoronoiPoints[i];
                        dDistance3 = (VoronoiPoint)VoronoiPoints[iIndex3] - (VoronoiPoint)VoronoiPoints[i];
                        if ((dDistance1 + dDistance2 + dDistance3) > dCurrMaxDis)
                        {
                            dCurrMaxDis = dDistance1 + dDistance2 + dDistance3;
                            iIndex4 = i;
                        }
                    }
                }
                this.VoronoiFarestPointIndex4 = iIndex4;
            }

        }

    }
}
