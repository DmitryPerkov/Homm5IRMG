using System;
using System.Collections.Generic;
using System.Text;

namespace Homm5RMG
{
    public class MapPoint
    {
        public int x;
        public int y;

        public MapPoint(int iX, int iY)
        {
            x = iX;
            y = iY;
        }

        public override string ToString()
        {
            return (x.ToString() + "," + y.ToString());
            //return base.ToString();
        }

        public static double operator -(MapPoint mpFirst, MapPoint mpSecond)
        {
            int yDiff = Math.Abs(mpSecond.y - mpFirst.y);
            int xDiff = Math.Abs(mpSecond.x - mpFirst.x);
            return (Math.Sqrt(xDiff * xDiff + yDiff * yDiff));
        }

        //this is overridden for the compare of map points
        public override bool Equals(object obj)
        {            
            return ((this.x == ((MapPoint)obj).x) && this.y == ((MapPoint)obj).y);
        }

        public static MapPoint operator +(MapPoint mpFirstAdd , MapPoint mpSecondAdd)
        {
            try
            {
                return new MapPoint(mpFirstAdd.x + mpSecondAdd.x, mpFirstAdd.y + mpSecondAdd.y);
            }
            catch
            {
                return null;
            }
        }



        internal bool IsPointInDirectionCompareToPoint(MapPoint mpBasePoint, ObjectDirection objectDirection)
        {
            switch (objectDirection)
            {
                case ObjectDirection.Down:
                    if (this.y < mpBasePoint.y)
                        return true;
                    else
                        return false;
                case ObjectDirection.Right:
                    if (this.x > mpBasePoint.x)
                        return true;
                    else
                        return false;
                case ObjectDirection.Up:
                    if (this.y > mpBasePoint.y)
                        return true;
                    else
                        return false;
                case ObjectDirection.Left:
                    if (this.x < mpBasePoint.x)
                        return true;
                    else
                        return false;
                default:
                    return false;
            }
        }
    }

    
}
