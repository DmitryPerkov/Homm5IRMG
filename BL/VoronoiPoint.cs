using System;
using System.Collections.Generic;
using System.Text;
using Homm5RMG.BL;

namespace Homm5RMG
{
    public class VoronoiPoint : MapPoint 
    {
        public int iAreaCode;
        //#if (DEBUG)
        //   private static Random Randomizer.rnd = new Random(3);
        //   #warning "Debug mode - Randomization is constant"
        //#else
        //    private static Random Randomizer.rnd = new Random();   
        //#endif

        public VoronoiPoint(int x ,int y ) : base( x , y)
        {
            //next area code gets its value inside the voronoi algorithem ,no reason here.
           // iAreaCode = iNextAreaCode;
            
        }

        //this is overridden for the compare of voronoi points
        public override bool Equals(object obj)
        {            
            //return ( this.iAreaCode == ((VoronoiPoint)obj).iAreaCode );
            return ((this.x == ((VoronoiPoint)obj).x) && this.y == ((VoronoiPoint)obj).y);
        }


        //generates random points withing maxsize values
        public static VoronoiPoint GenerateRandomNewPoint(MapSize maxSize)
        {
           
            
            return new VoronoiPoint(Randomizer.rnd.Next(1, (int)maxSize) , Randomizer.rnd.Next(1, (int)maxSize));//, 0);
        }

        public static double operator -( MapPoint mpToAdd , VoronoiPoint vrP)
        {
            int yDiff = Math.Abs(vrP.y - mpToAdd.y);
            int xDiff = Math.Abs(vrP.x - mpToAdd.x);
            return  ( Math.Sqrt ( xDiff * xDiff + yDiff * yDiff )); 
        }
    }
}
