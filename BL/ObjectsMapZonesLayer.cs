using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using Homm5RMG.BL;
using Homm5RMG.Properties;


namespace Homm5RMG
{

    #region Map Object Enums
    
    public enum MapSize 
    {
        Tiny = 72 ,
        Small = 96 ,
        Medium = 136 ,
        Large = 176 ,
        ExtraLarge = 216 ,
        Huge = 256 ,
        Impossible = 320
    }

    public enum MapLayer
    {
        Zones = 0,
        VoronoiZones = 1,
        Objects = 2,
        Objects2 = 3 ,
        Directions = 4 ,
        Blocks = 5, 
        AStarDistance = 6 ,
        Terrain = 7

    }


    public enum eZonesStatus
    {
        ZoneDefined = 0,
        Undefineable
    }


    public enum eBlockStatus
    {
        Block ,
        Done ,
        MaybeBlock ,
        Preserved ,
        Free
    }
    #endregion


    public partial class ObjectsMap
    {
        public TemplateHandler thSelectedTemplate;
        public MapSize eMapSize;
        public object[,,] iarrMap;
        public System.Collections.ArrayList[] ZonesListPoints;
        public System.Collections.ArrayList FreeZonesList; //dwp ведем глобальный список свобоных "вороных"
        public System.Collections.ArrayList MapPointsInVP; 
        public double freePrecentage;
        public double LastPrecentage;
        public double busyPrecentage;
        public int countAttempt;
        public VoronoiPoint LastAddedVP;
        public int[] iarrPrecentageList;
        public int[] iarrThicknessList;
        public bool[] EvaluatedIndices;
        public bool[,] barrAStarVisit;

        public Voronoi vZoneMaker;
        public double dMinDifference = 1.5;

        #region Class Consts
        //number of layers in map
        public const int IMAP_LAYERS = 8;
        //minimum difference for zone precetage for it to be considered as "fit"
        
        #endregion


        /// <summary>
        ///  constructor  - get map size and detrmine map matrix 
        /// </summary>
        /// <param name="eMapSize">map size</param>
        public ObjectsMap( MapSize eMapSize)
        {
           barrAStarVisit = new bool[(int)eMapSize, (int)eMapSize];
            iarrMap = new object[IMAP_LAYERS, (int)eMapSize , (int)eMapSize ];//, IMAP_LAYERS];
            this.eMapSize = eMapSize;
        }
        #region Old Unused Code
        /// <summary>
        /// get an array with square number -converts it from precentage list
        /// </summary>
        /// <param name="ZonesPrecentageList">precentage list</param>
        /// <returns></returns>
        public int[] GetSquareNumberPerZone(int[] ZonesPrecentageList)
        {
            int[] iarrTempSquareNumberPerZone = new int[ZonesPrecentageList.Length];
            int iMapSize = (int)eMapSize;
            int iTotalSquares = iMapSize * iMapSize;
            int iSumTotalSquares = 0;
            //go over zones and calculate square number by precentage And Fill Gaps In End
            for (int i = 0; i < ZonesPrecentageList.Length; i++)
            {
                iarrTempSquareNumberPerZone[i] = ZonesPrecentageList[i] * (iTotalSquares) / 100;
                iSumTotalSquares += iarrTempSquareNumberPerZone[i];
            }

            //add negligible difference
            iarrTempSquareNumberPerZone[iarrTempSquareNumberPerZone.Length - 1] += (iTotalSquares - iSumTotalSquares);


            return iarrTempSquareNumberPerZone;
        }



        /// <summary>
        /// old place object on the map - and put position number in case object is bigger then 1
        /// </summary>
        /// <param name="strObjectName">the name of the object places</param>
        /// <param name="x">x pos</param>
        /// <param name="y">y pos</param>
        /// <param name="z"></param>
        /// <param name="iObjectSize"></param>
        public void PlaceObject(string strObjectName, int x, int y, int z, int iObjectSize)
        {
            if (CheckObjectPlacing(x, y, iObjectSize))
            {
                switch (iObjectSize)
                {
                    case 1:
                        this.iarrMap[(int)MapLayer.Objects, x, y] = strObjectName;
                        break;
                    case 3:
                        this.iarrMap[(int)MapLayer.Objects, x, y] = strObjectName;
                        this.iarrMap[(int)MapLayer.Objects, x + 1, y] = x.ToString() + "," + y.ToString();
                        this.iarrMap[(int)MapLayer.Objects, x - 1, y] = x.ToString() + "," + y.ToString();
                        break;
                    case 4:
                        this.iarrMap[(int)MapLayer.Objects, x, y] = strObjectName;
                        this.iarrMap[(int)MapLayer.Objects, x + 1, y] = x.ToString() + "," + y.ToString();
                        this.iarrMap[(int)MapLayer.Objects, x, y - 1] = x.ToString() + "," + y.ToString();
                        this.iarrMap[(int)MapLayer.Objects, x + 1, y - 1] = x.ToString() + "," + y.ToString();
                        break;
                    case 6:
                        this.iarrMap[(int)MapLayer.Objects, x, y] = strObjectName;
                        this.iarrMap[(int)MapLayer.Objects, x + 1, y] = x.ToString() + "," + y.ToString();
                        this.iarrMap[(int)MapLayer.Objects, x - 1, y] = x.ToString() + "," + y.ToString();
                        this.iarrMap[(int)MapLayer.Objects, x, y + 1] = x.ToString() + "," + y.ToString();
                        this.iarrMap[(int)MapLayer.Objects, x + 1, y + 1] = x.ToString() + "," + y.ToString();
                        this.iarrMap[(int)MapLayer.Objects, x - 1, y + 1] = x.ToString() + "," + y.ToString();
                        break;
                    case 9:
                        this.iarrMap[(int)MapLayer.Objects, x, y] = strObjectName;
                        this.iarrMap[(int)MapLayer.Objects, x + 1, y] = x.ToString() + "," + y.ToString();
                        this.iarrMap[(int)MapLayer.Objects, x - 1, y] = x.ToString() + "," + y.ToString();
                        this.iarrMap[(int)MapLayer.Objects, x, y + 1] = x.ToString() + "," + y.ToString();
                        this.iarrMap[(int)MapLayer.Objects, x + 1, y + 1] = x.ToString() + "," + y.ToString();
                        this.iarrMap[(int)MapLayer.Objects, x - 1, y + 1] = x.ToString() + "," + y.ToString();
                        this.iarrMap[(int)MapLayer.Objects, x, y - 1] = x.ToString() + "," + y.ToString();
                        this.iarrMap[(int)MapLayer.Objects, x + 1, y - 1] = x.ToString() + "," + y.ToString();
                        this.iarrMap[(int)MapLayer.Objects, x - 1, y - 1] = x.ToString() + "," + y.ToString();
                        break;

                }
            }

        }

        /// <summary>
        /// checkes to see if object can be placed in chosen spot
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="iObjectSize"></param>
        /// <returns></returns>
        public bool CheckObjectPlacing(int x, int y, int iObjectSize)
        {
            switch (iObjectSize)
            {
                case 1:
                    if (this.iarrMap[(int)MapLayer.Objects, x, y] != null)
                        return false;
                    break;
                case 3:
                    if (this.iarrMap[(int)MapLayer.Objects, x, y] != null)
                        return false;
                    if (this.iarrMap[(int)MapLayer.Objects, x + 1, y] != null)
                        return false;
                    if (this.iarrMap[(int)MapLayer.Objects, x - 1, y] != null)
                        return false;
                    break;
                case 4:
                    if (this.iarrMap[(int)MapLayer.Objects, x, y] != null)
                        return false;
                    if (this.iarrMap[(int)MapLayer.Objects, x + 1, y] != null)
                        return false;
                    if (this.iarrMap[(int)MapLayer.Objects, x, y + 1] != null)
                        return false;
                    if (this.iarrMap[(int)MapLayer.Objects, x + 1, y + 1] != null)
                        return false;
                    break;
                case 6:
                    if (this.iarrMap[(int)MapLayer.Objects, x, y] != null)
                        return false;
                    if (this.iarrMap[(int)MapLayer.Objects, x + 1, y] != null)
                        return false;
                    if (this.iarrMap[(int)MapLayer.Objects, x - 1, y] != null)
                        return false;
                    if (this.iarrMap[(int)MapLayer.Objects, x, y + 1] != null)
                        return false;
                    if (this.iarrMap[(int)MapLayer.Objects, x + 1, y + 1] != null)
                        return false;
                    if (this.iarrMap[(int)MapLayer.Objects, x - 1, y + 1] != null)
                        return false;
                    break;
                case 9:
                    if (this.iarrMap[(int)MapLayer.Objects, x, y] != null)
                        return false;
                    if (this.iarrMap[(int)MapLayer.Objects, x + 1, y] != null)
                        return false;
                    if (this.iarrMap[(int)MapLayer.Objects, x - 1, y] != null)
                        return false;
                    if (this.iarrMap[(int)MapLayer.Objects, x, y + 1] != null)
                        return false;
                    if (this.iarrMap[(int)MapLayer.Objects, x + 1, y + 1] != null)
                        return false;
                    if (this.iarrMap[(int)MapLayer.Objects, x - 1, y + 1] != null)
                        return false;
                    if (this.iarrMap[(int)MapLayer.Objects, x, y + 1] != null)
                        return false;
                    if (this.iarrMap[(int)MapLayer.Objects, x + 1, y + 1] != null)
                        return false;
                    if (this.iarrMap[(int)MapLayer.Objects, x - 1, y + 1] != null)
                        return false;
                    break;

            }
            return true;

        }

        public void TestVoronoi()
        {
            #region Old Voronoi Test
            //Vector[] DataPoints = new Vector[] { new Vector(1, 1), new Vector(1, 2), new Vector(2, 1), new Vector(2, 2) };

            //VoronoiGraph vg =  Fortune.ComputeVoronoiGraph(DataPoints);


            //StringBuilder sb = new StringBuilder();
            //sb.AppendLine(" ----------------------------------- ");

            //foreach (VoronoiEdge veItem in vg.Edges)
            //{                
            //  //  sb.Append(veItem.FixedPoint.ToString() + " ; ");
            //    //sb.Append(veItem.VVertexA.ToString() + " ; ");
            //    //sb.Append(veItem.VVertexB.ToString() + " ; ");
            //    sb.Append(veItem.DirectionVector.ToString() + " ; ");
            //    //sb.Append ( "LeftData: " + veItem.LeftData.ToString() + " ; ");
            //    //sb.Append ( "RightData: " + veItem.RightData.ToString() + " ; ");
            //}
            //sb.AppendLine(" ----------------------------------- ");



            //foreach (Vector vItem in vg.Vertizes )
            //{
            //    sb.Append(vItem.ToString() + " ; ");
            //    //veItem.LeftData.ToString();
            //    //veItem.RightData.ToString();
            //}
            //sb.AppendLine(" ----------------------------------- ");

            //return sb.ToString();
            #endregion

            


            Voronoi vZoneMaker = new Voronoi();
            VoronoiPoint vpDataPoint;
            //get 15 random points And add them to the voronoi algorithem as data points
            for (int i = 0; i < 15; i++)
            {

                vpDataPoint = VoronoiPoint.GenerateRandomNewPoint(this.eMapSize);
                // vpDataPoint = new VoronoiPoint(3 + i*2 , 4 + i*3);
                vZoneMaker.AddVPoint(vpDataPoint);
            }


            //iterate through the area array and set areas base on voronoi

            for (int i = 0; i < (int)eMapSize; i++)
            {
                for (int j = 0; j < (int)eMapSize; j++)
                {
                    this.iarrMap[(int)MapLayer.VoronoiZones, i, j] = vZoneMaker.AreaCode(new MapPoint(i, j));
                }
            }

            //this.dumbMapToFile();     
        }

        /// <summary>
        /// split the map to zones (insert zone numbe in each square )
        /// </summary>
        /// <param name="ZonesPrecentageList">list of all zones by precantage sum must be 100 -no need to check , check is in template file save</param>
        public void SplitToZones(int[] ZonesPrecentageList)
        {
            int[] iarrSquareNumberPerZone = GetSquareNumberPerZone(ZonesPrecentageList);


            #region FirstZone
            int iSquareLength = RoundDown(Math.Sqrt(iarrSquareNumberPerZone[0]));

            SetSquareZone(iarrSquareNumberPerZone[0], 0, 0, 1);
            SetSquareZone(iarrSquareNumberPerZone[1], 0 + iSquareLength, 0, 2);
            SetSquareZone(iarrSquareNumberPerZone[2], 0, 0 + iSquareLength, 3);
            SetSquareZone(iarrSquareNumberPerZone[3], iSquareLength, iSquareLength, 4);

            #endregion

            #if (DEBUG)
                 dumpMapToFile(MapLayer.VoronoiZones);
            #endif

        }

        /// <summary>
        /// set a square zone
        /// </summary>
        /// <param name="iTotalSquares"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="iZoneIndex"></param>
        public void SetSquareZone(int iTotalSquares, int x, int y, int iZoneIndex)
        {
            int iSquareLength = RoundDown(Math.Sqrt(iTotalSquares));


            //if no room for a square , fabric some other form .
            if ((int)eMapSize - x < iSquareLength)
            {
                PlaceZoneIn(x, y, (int)eMapSize, y, iZoneIndex);


            }
            else
            {
                PlaceZoneIn(x, y, x + iSquareLength, y + iSquareLength, iZoneIndex);
            }

        }

        /// <summary>
        /// set a square zone
        /// </summary>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        /// <param name="iZoneIndex"></param>
        public void PlaceZoneIn(int x1, int y1, int x2, int y2, int iZoneIndex)
        {
            for (int i = x1; i <= x2; i++)
                for (int j = y1; j <= y2; j++)
                {
                    iarrMap[(int)MapLayer.Zones, i, j] = iZoneIndex;//,(int)MapLayer.Zones] = iZoneIndex;
                }
        }

        #endregion



        /// <summary>
        /// round a double down
        /// </summary>
        /// <param name="valueToRound">number to be rounded down</param>
        /// <returns></returns>
        public static int RoundDown(double valueToRound)
        {
            double floorValue = Math.Floor(valueToRound);
            if ((valueToRound - floorValue) > .5)
            {
                return ((int)(floorValue + 1));
            }
            else
            {
                return ((int)floorValue);
            }
        }




        /// <summary>
        /// aid in debugging
        /// </summary>
        public void dumpMapToFile( MapLayer eLayer)
        {
            try
            {
                System.IO.StreamWriter s = System.IO.File.CreateText("MapZones"+eLayer.ToString() +".txt");
                for (int i = 0; i < (int)eMapSize; i++)
                {
                    for (int j = 0; j < (int)eMapSize; j++)
                    {
                        if ((int)iarrMap[(int)eLayer, i, j] == -1)
                        {
                            s.Write("00 ");
                        }
                        else
                        {
                            if ((int)iarrMap[(int)eLayer, i, j] > 9)
                                s.Write(iarrMap[(int)eLayer, i, j].ToString() + " ");
                            else
                                s.Write("0" + iarrMap[(int)eLayer, i, j].ToString() + " ");
                        }
                    }
                    s.WriteLine(string.Empty);
                }
                s.Flush();
                s.Close();
                s.Dispose();
            }
            catch (Exception)
            {
                System.IO.StreamWriter s = System.IO.File.CreateText("MapZones3.txt");
                for (int i = 0; i < (int)eMapSize; i++)
                {
                    for (int j = 0; j < (int)eMapSize; j++)
                    {
                        s.Write(iarrMap[(int)MapLayer.VoronoiZones, i, j].ToString() + " ");

                    }
                    s.WriteLine(string.Empty);
                }
                s.Flush();
                s.Close();
                s.Dispose();

            }
        }


        /// <summary>
        /// aid in debugging
        /// </summary>
        public void dumpAStarToFile()
        {
            try
            {
                System.IO.StreamWriter s = System.IO.File.CreateText("AStar.txt");
                for (int i = 0; i < (int)eMapSize; i++)
                {
                    for (int j = 0; j < (int)eMapSize; j++)
                    {
                        if ( iarrMap[(int)MapLayer.AStarDistance, i, j] != null)
                        {
                        string strNumber =  iarrMap[(int)MapLayer.AStarDistance, i, j].ToString();
                        if ( strNumber.Length != 6 )
                        {
                            int y = 6 - strNumber.Length;
                            for (int x = 0; x < y; x++)
                            {
			                    strNumber = "0" + strNumber;
                            }
                        }
                        
                            s.Write( strNumber + " ");
                        }
                        else
                            s.Write("909090 ");
                    }
                    s.WriteLine(string.Empty);
                }
                s.Flush();
                s.Close();
                s.Dispose();
            }
            catch (Exception)
            {
                System.IO.StreamWriter s = System.IO.File.CreateText("AStar2.txt");
                for (int i = 0; i < (int)eMapSize; i++)
                {
                    for (int j = 0; j < (int)eMapSize; j++)
                    {
                        if ( iarrMap[(int)MapLayer.AStarDistance, i, j] != null)
                        {
                        string strNumber =  iarrMap[(int)MapLayer.AStarDistance, i, j].ToString();
                        if ( strNumber.Length != 6 )
                        {
                            for (int x = 0; x < 6 - strNumber.Length; x++)
                            {
			                    strNumber = "0" + strNumber;
                            }
                        }

                        
                            s.Write( strNumber + " ");
                        }
                        else
                            s.Write("909090 ");
                    }
                    s.WriteLine(string.Empty);
                }
                s.Flush();
                s.Close();
                s.Dispose();

            }
        }

        static string[] BlockStatusToImage = { "X", "I", "?", " ", "." };
        public void dumpBlockMaskToFile()
        {
            if (!dumpBlockMaskToFileImpl(Enum.GetNames(typeof(eBlockStatus)), "MapBlockMask.txt", false)) {
                dumpBlockMaskToFileImpl(Enum.GetNames(typeof(eBlockStatus)), "MapBlockMask2.txt", false);
            }
        }

        public void dumpBlockMaskToFileErgo()
        {
            if (!dumpBlockMaskToFileImpl(BlockStatusToImage, "MapBlockMask.txt", true)) {
                dumpBlockMaskToFileImpl(BlockStatusToImage, "MapBlockMask2.txt", true);
            }
        }

        public bool dumpBlockMaskToFileImpl(string[] images, string fname, bool ergo)
        {
            bool res = false;
            try
            {
                System.IO.StreamWriter s = System.IO.File.CreateText(fname);
                for (int y = ergo ? (int)eMapSize - 1 : 0;
                    ergo ? y >= 0 : y < (int)eMapSize;
                    y = ergo ? y - 1 : y + 1) {
                    for (int x = 0; x < (int)eMapSize; x++) {
                        eBlockStatus st = (eBlockStatus)iarrMap[(int)MapLayer.Blocks, x, y];
                        if (st < 0) {
                            s.Write("# ");
                        }
                        else {
                            s.Write(images[(int)st] + " ");
                        }
                    }
                    s.WriteLine(string.Empty);
                }
                s.Flush();
                s.Close();
                s.Dispose();
                res = true;
            }
            catch (Exception)
            {
                res = false;
            }
            return res;
        }



        public void dumpTerrainsToFile()
        {
            try
            {
                System.IO.StreamWriter s = System.IO.File.CreateText("Terrains.txt");
                for (int i = 0; i < (int)eMapSize; i++)
                {
                    for (int j = 0; j < (int)eMapSize; j++)
                    {

                        s.Write(((int)iarrMap[(int)MapLayer.Terrain, i, j]).ToString() + " ");
                    }
                    s.WriteLine(string.Empty);
                }
                s.Flush();
                s.Close();
                s.Dispose();
            }
            catch (Exception)
            {
                System.IO.StreamWriter s = System.IO.File.CreateText("Terrains2.txt");
                for (int i = 0; i < (int)eMapSize; i++)
                {
                    for (int j = 0; j < (int)eMapSize; j++)
                    {

                        s.Write(((int)iarrMap[(int)MapLayer.Terrain, i, j]).ToString() + " ");
                    }
                    s.WriteLine(string.Empty);
                }
                s.Flush();
                s.Close();
                s.Dispose();

            }
        }

        public void ComputeAllDirections()
        {
            for (int i = 0; i < (int)eMapSize; i++)
            {
                for (int j = 0; j < (int)eMapSize; j++)
                {
                    iarrMap[(int)MapLayer.Directions, i, j] = ComputeDirectionForPoint(new MapPoint(i, j));
                }
            }

        }


        ////array of points which define a round trip around a sqaure to check all its adjacencies 
        //MapPoint[] mpDirections =  { 
        //        new MapPoint(0,1) , new MapPoint (1,1) , new MapPoint (1,0),
        //        new MapPoint(1,-1) , new MapPoint (0,-1) , new MapPoint(-1,-1),
        //        new MapPoint( -1, 0) , new MapPoint(-1,1) 
        //    };

        private ObjectDirection ComputeDirectionForPoint(MapPoint mpDirectionPoint)
        {

            int[] iarrDirectionFarness = new int[4];
            //bool bToReverseDirection = true;
/*dwp!!
            if ((eBlockStatus)iarrMap[(int)MapLayer.Blocks, mpDirectionPoint.x, mpDirectionPoint.y] == eBlockStatus.MaybeBlock)
            {
                //find free and point there
                //compute left free farness
                iarrDirectionFarness[(int)ObjectDirection.Left] = ComputeOpeningFarnessInDirection(mpDirectionPoint, new MapPoint(-1, 0));

                //compute right free farness
                iarrDirectionFarness[(int)ObjectDirection.Right] = ComputeOpeningFarnessInDirection(mpDirectionPoint, new MapPoint(1, 0));

                //compute up free farness
                iarrDirectionFarness[(int)ObjectDirection.Up] = ComputeOpeningFarnessInDirection(mpDirectionPoint, new MapPoint(0, 1));

                //compute down free farness
                iarrDirectionFarness[(int)ObjectDirection.Down] = ComputeOpeningFarnessInDirection(mpDirectionPoint, new MapPoint(0, -1));
                bToReverseDirection = false;
            }

            else
            {
 */
                //find blocks and point there
                //compute left obstacle farness
                iarrDirectionFarness[(int)ObjectDirection.Left] = ComputeObstaclesFarnessInDirection(mpDirectionPoint, new MapPoint(-1, 0));

                //compute right obstacle farness
                iarrDirectionFarness[(int)ObjectDirection.Right] = ComputeObstaclesFarnessInDirection(mpDirectionPoint, new MapPoint(1, 0));

                //compute up obstacle farness
                iarrDirectionFarness[(int)ObjectDirection.Up] = ComputeObstaclesFarnessInDirection(mpDirectionPoint, new MapPoint(0, 1));

                //compute down obstacle farness
                iarrDirectionFarness[(int)ObjectDirection.Down] = ComputeObstaclesFarnessInDirection(mpDirectionPoint, new MapPoint(0, -1));
            /*dwp!!}*/

            //find minumum farness (that reverse direction will be the cell direction )
            int iMinFarness = int.MaxValue;
            ObjectDirection MinimumDirection = ObjectDirection.Down;

            for (int i = 0; i < 4; i++)
            {
                if (iarrDirectionFarness[i] < iMinFarness)
                {
                    iMinFarness = iarrDirectionFarness[i];
                    MinimumDirection = (ObjectDirection)i;                    
                }

                if (iarrDirectionFarness[i] == iMinFarness)
                {
                    //draw 50 chance to set or not for randomness of direction
                    //Random Randomizer.rnd = new Random();

                    if (Randomizer.rnd.Next(1) == 1)
                    {
                        iMinFarness = iarrDirectionFarness[i];
                        MinimumDirection = (ObjectDirection)i;
                    }
                }
            }

            //return backward of minimum direction
            switch (MinimumDirection)
            {
                    case ObjectDirection.Down:
                        return ObjectDirection.Up;
                    case ObjectDirection.Right:
                        return ObjectDirection.Left;
                    case ObjectDirection.Up:
                        return ObjectDirection.Down;
                    case ObjectDirection.Left:
                        return ObjectDirection.Right;
            }
            return ObjectDirection.Up;
        }


        private int ComputeObstaclesFarnessInDirection(MapPoint mpDirectionPoint, MapPoint mpDirectionWay)
        {
            int iFarness = 0;

            MapPoint NextPoint = mpDirectionPoint + mpDirectionWay;
            //check if next point is not a block /end of map  if so return the number added, if not add +1 and continue to next point
            while (IsInsideMapLimit(NextPoint) &&  IsNotBlock ( iarrMap[(int)MapLayer.Blocks, NextPoint.x, NextPoint.y] ) )
            {
                iFarness++;
                NextPoint = NextPoint + mpDirectionWay;

            }

            return iFarness;
        }


        private int ComputeOpeningFarnessInDirection(MapPoint mpDirectionPoint, MapPoint mpDirectionWay)
        {
            int iFarness = 0;

            MapPoint NextPoint = mpDirectionPoint + mpDirectionWay;
            //check if next point is not a block /end of map  if so return the number added, if not add +1 and continue to next point
            while (IsInsideMapLimit(NextPoint) && !IsNotBlock(iarrMap[(int)MapLayer.Blocks, NextPoint.x, NextPoint.y]))
            {
                iFarness++;
                NextPoint = NextPoint + mpDirectionWay;

            }

            return iFarness;
        }



        /// <summary>
        /// check if a point is some sort of block (2 kinds)
        /// </summary>
        /// <param name="eBlock"></param>
        /// <returns></returns>
        private bool IsNotBlock(object eBlock)
        {
            
            if (eBlock != null)
            {
                eBlockStatus ePossibleBlock = (eBlockStatus)eBlock;
                if (ePossibleBlock == eBlockStatus.Block || ePossibleBlock == eBlockStatus.MaybeBlock)
                    return false;
            }
            return true;
        }

        private bool IsInsideMapLimit(MapPoint mpLimitedPoint)
        {
            if (mpLimitedPoint.x > (int)eMapSize - 1)
                return false;
            if (mpLimitedPoint.y > (int)eMapSize - 1)
                return false;
            if (mpLimitedPoint.x < 0)
                return false;
            if (mpLimitedPoint.y < 0)
                return false;
            return true;


        }

        private bool IsInsideMapLimit(int x,int y)
        {
            if (x > (int)eMapSize - 1)
                return false;
            if (y > (int)eMapSize - 1)
                return false;
            if (x < 0)
                return false;
            if (y < 0)
                return false;
            return true;


        }

        /// <summary>
        /// Goes over all borders between zones and place Seperation objects between
        /// </summary>
        public void SetTerrainSeperationObjects()
        {
           int iWidth, TotalWidth, deltaWidth1, deltaWidth2;
           // dwp!! толщина границ настраивается в шаблоне
           for (int xy = 0; xy < (int)eMapSize; xy++) {
               iarrMap[(int)MapLayer.Blocks, 0, xy] = eBlockStatus.Block;  
               iarrMap[(int)MapLayer.Blocks, xy, 0] = eBlockStatus.Block;  
               iarrMap[(int)MapLayer.Blocks, (int)eMapSize - 1, xy] = eBlockStatus.Block;  
               iarrMap[(int)MapLayer.Blocks, xy, (int)eMapSize - 1] = eBlockStatus.Block;  
           }
           for (int x = 1; x < (int)eMapSize - 1   ; x++)
            {
               for (int y = 1; y < (int)eMapSize - 1   ; y++)
                {
                  if ((int)iarrMap[(int)MapLayer.Zones, x, y] == -1) 
                  {     
                    //mark zone for blocking with grass blcok
                    iarrMap[(int)MapLayer.Blocks, x, y] = eBlockStatus.Block;
                    continue;
                  }
                  //check if there is a change in the zone up and right
                  if ((int)iarrMap[(int)MapLayer.Zones, x, y] != (int)iarrMap[(int)MapLayer.Zones, x + 1, y] &&
                      (int)iarrMap[(int)MapLayer.Zones, x + 1, y] != -1
                     )
                   {
                     TotalWidth = Math.Max(iarrThicknessList[(int)iarrMap[(int)MapLayer.Zones, x, y] - 1],
                                           iarrThicknessList[(int)iarrMap[(int)MapLayer.Zones, x + 1, y] - 1]);
                     iWidth = TotalWidth / 2;
                     if (iWidth * 2 != TotalWidth)
                         if (iarrThicknessList[(int)iarrMap[(int)MapLayer.Zones, x, y] - 1] >
                            iarrThicknessList[(int)iarrMap[(int)MapLayer.Zones, x + 1, y] - 1])
                         {
                             deltaWidth1 = -1;
                             deltaWidth2 = 0;
                         }
                         else
                         {
                             deltaWidth1 = 0;
                             deltaWidth2 = 1;
                         }
                     else
                     {
                         deltaWidth1 = 0;
                         deltaWidth2 = 0;
                     }

                     //set a block width for each zone 
                     for (int i = -iWidth + deltaWidth1; i < iWidth + deltaWidth2; i++)
                      {
                       //check if we don't exceed array borders
                       if ((x + i >= 0) && (x + i <= (int)eMapSize - 1)) 
                           iarrMap[(int)MapLayer.Blocks, x + i, y] = eBlockStatus.Block; 
                      }
                      //dwp! вокруг границ ставим защиту для пробивания стен, в последующем алгоритме соединения зон.
                      for (int k = deltaWidth1 - 1; x + k - iWidth>= 0 && k >= deltaWidth1 - 2; k--)
                      {
                       if ((eBlockStatus)iarrMap[(int)MapLayer.Blocks, x + k - iWidth, y] != eBlockStatus.Block)
                           iarrMap[(int)MapLayer.Blocks, x + k - iWidth, y] = eBlockStatus.Preserved;
                      }
                      for (int k = deltaWidth2; x + k + iWidth <= (int)eMapSize - 1 && k < deltaWidth2 + 2; k++)
                      {
                       if ((eBlockStatus)iarrMap[(int)MapLayer.Blocks, x + k + iWidth, y] != eBlockStatus.Block)
                           iarrMap[(int)MapLayer.Blocks, x + k + iWidth, y] = eBlockStatus.Preserved;
                      }
                    }
                   if ((int)iarrMap[(int)MapLayer.Zones, x, y] != (int)iarrMap[(int)MapLayer.Zones, x, y + 1] &&
                       (int)iarrMap[(int)MapLayer.Zones, x, y + 1] != -1
                      )
                    {
                     TotalWidth = Math.Max(iarrThicknessList[(int)iarrMap[(int)MapLayer.Zones, x, y] - 1],
                                           iarrThicknessList[(int)iarrMap[(int)MapLayer.Zones, x, y + 1] - 1]);
                     iWidth = TotalWidth / 2;
                     if (iWidth * 2 != TotalWidth)
                       if (iarrThicknessList[(int)iarrMap[(int)MapLayer.Zones, x, y] - 1] >
                           iarrThicknessList[(int)iarrMap[(int)MapLayer.Zones, x, y + 1] - 1])
                        {
                            deltaWidth1 = -1;
                            deltaWidth2 = 0;
                        }
                        else
                        {
                            deltaWidth1 = 0;
                            deltaWidth2 = 1;
                        }
                        else
                        {
                            deltaWidth1 = 0;
                            deltaWidth2 = 0;
                        }
                      //set a random block width for each zone 
                      for (int i = -iWidth + deltaWidth1; i < iWidth + deltaWidth2; i++)
                       {
                        //check if we don't exceed array borders
                        if ((y + i >= 0)&& (y + i <= (int)eMapSize - 1))
                           iarrMap[(int)MapLayer.Blocks, x, y + i] = eBlockStatus.Block;
                       }
                       //dwp! вокруг границ ставим защиту для пробивания стен, в последующем алгоритме соединения зон.
                       for (int k = deltaWidth1 - 1; y + k - iWidth >= 0 && k >= deltaWidth1 - 2; k--)
                       {
                          if ((eBlockStatus)iarrMap[(int)MapLayer.Blocks, x, y + k - iWidth] != eBlockStatus.Block)
                           iarrMap[(int)MapLayer.Blocks, x, y + k - iWidth] = eBlockStatus.Preserved;
                       }
                       for (int k = deltaWidth2; y + k + iWidth <= (int)eMapSize - 1 && k < deltaWidth1 + 2; k++)
                       {
                           if ((eBlockStatus)iarrMap[(int)MapLayer.Blocks, x, y + k + iWidth] != eBlockStatus.Block)
                           iarrMap[(int)MapLayer.Blocks, x, y + k + iWidth] = eBlockStatus.Preserved;
                       }
                     }
                  }
              }
        }


        /// <summary>
        /// goes over the blocks layer and transforms squares marked for blocking to random block objects in object layer
        /// 
        /// </summary>
        public void PlaceBlockObjects()
        {

            ObjectsReader obrdBlocksReader = new ObjectsReader("Blocks.xml");
            //first get a list of points of all needed block point
            System.Collections.ArrayList arrBlockPoint = GetBlockPoints();

            //get all block object (as of now just for grass terrain)
            ArrayList[] arrAllBlocks =new ArrayList[8] ;

            arrAllBlocks[(int)eTerrain.Grass]      = obrdBlocksReader.GetAllObjectsByCategory(eTerrain.Grass);
            arrAllBlocks[(int)eTerrain.Dirt]       = obrdBlocksReader.GetAllObjectsByCategory(eTerrain.Dirt);
            arrAllBlocks[(int)eTerrain.Sand]       = obrdBlocksReader.GetAllObjectsByCategory(eTerrain.Sand);
            arrAllBlocks[(int)eTerrain.Lava]       = obrdBlocksReader.GetAllObjectsByCategory(eTerrain.Lava);
            arrAllBlocks[(int)eTerrain.Snow]       = obrdBlocksReader.GetAllObjectsByCategory(eTerrain.Snow);
            arrAllBlocks[(int)eTerrain.Conquest]   = obrdBlocksReader.GetAllObjectsByCategory(eTerrain.Conquest);
            arrAllBlocks[(int)eTerrain.Orcish]     = obrdBlocksReader.GetAllObjectsByCategory(eTerrain.Orcish);
            arrAllBlocks[(int)eTerrain.Subterrain] = obrdBlocksReader.GetAllObjectsByCategory(eTerrain.Subterrain);
            //pick some random point and place block object (if possible based on the following restrictions:
            // *Object can only placed on points marked as blocks (any blocks )
            // *Any point placed will be removed from block points if it exist (can not exist if its already been filled)
            // *No rejection to place more then 1 object on a point - block objects can overlap in contrast to all other objects.
            // *

            do
            {

                //select a random point and place a block according to its terrain 

                // Random Randomizer.rnd = new Random();
                int iRandomPoint = Randomizer.rnd.Next(arrBlockPoint.Count);
                MapPoint mpRandomBlockPoint = (MapPoint)arrBlockPoint[iRandomPoint];
                eTerrain BlockType = (eBlockStatus)iarrMap[(int)MapLayer.Blocks, mpRandomBlockPoint.x, mpRandomBlockPoint .y] == eBlockStatus.MaybeBlock ?
                                     GetBlockTypeForPoint(mpRandomBlockPoint) : eTerrain.Subterrain;
                // dwp разделитель между зонами будет всегда eTerrain.Subterrain
                // в пустыне и других типах есть нехорошие блоки.

     
                //examine all objects possible to place , if they place find out their max size and place only random fitting object with max size  
                ArrayList arrFittingBlocks = new ArrayList();

                int iMaxSize = int.MinValue;

                //in a random low chance (1 in 20) force placing a size 1 object for better cosmetic and randomness
                double dRandomLowChance = Randomizer.rnd.NextDouble();

                if (dRandomLowChance < 0.75) /* dwp увеличиваем количество красоты на карте v 1.6.1. */
                {
                    //set selected point for all objects
                    foreach (MapObject moBlockObject in arrAllBlocks[(int)BlockType])
                    {
                        if (moBlockObject.ObjectSize == 1)
                        {                            
                            moBlockObject.BasePoint = mpRandomBlockPoint;                            
                            arrFittingBlocks.Add(moBlockObject);                            
                        }
                    }                    
                }
                else
                {
                    //set selected point for all objects
                    foreach (MapObject moBlockObject in arrAllBlocks[(int)BlockType])
                    {
                        if (moBlockObject.ObjectSize >= iMaxSize)
                        {
                            //try to place it and compile a new list with placeable objects while finding max size

                            moBlockObject.BasePoint = mpRandomBlockPoint;
                            //try to place it and compile a new list with placeable objects while finding max size
                            if (CanPlaceBlockObject(moBlockObject))
                            {
                                iMaxSize = moBlockObject.ObjectSize;
                                arrFittingBlocks.Add(moBlockObject);
                            }
                        }
                    }

                    //in a random low chance put maxsize

                    //remove all objects that below max size
                    for (int i = 0; i < arrFittingBlocks.Count; i++)
                    {
                        if (((MapObject)arrFittingBlocks[i]).ObjectSize < iMaxSize)
                        {
                            arrFittingBlocks.RemoveAt(i);
                            i--; // need to repeat same index again since current object in index changed
                        }
                    }
                }
                //pick a random object withing selected object and place its clone
                int iRandomIndex = Randomizer.rnd.Next(arrFittingBlocks.Count);
                MapObject moRandomBlock = ((MapObject)arrFittingBlocks[iRandomIndex]).Clone();

                    //moRandomBlock.BasePoint = mpRandomBlockPoint;
               
                if (PlaceBlockObject(moRandomBlock))
                {
                    //remove base point (base points must be in the array or its a bug
                    arrBlockPoint.Remove(moRandomBlock.BasePoint);

                    //bWasObjectNotPlaced = false;
                    //if it fits then remove all assaigned points from block array
                    if (moRandomBlock.Area != null)
                    {

                        foreach (MapPoint PArea in moRandomBlock.Area)
                        {
                            MapPoint mpNewArea = TransformPointDirection(PArea, moRandomBlock.Direction);
                            MapPoint mpTranformedArea = moRandomBlock.BasePoint + mpNewArea;

                            if (arrBlockPoint.Contains(mpTranformedArea))
                            {
                                arrBlockPoint.Remove(mpTranformedArea);
                            }
                        }
                    }

                }





                //} while (bWasObjectNotPlaced); // an object with size 1 must fit a point so no neverending loops here

            } while (arrBlockPoint.Count != 0); // keep going until all points are assaigned

        }


        /// <summary>
        /// in difference with normal objects block objects has different checks
        /// needs to check if object in its direction does not exceed normal map boundaries
        /// need to check if object is places on only block area
        /// now in normal object placing need to check if its not on block area 
        /// and ofcourse mark the placing on object map ** now the object can overlap so while adding no deletion of other object existence
        /// </summary>
        /// <param name="moRandomBlock"></param>
        /// <returns></returns>
        private bool PlaceBlockObject(MapObject moObjectToBePlaced)
        {
            
            if (CanPlaceBlockObject(moObjectToBePlaced))
            {
                //mark blocking as done ( change its status - "Done" seems approperate)
              //  TagBlockAsDone(moObjectToBePlaced.BasePoint);

                this.iarrMap[(int)MapLayer.Objects, moObjectToBePlaced.BasePoint.x, moObjectToBePlaced.BasePoint.y] = moObjectToBePlaced;
 
                //place object in object layer
                //if need to use directions
                if (moObjectToBePlaced.Direction != ObjectDirection.Down) //for performance issue avoid unnesecery calculation
                {

                    //MapPoint mpTransformMultiplier = this.mparrDirectionTransformationPoints[(int)moObjectToBePlaced.Direction];
                    if (moObjectToBePlaced.Area != null)
                    {

                        //if one of the squares is taken can't place
                        foreach (MapPoint PArea in moObjectToBePlaced.Area)
                        {
                            MapPoint mpNewArea = TransformPointDirection(PArea, moObjectToBePlaced.Direction);

                           // TagBlockAsDone(moObjectToBePlaced.BasePoint+mpNewArea);

                            //check for place in block area if its null then can't place 
                            if (iarrMap[(int)MapLayer.Objects, moObjectToBePlaced.BasePoint.x + mpNewArea.x, moObjectToBePlaced.BasePoint.y + mpNewArea.y] == null)
                                this.iarrMap[(int)MapLayer.Objects, moObjectToBePlaced.BasePoint.x + mpNewArea.x, moObjectToBePlaced.BasePoint.y + mpNewArea.y] = moObjectToBePlaced.BasePoint.ToString();

                        }
                    }
                }
                else
                {

                    if (moObjectToBePlaced.Area != null)
                    {
                        //if one of the squares is taken can't place
                        foreach (MapPoint PArea in moObjectToBePlaced.Area)
                        {
                            //TagBlockAsDone(moObjectToBePlaced.BasePoint + PArea);
                            //check for place in block area if its null then can't place
                            if (this.iarrMap[(int)MapLayer.Objects, moObjectToBePlaced.BasePoint.x + PArea.x, moObjectToBePlaced.BasePoint.y + PArea.y] == null)
                            {
                                this.iarrMap[(int)MapLayer.Objects, moObjectToBePlaced.BasePoint.x + PArea.x, moObjectToBePlaced.BasePoint.y + PArea.y] = moObjectToBePlaced.BasePoint.ToString();
                            }
                        }
                    }
                }




                return true;
            }
            else
                return false;   
        }


        /// <summary>
        /// needs to check if object in its direction does not exceed normal map boundaries
        /// need to check if object is places on only block area
        /// </summary>
        /// <param name="moObjectToBePlaced">the object that will be checked</param>
        /// <returns></returns>
        public bool CanPlaceBlockObject(MapObject moObjectToBePlaced)
        {


            SetObjectDirection(moObjectToBePlaced);
            ////if origin is taken can't place
            //if (!(this.iarrMap[(int)MapLayer.Objects, moObjectToBePlaced.BasePoint.x, moObjectToBePlaced.BasePoint.y] == null))
            //{
            //    if (moObjectToBePlaced.Type == eObjectType.Guard && this.iarrMap[(int)MapLayer.Objects, moObjectToBePlaced.BasePoint.x, moObjectToBePlaced.BasePoint.y].ToString() == "LogicBlock")
            //    {
            //        return true;
            //    }
            //    if (moObjectToBePlaced.Type == eObjectType.Treasure && this.iarrMap[(int)MapLayer.Objects, moObjectToBePlaced.BasePoint.x, moObjectToBePlaced.BasePoint.y].ToString() == "LogicBlock")
            //    {
            //        return true;
            //    }
            //    return false;
            //}



            if (moObjectToBePlaced.Area != null)
            {
                //if need to use directions
                if (moObjectToBePlaced.Direction != ObjectDirection.Down) //for performance issue avoid unnesecery calculation
                {

                    //MapPoint mpTransformMultiplier = this.mparrDirectionTransformationPoints[(int)moObjectToBePlaced.Direction];
                    if (moObjectToBePlaced.Area != null)
                    {
                        //if one of the squares is taken can't place
                        foreach (MapPoint PArea in moObjectToBePlaced.Area)
                        {
                            MapPoint mpNewArea = TransformPointDirection(PArea, moObjectToBePlaced.Direction);
                            //check outing of boundaries
                            if (moObjectToBePlaced.BasePoint.x + mpNewArea.x > (int)eMapSize - 1 || moObjectToBePlaced.BasePoint.y + mpNewArea.y > (int)eMapSize - 1 || moObjectToBePlaced.BasePoint.x + mpNewArea.x < 0 || moObjectToBePlaced.BasePoint.y + mpNewArea.y < 0)
                                return false;
                            //check for place in block area if its null then can't place 
                            if (this.iarrMap[(int)MapLayer.Blocks, moObjectToBePlaced.BasePoint.x + mpNewArea.x, moObjectToBePlaced.BasePoint.y + mpNewArea.y] != null)
                            {
                                eBlockStatus StatusOfPoint =(eBlockStatus) this.iarrMap[(int)MapLayer.Blocks, moObjectToBePlaced.BasePoint.x + mpNewArea.x, moObjectToBePlaced.BasePoint.y + mpNewArea.y];
                                if (StatusOfPoint != eBlockStatus.Block && StatusOfPoint != eBlockStatus.MaybeBlock)
                                    return false;
                            }
                            else
                                return false;
                        }
                    }
                }
                else
                {

                    if (moObjectToBePlaced.Area != null)
                    {
                        //if one of the squares is taken can't place
                        foreach (MapPoint PArea in moObjectToBePlaced.Area)
                        {
                            //check outing of boundaries

                            if (moObjectToBePlaced.BasePoint.x + PArea.x > (int)eMapSize - 1 || moObjectToBePlaced.BasePoint.y + PArea.y > (int)eMapSize - 1 || moObjectToBePlaced.BasePoint.x + PArea.x < 0 || moObjectToBePlaced.BasePoint.y + PArea.y < 0)
                                return false;

                            //check for place in block area if its null then can't place
                            if (this.iarrMap[(int)MapLayer.Blocks, moObjectToBePlaced.BasePoint.x + PArea.x, moObjectToBePlaced.BasePoint.y + PArea.y] != null)
                            {
                                eBlockStatus StatusOfPoint =(eBlockStatus) this.iarrMap[(int)MapLayer.Blocks, moObjectToBePlaced.BasePoint.x + PArea.x, moObjectToBePlaced.BasePoint.y + PArea.y] ;
                                if (StatusOfPoint != eBlockStatus.Block && StatusOfPoint != eBlockStatus.MaybeBlock)
                                    return false;
                            }
                            else
                                return false;
                        }
                    }
                }
            }

            #region pathway handling not necessery for blocks
            //if (moObjectToBePlaced.PathWays != null)
            //{
            //    if (moObjectToBePlaced.Direction != ObjectDirection.Down) //for performance issue avoid unnesecery calculation
            //    {
            //        //MapPoint mpTransformMultiplier = this.mparrDirectionTransformationPoints[(int)moObjectToBePlaced.Direction];

            //        //if one of the pathway squares is taken can't place
            //        foreach (MapPoint PPathWay in moObjectToBePlaced.PathWays)
            //        {
            //            MapPoint mpNewArea = TransformPointDirection(PPathWay, moObjectToBePlaced.Direction);

            //            if (moObjectToBePlaced.BasePoint.x + mpNewArea.x >= (int)eMapSize || moObjectToBePlaced.BasePoint.y + mpNewArea.y >= (int)eMapSize || moObjectToBePlaced.BasePoint.x + mpNewArea.x < 0 || moObjectToBePlaced.BasePoint.y + mpNewArea.y < 0)
            //                return false;
            //            if (!(this.iarrMap[(int)MapLayer.Objects, moObjectToBePlaced.BasePoint.x + mpNewArea.x, moObjectToBePlaced.BasePoint.y + mpNewArea.y] == null))
            //            {
            //                return false;
            //            }
            //        }
            //    }
            //    else
            //    {
            //        //if one of the pathway squares is taken can't place
            //        foreach (MapPoint PPathWay in moObjectToBePlaced.PathWays)
            //        {
            //            if (moObjectToBePlaced.BasePoint.x + PPathWay.x >= (int)eMapSize || moObjectToBePlaced.BasePoint.y + PPathWay.y >= (int)eMapSize || moObjectToBePlaced.BasePoint.x + PPathWay.x < 0 || moObjectToBePlaced.BasePoint.y + PPathWay.y < 0)
            //                return false;
            //            if (!(this.iarrMap[(int)MapLayer.Objects, moObjectToBePlaced.BasePoint.x + PPathWay.x, moObjectToBePlaced.BasePoint.y + PPathWay.y] == null))
            //            {
            //                return false;
            //            }
            //        }
            //    }
            //}
            #endregion
            //if all is well ok to place..
            return true;

        }

        /// <summary>
        /// sets status as done
        /// </summary>
        /// <param name="moRandomBlock">point to tag</param>
        private void TagBlockAsDone(MapPoint mpBlockPoint)
        {
            iarrMap[(int)MapLayer.Blocks, mpBlockPoint.x, mpBlockPoint.y] = eBlockStatus.Done;
        }




        /// <summary>
        /// goes over all block array and adds every point marked for blocking
        /// </summary>
        /// <returns>array of points marked for blocking </returns>
        private System.Collections.ArrayList GetBlockPoints()
        {
            ArrayList BlockPoints = new ArrayList();
            //dwp. Новая настройка по установке блоков внутри зоны.
            bool NotuseMaybeblock = Settings.Default.NoBlocks;

            //go over block array and get all places that are marked for blocking 
            //(they will be of type eterrain - describing their terrain type
           
            for (int i = 0; i < (int)eMapSize ; i++)
            {
                for (int j = 0; j < (int)eMapSize ; j++)
                {
                    
                    //if its a block
                    if (iarrMap[(int)MapLayer.Blocks, i, j] is eBlockStatus)
                    {
                        if ((eBlockStatus)iarrMap[(int)MapLayer.Blocks, i, j] == eBlockStatus.Block || 
                            (!NotuseMaybeblock && (eBlockStatus)iarrMap[(int)MapLayer.Blocks, i, j] == eBlockStatus.MaybeBlock))
                            BlockPoints.Add(new MapPoint(i, j));
                    }
                }
            }

            return BlockPoints;
        }




        private eTerrain GetBlockTypeForPoint(MapPoint mpBlockPoint)
        {
            eTerrain ZoneTerrain = this.thSelectedTemplate.GetZoneTypeForZone((int)iarrMap[(int)MapLayer.Zones, mpBlockPoint.x, mpBlockPoint.y] - 1);
            //if its native then get the terrain set in table
            if (ZoneTerrain == eTerrain.Native)
                return (eTerrain)thSelectedTemplate.iarrTerrainTable[(int)iarrMap[(int)MapLayer.Zones, mpBlockPoint.x, mpBlockPoint.y] - 1];
            return ZoneTerrain;

        }


        public void FlagZonesForDebug()
        {


            for (int i = 1; i < this.thSelectedTemplate.ZoneNumber + 1; i++)
            {
                for (int j = 1; j < i + 1; j++)
                {
                    MapObject mpobFlagSeperationObject = new ObjectsReader("OtherObjects.xml").GetObjectByName("Witch_Hut");

                    MapPoint mpFlagSeperationObject = GetFreeFittingPointInZone(i, mpobFlagSeperationObject);

                    mpobFlagSeperationObject.BasePoint = mpFlagSeperationObject;
                   // mpobTwoWayPortal.ObjectSpacificProperties.Add(eObjectsSpacificProperties.TwoWayPortalGroupID.ToString(), ObjectSpacificPropertiesHelper.GetNextTwoWayPortalGroupId().ToString());
                    this.PlaceObject(mpobFlagSeperationObject, i);   //if false it placing was failed;
                        //throw new Exception("Can't place object error in algorithem ");
                }

            }
        }

        /// <summary>
        /// Writes the voronoi layer the zone number based on points
        /// </summary>
        public void TransformVoronoiPointsToAreas() //dwp меняем всю функцию
        {
            #region put voronoi area to map and output to file
            //iterate through the area array and set areas base on voronoi
            VoronoiPoint CurVp;
            MapPoint     CurMp;
            ArrayList    Curlist;
            int          CurArea;

            for (int k = 0; k < this.MapPointsInVP.Count; k++) {
                Curlist = (ArrayList)this.MapPointsInVP[k];
                Curlist.Clear();
            }

            for (int i = 0; i < (int)eMapSize; i++)
            {
                for (int j = 0; j < (int)eMapSize; j++)
                {
                    CurMp = new MapPoint(i, j);
                    CurArea = vZoneMaker.AreaCode(CurMp);
                    this.iarrMap[(int)MapLayer.VoronoiZones, i, j] = CurArea;
                    for (int k = 0; k < this.MapPointsInVP.Count; k++)
                    {
                        CurVp = (VoronoiPoint)vZoneMaker.VoronoiPoints[k];
                        if (CurVp.iAreaCode == CurArea)
                        {
                          if(CurVp.x != i || CurVp.y != j){
                            Curlist = (ArrayList)this.MapPointsInVP[k];
                            Curlist.Add(CurMp);
                          }
                          k = this.MapPointsInVP.Count;
                        }
                    }
                }
            }

            //this.dumpMapToFile(MapLayer.VoronoiZones); for testing uses
            #endregion

        }


        /// <summary>
        /// Will generate zones ,starting with random points and then via the algorithem will assaign areas to zones
        /// </summary>
        public string ZonesGenerator()
        {
            const bool ToRandomizePoints = true ;

            vZoneMaker = new Voronoi();
            VoronoiPoint vpDataPoint;
            eZonesStatus eStatus;

            this.FreeZonesList = new System.Collections.ArrayList(); //dwp новый набор "вороных"
            this.MapPointsInVP = new System.Collections.ArrayList(); 
            if (ToRandomizePoints)
            {
                #region Generate 30 priliminary zones
                //get 30 random points And add them to the voronoi algorithem as data points
                // увеличиваем до 50, так как расчет связанности стал лучше
                for (int i = 0; i < 30; i++)
                {

                    vpDataPoint = VoronoiPoint.GenerateRandomNewPoint(this.eMapSize);
                    // vpDataPoint = new VoronoiPoint(3 + i*2 , 4 + i*3);
                    try
                    {
                        vZoneMaker.AddVPoint(vpDataPoint);
                        this.FreeZonesList.Add((VoronoiPoint)vpDataPoint); //dwp новая точка добавляется к свободным
                        this.MapPointsInVP.Add(new ArrayList());
                    }
                    catch (VoronoiPointExistException) // point generated already exist
                    {
                        i = i - 1; // generate again the point 
                    }
                }
                #endregion
            }
            else
            {
                string strPointInput = "44,68;117,32;47,1;17,61;173,106;145,11;33,116;29,43;108,76;79,42;89,88;146,65;16,41;41,64;95,129;167,16;17,113;49,108;90,92";
                string[] strPoints = strPointInput.Split(';');
                foreach (string strPoint in strPoints)
                {
                    vpDataPoint = new VoronoiPoint(Convert.ToInt32( strPoint.Split(',')[0] ), Convert.ToInt32( strPoint.Split(',')[1] ));
                    vZoneMaker.AddVPoint(vpDataPoint);
                    this.FreeZonesList.Add((VoronoiPoint)vpDataPoint); //dwp новая точка добавляется к свободным
                    this.MapPointsInVP.Add(new ArrayList());
                }

            }

            //Puts voronoi zone number in voronoi map layer
            TransformVoronoiPointsToAreas();
            freePrecentage = 100.0;
            busyPrecentage = 0;
            countAttempt++;

#if (DEBUG)
            this.dumpMapToFile(MapLayer.VoronoiZones);
#endif

            //get min difference from gui
            dMinDifference = Convert.ToDouble(Program.frmMainMenu.msktxtCompatiblityFactor.Text , System.Globalization.CultureInfo.InvariantCulture); 

            //read from selected template zones data (array of all zone precentages)
            //todo:add error handling in case file don't exist

            //get zone precentage list
            iarrPrecentageList = thSelectedTemplate.GetZonePrecentageList();
            iarrThicknessList  = thSelectedTemplate.GetZoneBordersThickness();
            //int[,] iarr = thSelectedTemplate.GetConnectionList();

            //initilize list of points that defines zone
            this.ZonesListPoints = new System.Collections.ArrayList[iarrPrecentageList.Length];

            //initilize list of evaluated zones array
            this.EvaluatedIndices = new bool[iarrPrecentageList.Length];
            //set all values to false
            for (int i = 0; i < EvaluatedIndices.Length; i++)
            {
                this.EvaluatedIndices[i] = false;
            }

            //dwp. максимально удаленные друг от друга точки от 2 до 4 в зависимости от количества игроков
            vZoneMaker.ComputeFarestPoints();

            //dwp оценка выбраных зон в качестве стартовых
            VoronoiPoint vpPointToEvaluate, vpFirstPrevPoint=null, vpSecondPrevPoint=null;

            for (int i = 0; i < thSelectedTemplate.IPLAYERNUMBER; i++)
            {
                int iStartingZoneIndex = thSelectedTemplate.getStartingZoneIndex(i + 1);

                eStatus = this.TryToEvaluate(vZoneMaker.FarestPoint(i + 1), iarrPrecentageList[iStartingZoneIndex - 1], iStartingZoneIndex - 1, true);
                if (i == 0) vpFirstPrevPoint = LastAddedVP;
                if (i == 1) vpSecondPrevPoint = LastAddedVP;
                if (eStatus == eZonesStatus.Undefineable)
                    return "Failed";
                busyPrecentage = busyPrecentage + iarrPrecentageList[iStartingZoneIndex - 1];
            }
            if (vpFirstPrevPoint == null) vpFirstPrevPoint = vZoneMaker.FarestPoint(1);
            if (vpSecondPrevPoint == null) vpSecondPrevPoint = vZoneMaker.FarestPoint(2);
            #region Eval remaining zones

            //now until we finish points get an adjacent free voronoi point to none voronoi latest evaluated zone once in first zone path then in 2nd path
            //And get next connected free zone or any free zone and evaulate it next.

            //this list will determine the order in which zones will be evaluated..
            int[] iarrEvaluatioinOrderListIndecies = thSelectedTemplate.GenerateEvaluationOrderList();


            for (int i = thSelectedTemplate.IPLAYERNUMBER; i < iarrEvaluatioinOrderListIndecies.Length; i++)
            {
                if (//dwp использовали места больше чем разместили зон
                    freePrecentage + //2*dMinDifference  // не допускаем убегание "вперед" более двойной погрешности
                    (iarrEvaluatioinOrderListIndecies.Length - i)*dMinDifference  // раньше было по количеству оставшихся неразмещенных зон, что много 
                    < 100 - busyPrecentage) {
                    return "Failed"; //dwp места точно не хватит - ускоряем выбор следующего варианта
                }
                //first for first zone and its adjacencies group
                vpPointToEvaluate = GetAdjacentFreeZone(vpFirstPrevPoint);
                if (vpPointToEvaluate == null) vpPointToEvaluate = GetRandomFreeZone();
                if (vpPointToEvaluate == null) return "Failed";
                eStatus = this.TryToEvaluate(vpPointToEvaluate, iarrPrecentageList[iarrEvaluatioinOrderListIndecies[i] - 1], iarrEvaluatioinOrderListIndecies[i] - 1, false);
                busyPrecentage = busyPrecentage + iarrPrecentageList[iarrEvaluatioinOrderListIndecies[i] - 1];
                vpFirstPrevPoint = LastAddedVP;
                if (eStatus == eZonesStatus.Undefineable ||
                    //dwp использовали места больше чем разместили зон
                    freePrecentage + //2*dMinDifference // не допускаем убегание "вперед" более двойной погрешности 
                    (iarrEvaluatioinOrderListIndecies.Length - i)*dMinDifference // раньше было по количеству оставшихся неразмещенных зон, что много 
                    <  100 - busyPrecentage) {
                    return "Failed"; //dwp места точно не хватит - ускоряем выбор следующего варианта
                }
                i++;
                if (i < iarrEvaluatioinOrderListIndecies.Length)
                {
                    //then for second zone and its adjacencies group
                    vpPointToEvaluate = GetAdjacentFreeZone(vpSecondPrevPoint);
                    if (vpPointToEvaluate == null) vpPointToEvaluate = GetRandomFreeZone();
                    if (vpPointToEvaluate == null) return "Failed";
                    eStatus = this.TryToEvaluate(vpPointToEvaluate, iarrPrecentageList[iarrEvaluatioinOrderListIndecies[i] - 1], iarrEvaluatioinOrderListIndecies[i] - 1, false);
                    if (eStatus == eZonesStatus.Undefineable)
                        return "Failed";
                    busyPrecentage = busyPrecentage + iarrPrecentageList[iarrEvaluatioinOrderListIndecies[i] - 1];
                    vpSecondPrevPoint = LastAddedVP;
                }

            }

            #endregion
            this.TransformZonesListPointToFinalAreas();
             
            #region old code
               
            ////                         (index is +1)
            //while (eFirstZone != eZonesStatus.ZoneDefined) 
            //{
            //    //change this to a point that changes with each evaluate
                
            //    if (eFirstZone == eZonesStatus.NewPointAdded)
            //    {
            //        //check if by adding the new point , the current stracture is still good 
            //        if (CheckCurrentZoneSetup())
            //        {
            //            //get adjacent point
            //            //this. vpIteratePoint
            //        }
            //    }
            //    else if ( eFirstZone == eZonesStatus.Undefineable)
            //    {
            //        //return back and start alrogithem again
            //        return "Failed"; 

            //    }
            //}
#endregion

            #if (DEBUG)
                #warning Debugging Zones option is enabled
                this.dumpMapToFile(MapLayer.Zones);
                this.dumpZonesInfo();
            #endif
            return "Success";
        }


        /// <summary>
        /// dump zones info for debug purposes
        /// </summary>
        private void dumpZonesInfo()
        {
            try
            {
                System.IO.StreamWriter s = System.IO.File.CreateText("MapInfoZones.txt");

                s.WriteLine("Begin Voronoi Zones Info ");
                foreach (VoronoiPoint vpSourcePoint in this.vZoneMaker.VoronoiPoints)
                {
                    double AreaPrecentageSize = this.GetZonePrecentage(vpSourcePoint);
                    s.WriteLine("Code :" + vpSourcePoint.iAreaCode.ToString() + ",PrecentageSize:" + AreaPrecentageSize.ToString());
                }
                s.WriteLine("End Voronoi Zones Info ");

                s.WriteLine("*************************************");
                s.WriteLine("Begin Voronoi Zones Assaignment Info ");

                for ( int i = 0 ; i < this.ZonesListPoints.Length ; i++ )
                {
                    s.WriteLine("Zone Index : " + (i + 1).ToString());
                    double iSum = 0;
                    foreach (VoronoiPoint vpAssaignZone in this.ZonesListPoints[i])
                    {                        
                        double AreaPrecentageSize = this.GetZonePrecentage(vpAssaignZone);
                        iSum += AreaPrecentageSize;
                        s.WriteLine("Code: " + vpAssaignZone.iAreaCode + " ,Size: " + AreaPrecentageSize.ToString());
                    }
                    s.WriteLine("TotalSize: " + iSum.ToString());
                }
                s.WriteLine("End Voronoi Zones Assaignment Info ");
                s.WriteLine("Voronoi Zones Points:");

                foreach (VoronoiPoint vp in vZoneMaker.VoronoiPoints)
                {
                    s.Write(vp.x.ToString() + "," + vp.y.ToString() + ";");
                }

                s.WriteLine(string.Empty);
                s.WriteLine("END OF Voronoi Zones Points");

                s.Flush();
                s.Close();
                s.Dispose();
            }
            catch (Exception)
            {
                System.IO.StreamWriter s = System.IO.File.CreateText("MapInfoZones3.txt");
                s.WriteLine("Begin Voronoi Zones Info ");
                foreach (VoronoiPoint vpSourcePoint in this.vZoneMaker.VoronoiPoints)
                {
                    double AreaPrecentageSize = this.GetZonePrecentage(vpSourcePoint);
                    s.WriteLine("Code :" + vpSourcePoint.iAreaCode.ToString() + ",PrecentageSize:" + AreaPrecentageSize.ToString());
                }
                s.WriteLine("End Voronoi Zones Info ");

                s.WriteLine("*************************************");
                s.WriteLine("Begin Voronoi Zones Assaignment Info ");

                for (int i = 0; i < this.ZonesListPoints.Length; i++)
                {
                    s.WriteLine("Zone Index : " + (i + 1).ToString());
                    double iSum = 0;
                    foreach (VoronoiPoint vpAssaignZone in this.ZonesListPoints[i])
                    {
                        double AreaPrecentageSize = this.GetZonePrecentage(vpAssaignZone);
                        iSum += AreaPrecentageSize;
                        s.WriteLine("Code: " + vpAssaignZone.iAreaCode + " ,Size: " + AreaPrecentageSize.ToString());
                    }
                    s.WriteLine("TotalSize: " + iSum.ToString());
                }
                s.WriteLine("End Voronoi Zones Assaignment Info ");

                s.Flush();
                s.Close();
                s.Dispose();

            }
        }


        /// <summary>
        /// transforms the point list to final areas
        /// </summary>
        private void TransformZonesListPointToFinalAreas()
        {

            for (int i = 0; i < (int)eMapSize; i++)
            {
                for (int j = 0; j < (int)eMapSize; j++)
                {
                    for (int Index = 0; Index < this.ZonesListPoints.Length; Index++)
                    {
                        foreach (VoronoiPoint vpSubZone in this.ZonesListPoints[Index])
                        {
                            if ((int)this.iarrMap[(int)MapLayer.VoronoiZones, i, j] == vpSubZone.iAreaCode)
                            {
                                this.iarrMap[(int)MapLayer.Zones, i, j] = Index + 1;
                            }
                        }
                    }

                }
            }



            for (int i = 0; i < (int)eMapSize; i++)
            {
                for (int j = 0; j < (int)eMapSize; j++)
                {
                    if (this.iarrMap[(int)MapLayer.Zones, i, j] == null)
                    {
                        this.iarrMap[(int)MapLayer.Zones, i, j] = -1;
                    }
                }
            }

        }


        /// <summary>
        /// initilize zones list points array and evauluates the zone ,updates evaluat index in proper array
        /// </summary>
        /// <param name="vpStartZone">Origin start point to evaluate from</param>
        /// <param name="iZonePrecentage">Size goal to achieve</param>
        /// <param name="iZoneIndex">index of zone evaluated</param>
        /// <returns>status of success</returns>
        public eZonesStatus TryToEvaluate(VoronoiPoint vpStartZone, int iZonePrecentage, int iZoneIndex, bool BestAccuracy)
        {
            //initilize zone list points arraylist
            this.ZonesListPoints[iZoneIndex] = new System.Collections.ArrayList();

            eZonesStatus eZoneEvaluation = EvaluateAsZone(vpStartZone, iarrPrecentageList[iZoneIndex], iZoneIndex + 1, BestAccuracy);

            if (eZoneEvaluation == eZonesStatus.Undefineable)
            {
                //need repeat this method/algorithem
                //maybe delete evaluate data
                return eZoneEvaluation;
            }

            EvaluatedIndices[iZoneIndex] = true;
            return eZoneEvaluation;
        }


        /// <summary>
        /// this method goes over the ZonesListPoints which defines the zones based on the voronoi zones
        /// and checks whether the addition of the last point violated the zone size restriction
        /// </summary>
        /// <param name="iLastZoneIndexToCheck">last index to check zone setup</param>
        /// <returns>whether change in zones violated zone size restrictions or not</returns>
        public bool CheckCurrentZoneSetup(int iLastZoneIndexToCheck)
        {
            int iZoneIndex = 0;
            for (int i = 0; i < iLastZoneIndexToCheck; i++)
            {

                if (ZonesListPoints[i] != null)
                {
                    double dTotalZonePrecentage = 0;
                    foreach (VoronoiPoint vpItem in ZonesListPoints[i])
                    {
                        dTotalZonePrecentage += GetZonePrecentage(vpItem);
                    }

                    if (Math.Abs(dTotalZonePrecentage - this.iarrPrecentageList[iZoneIndex]) > dMinDifference + 1)
                        return false;
                    iZoneIndex++;
                }
            }
            return true;                          

        }

        /// <summary>
        /// Gets an area code and return its zone precetange 
        /// </summary>
        /// <param name="iAreaCode">code in voronoi code layer</param>
        /// <returns></returns>
        public double GetZonePrecentage(VoronoiPoint iVP)
        {
            int        vpindex = vZoneMaker.VoronoiPoints.IndexOf(iVP);
            ArrayList  CurList = (ArrayList)this.MapPointsInVP[vpindex];
            return (double)(((double)(CurList.Count + 1)/((int)eMapSize * (int)eMapSize)) * 100);
        }

        /// <summary>
        /// Will set point as zone , checks zone within const a range if smaller - add neighbour zone until is bigger ,if bigger split till range is ok
        /// meaning will translate point to a zone and will split/create other zones in the process
        /// </summary>
        /// <param name="vpZonePoint">a point representing a voronoi zone</param>
        /// <param name="iZonePrecentage">precentage of zone to set as a zone</param>
        public eZonesStatus EvaluateAsZone(VoronoiPoint vpZonePoint, int iZonePrecentage , int iZoneIndex, bool BestAccuracy)
        {
           if (vpZonePoint == null)
                return eZonesStatus.Undefineable;

           double dVoronoiSize = GetZonePrecentage(vpZonePoint);

           if (this.ZonesListPoints[iZoneIndex - 1].Count != 0) //if more zones define this zone then add them to total
           {                                                    //its 1 because first element is total zone precentage
               foreach (VoronoiPoint vpItem in this.ZonesListPoints[iZoneIndex - 1])
               {
                  dVoronoiSize += GetZonePrecentage(vpItem);
               }
           }
           //if entire zone is bigger or smaller withing the min diff definition (2)
           //dwp для стартовых зон выбираем максимальную точность по размеру 0.5%, точнее думаю не стоит уже
           if ((Math.Abs(dVoronoiSize - iZonePrecentage) < (BestAccuracy ? 0.5 : dMinDifference)) &&
                (dVoronoiSize >= dMinDifference)) //dwp. зона не должна стать меньше самой погрешности
           {
               #region not needed
               //set zone by going over vornoi area and adding its zone index ,in primary zone array
               //SetAsZone(vpZonePoint, iZoneIndex);
               #endregion
               //add point to primary zone list for current zone index
               this.ZonesListPoints[iZoneIndex - 1].Add(vpZonePoint);
               this.FreeZonesList.Remove(vpZonePoint); //dwp удаляем занятую точку
               freePrecentage = freePrecentage - dVoronoiSize;
               LastPrecentage = dVoronoiSize;
               LastAddedVP = vpZonePoint;
           }
           else
           {
               if (dVoronoiSize - iZonePrecentage < 0) 
               // if zone precentage is bigger that means we need to add zones to reach size
               {
                   //so get adjacent zone point , add current to zone list as it is smaller then needed and will be a part of the zone
                   //and continue until zone is evaluated

                   //first add point to list as it will be included in the zone and is not free for use
                   this.ZonesListPoints[iZoneIndex - 1].Add(vpZonePoint);
                   this.FreeZonesList.Remove(vpZonePoint); //dwp удаляем занятую точку
                   // VoronoiPoint vpAdjacent = GetAdjacentFreeZoneForEntireZoneGroup(vpZonePoint);//Gets first free zone

                   VoronoiPoint vpAdjacent = GetAdjacentFreeZone(vpZonePoint);//Gets first free zone
                  
                   //todo:unmark next 2 rows and handle if no adjacenies found
                   if (vpAdjacent == null)//if no adjacent zones get any free zones and continue algorithem
                      vpAdjacent = GetAdjacentFreeZoneForEntireZoneGroup(vpZonePoint,iZoneIndex - 1);
                   
                   if (vpAdjacent == null)//if no free zones at all - can't be error in algorithem
                       return eZonesStatus.Undefineable;
                  
                       //throw new Exception("No More Free Zones Error In Algorithem");
                   
                   eZonesStatus eAdjacentResult = EvaluateAsZone(vpAdjacent, iZonePrecentage, iZoneIndex, BestAccuracy) ;
                   //if (eAdjacentResult == eZonesStatus.ZoneDefined)
                   //{
                   //    if (CheckCurrentZoneSetup(iZoneIndex - 1))
                   //        return eZonesStatus.ZoneDefined;
                   //    else
                   //        return eZonesStatus.Undefineable;
                   //}
                   return eAdjacentResult;
               }
               else  // (its smaller) which means zone is too big so need to add a point to split it so its small enough
               {
                   //bool bKeepGeneratingNewPoint = true;
                   //so add the point to voronoi areas (not evaluate areas) and then evaluate if stracture is still ok

                   try
                   {
                       //so get a random point to split the zone (the point needs to be not between the point of current zone and last added adjacent zone )
                       VoronoiPoint vpSplitPoint = SelectRandomPointInVZone(vpZonePoint, iZoneIndex);
                       this.vZoneMaker.AddVPoint(vpSplitPoint);
                       this.FreeZonesList.Add(vpSplitPoint); //dwp добавили новый свободный "вороной"
                       this.MapPointsInVP.Add(new ArrayList());
                       //bKeepGeneratingNewPoint = false;
                   }
                   catch (VoronoiPointExistException)
                   {
                       return eZonesStatus.Undefineable;
                   }

                   //no need to add since we split we need to keep evaluating the point

                   //restructure the voronoi map layer with the new added point
                   TransformVoronoiPointsToAreas();

                   //now check if still current structure is ok (check only until current zone placement ) 

                   if (!CheckCurrentZoneSetup(iZoneIndex - 1)) //if test is bad return false
                       return eZonesStatus.Undefineable;


                   //return eZonesStatus.ZoneDefined;
                   eZonesStatus eAdjacentResult = EvaluateAsZone(vpZonePoint, iZonePrecentage, iZoneIndex, BestAccuracy);

                   return eAdjacentResult;
               }
           }
           return eZonesStatus.ZoneDefined;
        }


        /// <summary>
        /// gets the last added point in evaluation algorithem
        /// </summary>
        /// <returns>last point</returns>
        public VoronoiPoint GetLastAddedPoint(int iLastIndex)
        {
            //int i = this.ZonesListPoints[iLastIndex].Capacity;

            if (this.ZonesListPoints[iLastIndex - 1].Count == 0)
                return null;
            
            return (VoronoiPoint)this.ZonesListPoints[iLastIndex - 1][this.ZonesListPoints[iLastIndex - 1].Count - 1 ];
        }

        // улучшеная проверка связанности соседней зоны, со списком всех точек размещенных вороных зон
        public bool IsAdjacentForEntireZoneGroup(ArrayList Areas, VoronoiPoint vpNext, out int HowAdjacentpoint)
        {
            int iAdjacenciesCount, totalAdjacenciesCount = 0;
            MapPoint CurMp;

            for (int i = 0; i < Areas.Count; i++)
            {
                CurMp = (MapPoint)Areas[i];
                if (CurMp.x >= 1 && CurMp.y >= 1 &&
                    CurMp.x < (int)eMapSize - 1 && CurMp.y < (int)eMapSize - 1)
                {
                    iAdjacenciesCount = 0;
                    //array of points which define a round trip around a sqaure to check all its adjacencies 
                    MapPoint[] arrmp =  {
                            new MapPoint(0,1) , new MapPoint (1,1) , new MapPoint (1,0),
                            new MapPoint(1,-1) , new MapPoint (0,-1) , new MapPoint(-1,-1),
                            new MapPoint( -1, 0) , new MapPoint(-1,1)
                        };
                    foreach (MapPoint mpItem in arrmp)
                    {
                        //if one of the surrounding sqaures belongs to questioned zone then raise counter
                        // if (IsInsideMapLimit(i + mpItem.x , j + mpItem.y) )
                        // {
                        if ((int)this.iarrMap[(int)MapLayer.VoronoiZones, CurMp.x + mpItem.x, CurMp.y + mpItem.y] == vpNext.iAreaCode)
                        {
                            iAdjacenciesCount++;
                        }
                        // }    
                    }
                    if (iAdjacenciesCount >= 3) totalAdjacenciesCount++;
                }
            }

            //check if point have 6 straight adjacencies in this count if true return true else flase
            HowAdjacentpoint = totalAdjacenciesCount;
            if (totalAdjacenciesCount < I_ADJACENCIES_FACTOR)
                return false;
            else
                return true;
        }
        /// <summary>
        /// returns an adjacent zone to the zone given
        /// adjacent means there is a connection path of 
        /// </summary>
        /// <param name="vpCenterZone">zone given</param>
        /// <returns>adjacent free zone or null if non exist</returns>
        public VoronoiPoint GetAdjacentFreeZone(VoronoiPoint vpCenterZone) 
        {
            //System.Collections.ArrayList arrlstFreeZones = GetFreeZonesList();//dwp замена на глобальный список

            //go over all free zones and check if zone is adjacent(returns first zone match )
            // dwp!! возвращаем  в качестве сопряженной зоны для расширения , наиболее сопряженную
            int MaxAPoints = 0, CurAPoints;
            VoronoiPoint BestAZone = null;

            foreach (VoronoiPoint vpZone in this.FreeZonesList)//dwp глобальный список
            {
                if (IsAdjacent(vpCenterZone, vpZone, out CurAPoints))
                {
                    if (CurAPoints > MaxAPoints)
                    {
                        BestAZone = vpZone;
                        MaxAPoints = CurAPoints;
                    }
                }
            }
            return BestAZone;
        }


        /// <summary>
        /// will return a free adjacent zone for every zone (returns just 1 -first zone) besides the zone already checked..
        /// </summary>
        /// <param name="iZoneIndex">Zone not to get a free zone for</param>
        /// <returns>vp point of the free adjacent zone</returns>
        /// процедура будет искать связанную зону не только к одной из зон группы(было ранее), а ко всей группе
        public VoronoiPoint GetAdjacentFreeZoneForEntireZoneGroup(VoronoiPoint vpCenterZone,int iZoneIndex)
        {
            int MaxAPoints = 0, CurAPoints;
            VoronoiPoint BestAZone = null;
            int vpindex = vZoneMaker.VoronoiPoints.IndexOf(vpCenterZone);
            ArrayList CurList = new ArrayList();
            CurList.AddRange((ArrayList)this.MapPointsInVP[vpindex]);

            foreach (VoronoiPoint vpSourceZone in ZonesListPoints[iZoneIndex])
            {
                vpindex = vZoneMaker.VoronoiPoints.IndexOf(vpSourceZone);
                CurList.AddRange((ArrayList)this.MapPointsInVP[vpindex]);
            }

            foreach (VoronoiPoint vpZone in this.FreeZonesList)
            {
              if(IsAdjacentForEntireZoneGroup(CurList, vpZone, out CurAPoints))
              {
                    if (CurAPoints > MaxAPoints)
                    {
                        BestAZone = vpZone;
                        MaxAPoints = CurAPoints;
                    }
              }
            }
            CurList.Clear();
            return BestAZone;
        }

        //will determine number of common squares for zones to be defined as adjacent
        //dwp так как алгоритм пробивания усилен. делаем достаточным для пробивания 10 точек
        // у автора было 30, ставил 10 оказалось мало, поставлю 20 - зоны должны быть сильней связаны
        //dwp увеличим фактор связанности немного 
        public const int I_ADJACENCIES_FACTOR = 20; 

        /// <summary>
        /// Gets 2 zones and check if there is atleast 8 sQuares adjacent betWeen zones
        /// </summary>
        /// <param name="vpSource">first zone </param>
        /// <param name="vpNext">2nd Zone</param>
        /// <returns>true if adjacent , false if no</returns>
        public bool IsAdjacent(VoronoiPoint vpSource, VoronoiPoint vpNext, out int HowAdjacentpoint)
        {
            int iAdjacenciesCount,totalAdjacenciesCount = 0;
            int vpindex = vZoneMaker.VoronoiPoints.IndexOf(vpSource);
            MapPoint  CurMp;
            ArrayList CurList = (ArrayList)this.MapPointsInVP[vpindex];

            //count all straight adjacencies
            //no check for straight because voronoi algorithem does not allow adjacent zones to be connected not in a straight way
            //edges are skiped because they are taken care of with round trip
            for (int i = 0; i < CurList.Count; i++)
            {
                CurMp = (MapPoint)CurList[i];
                if (CurMp.x >= 1                && CurMp.y >= 1 &&
                    CurMp.x < (int)eMapSize - 1 && CurMp.y < (int)eMapSize - 1)
                {
                    iAdjacenciesCount = 0;
                    //array of points which define a round trip around a sqaure to check all its adjacencies 
                    MapPoint[] arrmp =  { 
                            new MapPoint(0,1) , new MapPoint (1,1) , new MapPoint (1,0),
                            new MapPoint(1,-1) , new MapPoint (0,-1) , new MapPoint(-1,-1),
                            new MapPoint( -1, 0) , new MapPoint(-1,1) 
                        };
                    foreach (MapPoint mpItem in arrmp)
                    {
                        //if one of the surrounding sqaures belongs to questioned zone then raise counter
                        // if (IsInsideMapLimit(i + mpItem.x , j + mpItem.y) )
                        // {
                        if ((int)this.iarrMap[(int)MapLayer.VoronoiZones, CurMp.x + mpItem.x, CurMp.y + mpItem.y] == vpNext.iAreaCode)
                        {
                            iAdjacenciesCount++;
                        }
                        // }    
                    }
                    if (iAdjacenciesCount >= 3) totalAdjacenciesCount++;
                }
            }
 
            //check if point have 6 straight adjacencies in this count if true return true else flase
            HowAdjacentpoint = totalAdjacenciesCount;
            if (totalAdjacenciesCount < I_ADJACENCIES_FACTOR)
                return false;
            else
                return true;
        }


        /// <summary>
        /// gets a list of free zones 
        /// </summary>
        /// <returns>list of free zones</returns>
        public System.Collections.ArrayList GetFreeZonesList()
        {
            System.Collections.ArrayList arrlstFreeZonesList=  new System.Collections.ArrayList();
            foreach (VoronoiPoint vpZone in vZoneMaker.VoronoiPoints)
            {
                if (IsFreeZone(vpZone))
                {
                    arrlstFreeZonesList.Add(vpZone);
                }
            }
            return arrlstFreeZonesList;


        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool IsFreeZone(VoronoiPoint vpZone)
        {

            bool bIsZoneFree = true;

            //go over list Which contains already allocated zones to check if its free
            foreach (System.Collections.ArrayList arrlstZone in this.ZonesListPoints)
            {
                if (arrlstZone != null)
                {
                    if (arrlstZone.Contains(vpZone))
                    {
                        bIsZoneFree = false;
                    }
                }
            }
            return bIsZoneFree;



        }


        /// <summary>
        /// returns a random free zone
        /// </summary>
        /// <returns>free zone or null if non exist</returns>
        public VoronoiPoint GetRandomFreeZone() //dwp тут идет случайная выборка из заранее построенного глобального списка
        {
            /* //dwp заменил случайный поиск на направленный поиск свободной зоны
            ArrayList free_zones = new ArrayList();
            for (int i = 0; i < vZoneMaker.VoronoiPoints.Count; i++)
            {
                if (IsFreeZone((VoronoiPoint)vZoneMaker.VoronoiPoints[i]))
                {
                    int  index;
                    index = i;
                    free_zones.Add(index);
                }
            }
            */
            if (this.FreeZonesList.Count <= 0) //dwp список готов берем количество свободных точек
            {
              //throw new Exception("Critical error: impossible to get free zone."); //no free zones //dwp
              return null;
            }

            int iRandomVZone = Randomizer.rnd.Next(this.FreeZonesList.Count); //dwp 
            //return ((VoronoiPoint)vZoneMaker.VoronoiPoints[(int)free_zones[iRandomVZone]]); //dwp убираем
            return ((VoronoiPoint)this.FreeZonesList[iRandomVZone]);
        }


        /// <summary>
        /// Selects a randompoint - point must not be between the 2 adjacent zones
        /// </summary>
        /// <param name="vpZonePoint"></param>
        /// <returns>a random point inside a zone (to minimise zone size )</returns>
        public VoronoiPoint SelectRandomPointInVZone(VoronoiPoint vpZonePoint, int iCurrentIndex)
        {
            VoronoiPoint vpLastAddedPoint = GetLastAddedPoint(iCurrentIndex);
            //to minimise time should confine zone bondaries as  square(if creation time is too big)

            // bool bIsPreviusePointExist = true;
            if (vpLastAddedPoint == null)// if its null means its the first point in this zone which means any random point is good
                return (new VoronoiPoint(vpZonePoint.x + 2, vpZonePoint.y + 2));

            //if line is vertical then a = 0  and needs to be dealt different
            if (vpZonePoint.x != vpLastAddedPoint.x)
            {
                //calculate line equation
                double a = (double)(vpZonePoint.y - vpLastAddedPoint.y) / (double)(vpZonePoint.x - vpLastAddedPoint.x);
                double b = a * vpZonePoint.x * -1 + vpZonePoint.y;

                if (vpZonePoint.x > vpLastAddedPoint.x)
                {
                    int iNewX = vpZonePoint.x + 2;
                    return new VoronoiPoint(iNewX, (int)(a * iNewX + b));
                }
                else
                {
                    int iNewX = vpZonePoint.x - 2;
                    return new VoronoiPoint(iNewX, (int)(a * iNewX + b));
                }

            }
            else
            {
                //todo add y checking and point placing
                int iNewX = vpZonePoint.x - 3;
                return new VoronoiPoint(iNewX, vpZonePoint.y);
            }
        }
        /*
        public VoronoiPoint SelectRandomPointInVZone(VoronoiPoint vpZonePoint , int iCurrentIndex)
        {
            VoronoiPoint vpLastAddedPoint = vpZonePoint; //GetLastAddedPoint(iCurrentIndex);
            VoronoiPoint reslt;
            ArrayList Curlist;
            MapPoint  CurMp;
            //to minimise time should confine zone bondaries as  square(if creation time is too big)

            int vpindex = vZoneMaker.VoronoiPoints.IndexOf(vpLastAddedPoint);
            Curlist = (ArrayList)this.MapPointsInVP[vpindex];
            if (Curlist.Count <= 0) return null;
            int rndindex = Randomizer.rnd.Next(Curlist.Count);
            CurMp = (MapPoint)Curlist[rndindex];
            reslt = new VoronoiPoint(CurMp.x, CurMp.y);
//            Curlist.RemoveAt(rndindex);
            return reslt;
        }
        */

        /// <summary>
        /// range of sqaure between two points defines the area in which another point will create an area between these 2 points, 
        /// to avoid that when spliting a zone that point will not be included in that area
        /// </summary>
        /// <param name="x">x coordinate of questioned point</param>
        /// <param name="y">y coordinate of questioned point</param>
        /// <param name="vpZonePoint">origin point</param>
        /// <param name="vpLastAddedPoint">last point </param>
        /// <returns></returns>
        public bool NotInRange ( int x, int y ,VoronoiPoint vpZonePoint, VoronoiPoint vpLastAddedPoint )
        {
            //if both diffs are negative or positive x is not in points square range
            if ((x - vpZonePoint.x) * (x - vpLastAddedPoint.x) > 0 || (y - vpZonePoint.y) * (y - vpLastAddedPoint.y) > 0) 
            {
                return true;
            }
            return false;
        }

        //iterates through array and set area confined by point assaign for the zone index
        //basically sets a voronoi zone as zone in zones layer ,Any zones defining 1 zone will call this method
        public void SetAsZone(VoronoiPoint vpZonePoint, int iZoneIndex)
        {
            for (int i = 0; i < (int)eMapSize; i++)
            {
                for (int j = 0; j < (int)eMapSize; j++)
                {
                    if ((int)this.iarrMap[(int)MapLayer.VoronoiZones, i, j] == vpZonePoint.iAreaCode)
                    {
                        this.iarrMap[(int)MapLayer.Zones, i, j] = iZoneIndex;
                    }
                }
            }       

        }



       
    }
}
