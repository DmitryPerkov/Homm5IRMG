using System;
using System.Collections.Generic;
using System.Text;
using Homm5RMG.BL;
using Homm5RMG.Data_Layer;
using System.Collections;

namespace Homm5RMG
{
    enum ePointLayer
    {
        OpenningPoints = 0,
        OtherTownsPoints,
        StartingTownsPoints,
        MinePoints
    }

    /// <summary>
    /// this section will handle the terrain placement
    /// </summary>
    public partial class ObjectsMap
    {
        ArrayList[,] AccessPoints;

        //first one will be the main terrain others will vary based on sub(original) voronoi areas
        public static int[][] iarrTerrains = new int[][]
            {
                new int[] {10,9,11,12,13,27,50,55,58,60} ,//grass numbers
                new int[] {5,7,8,48,61}, //dirt numbers
                new int[] {34,19,35,36,37,38,39,40,42,47}, //sand numbers
                new int[] {14,1,15,16,17,18,30},//lava numbers
                new int[] {32,31,33},//snow numbers
                new int[] {20,21,22,26},//subterrain numbers
                new int[] {53,63,52,49,51,54,56,57,59},//orcish numbers
                new int[] {4,0,2,3,6}//conquest numbers
            };



        public void SetTerrains()
        {
            //returns a table of terrain per each zone
            int[] iarrGeneralTerrains = thSelectedTemplate.iarrTerrainTable;
            int[] iarrSpacificTerrains = new int[this.vZoneMaker.VoronoiPoints.Count];
 
            //first set a random spacific terrain default
            for (int i = 0; i < iarrGeneralTerrains.Length; i++)
            {
                foreach (VoronoiPoint vpVArea in this.ZonesListPoints[i])
                {
                    //get random in general terrain
                    //first is default it will already appear so no need to draw it
                    int iRandomSpacificTerrainIndex = Randomizer.rnd.Next(1,iarrTerrains[iarrGeneralTerrains[i]].Length);

                    iarrSpacificTerrains[vpVArea.iAreaCode] = iarrTerrains[iarrGeneralTerrains[i]][iRandomSpacificTerrainIndex]; 
                }
 
            }
           
            for (int i = 0; i < (int)eMapSize; i++)
            {
                for (int j = 0; j < (int)eMapSize; j++)
                {
                    if ((eBlockStatus)iarrMap[(int)MapLayer.Blocks, i, j] == eBlockStatus.Preserved)
                    {
                        //in pathway roads it equals to default zone terrain 
                        if ((int)iarrMap[(int)MapLayer.Zones, i, j] == -1)//if its -1 then unassigned zone - give it grass zone
                        {
                            iarrMap[(int)MapLayer.Terrain, i, j] = (int)eTerrainAddressOffset.Grass_Grass;
                        }
                        else
                        {
                            iarrMap[(int)MapLayer.Terrain, i, j] = iarrTerrains[iarrGeneralTerrains[(int)iarrMap[(int)MapLayer.Zones, i, j] - 1]][0];
                        }
                    }
                    else
                    {

                        if ((int)iarrMap[(int)MapLayer.Zones, i, j] == -1)//if its -1 then unassigned zone - give it grass zone
                        {
                            iarrMap[(int)MapLayer.Terrain, i, j] = (int)eTerrainAddressOffset.Grass_Grass;
                        }
                        else
                        {
                            iarrMap[(int)MapLayer.Terrain, i, j] = iarrSpacificTerrains[(int)iarrMap[(int)MapLayer.VoronoiZones, i, j]];
                            //in random chance replace terrain
                            if (Randomizer.rnd.NextDouble() < 0.001)
                            {
                                //find a random from same kind
                                int iRandomSpacificTerrainIndex = Randomizer.rnd.Next(iarrTerrains[iarrGeneralTerrains[(int)iarrMap[(int)MapLayer.Zones, i, j] - 1]].Length);

                                iarrSpacificTerrains[(int)iarrMap[(int)MapLayer.VoronoiZones, i, j]] = iarrTerrains[iarrGeneralTerrains[(int)iarrMap[(int)MapLayer.Zones, i, j] - 1]][iRandomSpacificTerrainIndex];
                            }
                        }
                    }
                }
            }




        }


        /// <summary>
        /// 
        /// </summary>
        internal void CreateRoads(int iRoadStatus)
        {
            //first clear the a star array
            for (int x = 0; x < (int)eMapSize; x++)
            {
                for (int y = 0; y < (int)eMapSize; y++)
                {
                    iarrMap[(int)MapLayer.AStarDistance, x, y] = null;
                }
            }

            //go over all zones and create a road inside each one
            for (int i = 0; i < thSelectedTemplate.ZoneNumber; i++)
            {
                CreateRoadsInZone(i,iRoadStatus);
            }

            //process roads (meaning transform the astar table to terrain layer)
        }



        /// <summary>
        /// goes over each zone and create roads
        /// </summary>
        /// <param name="iZoneIndex"></param>
        private void CreateRoadsInZone(int iZoneIndex, int iRoadStatus)
        {
            //get road for zone
            int iRoadIndexNumber = GetRoadIndexNumberForZone(iZoneIndex);

            //first check is this zone has a starting town
            if (AccessPoints[iZoneIndex, (int)ePointLayer.StartingTownsPoints].Count == 1)
            {
                MapPoint mpStartingTown = (MapPoint)AccessPoints[iZoneIndex, (int)ePointLayer.StartingTownsPoints][0];
                //first need to set the origin as 0 distance (will be minimum)
                iarrMap[(int)MapLayer.AStarDistance, mpStartingTown.x, mpStartingTown.y] = 0;
                //if so compute a star from this point
                ComputeAStarForRoads( mpStartingTown, 0, iZoneIndex+1);
#if (DEBUG)
                dumpAStarToFile();
#endif           
                //now mark road at terrain layer from the town point to all other points

                //mark from town point to all other town points
                foreach (MapPoint mpTownAccessPoint in AccessPoints[iZoneIndex,(int)ePointLayer.OtherTownsPoints])
                {
                    ReturnSignalArrayDefaults();
                    MarkRoadInZone(mpTownAccessPoint, mpStartingTown, iRoadIndexNumber,iZoneIndex+1);
                }

                //mark from town to all openings
                foreach (MapPoint mpOpeningPoint in AccessPoints[iZoneIndex, (int)ePointLayer.OpenningPoints])
                {
                    ReturnSignalArrayDefaults();
                    MarkRoadInZone(mpOpeningPoint, mpStartingTown, iRoadIndexNumber,iZoneIndex+1);
                }


                if (iRoadStatus == (int)eRoadStatus.Roads_To_Towns_And_Mines)
                {
                    //mark from town to all Mines
                    foreach (MapPoint mpMinePoint in AccessPoints[iZoneIndex, (int)ePointLayer.MinePoints])
                    {
                        ReturnSignalArrayDefaults();
                        MarkRoadInZone(mpMinePoint, mpStartingTown, iRoadIndexNumber, iZoneIndex + 1);
                    }
                }
                
                

            }
            else//each zone must have an access point
            {

                MapPoint mpOriginPoint = (MapPoint)AccessPoints[iZoneIndex, (int)ePointLayer.OpenningPoints][0];
                //first need to set the origin as 0 distance (will be minimum)
                iarrMap[(int)MapLayer.AStarDistance, mpOriginPoint.x, mpOriginPoint.y] = 0;
                //compute a star from first access point
                ComputeAStarForRoads(mpOriginPoint , 0, iZoneIndex + 1);

                //now mark road at terrain layer from the access point to all other points

                //mark from Access point to all other town points
                foreach (MapPoint mpTownAccessPoint in AccessPoints[iZoneIndex, (int)ePointLayer.OtherTownsPoints])
                {
                    ReturnSignalArrayDefaults();
                    MarkRoadInZone(mpTownAccessPoint, mpOriginPoint, iRoadIndexNumber,iZoneIndex+1);
                }

                //mark from Access point to all openings
                foreach (MapPoint mpOpeningPoint in AccessPoints[iZoneIndex, (int)ePointLayer.OpenningPoints])
                {
                    ReturnSignalArrayDefaults();
                    if ( mpOriginPoint != mpOpeningPoint)
                        MarkRoadInZone(mpOpeningPoint, mpOriginPoint, iRoadIndexNumber,iZoneIndex+1);
                }

                if (iRoadStatus == (int)eRoadStatus.Roads_To_Towns_And_Mines)
                {
                    //mark from town to all Mines
                    foreach (MapPoint mpMinePoint in AccessPoints[iZoneIndex, (int)ePointLayer.MinePoints])
                    {
                        ReturnSignalArrayDefaults();
                        MarkRoadInZone(mpMinePoint, mpOriginPoint, iRoadIndexNumber, iZoneIndex + 1);
                    }
                }
            }

            

        }


        int[] iarrRoadsIndexNumbersForZones = new int[] {
            28 , //grass
            41 , //dirt
            45 , //sand
            43 , //lava            
            46 , //snow
            44 , //subterrain
            65 , //orc
            41 //conquest
        };

        private int GetRoadIndexNumberForZone(int iZoneIndex)
        {
            //returns a table of terrain per each zone
            int[] iarrGeneralTerrains = thSelectedTemplate.iarrTerrainTable;

            return iarrRoadsIndexNumbersForZones[iarrGeneralTerrains[iZoneIndex]];

        }

        private void ReturnSignalArrayDefaults()
        {
            for (int i = 0; i < (int)eMapSize; i++)
            {
                for (int j = 0; j < (int)eMapSize; j++)
                {
                    barrAStarVisit[i, j] = false;
                }
            }
        }






        /// <summary>
        /// marks the path from origin to destination - only marks short path by astar algorithem
        /// </summary>
        /// <param name="mapPoint">origin</param>
        /// <param name="mpTownAccessPoint">destination</param>
        private void MarkRoadInZone(MapPoint mpOrigin, MapPoint mpDestination, int iRoadIndexNumber ,int iZoneIndex)
        {
            //signal i've already been here
            barrAStarVisit[mpOrigin.x, mpOrigin.y] = true;

            //mark point as road
            iarrMap[(int)MapLayer.Terrain, mpOrigin.x, mpOrigin.y] = iRoadIndexNumber;

            //stop condition
            if (!mpOrigin.Equals(mpDestination))
            {


                int iMinDirectionValue = int.MaxValue;
                ArrayList mparrRoundPoints = new ArrayList();
                //examine all around blocks and choose a random minimal then go there 
                for (int i = 0; i < mpRoundTrip.Length; i++)
                {
                    MapPoint mpDirection = mpRoundTrip[i];
                    MapPoint mpPointToExamine = mpOrigin + mpDirection;
                    //if its not null that means there's a way in this direction
                    if (IsInsideMapLimit(mpPointToExamine.x, mpPointToExamine.y))
                    {
                        mparrRoundPoints.Add(mpPointToExamine);
                        if (iarrMap[(int)MapLayer.AStarDistance, mpPointToExamine.x, mpPointToExamine.y] != null &&
                            (int)iarrMap[(int)MapLayer.Zones, mpPointToExamine.x, mpPointToExamine.y] == iZoneIndex)
                        {
                            //this is condition to prevent going on the same road again and again and again
                            if (!barrAStarVisit[mpPointToExamine.x, mpPointToExamine.y])
                                //the minimum will be in the direction we want to go
                                if ((int)iarrMap[(int)MapLayer.AStarDistance, mpPointToExamine.x, mpPointToExamine.y] < iMinDirectionValue)
                                    iMinDirectionValue = (int)iarrMap[(int)MapLayer.AStarDistance, mpPointToExamine.x, mpPointToExamine.y];
                        }
                    }
                }



                for (int i = 0; i < mparrRoundPoints.Count; i++)
                {
                    if (iarrMap[(int)MapLayer.AStarDistance, ((MapPoint)mparrRoundPoints[i]).x, ((MapPoint)mparrRoundPoints[i]).y] != null)
                    {
                        if ((int)iarrMap[(int)MapLayer.AStarDistance, ((MapPoint)mparrRoundPoints[i]).x, ((MapPoint)mparrRoundPoints[i]).y] != iMinDirectionValue || 
                            (int)iarrMap[(int)MapLayer.Zones,((MapPoint)mparrRoundPoints[i]).x,((MapPoint)mparrRoundPoints[i]).y] != iZoneIndex)
                        {
                            mparrRoundPoints.RemoveAt(i);
                            i--;
                        }
                    }
                    else
                    {
                        mparrRoundPoints.RemoveAt(i);
                        i--;
                    }
                }


                //out of the minimums any random direction is ok
                //pick some random direction point and continue algorithem
                int iRandomIndex = Randomizer.rnd.Next(mparrRoundPoints.Count);

                if (mparrRoundPoints.Count == 0)
                    return;

                MarkRoadInZone((MapPoint)mparrRoundPoints[iRandomIndex], mpDestination, iRoadIndexNumber,iZoneIndex);
            }
        }
    }
}
