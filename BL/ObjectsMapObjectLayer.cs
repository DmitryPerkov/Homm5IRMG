using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Collections;
using System.Drawing;
using Homm5RMG.BL;
using Homm5RMG.Properties;
using Homm5RMG.Testing;

namespace Homm5RMG
{

    public enum ObjectDirection
    {
        Down = 0,
        Right,
        Up,                
        Left ,
        Error
    }

    /// <summary>
    /// will handle the object layer part of the class
    /// </summary>
    public partial class ObjectsMap
    {
        private MapPoint[] mparrDirectionTransformationPoints = { new MapPoint (1,1)   ,//down
                                                                  new MapPoint (1,-1)  ,//right
                                                                  new MapPoint (-1,-1) ,//up
                                                                  new MapPoint (-1,1)}; //left

        /// <summary>
        /// checks all sqaures in objects layer2
        /// </summary>
        /// <param name="moObjectToBePlaced">the object that will be checked</param>
        /// <returns></returns>
        public bool CanPlaceObjectInLayer2(MapObject moObjectToBePlaced)
        {



            //if origin is taken can't place
            if (!(this.iarrMap[(int)MapLayer.Objects2, moObjectToBePlaced.BasePoint.x, moObjectToBePlaced.BasePoint.y] == null))
            {
                if (moObjectToBePlaced.Type == eObjectType.Guard && this.iarrMap[(int)MapLayer.Objects2, moObjectToBePlaced.BasePoint.x, moObjectToBePlaced.BasePoint.y].ToString() == "LogicBlock")
                {
                    return true;
                }
                if (moObjectToBePlaced.Type == eObjectType.Treasure && this.iarrMap[(int)MapLayer.Objects2, moObjectToBePlaced.BasePoint.x, moObjectToBePlaced.BasePoint.y].ToString() == "LogicBlock")
                {
                    return true;
                }
                return false;
            }

            if (moObjectToBePlaced.Area != null)
            {
                if (moObjectToBePlaced.Direction != ObjectDirection.Down) //for performance issue avoid unnesecery calculation
                {

                    //MapPoint mpTransformMultiplier = this.mparrDirectionTransformationPoints[(int)moObjectToBePlaced.Direction];
                    //if one of the squares is taken can't place
                    foreach (MapPoint PArea in moObjectToBePlaced.Area)
                    {
                        MapPoint mpNewArea = TransformPointDirection(PArea, moObjectToBePlaced.Direction);
                        if (moObjectToBePlaced.BasePoint.x + mpNewArea.x >= (int)eMapSize || moObjectToBePlaced.BasePoint.y + mpNewArea.y >= (int)eMapSize || moObjectToBePlaced.BasePoint.x + mpNewArea.x < 0 || moObjectToBePlaced.BasePoint.y + mpNewArea.y < 0)
                            return false;
                        if (!(this.iarrMap[(int)MapLayer.Objects2, moObjectToBePlaced.BasePoint.x + mpNewArea.x, moObjectToBePlaced.BasePoint.y + mpNewArea.y] == null))
                        {
                            return false;
                        }
                    }
                }
                else
                {

                    //if one of the squares is taken can't place
                    foreach (MapPoint PArea in moObjectToBePlaced.Area)
                    {
                        if (moObjectToBePlaced.BasePoint.x + PArea.x >= (int)eMapSize || moObjectToBePlaced.BasePoint.y + PArea.y >= (int)eMapSize || moObjectToBePlaced.BasePoint.x + PArea.x < 0 || moObjectToBePlaced.BasePoint.y + PArea.y < 0)
                            return false;
                        if (!(this.iarrMap[(int)MapLayer.Objects2, moObjectToBePlaced.BasePoint.x + PArea.x, moObjectToBePlaced.BasePoint.y + PArea.y] == null))
                        {
                            return false;
                        }
                    }
                }
            }


            if (moObjectToBePlaced.PathWays != null)
            {
                if (moObjectToBePlaced.Direction != ObjectDirection.Down) //for performance issue avoid unnesecery calculation
                {
                    //MapPoint mpTransformMultiplier = this.mparrDirectionTransformationPoints[(int)moObjectToBePlaced.Direction];

                    //if one of the pathway squares is taken can't place
                    foreach (MapPoint PPathWay in moObjectToBePlaced.PathWays)
                    {
                        MapPoint mpNewArea = TransformPointDirection(PPathWay, moObjectToBePlaced.Direction);

                        if (moObjectToBePlaced.BasePoint.x + mpNewArea.x >= (int)eMapSize || moObjectToBePlaced.BasePoint.y + mpNewArea.y >= (int)eMapSize || moObjectToBePlaced.BasePoint.x + mpNewArea.x < 0 || moObjectToBePlaced.BasePoint.y + mpNewArea.y < 0)
                            return false;
                        if (!(this.iarrMap[(int)MapLayer.Objects2, moObjectToBePlaced.BasePoint.x + mpNewArea.x, moObjectToBePlaced.BasePoint.y + mpNewArea.y] == null))
                        {
                            return false;
                        }
                    }
                }
                else
                {
                    //if one of the pathway squares is taken can't place
                    foreach (MapPoint PPathWay in moObjectToBePlaced.PathWays)
                    {
                        if (moObjectToBePlaced.BasePoint.x + PPathWay.x >= (int)eMapSize || moObjectToBePlaced.BasePoint.y + PPathWay.y >= (int)eMapSize || moObjectToBePlaced.BasePoint.x + PPathWay.x < 0 || moObjectToBePlaced.BasePoint.y + PPathWay.y < 0)
                            return false;
                        if (!(this.iarrMap[(int)MapLayer.Objects2, moObjectToBePlaced.BasePoint.x + PPathWay.x, moObjectToBePlaced.BasePoint.y + PPathWay.y] == null))
                        {
                            return false;
                        }
                    }
                }
            }
            //if all is well ok to place..
            return true;

        }

        /// <summary>
        /// call CanPlaceObject and checks distance between objects of the same type as well
        /// </summary>
        /// <returns></returns>
        public bool CanPlaceObjectWithDistanceCheck(MapObject moObjectToBePlaced, int iZoneIndex)
        {
            if (!CanPlaceObject(moObjectToBePlaced)) {
                return false;
            }

            //check the distance
            return CheckDistance(moObjectToBePlaced, iZoneIndex);
        }

        /// <summary>
        /// check distance to the same type object
        /// </summary>
        /// <param name="moObjectToBePlaced"></param>
        /// <returns></returns>
        public bool CheckDistance(MapObject moObjectToBePlaced, int iZoneIndex)
        {
            int distance = get_distance_value(moObjectToBePlaced, iZoneIndex);
            if (distance > 0) { //check distance
                return check_distance(moObjectToBePlaced, distance, iZoneIndex);
            }

            return true;
        }

        /// <summary>
        /// check number of particalar object in zone
        /// </summary>
        /// <param name="moObjectToBePlaced"></param>
        /// <param name="iZoneIndex"></param>
        /// <returns></returns>
        public bool CheckObjectQuantity(MapObject moObjectToBePlaced, int iZoneIndex)
        {
            if (moObjectToBePlaced.max_number_ > 0) { //check quantity
                int n = get_object_quantity(moObjectToBePlaced.strName, iZoneIndex);
                return n < moObjectToBePlaced.max_number_;
            }

            return true;
        }

        private const int DAY_DISTANCE = 25; //distance for a day for a hero w/o logistic on native land
        private const int MIN_DISTANCE = 2; //min distance between objects
        private int get_distance_value(MapObject moObjectToBePlaced, int zone)
        {
            string name = moObjectToBePlaced.strName;
            //1/3 day distance
            if (name == "Mercenary_Camp" || //+1 attack
                name == "Marletto_Tower" || //+1 defence
                name == "Crystal_of_Revelation" || //+1 knowledge
                name == "Star_Axis" || //+1 spell power
                name == "SchoolofMagic" || //+1 spell power or knowledge
                name == "WarAcademy" || //+1 attack or defence
                name == "Learning_Stone" || //+1000 exp
                
                //weak treasure banks
                name == "DwarvenTreasury" ||
                name == "Crypt" ||
                name == "Elemantal_Stockpile" ||
                name == "GargoyleStonevault" ||
                                
                name == "Witch_Hut" ||
                name == "ElementalConflux" ||
                name == "Water_Wheel" ||
                name == "Shrine_Of_Magic_1" ||
                name == "Shrine_Of_Magic_2" ||
                name == "Windmill" ||
                name == "Rally_Flag") {
                return (int)(DAY_DISTANCE / 3);
            }

            //a half day distance
            if (name == "Arena" || //+2 attack or defence
                name == "LibraryOfEnlightenment" || //+1 spell power and knowledge

                //strong treasure banks
                name == "MagiVault" ||
                name == "WitchBank" ||
                name == "TreantThicket" ||
                name == "Pyramid" ||
                name == "SunkenTemple" ||
                name == "Dragon_Utopia" ||
                name == "Manticore_Cave" ||
                name == "Dark_halls" ||
                name == "Mummy_Graves" ||
                name == "Oasis" ||
                name == "Shrine_Of_Magic_3") {
                return (int)(DAY_DISTANCE / 2);
            }

            //a day distance
            if (name == "Prison" ||
                name == "Redwood_Observatory" ||

                //important adventure map objects
                name == "Stables" ||
                name == "Hill_Fort" ||
                name == "Black_Market" ||
                name == "House_Of_Astrologer" ||
                name == "Den_Of_Thieves" ||
                name == "Magic_Spring" ||
                name == "War_Machine_Factory" ||
                name == "SpellShop" ||
                name == "Trading_Post" ||
                name == "TombOfTheWarrior" ||
                name == "Tree_of_Knowledge" ||
                name == "Fire_Lake" ||
                name == "Magic_Well") {
                return DAY_DISTANCE;
            }
            //portals
            if ((name == "Monolith_Two_Way") ||
                name.Contains("portal"))
            {
                int n = get_zone_connections(zone);
                //dwp. изменил ограничение в сторону "облегчения"
                if (n >= 5) return (int)(DAY_DISTANCE / 3);
                if (n >= 3) return (int)(DAY_DISTANCE / 2);
                if (n <= 2) return (int)(DAY_DISTANCE * 0.7);
                return DAY_DISTANCE;
            }

            if (is_common_object(moObjectToBePlaced)) {
                return MIN_DISTANCE; //min distance between any two common objects
            }

            return 0;
        }

        private bool is_common_object(MapObject moObjectToBePlaced)
        {
            if (moObjectToBePlaced.Type == eObjectType.General ||
                moObjectToBePlaced.Type == eObjectType.LowDwelling ||
                moObjectToBePlaced.Type == eObjectType.HighDwelling) {
                return true;
            }
            return false;
        }

        private bool check_distance(MapObject moObjectToBePlaced, int distance, int zone)
        {
            Rectangle obj_reserved_space = new Rectangle(
                moObjectToBePlaced.BasePoint.x - MIN_DISTANCE,
                moObjectToBePlaced.BasePoint.y - MIN_DISTANCE,
                MIN_DISTANCE * 2,
                MIN_DISTANCE * 2); //reserved space for the object (200mp around center point)

            for (int y = -distance; y <= distance; ++y) {
                for (int x = -distance; x <= distance; ++x) {
                    int obj_x = moObjectToBePlaced.BasePoint.x + x;
                    int obj_y = moObjectToBePlaced.BasePoint.y + y;

                    if (!IsInsideMapLimit(obj_x, obj_y)) {
                        continue;
                    }
                    if ((int)this.iarrMap[(int)MapLayer.Zones, obj_x, obj_y] != zone) {
                        continue;
                    }
                    MapObject mo = this.iarrMap[(int)MapLayer.Objects, obj_x, obj_y] as MapObject;
                    if (mo == null) {
                        continue;
                    }

                    //checking min distance between any two common objects
                    if (is_common_object(moObjectToBePlaced) &&
                        is_common_object(mo) &&
                        obj_reserved_space.Contains(new Point(obj_x, obj_y))) {
                        return false;
                    }

                    //checking object with the same type
                    if (mo.strName == moObjectToBePlaced.strName) {
                        return false; //already exists
                    }
                }
            }
            return true;
        }

        private int get_zone_connections(int zone)
        {
            int[,] conn_list = thSelectedTemplate.GetConnectionList();
            int n = 0;
            
            for (int i = 0; i < thSelectedTemplate.ConnectionNumber; ++i) {
                if (conn_list[0, i] == zone || conn_list[1, i] == zone) {
                    ++n;
                }
            }
            return n;
        }

        /// <summary>
        /// Get number of particular object in zone
        /// </summary>
        private int get_object_quantity(string obj_name, int zone)
        {
            int n = 0;

            for (int i = 0; i < (int)eMapSize; i++) {
                for (int j = 0; j < (int)eMapSize; j++) {
                    if ((int)iarrMap[(int)MapLayer.Zones, i, j] == zone) {
                        MapObject mo = this.iarrMap[(int)MapLayer.Objects, i, j] as MapObject;
                        if (mo == null) {
                            continue;
                        }
                        if (mo.strName == obj_name) {
                            n++;
                        }
                    }
                }
            }
            return n;
        }

        /// <summary>
        /// checks all sqaures in objects
        /// check is in objects layer and also in blocks layer
        /// </summary>
        /// <param name="moObjectToBePlaced">the object that will be checked</param>
        /// <returns></returns>
        public bool CanPlaceObject(MapObject moObjectToBePlaced)
        {
            SetObjectDirection(moObjectToBePlaced);
            int iCountFreePoints = 0;
 
            //if origin is taken can't place
            if (moObjectToBePlaced.ObjectSize != 1)
            {
                eBlockStatus st = (eBlockStatus)iarrMap[(int)MapLayer.Blocks, moObjectToBePlaced.BasePoint.x, moObjectToBePlaced.BasePoint.y];
                if (st == eBlockStatus.Free || st == eBlockStatus.MaybeBlock) 
                {
                    if (!(this.iarrMap[(int)MapLayer.Objects, moObjectToBePlaced.BasePoint.x, moObjectToBePlaced.BasePoint.y] == null))
                    {
                        if (moObjectToBePlaced.Type == eObjectType.Guard && this.iarrMap[(int)MapLayer.Objects, moObjectToBePlaced.BasePoint.x, moObjectToBePlaced.BasePoint.y].ToString() == "LogicBlock")
                        {
                            return true;
                        }

                        if (moObjectToBePlaced.Type == eObjectType.Guard && this.iarrMap[(int)MapLayer.Objects, moObjectToBePlaced.BasePoint.x, moObjectToBePlaced.BasePoint.y].ToString() == "AccessPoint")
                        {
                            return true;
                        }
                        if (moObjectToBePlaced.Type == eObjectType.Treasure && this.iarrMap[(int)MapLayer.Objects, moObjectToBePlaced.BasePoint.x, moObjectToBePlaced.BasePoint.y].ToString() == "LogicBlock")
                        {
                            return true;
                        }
                        return false;
                    }

                    if ((eBlockStatus)iarrMap[(int)MapLayer.Blocks, moObjectToBePlaced.BasePoint.x, moObjectToBePlaced.BasePoint.y] == eBlockStatus.Free)
                        iCountFreePoints++;
                }
                else
                    return false;

                if (moObjectToBePlaced.Area != null)
                {
                        //MapPoint mpTransformMultiplier = this.mparrDirectionTransformationPoints[(int)moObjectToBePlaced.Direction];
                        //if one of the squares is taken can't place
                        foreach (MapPoint PArea in moObjectToBePlaced.Area)
                        {
                            MapPoint mpNewArea = TransformPointDirection(PArea, moObjectToBePlaced.Direction);
                            int iNewX = moObjectToBePlaced.BasePoint.x + mpNewArea.x;
                            int iNewY = moObjectToBePlaced.BasePoint.y + mpNewArea.y;

                            if (!IsInsideMapLimit(iNewX, iNewY))
                                return false;

                            //check if status is free or maybe block , any other is unplaceable
                            if ((eBlockStatus)iarrMap[(int)MapLayer.Blocks, iNewX, iNewY] == eBlockStatus.Free || 
                                (eBlockStatus)iarrMap[(int)MapLayer.Blocks, iNewX, iNewY] == eBlockStatus.MaybeBlock ||
                                (eBlockStatus)iarrMap[(int)MapLayer.Blocks, iNewX, iNewY] == eBlockStatus.Preserved)
                            {
                                //check objects layer
                                if (this.iarrMap[(int)MapLayer.Objects, iNewX, iNewY] != null) return false;
                            
                            }
                            else return false;

                            //dwp! проверяем облако вокруг обьекта толщиной 1(один)
                            MapPoint[] arrCloude = mpRoundTripAllDirections;
                            foreach (MapPoint mpNewPoint in arrCloude)
                            {
                                int iCloudX = iNewX + mpNewPoint.x;
                                int iCloudY = iNewY + mpNewPoint.y;

                                if (!IsInsideMapLimit(iCloudX, iCloudY)) return false;
                                if ((eBlockStatus)iarrMap[(int)MapLayer.Blocks, iCloudX, iCloudY] == eBlockStatus.Free ||
                                    (eBlockStatus)iarrMap[(int)MapLayer.Blocks, iCloudX, iCloudY] == eBlockStatus.MaybeBlock ||
                                    (eBlockStatus)iarrMap[(int)MapLayer.Blocks, iCloudX, iCloudY] == eBlockStatus.Preserved) //dwp!
                                    
                                {
                                    //check objects layer
                                    //DWP!!  проверка облака вокруг обьекта
                                    //в облаке могут быть только кучки и охраны, чтобы не было блокировок
                                    MapObject mo = this.iarrMap[(int)MapLayer.Objects, iCloudX, iCloudY] as MapObject;
                                    if (mo != null) return false;
                                }
                                else return false;
                            }

                            if ((eBlockStatus)iarrMap[(int)MapLayer.Blocks, iNewX, iNewY] == eBlockStatus.Free)
                                iCountFreePoints++;
                        }
                }
                //CHECK IF ACCESS POINT IS FREE IF IT EXITS
                if (moObjectToBePlaced.AccessPoint != null)
                {
                       MapPoint mpNewAccess = TransformPointDirection(moObjectToBePlaced.AccessPoint, moObjectToBePlaced.Direction);
                       MapPoint mpAccessPoint = moObjectToBePlaced.BasePoint + mpNewAccess;
                       if (IsInsideMapLimit(mpAccessPoint))
                       {
                           if ((eBlockStatus)iarrMap[(int)MapLayer.Blocks, mpAccessPoint.x, mpAccessPoint.y] == eBlockStatus.Free)
                           {
                               //check objects layer
                               if (!(this.iarrMap[(int)MapLayer.Objects, mpAccessPoint.x, mpAccessPoint.y] == null))
                               {
                                   return false;
                               }
                           }
                           else return false;
                           //dwp! считаем точки облака вокруг точки входа толщина 3(три !!!)
                           MapPoint[] arrCloude = mpRoundTripAllDirections2;
                           foreach (MapPoint mpNewPoint in arrCloude)
                           {
                               int iCloudX = mpAccessPoint.x + mpNewPoint.x;
                               int iCloudY = mpAccessPoint.y + mpNewPoint.y;
                               if (IsInsideMapLimit(new MapPoint(iCloudX, iCloudY)))
                               {
                                   if ((eBlockStatus)iarrMap[(int)MapLayer.Blocks, iCloudX, iCloudY] == eBlockStatus.Free ||
                                       (eBlockStatus)iarrMap[(int)MapLayer.Blocks, iCloudX, iCloudY] == eBlockStatus.MaybeBlock ||
                                       (eBlockStatus)iarrMap[(int)MapLayer.Blocks, iCloudX, iCloudY] == eBlockStatus.Preserved)
                                   {
                                       //DWP!!  проверка облака вокруг обьекта
                                       //в облаке могут быть только кучки и охраны, чтобы не было блокировок
                                       //в зоне доступа оставляем пока только охраны. кучки убираю
                                       //check objects layer
                                       MapObject mo = this.iarrMap[(int)MapLayer.Objects, iCloudX, iCloudY] as MapObject;
                                       if (mo == null) continue;
                                       if (!(/*mo.Type == eObjectType.Treasure ||*/ mo.Type == eObjectType.Guard))
                                       {
                                           return false;
                                       }
                                   }
                                   else return false;
                               }
                               else return false;
                           }
                       }
                       else return false;
                }                
            }
            else //check origin
            {
                eBlockStatus st = (eBlockStatus)iarrMap[(int)MapLayer.Blocks,
                                                        moObjectToBePlaced.BasePoint.x,
                                                        moObjectToBePlaced.BasePoint.y];
                if (st == eBlockStatus.Free || st == eBlockStatus.MaybeBlock /*|| st == eBlockStatus.Preserved*/) 
                {
                    //if all is well ok to place.. if its a guard
                    if (moObjectToBePlaced.Type == eObjectType.Guard)
                        return true;
                    //check access point too
                    //first transform accesspoint
                    MapPoint mpTransformedAP = TransformPointDirection(moObjectToBePlaced.AccessPoint, moObjectToBePlaced.Direction);

                    if ((eBlockStatus)iarrMap[(int)MapLayer.Blocks, moObjectToBePlaced.BasePoint.x + mpTransformedAP.x, moObjectToBePlaced.BasePoint.y + mpTransformedAP.y] == eBlockStatus.Free)
                    {
                        iCountFreePoints++;
                        //check objects layer
                        if (!(this.iarrMap[(int)MapLayer.Objects, moObjectToBePlaced.BasePoint.x + mpTransformedAP.x, moObjectToBePlaced.BasePoint.y + mpTransformedAP.y] == null))
                        {
                            return false;
                        }
                    }
                    else return false;

                }
            }

            //if some is free then can place object
            if ((double)iCountFreePoints > 0) //dwp модифицировал в 1.8.3 было > 15%
            {
                return true;
            }
            else return false;
        }

        /// <summary>
        /// checks all sqaures in objects
        /// </summary>
        /// <param name="moObjectToBePlaced">the object that will be checked</param>
        /// <returns></returns>
        public bool CanPlaceTightObject(MapObject moObjectToBePlaced)
        {
            //if origin is taken can't place
            if (!(this.iarrMap[(int)MapLayer.Objects, moObjectToBePlaced.BasePoint.x, moObjectToBePlaced.BasePoint.y] == null))
            {
                if (moObjectToBePlaced.Type == eObjectType.Guard && this.iarrMap[(int)MapLayer.Objects, moObjectToBePlaced.BasePoint.x, moObjectToBePlaced.BasePoint.y].ToString() == "LogicBlock")
                {
                    return true;
                }
                if (moObjectToBePlaced.Type == eObjectType.Treasure && this.iarrMap[(int)MapLayer.Objects, moObjectToBePlaced.BasePoint.x, moObjectToBePlaced.BasePoint.y].ToString() == "LogicBlock")
                {
                    return true;
                }
                return false;
            }

            if (moObjectToBePlaced.Area != null)
            {
                if (moObjectToBePlaced.Direction != ObjectDirection.Down) //for performance issue avoid unnesecery calculation
                {

                    //MapPoint mpTransformMultiplier = this.mparrDirectionTransformationPoints[(int)moObjectToBePlaced.Direction];
                    //if one of the squares is taken can't place
                    foreach (MapPoint PArea in moObjectToBePlaced.Area)
                    {
                        MapPoint mpNewArea = TransformPointDirection(PArea, moObjectToBePlaced.Direction);
                        if (moObjectToBePlaced.BasePoint.x + mpNewArea.x >= (int)eMapSize || moObjectToBePlaced.BasePoint.y + mpNewArea.y >= (int)eMapSize || moObjectToBePlaced.BasePoint.x + mpNewArea.x < 0 || moObjectToBePlaced.BasePoint.y + mpNewArea.y < 0)
                            return false;
                        if (!(this.iarrMap[(int)MapLayer.Objects, moObjectToBePlaced.BasePoint.x + mpNewArea.x, moObjectToBePlaced.BasePoint.y + mpNewArea.y] == null))
                        {
                            return false;
                        }
                    }
                }
                else
                {

                    //if one of the squares is taken can't place
                    foreach (MapPoint PArea in moObjectToBePlaced.Area)
                    {
                        if (moObjectToBePlaced.BasePoint.x + PArea.x >= (int)eMapSize || moObjectToBePlaced.BasePoint.y + PArea.y >= (int)eMapSize || moObjectToBePlaced.BasePoint.x + PArea.x < 0 || moObjectToBePlaced.BasePoint.y + PArea.y < 0)
                            return false;
                        if (!(this.iarrMap[(int)MapLayer.Objects, moObjectToBePlaced.BasePoint.x + PArea.x, moObjectToBePlaced.BasePoint.y + PArea.y] == null))
                        {
                            return false;
                        }
                    }
                }
            }

            #region pathway check - only check if any logic blocks are close then can't place
            if (moObjectToBePlaced.PathWays != null)
            {
                if (moObjectToBePlaced.Direction != ObjectDirection.Down) //for performance issue avoid unnesecery calculation
                {
                    //MapPoint mpTransformMultiplier = this.mparrDirectionTransformationPoints[(int)moObjectToBePlaced.Direction];

                    //if one of the pathway squares is taken can't place
                    foreach (MapPoint PPathWay in moObjectToBePlaced.PathWays)
                    {
                        MapPoint mpNewArea = TransformPointDirection(PPathWay, moObjectToBePlaced.Direction);

                        if (moObjectToBePlaced.BasePoint.x + mpNewArea.x >= (int)eMapSize || moObjectToBePlaced.BasePoint.y + mpNewArea.y >= (int)eMapSize || moObjectToBePlaced.BasePoint.x + mpNewArea.x < 0 || moObjectToBePlaced.BasePoint.y + mpNewArea.y < 0)
                        {

                        }
                        else
                        {
                            if ( this.iarrMap[(int)MapLayer.Objects, moObjectToBePlaced.BasePoint.x + mpNewArea.x, moObjectToBePlaced.BasePoint.y + mpNewArea.y] != null)
                            {
                                if (this.iarrMap[(int)MapLayer.Objects, moObjectToBePlaced.BasePoint.x + mpNewArea.x, moObjectToBePlaced.BasePoint.y + mpNewArea.y].ToString() == "LogicBlock")            
                                return false;
                            }
                        }
                    }
                }
                else
                {
                    //if one of the pathway squares is taken can't place
                    foreach (MapPoint PPathWay in moObjectToBePlaced.PathWays)
                    {
                        if (moObjectToBePlaced.BasePoint.x + PPathWay.x >= (int)eMapSize || moObjectToBePlaced.BasePoint.y + PPathWay.y >= (int)eMapSize || moObjectToBePlaced.BasePoint.x + PPathWay.x < 0 || moObjectToBePlaced.BasePoint.y + PPathWay.y < 0)
                        {

                        }
                        else
                        {

                            if ( this.iarrMap[(int)MapLayer.Objects, moObjectToBePlaced.BasePoint.x + PPathWay.x, moObjectToBePlaced.BasePoint.y + PPathWay.y] != null)
                            {
                                if (this.iarrMap[(int)MapLayer.Objects, moObjectToBePlaced.BasePoint.x + PPathWay.x, moObjectToBePlaced.BasePoint.y + PPathWay.y].ToString() == "LogicBlock")
                                return false;
                            }
                        }
                    }
                }
            }

            #endregion
            //if all is well ok to place..
            return true;

        }

        /// <summary>
        /// new version place object on the map - and put position number in case object is bigger then 1
        /// </summary>
        /// <param name="moObjectToBePlaced"></param>
        public bool PlaceObject(MapObject moObjectToBePlaced, int iZoneIndex)
        {
            if (moObjectToBePlaced.BasePoint.x < 0 || moObjectToBePlaced.BasePoint.y  < 0)
                return false;
            
                    SetObjectDirection(moObjectToBePlaced);
                    this.iarrMap[(int)MapLayer.Objects, moObjectToBePlaced.BasePoint.x, moObjectToBePlaced.BasePoint.y] = moObjectToBePlaced;
                    this.iarrMap[(int)MapLayer.Blocks, moObjectToBePlaced.BasePoint.x, moObjectToBePlaced.BasePoint.y] = eBlockStatus.Done;
                    //MapPoint mpTransformMultiplier = this.mparrDirectionTransformationPoints[(int)moObjectToBePlaced.Direction];

                    //place point of origin to indicate place is taken by certain object if any
                    if (moObjectToBePlaced.Area != null)
                    {
                        foreach (MapPoint PArea in moObjectToBePlaced.Area)
                        {
                            MapPoint mpNewArea = TransformPointDirection(PArea, moObjectToBePlaced.Direction);
                            this.iarrMap[(int)MapLayer.Objects, moObjectToBePlaced.BasePoint.x + mpNewArea.x, moObjectToBePlaced.BasePoint.y + mpNewArea.y] = moObjectToBePlaced.BasePoint.ToString();
                            iarrMap[(int)MapLayer.Blocks, moObjectToBePlaced.BasePoint.x + mpNewArea.x, moObjectToBePlaced.BasePoint.y + mpNewArea.y] = eBlockStatus.Done;
                            //dwp облако вокруг обьекта тоже помечаем как свободное от блоков. толщина 1!
                            MapPoint[] arrCloude = mpRoundTripAllDirections;
                            foreach (MapPoint mpNewPoint in arrCloude)
                            {
                                int iCloudX = moObjectToBePlaced.BasePoint.x + mpNewArea.x + mpNewPoint.x;
                                int iCloudY = moObjectToBePlaced.BasePoint.y + mpNewArea.y + mpNewPoint.y;

                                if (IsInsideMapLimit(iCloudX, iCloudY))
                                    if ((eBlockStatus)iarrMap[(int)MapLayer.Blocks, iCloudX, iCloudY] == eBlockStatus.MaybeBlock)
                                    iarrMap[(int)MapLayer.Blocks, iCloudX, iCloudY] = eBlockStatus.Free;
                            }
                        }
                    }
                    //place pathway to block any adjacent squares
                    if (moObjectToBePlaced.PathWays != null)
                    {
                        foreach (MapPoint PArea in moObjectToBePlaced.PathWays)
                        {
                            MapPoint mpNewArea = TransformPointDirection(PArea, moObjectToBePlaced.Direction);

                            if (this.iarrMap[(int)MapLayer.Objects, moObjectToBePlaced.BasePoint.x + mpNewArea.x, moObjectToBePlaced.BasePoint.y + mpNewArea.y] == null)
                            {
                                iarrMap[(int)MapLayer.Blocks, moObjectToBePlaced.BasePoint.x + mpNewArea.x, moObjectToBePlaced.BasePoint.y + mpNewArea.y] = eBlockStatus.Done;
                                this.iarrMap[(int)MapLayer.Objects, moObjectToBePlaced.BasePoint.x + mpNewArea.x, moObjectToBePlaced.BasePoint.y + mpNewArea.y] = "LogicBlock";
                            }
                        }
                    }
                    if (moObjectToBePlaced.AccessPoint != null)
                    {
                        MapPoint mpNewArea = TransformPointDirection(moObjectToBePlaced.AccessPoint, moObjectToBePlaced.Direction);
                        //dwp облако вокруг точки входа резервируем под дороги, чтобы не было ни блоков ни одиночных объектов
                        MapPoint[] arrCloude = mpRoundTripAllDirections2;
                        foreach (MapPoint mpNewPoint in arrCloude)
                        {
                           int iCloudX = moObjectToBePlaced.BasePoint.x + mpNewArea.x + mpNewPoint.x;
                           int iCloudY = moObjectToBePlaced.BasePoint.y + mpNewArea.y + mpNewPoint.y;

                           if (IsInsideMapLimit(iCloudX, iCloudY))
                            if ((eBlockStatus)iarrMap[(int)MapLayer.Blocks, iCloudX, iCloudY] == eBlockStatus.MaybeBlock ||
                                (eBlockStatus)iarrMap[(int)MapLayer.Blocks, iCloudX, iCloudY] == eBlockStatus.Free)
                                //dwp блокируем возникновение мелких объектов в зоне доступа.
                                iarrMap[(int)MapLayer.Blocks, iCloudX, iCloudY] = eBlockStatus.Preserved;
                        }

                    }

                    return true;
        }


        /// <summary>
        /// new version place object on the map - and put position number in case object is bigger then 1
        /// </summary>
        /// <param name="moObjectToBePlaced"></param>
        public bool PlaceTightObject(MapObject moObjectToBePlaced)
        {

            if (moObjectToBePlaced.BasePoint.x < 0 || moObjectToBePlaced.BasePoint.y < 0)
                return false;

            SetObjectDirection(moObjectToBePlaced);
            //if place is not taken
            if (CanPlaceTightObject(moObjectToBePlaced))
            {
                if (moObjectToBePlaced.Direction != ObjectDirection.Down)
                {
                    this.iarrMap[(int)MapLayer.Objects, moObjectToBePlaced.BasePoint.x, moObjectToBePlaced.BasePoint.y] = moObjectToBePlaced;

                    //MapPoint mpTransformMultiplier = this.mparrDirectionTransformationPoints[(int)moObjectToBePlaced.Direction];

                    //place point of origin to indicate place is taken by certain object if any
                    if (moObjectToBePlaced.Area != null)
                    {
                        foreach (MapPoint PArea in moObjectToBePlaced.Area)
                        {
                            MapPoint mpNewArea = TransformPointDirection(PArea, moObjectToBePlaced.Direction);

                            this.iarrMap[(int)MapLayer.Objects, moObjectToBePlaced.BasePoint.x + mpNewArea.x, moObjectToBePlaced.BasePoint.y + mpNewArea.y] = moObjectToBePlaced.BasePoint.ToString();
                        }
                    }

                    #region placement of pathway not neccessery for tight objects
                    ////place pathway to block any adjacent squares
                    //if (moObjectToBePlaced.PathWays != null)
                    //{
                    //    foreach (MapPoint PArea in moObjectToBePlaced.PathWays)
                    //    {
                    //        MapPoint mpNewArea = TransformPointDirection(PArea, moObjectToBePlaced.Direction);

                    //        if (this.iarrMap[(int)MapLayer.Objects, moObjectToBePlaced.BasePoint.x + mpNewArea.x, moObjectToBePlaced.BasePoint.y + mpNewArea.y] == null)
                    //            this.iarrMap[(int)MapLayer.Objects, moObjectToBePlaced.BasePoint.x + mpNewArea.x, moObjectToBePlaced.BasePoint.y + mpNewArea.y] = "LogicBlock";
                    //    }
                    //}
                    #endregion
                    return true;
                }
                else
                {
                    this.iarrMap[(int)MapLayer.Objects, moObjectToBePlaced.BasePoint.x, moObjectToBePlaced.BasePoint.y] = moObjectToBePlaced;

                    //place point of origin to indicate place is taken by certain object if any
                    if (moObjectToBePlaced.Area != null)
                    {
                        foreach (MapPoint PArea in moObjectToBePlaced.Area)
                        {
                            this.iarrMap[(int)MapLayer.Objects, moObjectToBePlaced.BasePoint.x + PArea.x, moObjectToBePlaced.BasePoint.y + PArea.y] = moObjectToBePlaced.BasePoint.ToString();
                        }
                    }


                    ////place point of origin to indicate place is taken by certain object if any
                    //if (moObjectToBePlaced.PathWays != null)
                    //{
                    //    foreach (MapPoint PArea in moObjectToBePlaced.PathWays)
                    //    {
                    //        if (this.iarrMap[(int)MapLayer.Objects, moObjectToBePlaced.BasePoint.x + PArea.x, moObjectToBePlaced.BasePoint.y + PArea.y] == null)
                    //            this.iarrMap[(int)MapLayer.Objects, moObjectToBePlaced.BasePoint.x + PArea.x, moObjectToBePlaced.BasePoint.y + PArea.y] = "LogicBlock";
                    //    }
                    //}
                    return true;
                }
            }
            return false;
        }

        //sets direction for object 
        private void SetObjectDirection(MapObject moObjectToBePlaced)
        {
            if (iarrMap[(int)MapLayer.Directions, moObjectToBePlaced.BasePoint.x, moObjectToBePlaced.BasePoint.y] != null)
            {
                moObjectToBePlaced.Direction = (ObjectDirection) iarrMap[(int)MapLayer.Directions, moObjectToBePlaced.BasePoint.x, moObjectToBePlaced.BasePoint.y];

            }
        }

        public void PopulateObjectsInZones()
        {
            //removes all objects with 0 chance to appear
            thSelectedTemplate.RemoveObjectWith0Chance();
            for (int i = 0; i < thSelectedTemplate.ZoneNumber; i++)
            {
                PopulateObjectsInZone(i);
            }

        }

        private void PopulateObjectsInZone(int iZoneIndex)
        {
            //Get minimum sized object to place , keep placing until minimum sized is unplaceable
            //Todo:maybe implement a density mechanism
            MapObject mpobjRandomObject;
            MapPoint mpRandomPoint;

            //int iZoneSize = GetZoneSize(iZoneIndex);
            //int iFilledSize = 0;
            ObjectsReader obrdResources = new ObjectsReader("Resources.xml");
            MapPoint mpAccessPoint;
            //MapPoint mpBaseAccessPoint;
            MapObject mpResource1;

            //get zone size
            /* dwp. механизм плотности размещения отключен.
            int iZoneSize = CalculateZoneSize(iZoneIndex + 1);
            int iZoneFilled = CalculateZoneFilled(iZoneIndex + 1);*/

            double dObjectAppearChance;
            System.Collections.ArrayList arrAvailObjetsinZone = thSelectedTemplate.GetALLObjectInZone(iZoneIndex);

            if (arrAvailObjetsinZone != null && arrAvailObjetsinZone.Count > 0) /*dwp. есть что ставить?*/
            {
                //dwp!!!! незацикливающийся механизм размещения объектов
                //dwp!!!! без механизма плотности размещения. за плотностью следит автор шаблона!!!
                for (int obj = 0; obj < arrAvailObjetsinZone.Count; obj++)
                {
                    int AmountAttempts = ((MapObject)arrAvailObjetsinZone[obj]).max_number_;
                    //dwp. делаем попытки установки обьекта с нужно вероятснотью и нужное количество раз.
                    for (int attempt = 0; attempt < AmountAttempts; attempt++)
                    {
                        dObjectAppearChance = Randomizer.rnd.NextDouble();
                        //dwp. кидаем кости и проверяем ставим объект или нет в текущей попытке.
                        if (((MapObject)arrAvailObjetsinZone[obj]).probability_ >= dObjectAppearChance)
                        {
                            mpobjRandomObject = ((MapObject)arrAvailObjetsinZone[obj]).Clone();
                            mpRandomPoint = GetFreeFittingPointInZone(iZoneIndex + 1, mpobjRandomObject);
                            if (mpobjRandomObject != null && mpRandomPoint.x != -1 && mpRandomPoint.y != -1)
                            {
                                mpobjRandomObject.BasePoint = mpRandomPoint;
                                if (PlaceObject(mpobjRandomObject, iZoneIndex + 1))
                                {
                                    if (mpobjRandomObject.bShouldBeGuarded && mpobjRandomObject.iValue != 0)
                                    {
                                        PlaceObjectGuard(mpobjRandomObject, iZoneIndex + 1);
                                        //set an access point for resources
                                        if (mpobjRandomObject.AccessPoint != null)
                                            mpAccessPoint = mpobjRandomObject.AccessPoint;
                                        else
                                            mpAccessPoint = new MapPoint(0, -1);
                                        //tranform point by object direction
                                        mpAccessPoint = TransformPointDirection(mpAccessPoint, mpobjRandomObject.Direction);
                                        MapPoint mpGuardPoint = mpobjRandomObject.BasePoint + mpAccessPoint;

                                        //also by random chance of 0.5 place a random resource right of left of guard point 
                                        //try for right direction
                                        double dRandomChance = Randomizer.rnd.NextDouble();
                                        if (dRandomChance < 0.33)
                                        {
                                            mpResource1 = obrdResources.GetRandomObjectByType(eObjectType.Treasure);
                                            mpResource1.BasePoint = mpGuardPoint + TransformPointDirection(new MapPoint(1, 0), mpobjRandomObject.Direction);
                                            if (CanPlaceObject(mpResource1)) PlaceObject(mpResource1, iZoneIndex + 1);
                                        }

                                        //do it again for left direction
                                        dRandomChance = Randomizer.rnd.NextDouble();
                                        if (dRandomChance < 0.33)
                                        {
                                            mpResource1 = obrdResources.GetRandomObjectByType(eObjectType.Treasure);
                                            mpResource1.BasePoint = mpGuardPoint + TransformPointDirection(new MapPoint(-1, 0), mpobjRandomObject.Direction);
                                            if (CanPlaceObject(mpResource1)) PlaceObject(mpResource1, iZoneIndex + 1);
                                        }

                                        //also go around guard point and mark place as done if is free 
                                        MapPoint mpAroundGuard;
                                        for (int i = 0; i < mpRoundTripAllDirections.Length; i++)
                                        {
                                            mpAroundGuard = mpGuardPoint + mpRoundTripAllDirections[i];
                                            if (IsInsideMapLimit(mpAroundGuard.x, mpAroundGuard.y))
                                            {
                                                if (iarrMap[(int)MapLayer.Objects, mpAroundGuard.x, mpAroundGuard.y] == null)
                                                    iarrMap[(int)MapLayer.Objects, mpAroundGuard.x, mpAroundGuard.y] = "AroundGuard";
                                            }
                                        }
                                    }
                                    else //dwp чтобы не затерлась точка входа какой нибудь кучкой.(к примеру у башни элементов)
                                        if (mpobjRandomObject.AccessPoint != null)
                                        {
                                            mpAccessPoint = TransformPointDirection(mpobjRandomObject.AccessPoint, mpobjRandomObject.Direction);
                                            iarrMap[(int)MapLayer.Blocks, mpobjRandomObject.BasePoint.x + mpAccessPoint.x, mpobjRandomObject.BasePoint.y + mpAccessPoint.y] = eBlockStatus.Done;
                                        }
                                }
                                //dwp. ошибка установки этого обьекта . отказываемся от дальнейших попыток.
                                else break;
                            }
                            //dwp. нет места для установки этого обьекта . отказываемся от дальнейших попыток.
                            else break;
                        }
                    }
                }
            }
        }


        private MapObject get_random_object(int zone)
        {
            bool fl = false;
            MapObject mo = null;
            while (!fl) {
                mo = thSelectedTemplate.GetRandomObjectInZone(zone);
                //check number of objects
                fl = CheckObjectQuantity(mo, zone + 1);
            }
            return mo;
        }

        public bool PlaceObjectInLayer2(MapObject moObjectToBePlaced)
        {

            if (moObjectToBePlaced.BasePoint.x < 0 || moObjectToBePlaced.BasePoint.y < 0)
                return false;

            SetObjectDirection(moObjectToBePlaced);
            //if place is not taken
            if (CanPlaceObjectInLayer2(moObjectToBePlaced))
            {
                if (moObjectToBePlaced.Direction != ObjectDirection.Down)
                {
                    this.iarrMap[(int)MapLayer.Objects2, moObjectToBePlaced.BasePoint.x, moObjectToBePlaced.BasePoint.y] = moObjectToBePlaced;

                    //MapPoint mpTransformMultiplier = this.mparrDirectionTransformationPoints[(int)moObjectToBePlaced.Direction];

                    //place point of origin to indicate place is taken by certain object if any
                    if (moObjectToBePlaced.Area != null)
                    {
                        foreach (MapPoint PArea in moObjectToBePlaced.Area)
                        {
                            MapPoint mpNewArea = TransformPointDirection(PArea, moObjectToBePlaced.Direction);

                            this.iarrMap[(int)MapLayer.Objects2, moObjectToBePlaced.BasePoint.x + mpNewArea.x, moObjectToBePlaced.BasePoint.y + mpNewArea.y] = moObjectToBePlaced.BasePoint.ToString();
                        }
                    }

                    return true;
                }
                else
                {
                    this.iarrMap[(int)MapLayer.Objects2, moObjectToBePlaced.BasePoint.x, moObjectToBePlaced.BasePoint.y] = moObjectToBePlaced;

                    //place point of origin to indicate place is taken by certain object if any
                    if (moObjectToBePlaced.Area != null)
                    {
                        foreach (MapPoint PArea in moObjectToBePlaced.Area)
                        {
                            this.iarrMap[(int)MapLayer.Objects2, moObjectToBePlaced.BasePoint.x + PArea.x, moObjectToBePlaced.BasePoint.y + PArea.y] = moObjectToBePlaced.BasePoint.ToString();
                        }
                    }
                    return true;
                }
            }
            return false;
        }

        //public bool PlaceObjectInLayer2(MapObject moObjectToBePlaced)
        //{
        //    //if place is not taken
        //    if (CanPlaceObjectInLayer2(moObjectToBePlaced))
        //    {
        //        this.iarrMap[(int)MapLayer.Objects2, moObjectToBePlaced.BasePoint.x, moObjectToBePlaced.BasePoint.y] = moObjectToBePlaced;
        //        //place point of origin to indicate place is taken by certain object
        //        foreach (MapPoint PArea in moObjectToBePlaced.Area)
        //        {
        //            this.iarrMap[(int)MapLayer.Objects2, moObjectToBePlaced.BasePoint.x + PArea.x, moObjectToBePlaced.BasePoint.y + PArea.y] = moObjectToBePlaced.BasePoint.ToString(); ;
        //        }

        //        return true;
        //    }
        //    return false;
        //}

        /// <summary>
        /// Tranforms the array of objects to map file
        /// </summary>
        public void RenderObjectsToMapFile(string strMapName,bool bToSetChaosWeeks  , bool bDisableIT)
        {
            //used to write objects to map file
            ObjectsWriter obwrTransformer = new ObjectsWriter();
            ObjectsReader obrdMines = new ObjectsReader("Mines.xml");
            ObjectsReader obrdResources = new ObjectsReader("Resources.xml");
            ObjectsReader obrdBlocks = new ObjectsReader("Blocks.xml");

            //currently get data from other objects file, should be changed to get it from template..
            System.Xml.XmlElement xObjects = new ObjectsReader("OtherObjects.xml").GetObjectsData();
            Guards guardsGenerator = new Guards();
            
            //go over all array values except the edges which are none object plaeable
            for (int x = 0; x < (int)eMapSize ; x++)
            {
                string strr = string.Empty;
                for (int y = 0; y < (int)eMapSize ; y++)
                {
                    MapObject mpobjObject = this.iarrMap[(int)MapLayer.Objects, x, y] as MapObject ;
                    if (mpobjObject != null) //if its null no object is to be places
                    {
                        switch (mpobjObject.Type)
                        {
                            case eObjectType.Barrier:
                                obwrTransformer.SetObjectAtPosAndGenerateID(x, y, obrdBlocks.GetObjectsData().SelectSingleNode("//Object[@Name='" + ((MapObject)this.iarrMap[(int)MapLayer.Objects, x, y]).strName + "']"), ((MapObject)this.iarrMap[(int)MapLayer.Objects, x, y]).Rotation, ((MapObject)this.iarrMap[(int)MapLayer.Objects, x, y]).strName);
                                break;
                            case eObjectType.Block:
                                obwrTransformer.SetObjectAtPosAndGenerateID(x, y, obrdBlocks.GetObjectsData().SelectSingleNode("//Object[@Name='" + ((MapObject)this.iarrMap[(int)MapLayer.Objects, x, y]).strName + "']"), ((MapObject)this.iarrMap[(int)MapLayer.Objects, x, y]).Rotation, ((MapObject)this.iarrMap[(int)MapLayer.Objects, x, y]).strName);
                                break;
                            case eObjectType.Monolith_Two_Way:
                                XmlNode xndGroupedTwoWayPortal = ObjectSpacificPropertiesHelper.SetTwoWayPortalId(xObjects.SelectSingleNode("//Object[@Name='Monolith_Two_Way']").Clone(), ((MapObject)this.iarrMap[(int)MapLayer.Objects, x, y]).ObjectSpacificProperties[eObjectsSpacificProperties.TwoWayPortalGroupID.ToString()]);
                                obwrTransformer.SetObjectAtPosAndGenerateID(x, y, xndGroupedTwoWayPortal, ((MapObject)this.iarrMap[(int)MapLayer.Objects, x, y]).Rotation, ((MapObject)this.iarrMap[(int)MapLayer.Objects, x, y]).strName);
                                break;
                            case eObjectType.Guard:                                  
                                    XmlNode xndGuard = guardsGenerator.GetRandomGuards( (int)  (mpobjObject.iValue * double.Parse( Settings.Default.MonsterFactor)) , false,(eMonsterStrength) Enum.Parse(typeof(eMonsterStrength), thSelectedTemplate.GetZoneStrength((int)iarrMap[(int)MapLayer.Zones,x,y ]-1)));
                                    obwrTransformer.SetObjectAtPosAndGenerateID(x, y, xndGuard, ((MapObject)this.iarrMap[(int)MapLayer.Objects, x, y]).Rotation, ((MapObject)this.iarrMap[(int)MapLayer.Objects, x, y]).strName);
                                break; /*
                            case eObjectType.GuardWithoutGrow:
                                XmlNode xndGuard = guardsGenerator.GetRandomGuards((int)(mpobjObject.iValue * double.Parse(Settings.Default.MonsterFactor)), false, (eMonsterStrength)Enum.Parse(typeof(eMonsterStrength), thSelectedTemplate.GetZoneStrength((int)iarrMap[(int)MapLayer.Zones, x, y] - 1)));
                                obwrTransformer.SetObjectAtPosAndGenerateID(x, y, xndGuard, ((MapObject)this.iarrMap[(int)MapLayer.Objects, x, y]).Rotation, ((MapObject)this.iarrMap[(int)MapLayer.Objects, x, y]).strName);
                                break;*/
                            case eObjectType.General:
                                    XmlNode xndTemplateZoneObjects = thSelectedTemplate.GetZoneXML((int) iarrMap[(int)MapLayer.Zones, x, y]);
                                    obwrTransformer.SetObjectAtPosAndGenerateID(x, y, xndTemplateZoneObjects.SelectSingleNode(".//Object[@Name='" + ((MapObject)this.iarrMap[(int)MapLayer.Objects, x, y]).strName + "']"), ((MapObject)this.iarrMap[(int)MapLayer.Objects, x, y]).Rotation, ((MapObject)this.iarrMap[(int)MapLayer.Objects, x, y]).strName);
                                break;
                            case eObjectType.Town:
                                    XmlNode xndTownData = ObjectSpacificPropertiesHelper.GenerateTownXML(mpobjObject, xObjects.SelectSingleNode(".//Object[@Name='RandomTown']").Clone(), guardsGenerator);
                                    obwrTransformer.SetObjectAtPosAndGenerateID(x, y, xndTownData, ((MapObject)this.iarrMap[(int)MapLayer.Objects, x, y]).Rotation, ((MapObject)this.iarrMap[(int)MapLayer.Objects, x, y]).strName);
                                break;
                            case eObjectType.Mine:
                                obwrTransformer.SetObjectAtPosAndGenerateID(x, y, obrdMines.GetObjectsData().SelectSingleNode("//Object[@Name='" + ((MapObject)this.iarrMap[(int)MapLayer.Objects, x, y]).strName + "']"), ((MapObject)this.iarrMap[(int)MapLayer.Objects, x, y]).Rotation, ((MapObject)this.iarrMap[(int)MapLayer.Objects, x, y]).strName);
                                break;
                            case eObjectType.Treasure:
                                obwrTransformer.SetObjectAtPosAndGenerateID(x, y, obrdResources.GetObjectsData().SelectSingleNode("//Object[@Name='" + ((MapObject)this.iarrMap[(int)MapLayer.Objects, x, y]).strName + "']"), ((MapObject)this.iarrMap[(int)MapLayer.Objects, x, y]).Rotation, ((MapObject)this.iarrMap[(int)MapLayer.Objects, x, y]).strName);
                                break;
                            case eObjectType.Artifacts:
                                XmlNode xndTemplateZoneObjectsArts = thSelectedTemplate.GetZoneXML((int)iarrMap[(int)MapLayer.Zones, x, y]);
                                obwrTransformer.SetObjectAtPosAndGenerateID(x, y, xndTemplateZoneObjectsArts.SelectSingleNode(".//Object[@Name='" + ((MapObject)this.iarrMap[(int)MapLayer.Objects, x, y]).strName + "']"), ((MapObject)this.iarrMap[(int)MapLayer.Objects, x, y]).Rotation, ((MapObject)this.iarrMap[(int)MapLayer.Objects, x, y]).strName);
                                break;
                            case eObjectType.Complex:
                                ObjectSpacificPropertiesHelper.RenderComplexObject(obwrTransformer, (MapObject)this.iarrMap[(int)MapLayer.Objects, x, y], xObjects);
                                break;
                            case eObjectType.HighDwelling:
                                XmlNode xndProcessedDwelling = ObjectSpacificPropertiesHelper.ProcessHighDwellingObject(xObjects.SelectSingleNode(".//Object[@Name='" + ((MapObject)this.iarrMap[(int)MapLayer.Objects, x, y]).strName + "']" ).Clone(), (MapObject)this.iarrMap[(int)MapLayer.Objects, x, y]);
                                obwrTransformer.SetObjectAtPosAndGenerateID(x, y, xndProcessedDwelling, ((MapObject)this.iarrMap[(int)MapLayer.Objects, x, y]).Rotation, ((MapObject)this.iarrMap[(int)MapLayer.Objects, x, y]).strName);
                                break;
                            case eObjectType.LowDwelling:
                                XmlNode xndProcessedLowDwelling = ObjectSpacificPropertiesHelper.ProcessLowDwellingObject(xObjects.SelectSingleNode(".//Object[@Name='" + ((MapObject)this.iarrMap[(int)MapLayer.Objects, x, y]).strName + "']").Clone(), (MapObject)this.iarrMap[(int)MapLayer.Objects, x, y]);
                                obwrTransformer.SetObjectAtPosAndGenerateID(x, y, xndProcessedLowDwelling, ((MapObject)this.iarrMap[(int)MapLayer.Objects, x, y]).Rotation, ((MapObject)this.iarrMap[(int)MapLayer.Objects, x, y]).strName);
                                break;
                            case eObjectType.Garrison: // обычные ворота 
                                XmlNode xndGarrison = ObjectSpacificPropertiesHelper.GenerateGarrisonXML(mpobjObject, xObjects.SelectSingleNode(".//Object[@Name='" + ((MapObject)this.iarrMap[(int)MapLayer.Objects, x, y]).strName + "']").Clone(), guardsGenerator, (eMonsterStrength)Enum.Parse(typeof(eMonsterStrength), thSelectedTemplate.GetZoneStrength((int)iarrMap[(int)MapLayer.Zones, x, y] - 1)));
                                obwrTransformer.SetObjectAtPosAndGenerateID(x, y, xndGarrison, ((MapObject)this.iarrMap[(int)MapLayer.Objects, x, y]).Rotation, ((MapObject)this.iarrMap[(int)MapLayer.Objects, x, y]).strName);
                                break;
                            case eObjectType.Forpost: // ворота со стенами
                                XmlNode xndForpost = ObjectSpacificPropertiesHelper.GenerateGarrisonXML(mpobjObject, xObjects.SelectSingleNode(".//Object[@Name='" + ((MapObject)this.iarrMap[(int)MapLayer.Objects, x, y]).strName + "']").Clone(), guardsGenerator, (eMonsterStrength)Enum.Parse(typeof(eMonsterStrength), thSelectedTemplate.GetZoneStrength((int)iarrMap[(int)MapLayer.Zones, x, y] - 1)));
                                obwrTransformer.SetObjectAtPosAndGenerateID(x, y, xndForpost, ((MapObject)this.iarrMap[(int)MapLayer.Objects, x, y]).Rotation, ((MapObject)this.iarrMap[(int)MapLayer.Objects, x, y]).strName);
                                break;
                            default:
                                break;
                        }

                    }

                       // if (!this.iarrMap[(int)MapLayer.Objects, x, y].ToString().Contains(",")) //if it contains point coordinate then its an object place but not the source
                            
                }
            }


            //go over all array values in layer 2
            for (int x = 1; x < (int)eMapSize - 1; x++)
            {
                for (int y = 1; y < (int)eMapSize - 1; y++)
                {
                    if (this.iarrMap[(int)MapLayer.Objects2, x, y] != null) //if its null no object is to be places
                        if (!this.iarrMap[(int)MapLayer.Objects2, x, y].ToString().Contains(",")) //if it contains point coordinate then its an object place but not the source
                            obwrTransformer.SetObjectAtPosAndGenerateID(x, y, obrdBlocks.GetObjectsData().SelectSingleNode("//Object[@Name='" + ((MapObject)this.iarrMap[(int)MapLayer.Objects2, x, y]).strName + "']"), ((MapObject)this.iarrMap[(int)MapLayer.Objects2, x, y]).Rotation, ((MapObject)this.iarrMap[(int)MapLayer.Objects2, x, y]).strName);
                }
            }

            if (bToSetChaosWeeks)
                obwrTransformer.SetMoonCalendar();
            if (bDisableIT)
                obwrTransformer.RemoveITSpell();

            obwrTransformer.InitPlayersParams(thSelectedTemplate.IPLAYERNUMBER);
            obwrTransformer.SaveMap(strMapName);

#if (DEBUG) // {
            //guardsGenerator.ShowComboArmyResults();
#endif //DEBUG }
        }

        /// <summary>
        /// mark all as maybe block
        /// </summary>
        public void FillBlockMask()
        {
            for (int i = 0; i < (int)eMapSize ; i++)
            {
                for (int j = 0; j < (int)eMapSize ; j++)
                {
                    iarrMap[(int)MapLayer.Blocks, i, j] = eBlockStatus.MaybeBlock;
                }
            }
        }

        public MapPoint[,] mparrMaxDistancePointsInZones;
        
        //array of points which define a round trip around a sqaure to check all its adjacencies 
        MapPoint[] mpRoundTrip =  { 
            new MapPoint(0,-1) , new MapPoint (1,0),
            new MapPoint (0,1) , new MapPoint( -1, 0) 
        };

        MapPoint[] mpRoundTripAllDirections =  {
            new MapPoint(0,-1) , new MapPoint (1,-1),
             new MapPoint (1,0), new MapPoint( 1, 1) ,
            new MapPoint (0,1) , new MapPoint(-1,1)
            , new MapPoint(-1,0) , new MapPoint(-1,-1) 
        };

        //dwp! "облако" точек толщиной 3
        MapPoint[] mpRoundTripAllDirections2 =  {
            new MapPoint(-3,-3), new MapPoint(-3,-2),  new MapPoint(-3,-1),  new MapPoint(-3,0),  new MapPoint(-3,1),  new MapPoint(-3,2), new MapPoint(-3,3),
            new MapPoint(-2,-3), new MapPoint(-2,-2) , new MapPoint(-2,-1) , new MapPoint(-2,0) , new MapPoint(-2,1) , new MapPoint(-2,2), new MapPoint(-2,3),
            new MapPoint(-1,-3), new MapPoint(-1,-2) , new MapPoint(-1,-1) , new MapPoint(-1,0) , new MapPoint(-1,1) , new MapPoint(-1,2), new MapPoint(-1,3), 
            new MapPoint(0,-3),  new MapPoint(0,-2) ,  new MapPoint(0,-1) ,                       new MapPoint(0,1) ,  new MapPoint(0,2),  new MapPoint(0, 3), 
            new MapPoint(1,-3),  new MapPoint(1,-2),   new MapPoint(1,-1) ,  new MapPoint(1,0) ,  new MapPoint(1,1) ,  new MapPoint(1,2) , new MapPoint(1,3),
            new MapPoint(2,-3),  new MapPoint(2,-2) ,  new MapPoint(2,-1) ,  new MapPoint(2,0) ,  new MapPoint(2,1) ,  new MapPoint(2,2),  new MapPoint(2,3),
            new MapPoint(3,-3),  new MapPoint(3,-2) ,  new MapPoint(3,-1) ,  new MapPoint(3,0) ,  new MapPoint(3,1) ,  new MapPoint(3,2),  new MapPoint(3,3)
        };

        public void FixIsolatedMaybeBlockSpace()
        {
            for (int i = 0; i < (int)eMapSize; i++)
            {
                for (int j = 0; j < (int)eMapSize; j++)
                {
                    if (IsSerroundedByBlocks(i, j))
                        iarrMap[(int)MapLayer.Blocks, i, j] = eBlockStatus.Block;
                }
            }

        }



        internal void FixDiagnolOnlyAccessToMaybeBlock()
        {
            for (int i = 0; i < (int)eMapSize; i++)
            {
                for (int j = 0; j < (int)eMapSize; j++)
                {
                    if ((eBlockStatus)iarrMap[(int)MapLayer.Blocks, i, j] == eBlockStatus.MaybeBlock)
                    {
                        if (IsDiagnolAcesssOnly(i, j))
                        {

                            iarrMap[(int)MapLayer.Blocks, i, j] = eBlockStatus.Block;

                            //mark all around as block too
                            foreach (MapPoint mpNewPoint in mpRoundTripAllDirections)
                            {
                                if (IsInsideMapLimit(i + mpNewPoint.x, j + mpNewPoint.y))
                                {
                                    iarrMap[(int)MapLayer.Blocks, i + mpNewPoint.x, j + mpNewPoint.y] = eBlockStatus.Block;
                                }
                            }

                            FixIsolatedMaybeBlockSpace();
                        }
                    }
                }
            }
        }



        private bool IsDiagnolAcesssOnly(int i, int j)
        {

            MapPoint mpFirst;
            MapPoint mpDiagnol;
            MapPoint mpSecond;

            for (int Index = 0; Index < 8; Index+=2)
			{
                if (2 + Index != 8)
                {
                    //check access only to right upper diagnol 
                    mpFirst = new MapPoint(i + mpRoundTripAllDirections[0 + Index].x, j + mpRoundTripAllDirections[0 + Index].y);
                    mpDiagnol = new MapPoint(i + mpRoundTripAllDirections[1 + Index].x, j + mpRoundTripAllDirections[1 + Index].y);
                    mpSecond = new MapPoint(i + mpRoundTripAllDirections[2 + Index].x, j + mpRoundTripAllDirections[2 + Index].y);
                }
                else
                {
                    //check access only to right upper diagnol 
                    mpFirst = new MapPoint(i + mpRoundTripAllDirections[0 + Index].x, j + mpRoundTripAllDirections[0 + Index].y);
                    mpDiagnol = new MapPoint(i + mpRoundTripAllDirections[1 + Index].x, j + mpRoundTripAllDirections[1 + Index].y);
                    mpSecond = new MapPoint(i + mpRoundTripAllDirections[0].x, j + mpRoundTripAllDirections[0].y);
 
                }

                if (IsInsideMapLimit(mpFirst) && IsInsideMapLimit(mpDiagnol) && IsInsideMapLimit(mpSecond))
                {

                    if ((eBlockStatus)iarrMap[(int)MapLayer.Blocks, mpFirst.x, mpFirst.y] == eBlockStatus.Block &&
                         ((eBlockStatus)iarrMap[(int)MapLayer.Blocks, mpDiagnol.x, mpDiagnol.y] == eBlockStatus.MaybeBlock ||
                          (eBlockStatus)iarrMap[(int)MapLayer.Blocks, mpDiagnol.x, mpDiagnol.y] == eBlockStatus.Preserved )&&
                         (eBlockStatus)iarrMap[(int)MapLayer.Blocks, mpSecond.x, mpSecond.y] == eBlockStatus.Block
                        )
                    {
                        return true;
                    }
                }
			 
			}

            return false;

        }

        /// <summary>
        /// checks 4 directions from point , if none are open then its a bugged place needs to be fixed
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <returns></returns>
        private bool IsSerroundedByBlocks(int i, int j)
        {
            int iPathWay = 0;
            foreach (MapPoint mpDirection in mpRoundTrip)
            {
                if (IsInsideMapLimit(i + mpDirection.x, j + mpDirection.y))
                    if ((eBlockStatus)iarrMap[(int)MapLayer.Blocks, i + mpDirection.x, j + mpDirection.y] == eBlockStatus.MaybeBlock ||
                        (eBlockStatus)iarrMap[(int)MapLayer.Blocks, i + mpDirection.x, j + mpDirection.y] == eBlockStatus.Preserved) /*dwp*/
                        iPathWay++;
            }
            if (iPathWay > 0)
                return false;
            else
                return true;
        }

        public void ComputeAStar(MapPoint mpOriginPoint , int x, int y, int iDistance)
        {
            int[] StackElement,CurElement;
            int CurStack = 0;
            int iNewx, iNewy;
            bool flfound;
            ArrayList Stacks = new ArrayList();

            StackElement = new int[3];
            StackElement[0] = x; StackElement[1] = y; StackElement[2] = iDistance;
            Stacks.Add(StackElement); 
            do
            {
              flfound = false;
              CurElement = (int[])Stacks[CurStack];
              foreach (MapPoint mpDirection in mpRoundTrip)
              {
                iNewx = mpOriginPoint.x + CurElement[0] + mpDirection.x;
                iNewy = mpOriginPoint.y + CurElement[1] + mpDirection.y;
                //at this stage maybe block basically is all inside a zone
                if ((eBlockStatus)iarrMap[(int)MapLayer.Blocks, iNewx, iNewy] == eBlockStatus.MaybeBlock ||
                    (eBlockStatus)iarrMap[(int)MapLayer.Blocks, iNewx, iNewy] == eBlockStatus.Preserved)  /*dwp */
                {
                    if (iarrMap[(int)MapLayer.AStarDistance, iNewx, iNewy] != null)
                    {
                        if ((int)iarrMap[(int)MapLayer.AStarDistance, iNewx, iNewy] > CurElement[2] + 2)
                        {
                          //set distance and later go in that direction
                          iarrMap[(int)MapLayer.AStarDistance, iNewx, iNewy] = CurElement[2] + 2;
                          StackElement = new int[3];
                          StackElement[0] = CurElement[0] + mpDirection.x;
                          StackElement[1] = CurElement[1] + mpDirection.y;
                          StackElement[2] = CurElement[2] + 2;
                          Stacks.Add(StackElement); CurStack++;
                          flfound = true;
                        }
                    }
                    else
                    {
                        //set distance and later go compute a path in that direction
                        iarrMap[(int)MapLayer.AStarDistance, iNewx, iNewy] = CurElement[2] + 2;
                        StackElement = new int[3];
                        StackElement[0] = CurElement[0] + mpDirection.x;
                        StackElement[1] = CurElement[1] + mpDirection.y;
                        StackElement[2] = CurElement[2] + 2;
                        Stacks.Add(StackElement); CurStack++;
                        flfound = true;
                    }
                }
              }
              if (!flfound)
              {
                  Stacks.RemoveAt(CurStack);
                  CurStack--;
              }
            } while (CurStack >= 0);
        }

        /// <summary>
        /// computes a road from point a to b only in selected zone and only in what is preserved block
        /// </summary>
        /// <param name="mpOriginPoint"></param>
        /// <param name="iDistance"></param>
        public void ComputeAStarForRoads(MapPoint mpOriginPoint, int iDistance,int iZoneIndex)
        {
            ArrayList arrDirectionsToCheck = new ArrayList();
            foreach (MapPoint mpDirection in mpRoundTrip)
            {
               int iNewx = mpOriginPoint.x + mpDirection.x;
               int iNewy = mpOriginPoint.y + mpDirection.y;
               if(IsInsideMapLimit(iNewx, iNewy)) {
                if ((eBlockStatus)iarrMap[(int)MapLayer.Blocks, iNewx, iNewy] == eBlockStatus.Preserved && (int)iarrMap[(int)MapLayer.Zones, iNewx, iNewy] == iZoneIndex )
                {
                    if (iarrMap[(int)MapLayer.AStarDistance, iNewx, iNewy] != null)
                    {
                        if ((int)iarrMap[(int)MapLayer.AStarDistance, iNewx, iNewy] > iDistance + 2)
                        {
                            //set distance and later go in that direction
                            iarrMap[(int)MapLayer.AStarDistance, iNewx, iNewy] = iDistance + 2;
                            arrDirectionsToCheck.Add(mpDirection);
                        }
                    }
                    else
                    {
                        //set distance and later go compute a path in that direction
                        iarrMap[(int)MapLayer.AStarDistance, iNewx, iNewy] = iDistance + 2;
                        arrDirectionsToCheck.Add(mpDirection);
                    }
                }
               }
            }

            foreach (MapPoint mpDirection in arrDirectionsToCheck)
            {
                ComputeAStarForRoads(mpOriginPoint + mpDirection, iDistance + 2,iZoneIndex);
            }

        }
        

        /// <summary>
        /// marks a pathway between the blocks 
        /// </summary>
        public void MarkPathWay()
        {
            ArrayList arrZoneFreePoints;
            int iRandomIndex;
            try
            {
                //compute a star for all points
                //ComputeAStar(mparrMaxDistancePointsInZones[0, 0], 0);

                for (int i = 0; i < thSelectedTemplate.ZoneNumber; i++)
                {
                    //first marks a long way  - basec on a star
                    int iZoneSize = GetZoneSize(i + 1);
                    iarrMap[(int)MapLayer.AStarDistance, mparrMaxDistancePointsInZones[0, i].x, mparrMaxDistancePointsInZones[0, i].y] = 0;
                    ComputeAStar(mparrMaxDistancePointsInZones[0, i], 0, 0, 0);
                    ReturnSignalArrayDefaults();
                    if (!MarkPathForZone(mparrMaxDistancePointsInZones[1, i], mparrMaxDistancePointsInZones[0, i]))
                    {
                        /*dwp нет пути из 1 в 0 . строим из 0 в 1*/
                        iarrMap[(int)MapLayer.AStarDistance, mparrMaxDistancePointsInZones[1, i].x, mparrMaxDistancePointsInZones[1, i].y] = 0;
                        ComputeAStar(mparrMaxDistancePointsInZones[1, i], 0, 0, 0);
                        ReturnSignalArrayDefaults();
                        MarkPathForZone(mparrMaxDistancePointsInZones[0, i], mparrMaxDistancePointsInZones[1, i]);
                    }

                    if (!Program.frmMainMenu.RadioButton1.Checked) // dwp игроков больше чем 2
                    {
                        iarrMap[(int)MapLayer.AStarDistance, mparrMaxDistancePointsInZones[2, i].x, mparrMaxDistancePointsInZones[2, i].y] = 0;
                        ComputeAStar(mparrMaxDistancePointsInZones[2, i], 0, 0, 0);
                        ReturnSignalArrayDefaults();
                        if(!MarkPathForZone(mparrMaxDistancePointsInZones[3, i], mparrMaxDistancePointsInZones[2, i])) 
                        {
                            /*dwp нет пути из 3 в 2 . строим из 2 в 3*/
                           iarrMap[(int)MapLayer.AStarDistance, mparrMaxDistancePointsInZones[3, i].x, mparrMaxDistancePointsInZones[3, i].y] = 0;
                           ComputeAStar(mparrMaxDistancePointsInZones[3, i], 0, 0, 0);
                           ReturnSignalArrayDefaults();
                           MarkPathForZone(mparrMaxDistancePointsInZones[2, i], mparrMaxDistancePointsInZones[3, i]);
                        }
                    }
                    arrZoneFreePoints = GetPreservedPointInZone(i);
                    //now mark randomly points in random direction until  density is reached 
                    do {
                        iRandomIndex = Randomizer.rnd.Next(arrZoneFreePoints.Count);
                        MapPoint mpRandomFreePoint = (MapPoint)arrZoneFreePoints[iRandomIndex];

                        int iRandomDir = Randomizer.rnd.Next(8);
                        MarkRandomPathForZone(mpRandomFreePoint + mpRoundTripAllDirections[iRandomDir], iRandomDir, true);
                        arrZoneFreePoints.RemoveAt(iRandomIndex);
                    } while ((double)CountPreservedPoints(i) / iZoneSize < 0.275 && arrZoneFreePoints.Count > 0);
                    arrZoneFreePoints.Clear();
                }
#if (DEBUG)
                //  RMGMap.FlagZonesForDebug();
                dumpBlockMaskToFileErgo();
#endif

            }
            catch (Exception ex)
            {
                // MessageBox.Show("Oops.. A bug just occoured - Please try again and send me the error report.. (PrintScreen and send )/n Error Message : " + ex.Message + "/n Stack Trace : " + ex.StackTrace);
#if (DEBUG)
                //  RMGMap.FlagZonesForDebug();
                dumpBlockMaskToFileErgo();
#endif

                throw ex;
            }
        }

        MapPoint[] mparrMarkingPoint = {
            new MapPoint (1,-1) , new MapPoint (2,-1), new MapPoint (3,-1),  new MapPoint (4,-1),
            new MapPoint (1,-2) , new MapPoint (2,-2), new MapPoint (3,-2),  new MapPoint (4,-2),
            new MapPoint (1,-3) , new MapPoint (2,-3), new MapPoint (3,-3),  new MapPoint (4,-3),
            new MapPoint (1,0) ,  new MapPoint (2,0) , new MapPoint (3,0) ,  new MapPoint (4,0) ,
            new MapPoint (1,1) ,  new MapPoint (2,1) , new MapPoint (3,1) ,  new MapPoint (4,1) ,
            new MapPoint (1,2) ,  new MapPoint (2,2) , new MapPoint (3,2) ,  new MapPoint (4,2) ,
            new MapPoint (1,3) ,  new MapPoint (2,3) , new MapPoint (3,3) ,  new MapPoint (4,3) ,
            new MapPoint (-1,0) , new MapPoint(-2,0),  new MapPoint (-3,0) , new MapPoint (-4,0),
            new MapPoint (-1,1) , new MapPoint(-2,1),  new MapPoint (-3,1),  new MapPoint(-4,1),
            new MapPoint (-1,2) , new MapPoint(-2,2),  new MapPoint (-3,2),  new MapPoint(-4,2),
            new MapPoint (-1,3) , new MapPoint(-2,3),  new MapPoint (-3,3),  new MapPoint(-4,3),
            new MapPoint (-1,-1), new MapPoint(-2,-1), new MapPoint (-3,-1), new MapPoint (-4,-1),
            new MapPoint (-1,-2), new MapPoint(-2,-2), new MapPoint (-3,-2), new MapPoint (-4,-2),
            new MapPoint (-1,-3), new MapPoint(-2,-3), new MapPoint (-3,-3), new MapPoint (-4,-3),
            new MapPoint (0, 1),  new MapPoint(0,-1),  new MapPoint (0, 2),  new MapPoint(0,-2), new MapPoint (0, 3),  new MapPoint(0,-3)
        };  


        /// <summary>
        /// go around preserved alots and mark it as free
        /// </summary>
        public void MarkSquaresAroundPreservedAsFree()
        {
            for (int i = 0; i < (int)eMapSize; i++)
            {
                for (int j = 0; j < (int)eMapSize; j++)
                {
                    //foreach square if its a preserve serround it with free tags ( only change maybe block status) 
                    if ( (eBlockStatus)iarrMap[(int)MapLayer.Blocks,i,j] == eBlockStatus.Preserved )
                        MarkSquaresAsFree(i, j,  (int) iarrMap[(int)MapLayer.Zones, i, j]);
                    
                }
            }
        }

        //goes over


        /// <summary>
        /// goes over point specified and mark around as defined in marking point array
        /// right now 3x3 squares
        /// </summary>
        /// <param name="i">x of point</param>
        /// <param name="j">y of  point</param>
        private void MarkSquaresAsFree(int i, int j , int iZoneIndex)
        {
            int iNewX,iNewY;
            foreach (MapPoint mpDirection in mparrMarkingPoint)
            {
                iNewX = i + mpDirection.x;
                iNewY = j + mpDirection.y;
                if ( IsInsideMapLimit ( iNewX , j + mpDirection.y ) )
                    if ((eBlockStatus)iarrMap[(int)MapLayer.Blocks, iNewX, iNewY] == eBlockStatus.MaybeBlock)
                    {
                        if ( (int) iarrMap[ (int ) MapLayer.Zones , iNewX , iNewY ] ==  iZoneIndex )
                            iarrMap[(int)MapLayer.Blocks, iNewX, iNewY] = eBlockStatus.Free;
                    }
            }
        }

        private int CountPreservedPoints(int iZoneIndex)
        {
            int iFreePointsCounter = 0;

            for (int i = 0; i < (int)eMapSize; i++)
            {
                for (int j = 0; j < (int)eMapSize; j++)
                {
                    if ((int) iarrMap[(int)MapLayer.Zones, i, j] == iZoneIndex + 1)
                    {
                        if ((eBlockStatus)iarrMap[(int)MapLayer.Blocks, i, j] == eBlockStatus.Preserved)
                            iFreePointsCounter++;
                    }
                }
            }
            return iFreePointsCounter;
        }

        /// <summary>
        /// marks the path from origin to destination - only marks short path by astar algorithem
        /// </summary>
        /// <param name="mpOrigin"></param>
        /// <param name="mpDestination"></param>
        private bool MarkPathForZone( MapPoint mpOrigin, MapPoint mpDestination)
        {
            //mark point as must free
            eBlockStatus st = (eBlockStatus)iarrMap[(int)MapLayer.Blocks, mpOrigin.x, mpOrigin.y];
            iarrMap[(int)MapLayer.Blocks, mpOrigin.x, mpOrigin.y] = eBlockStatus.Preserved;
            barrAStarVisit[mpOrigin.x, mpOrigin.y] = true;

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
                    mparrRoundPoints.Add(mpPointToExamine);
                    if (iarrMap[(int)MapLayer.AStarDistance, mpPointToExamine.x, mpPointToExamine.y] != null)
                    {
                        //this is condition to prevent going on the same road again and again
                        //if ((eBlockStatus)iarrMap[(int)MapLayer.Blocks, mpPointToExamine.x, mpPointToExamine.y] != eBlockStatus.Preserved)
                        if (!barrAStarVisit[mpPointToExamine.x, mpPointToExamine.y])
                            if ((int)iarrMap[(int)MapLayer.AStarDistance, mpPointToExamine.x, mpPointToExamine.y] < iMinDirectionValue)
                                iMinDirectionValue = (int)iarrMap[(int)MapLayer.AStarDistance, mpPointToExamine.x, mpPointToExamine.y];
                    }
                }

                for (int i = 0; i < mparrRoundPoints.Count; i++)
                {
                    if (iarrMap[(int)MapLayer.AStarDistance, ((MapPoint)mparrRoundPoints[i]).x, ((MapPoint)mparrRoundPoints[i]).y] != null)
                    {
                        if ((int)iarrMap[(int)MapLayer.AStarDistance, ((MapPoint)mparrRoundPoints[i]).x, ((MapPoint)mparrRoundPoints[i]).y] != iMinDirectionValue)
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
                 if (mparrRoundPoints.Count == 0)
                {   // dwp нет пути!
                    return (false);
                }
                int iRandomIndex = Randomizer.rnd.Next(mparrRoundPoints.Count);
                return (MarkPathForZone((MapPoint)mparrRoundPoints[iRandomIndex], mpDestination));
            }
            else return (true);
        }

        private void MarkRandomPathForZone(MapPoint mpOrigin , int iobjdirPrevDir, bool switchdir)
        {
            if ((eBlockStatus) iarrMap[(int)MapLayer.Blocks, mpOrigin.x, mpOrigin.y] == eBlockStatus.MaybeBlock)
            {
                //mark point as must be free
                iarrMap[(int)MapLayer.Blocks, mpOrigin.x, mpOrigin.y] = eBlockStatus.Preserved;

                if (switchdir)
                {
                    if (iobjdirPrevDir != 7)
                    {
                        //mark also in +1 direction
                        if ((eBlockStatus)iarrMap[(int)MapLayer.Blocks, mpOrigin.x + mpRoundTripAllDirections[iobjdirPrevDir + 1].x, mpOrigin.y + mpRoundTripAllDirections[iobjdirPrevDir + 1].y] != eBlockStatus.Block)
                            iarrMap[(int)MapLayer.Blocks, mpOrigin.x + mpRoundTripAllDirections[iobjdirPrevDir + 1].x, mpOrigin.y + mpRoundTripAllDirections[iobjdirPrevDir + 1].y] = eBlockStatus.Preserved;
                    }
                    else
                    {
                        if ((eBlockStatus)iarrMap[(int)MapLayer.Blocks, mpOrigin.x + mpRoundTripAllDirections[iobjdirPrevDir + 1 - 7].x, mpOrigin.y + mpRoundTripAllDirections[iobjdirPrevDir + 1 - 7].y] != eBlockStatus.Block)
                            iarrMap[(int)MapLayer.Blocks, mpOrigin.x + mpRoundTripAllDirections[iobjdirPrevDir + 1 - 7].x, mpOrigin.y + mpRoundTripAllDirections[iobjdirPrevDir + 1 - 7].y] = eBlockStatus.Preserved;
                    }
                }
                else
                {
                    //also mark in direction-1
                    if (iobjdirPrevDir > 0)
                    {
                        if ((eBlockStatus)iarrMap[(int)MapLayer.Blocks, mpOrigin.x + mpRoundTripAllDirections[iobjdirPrevDir - 1].x, mpOrigin.y + mpRoundTripAllDirections[iobjdirPrevDir - 1].y] != eBlockStatus.Block)
                        {
                            iarrMap[(int)MapLayer.Blocks, mpOrigin.x + mpRoundTripAllDirections[iobjdirPrevDir - 1].x, mpOrigin.y + mpRoundTripAllDirections[iobjdirPrevDir - 1].y] = eBlockStatus.Preserved;
                        }
                    }
                    else
                    {
                        if ((eBlockStatus)iarrMap[(int)MapLayer.Blocks, mpOrigin.x + mpRoundTripAllDirections[7].x, mpOrigin.y + mpRoundTripAllDirections[7].y] != eBlockStatus.Block)                        
                        {
                            iarrMap[(int)MapLayer.Blocks, mpOrigin.x + mpRoundTripAllDirections[7].x, mpOrigin.y + mpRoundTripAllDirections[7].y] = eBlockStatus.Preserved;
                        }
                    }
                }
                //go to some random direction with a better (50%) chance to stay in current dir
                double dRandomChance = Randomizer.rnd.NextDouble();

                if (dRandomChance < 0.8)
                {
                    MarkRandomPathForZone(mpOrigin + mpRoundTripAllDirections[iobjdirPrevDir], iobjdirPrevDir, !switchdir);
                }
                else
                {
                    //pick some other random direction 
                    int iRandomDir = Randomizer.rnd.Next(8);
                    MarkRandomPathForZone(mpOrigin + mpRoundTripAllDirections[iobjdirPrevDir], iRandomDir, !switchdir);
                }
            }
        }

        /// <summary>
        ///  Поиск 4х максимально удаленных точек внутри каждой зоны
        /// </summary>
        public void ComputeMaxDistancePointInZones()
        {
            mparrMaxDistancePointsInZones = new MapPoint[4, this.thSelectedTemplate.ZoneNumber]; //dwp четыре точки!
            System.Collections.ArrayList[] arrZonesBorderPoints = new System.Collections.ArrayList[thSelectedTemplate.ZoneNumber];
            for (int i = 0; i < thSelectedTemplate.ZoneNumber; i++)
            {
                arrZonesBorderPoints[i] = new System.Collections.ArrayList();
            }

            for (int i = 1; i < (int)eMapSize - 1; i++)
            {
                for (int j = 1; j < (int)eMapSize - 1; j++)
                {   //dwp если это край зоны.
                    if (  (eBlockStatus)iarrMap[(int)MapLayer.Blocks,i,j] == eBlockStatus.MaybeBlock && (
                          (eBlockStatus)iarrMap[(int)MapLayer.Blocks,i + 1,j] == eBlockStatus.Block ||
                          (eBlockStatus)iarrMap[(int)MapLayer.Blocks,i,j + 1] == eBlockStatus.Block ||
                          (eBlockStatus)iarrMap[(int)MapLayer.Blocks,i - 1,j] == eBlockStatus.Block ||
                          (eBlockStatus)iarrMap[(int)MapLayer.Blocks,i,j - 1] == eBlockStatus.Block ||
                          (eBlockStatus)iarrMap[(int)MapLayer.Blocks,i+1,j+1] == eBlockStatus.Block ||
                          (eBlockStatus)iarrMap[(int)MapLayer.Blocks,i-1,j-1] == eBlockStatus.Block ||
                          (eBlockStatus)iarrMap[(int)MapLayer.Blocks,i-1,j+1] == eBlockStatus.Block ||
                          (eBlockStatus)iarrMap[(int)MapLayer.Blocks,i+1,j-1] == eBlockStatus.Block ||

                          (eBlockStatus)iarrMap[(int)MapLayer.Blocks,i + 1,j] == eBlockStatus.Preserved ||
                          (eBlockStatus)iarrMap[(int)MapLayer.Blocks,i,j + 1] == eBlockStatus.Preserved ||
                          (eBlockStatus)iarrMap[(int)MapLayer.Blocks,i - 1,j] == eBlockStatus.Preserved ||
                          (eBlockStatus)iarrMap[(int)MapLayer.Blocks,i,j - 1] == eBlockStatus.Preserved ||
                          (eBlockStatus)iarrMap[(int)MapLayer.Blocks,i+1,j+1] == eBlockStatus.Preserved ||
                          (eBlockStatus)iarrMap[(int)MapLayer.Blocks,i-1,j-1] == eBlockStatus.Preserved ||
                          (eBlockStatus)iarrMap[(int)MapLayer.Blocks,i-1,j+1] == eBlockStatus.Preserved ||
                          (eBlockStatus)iarrMap[(int)MapLayer.Blocks,i+1,j-1] == eBlockStatus.Preserved
                        ))
                    {
                        int iZoneIndex = (int)iarrMap[(int)MapLayer.Zones, i, j] - 1;
                        arrZonesBorderPoints[iZoneIndex].Add(new MapPoint(i, j));
                    }
                }
            }
            for (int i = 0; i < this.thSelectedTemplate.ZoneNumber; i++)
            {
                int f1 = -1, f2 = -1, f3 = -1, f4 = -1, j, k;
                double maxdistance, distance1, distance2, distance3,curdistance;
                maxdistance = 0;
                for (j = 0; j < arrZonesBorderPoints[i].Count; j++)
                {
                    for (k = 0; k < arrZonesBorderPoints[i].Count; k++)
                    {
                        distance1 = (MapPoint)arrZonesBorderPoints[i][j] - (MapPoint)arrZonesBorderPoints[i][k];
                        if (distance1 > maxdistance)
                        {
                            maxdistance = distance1;
                            f1 = j;
                            f2 = k;
                        }
                    }
                }
                maxdistance = double.MaxValue;
                for (j = 0; j < arrZonesBorderPoints[i].Count; j++)
                {
                    if (j != f1 && j != f2)
                    {
                        distance1 = (MapPoint)arrZonesBorderPoints[i][j] - (MapPoint)arrZonesBorderPoints[i][f1];
                        distance2 = (MapPoint)arrZonesBorderPoints[i][j] - (MapPoint)arrZonesBorderPoints[i][f2];
                        //dwp. третья максимально удаленная точка должна быть равноудалена от первых двух
                        if (Math.Abs(distance1 - distance2) < maxdistance)
                        {
                            f3 = j; 
                            maxdistance = Math.Abs(distance1 - distance2);
                        }
                    }
                }
                maxdistance = double.MaxValue;
                for (j = 0; j < arrZonesBorderPoints[i].Count; j++)
                {
                    if (j != f1 && j != f2 && j != f3)
                    {
                        distance1 = (MapPoint)arrZonesBorderPoints[i][j] - (MapPoint)arrZonesBorderPoints[i][f1];
                        distance2 = (MapPoint)arrZonesBorderPoints[i][j] - (MapPoint)arrZonesBorderPoints[i][f2];
                        distance3 = (MapPoint)arrZonesBorderPoints[i][j] - (MapPoint)arrZonesBorderPoints[i][f3];
                        //dwp. четвертая максимально удаленная точка должна быть равноудалена от первых трех
                        curdistance = Math.Max(Math.Max(distance1, distance2), distance3) -
                                      Math.Min(Math.Min(distance1, distance2), distance3);
                        if (curdistance < maxdistance)
                        {
                            f4 = j;
                            maxdistance = curdistance;
                        }
                    }
                }
                //dwp запоминаем четыре максимально удаленные точки.
                mparrMaxDistancePointsInZones[0, i] = (MapPoint)arrZonesBorderPoints[i][f1];
                mparrMaxDistancePointsInZones[1, i] = (MapPoint)arrZonesBorderPoints[i][f2];
                mparrMaxDistancePointsInZones[2, i] = (MapPoint)arrZonesBorderPoints[i][f3];
                mparrMaxDistancePointsInZones[3, i] = (MapPoint)arrZonesBorderPoints[i][f4];
            }
        }

        private double ComputeDistance(int i, int j, int x, int y)
        {
            return ( Math.Sqrt (    Math.Abs(i-x) * Math.Abs(i-x)  + Math.Abs(j-y) *  Math.Abs(j-y) )  ) ;
        }
        public const double I_PATHWAY_DENSITY = 0.4;// (60% is opened)

        MapPoint[] mparrDirection = { new MapPoint ( 0,-1 ) ,
                                      new MapPoint ( 1,0 ) ,
                                      new MapPoint ( 0,1 )  ,
                                      new MapPoint ( -1,0 ) };

        private ObjectDirection GetSnakeVerticalDirectionByPoint(MapPoint mpStartPoint)
        {

            int iReservedCount = 0;

            //count reserved in left,right direction 
            for (int i = -4; i < 7; i++)
            {
                //first check limits

                if (IsInsideMapLimit(mpStartPoint.x + i, mpStartPoint.y))
                {
                    if ((eBlockStatus)iarrMap[(int)MapLayer.Blocks, mpStartPoint.x + i, mpStartPoint.y] == eBlockStatus.Preserved)
                    {
                        iReservedCount++;
                    }
                }

            }

            //if reserved number is 2 then do direction up down
            if (iReservedCount == 2)
            {
                return ObjectDirection.Down;
            }

            iReservedCount = 0;
            //count reserved in up,down direction 
            for (int i = -4; i < 7; i++)
            {
                //first check limits

                if (IsInsideMapLimit(mpStartPoint.x, mpStartPoint.y + i))
                {
                    if ((eBlockStatus)iarrMap[(int)MapLayer.Blocks, mpStartPoint.x, mpStartPoint.y + i] == eBlockStatus.Preserved)
                    {
                        iReservedCount++;
                    }
                }

            }
            //if now reserved number is 2 then do direction right,left
            if (iReservedCount == 2)
            {
                return ObjectDirection.Right;
            }

            //else return error
            return ObjectDirection.Error;
           
        }

        private MapPoint GetRandomReservedPointInZone(int iZoneIndex)
        {
            ArrayList mparrReservedPoints = new ArrayList();

            for (int i = 0; i < (int)eMapSize; i++)
            {
                for (int j = 0; j < (int)eMapSize; j++)
                {
                    if ((int)iarrMap[(int)MapLayer.Zones, i, j] == iZoneIndex + 1)
                    {
                        if ((eBlockStatus)iarrMap[(int)MapLayer.Blocks, i, j] == eBlockStatus.Preserved)
                        {
                            mparrReservedPoints.Add(new MapPoint(i, j));
                        }
                    }
                }
            }

            //return a random point
            int iRandomIndex = Randomizer.rnd.Next(mparrReservedPoints.Count);
            return (MapPoint)mparrReservedPoints[iRandomIndex];
        }

        private bool IsInNeededZone(int iXIndex, int iYIndex, int iZoneIndex)
        {
            return ((int) iarrMap[(int)MapLayer.Zones, iXIndex, iYIndex] == iZoneIndex + 1);
        }

        private ObjectDirection ReverseDir(ObjectDirection RandomDir)
        {
            switch (RandomDir)
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
            return ObjectDirection.Down;
        }

        /// <summary>
        /// returns a random point from zone marked as maybe block ( all but borders.. )
        /// </summary>
        /// <param name="iZoneIndex"></param>
        /// <returns></returns>
        private ArrayList GetPreservedPointInZone(int iZoneIndex)
        {
            //compile a list with all required points
            ArrayList arrZoneFreePoints = new ArrayList();

            for (int i = 0; i < (int)eMapSize; i++)
            {
                for (int j = 0; j < (int)eMapSize; j++)
                {
                    if ((int)iarrMap[(int)MapLayer.Zones, i, j] == iZoneIndex + 1)
                    {
                        if ((eBlockStatus)iarrMap[(int)MapLayer.Blocks, i, j] == eBlockStatus.Preserved)
                            arrZoneFreePoints.Add(new MapPoint(i, j));
                    }
                }
            }
            return arrZoneFreePoints;
        }
        
        private MapPoint GetRandomPreservedPointInZone(int iZoneIndex)
        {
            //compile a list with all required points
            ArrayList arrZoneFreePoints = new ArrayList();

            for (int i = 0; i < (int)eMapSize; i++)
            {
                for (int j = 0; j < (int)eMapSize; j++)
                {
                    if ((int)iarrMap[(int)MapLayer.Zones, i, j] == iZoneIndex + 1)
                    {
                        if ((eBlockStatus)iarrMap[(int)MapLayer.Blocks, i, j] == eBlockStatus.Preserved)
                            arrZoneFreePoints.Add(new MapPoint(i, j));
                    }
                }
            }

            //select a random point from list
            int iRandomIndex = Randomizer.rnd.Next(arrZoneFreePoints.Count);

            MapPoint mpSelectedPoint = (MapPoint)arrZoneFreePoints[iRandomIndex];

            //arrZoneFreePoints.Clear();
            return mpSelectedPoint;

        }

        /// <summary>
        /// returns a random point from zone marked as maybe block ( all but borders.. )
        /// </summary>
        /// <param name="iZoneIndex"></param>
        /// <returns></returns>
        private MapPoint GetRandomMaybeBlockPointInZone(int iZoneIndex)
        {
            //compile a list with all required points
            ArrayList arrZoneMaybeBlockPoints = new ArrayList();

            for (int i = 0; i < (int)eMapSize; i++)
            {
                for (int j = 0; j < (int)eMapSize; j++)
                {
                    if ( (int) iarrMap[(int)MapLayer.Zones ,i,j] == iZoneIndex+1 )
                    {
                        if ( (eBlockStatus) iarrMap[(int)MapLayer.Blocks , i,j] == eBlockStatus.MaybeBlock )
                            arrZoneMaybeBlockPoints.Add ( new MapPoint(i,j) );
                    }
                }
            }

            //select a random point from list
            int iRandomIndex = Randomizer.rnd.Next( arrZoneMaybeBlockPoints.Count ) ;

            MapPoint mpSelectedPoint = (MapPoint)arrZoneMaybeBlockPoints[iRandomIndex];

           // arrZoneMaybeBlockPoints.Clear();
            return mpSelectedPoint;

        }



        /// <summary>
        /// opens connections between zones according to connection list in template
        /// </summary>
        public void OpenConnections()
        {
            //Initilize the points array (will be used later )
            AccessPoints = new ArrayList[thSelectedTemplate.ZoneNumber,4 ];
            for (int i = 0; i < thSelectedTemplate.ZoneNumber; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    AccessPoints[i, j] = new ArrayList();
                }
            }

            


            int[,] iarrConnectionList = thSelectedTemplate.GetConnectionList();


            for (int i = 0; i < thSelectedTemplate.ConnectionNumber; i++)
            {
                //todo:implement GetConnectionGuardXML
                OpenConnection(iarrConnectionList[0, i], iarrConnectionList[1, i], thSelectedTemplate.GetConnectionGuardXML(i) , i);
            }
        }



        /// <summary>
        /// opens a single connection between 2 zones
        /// </summary>
        /// <param name="iFirstZoneIndex"></param>
        /// <param name="iSecondZoneIndex"></param>
        /// <param name="xndGuardXML"></param>
        private void OpenConnection(int iFirstZoneIndex, int iSecondZoneIndex, XmlNode  xndGuardXML , int iConnectionIndex)
        {
           ArrayList arrAdjPoints;
           int iDir, flHowManyDir; // dwp направление для пробивания
           bool flReady = false,fl1;
           MapPoint mpRandomOpenPoint, mpSecondOpenPoint;

           if (!Settings.Default.OnlyPortals) //dwp. новая настройка. по использованию порталов.
           {
               //select a random point in the adjacenies and unmark it as blocked also place a monster guard
               arrAdjPoints = GetPhisicallyAdjacentPoints(iFirstZoneIndex, iSecondZoneIndex);
               //if enough adjacency points
               if (arrAdjPoints.Count > I_ADJACENCIES_FACTOR) //create an openening
               {
                   ObjectsReader obrdGetPortal = new ObjectsReader("OtherObjects.xml");
                   MapObject mpobGarrison = obrdGetPortal.GetRandomObjectByType(eObjectType.Forpost); //dwp гарнизон со стенами
                   do
                   {
                      int iRandomPointIndex = Randomizer.rnd.Next(arrAdjPoints.Count);
                      mpRandomOpenPoint = (MapPoint)arrAdjPoints[iRandomPointIndex];
                      mpSecondOpenPoint = mpRandomOpenPoint;

                      flHowManyDir = 0; iDir = -1; fl1 = false;
                      mpobGarrison.BasePoint = mpRandomOpenPoint;
                      if (CheckDistance(mpobGarrison, iFirstZoneIndex))
                      {
                          int iWidth = Math.Max(iarrThicknessList[iFirstZoneIndex - 1], iarrThicknessList[iSecondZoneIndex - 1]);
                       //dwp! вычисляем предпочтительное направление для пробивания,
                       //dwp! ищем направление ровно на толщину зоны, v 1.5.6 
                          for (int i = (iWidth + 1); mpRandomOpenPoint.x + i <= (int)eMapSize - 1 &&
                                       mpRandomOpenPoint.y + i <= (int)eMapSize - 1 &&
                                       mpRandomOpenPoint.x - i > 0 &&
                                       mpRandomOpenPoint.y - i > 0 &&
                                       i <= (iWidth + 1); i += iWidth)
                       {
                           if ((eBlockStatus)iarrMap[(int)MapLayer.Blocks, mpRandomOpenPoint.x + i, mpRandomOpenPoint.y - i] != eBlockStatus.Block)
                           {
                               if ((int)iarrMap[(int)MapLayer.Zones, mpRandomOpenPoint.x + i, mpRandomOpenPoint.y - i] == iSecondZoneIndex)
                               {
                                   flHowManyDir++;
                                   iDir = 1;
                                   fl1 = true;
                               }
                           }
                           if ((eBlockStatus)iarrMap[(int)MapLayer.Blocks, mpRandomOpenPoint.x + i, mpRandomOpenPoint.y + i] != eBlockStatus.Block)
                           {
                               if ((int)iarrMap[(int)MapLayer.Zones, mpRandomOpenPoint.x + i, mpRandomOpenPoint.y + i] == iSecondZoneIndex)
                               {
                                   flHowManyDir++;
                                   iDir = flHowManyDir == 2 ? 2 : 3;
                               }
                           }
                           if ((eBlockStatus)iarrMap[(int)MapLayer.Blocks, mpRandomOpenPoint.x - i, mpRandomOpenPoint.y + i] != eBlockStatus.Block)
                           {
                               if ((int)iarrMap[(int)MapLayer.Zones, mpRandomOpenPoint.x - i, mpRandomOpenPoint.y + i] == iSecondZoneIndex)
                               {
                                   flHowManyDir++;
                                   iDir = flHowManyDir == 3 ? 3 : (flHowManyDir == 2 ? 4 : 5);
                               }
                           }
                           if ((eBlockStatus)iarrMap[(int)MapLayer.Blocks, mpRandomOpenPoint.x - i, mpRandomOpenPoint.y - i] != eBlockStatus.Block)
                           {
                               if ((int)iarrMap[(int)MapLayer.Zones, mpRandomOpenPoint.x - i, mpRandomOpenPoint.y - i] == iSecondZoneIndex)
                               {
                                   flHowManyDir++;
                                   iDir = flHowManyDir == 3 ? (fl1 ? (iDir == 5? 7 : 1) : 5) : (flHowManyDir == 2 ? (fl1 ? 0 : 6) : 7);
                               }
                           }
                           if (flHowManyDir > 0) break;
                       }
                       if (flHowManyDir == 1 || flHowManyDir == 2) /* dwp. три направления ненадежное пробивание - потому что есть противоположные v 1.7.1 */
                        {
                           mpSecondOpenPoint = mpRandomOpenPoint + mpRoundTripAllDirections[iDir] ;
                           flReady = OpenAroundToClear(mpRandomOpenPoint, mpSecondOpenPoint, iDir);
                       }
                       else arrAdjPoints.Remove(arrAdjPoints[iRandomPointIndex]);
                      }
                      else arrAdjPoints.Remove(arrAdjPoints[iRandomPointIndex]);

                       //choose another point
                   } while (!flReady && arrAdjPoints.Count > 0);
                   if (flReady)
                   {
                       int AntiDir = iDir < 4 ? iDir + 4 : iDir - 4;
                       /* чуть выдвигаю вторую охрану в том же направлении */
                       mpSecondOpenPoint = (mpRandomOpenPoint + mpRoundTripAllDirections[iDir]) + mpRoundTripAllDirections[iDir];
                       //add the access point for opening points list
                       AccessPoints[iFirstZoneIndex - 1, (int)ePointLayer.OpenningPoints].Add(mpRandomOpenPoint + mpRoundTripAllDirections[AntiDir]);
                       AccessPoints[iSecondZoneIndex - 1, (int)ePointLayer.OpenningPoints].Add(mpSecondOpenPoint);

                       PlaceGarrisons(mpRandomOpenPoint, mpSecondOpenPoint, iDir, iConnectionIndex);

                       #if (DEBUG)
                       dumpBlockMaskToFileErgo();
                       #endif

                    }
               }
           }
           if(!flReady)// pick a random spot and place portals in both zones
           {
             this.PlaceTwoWayPortalAndGuards(iFirstZoneIndex, iSecondZoneIndex, iConnectionIndex);
           }
        }

        //go around selected point and open a way ( by removing blocks )
        private bool OpenAroundToClear(MapPoint mpRandomOpenPointZone1, MapPoint mpRandomOpenPointZone2, int iDir)
        {
            int AntiDir = iDir < 4? iDir + 4: iDir - 4;
            //first check distance from edges is any on the point is too close to the edge then return false
            if (mpRandomOpenPointZone1.x >= (int)eMapSize - 2 ||
                mpRandomOpenPointZone1.y >= (int)eMapSize - 2 ||
                mpRandomOpenPointZone2.x >= (int)eMapSize - 2 ||
                mpRandomOpenPointZone2.y >= (int)eMapSize - 2 ||
                mpRandomOpenPointZone1.x <= 2 ||
                mpRandomOpenPointZone1.y <= 2 ||
                mpRandomOpenPointZone2.x <= 2 ||
                mpRandomOpenPointZone2.y <= 2)
            {
                return false;
            }

            //now go in the direction for first zone until the preserved is reached - marking preserved all the way in 2 way size
            MarkPreservedInDirection(mpRandomOpenPointZone2.x, mpRandomOpenPointZone2.y, iDir);

#if (DEBUG)
            dumpBlockMaskToFileErgo();
#endif

            //now do same for 2nd zone and direction 
            MarkPreservedInDirection(mpRandomOpenPointZone1.x, mpRandomOpenPointZone1.y, AntiDir);
#if (DEBUG)
            dumpBlockMaskToFileErgo();
#endif
            
            //opening is completed.
            return true;

        }

        private void MarkPreservedInDirection(int iX, int iY, int iDirection)
        {
            //stop condition
            if (!IsInsideMapLimit(iX, iY))
            {
               #if (DEBUG)
                dumpBlockMaskToFileErgo();
               #endif
               throw new Exception("Can't connect zones. Try again or check your template.");
            }

            if ((eBlockStatus)iarrMap[(int)MapLayer.Blocks, iX, iY] != eBlockStatus.Preserved)
            {
                iarrMap[(int)MapLayer.Blocks, iX, iY] = eBlockStatus.Preserved;
                if (iDirection < 7)
                    {
                        if (IsInsideMapLimit(iX + mpRoundTripAllDirections[iDirection + 1].x, iY + mpRoundTripAllDirections[iDirection + 1].y))
                        {
                            iarrMap[(int)MapLayer.Blocks, iX + mpRoundTripAllDirections[iDirection + 1].x, iY + mpRoundTripAllDirections[iDirection + 1].y] = eBlockStatus.Preserved;
                        }
                    }
                    else
                    {
                        if (IsInsideMapLimit(iX + mpRoundTripAllDirections[0].x, iY + mpRoundTripAllDirections[0].y))
                        {
                            iarrMap[(int)MapLayer.Blocks, iX + mpRoundTripAllDirections[0].x, iY + mpRoundTripAllDirections[0].y] = eBlockStatus.Preserved;
                        }
                    }
                if (iDirection > 0)
                    {
                        if (IsInsideMapLimit(iX + mpRoundTripAllDirections[iDirection - 1].x, iY + mpRoundTripAllDirections[iDirection - 1].y))
                        {
                            iarrMap[(int)MapLayer.Blocks, iX + mpRoundTripAllDirections[iDirection - 1].x, iY + mpRoundTripAllDirections[iDirection - 1].y] = eBlockStatus.Preserved;
                        }
                    }
                    else
                    {
                        if (IsInsideMapLimit(iX + mpRoundTripAllDirections[7].x, iY + mpRoundTripAllDirections[7].y))
                        {
                            iarrMap[(int)MapLayer.Blocks, iX + mpRoundTripAllDirections[7].x, iY + mpRoundTripAllDirections[7].y] = eBlockStatus.Preserved;
                        }
                    }
                MarkPreservedInDirection(iX + mpRoundTripAllDirections[iDirection].x, iY + mpRoundTripAllDirections[iDirection].y, iDirection);
            }
        }




        /// <summary>
        /// recursivly count in i direction for preserved - if reach another zone  return infinite distance
        /// </summary>
        /// <param name="mpRandomOpenPointZone2"></param>
        /// <param name="i"></param>
        /// <returns></returns>
        private int CountPreservedDistance(int iX ,int iY, int i)
        {
            //stop condition
            if ((eBlockStatus)iarrMap[(int)MapLayer.Blocks, iX, iY] == eBlockStatus.Preserved)
               return 0;

            //check next direction point in map boundaries
            int iNewX = iX + mpRoundTripAllDirections[i].x;
            int iNewY = iY + mpRoundTripAllDirections[i].y;
            if (!IsInsideMapLimit(iNewX, iNewY))
                return int.MaxValue;

            //if next direction is different zone then also stop and return infinite size (very big letz say 3000000 )
            if ((int)iarrMap[(int)MapLayer.Zones, iX, iY] != (int)iarrMap[(int)MapLayer.Zones, iNewX, iNewY])
                return int.MaxValue;


            int iPreservedLength = CountPreservedDistance(iNewX, iNewY, i);
            //if its an even direction then each length is 2 , if its an odd direction then(diagnolly) its 3 length
            if (i % 2 == 0)
            {
                
                if (iPreservedLength != int.MaxValue)
                    return (2 + iPreservedLength);
                else
                    return int.MaxValue;
            }
            else
            {
                if (iPreservedLength != int.MaxValue)
                    return (3 + iPreservedLength);
                else
                    return int.MaxValue;
            }

            
        }


        /// <summary>
        /// returns a list of objects seperating two zones
        /// </summary>
        /// <param name="iFirstZoneIndex"></param>
        /// <param name="iSecondZoneIndex"></param>
        /// <returns></returns>
        private ArrayList GetZonesSeperationObjectsList(int iFirstZoneIndex, int iSecondZoneIndex)
        {

            ArrayList mpobjlstSeperationObjects = new ArrayList();

            for (int i = 0; i < (int)eMapSize; i++)
            {
                for (int j = 0; j < (int)eMapSize; j++)
                {
                    if (iarrMap[(int)MapLayer.Objects, i, j] is MapObject)
                    {
                        //if (((MapObject)iarrMap[(int)MapLayer.Objects, i, j]).Type == eObjectType.Barrier)
                        //{
                        //}
                        if ((int)iarrMap[(int)MapLayer.Zones, i, j] == iFirstZoneIndex || (int)iarrMap[(int)MapLayer.Zones, i, j] == iSecondZoneIndex)
                        {
                            //if (IsObjectAZonesSeperation(iFirstZoneIndex, iSecondZoneIndex, (MapObject)iarrMap[(int)MapLayer.Objects, i, j]))
                            //{
                                mpobjlstSeperationObjects.Add( iarrMap[(int)MapLayer.Objects, i, j] );
                            //}
                        }

                    }
                }
            }

            return mpobjlstSeperationObjects;
        }




        /// <summary>
        /// checks if object is seperating between zones 
        /// </summary>
        /// <param name="iFirstZoneIndex">first zone to check</param>
        /// <param name="iSecondZoneIndex">second zone to check</param>
        /// <param name="mapObject">Object to check</param>
        /// <returns>if its seperation or not</returns>
        private bool IsObjectAZonesSeperation(int iFirstZoneIndex, int iSecondZoneIndex, MapObject mpobjSourceObject)
        {

            int iFirstZoneCounter = 0, iSecondZoneCounter = 0;
            MapPoint mpOriginPoint = mpobjSourceObject.BasePoint;

            if (mpobjSourceObject.Area == null)
                return false;

            //go over its area and check if its between the two zones 
            foreach (MapPoint mpDirection in mpobjSourceObject.Area)
            {

                    if ((int)iarrMap[(int)MapLayer.Zones, mpOriginPoint.x + mpDirection.x, mpOriginPoint.y + mpDirection.y] == iFirstZoneIndex)
                    {
                        iFirstZoneCounter++;
                    }
                    else if ((int)iarrMap[(int)MapLayer.Zones, mpOriginPoint.x + mpDirection.x, mpOriginPoint.y + mpDirection.y] == iSecondZoneIndex)
                    {
                        iSecondZoneCounter++;
                    }
                    else
                    {
                        return false;
                    }
                
            }


            if (iFirstZoneCounter >= 1 && iSecondZoneCounter >= 1)
                return true;

            return false;
        }


        /// <summary>
        /// removes an object from object map layer 
        /// </summary>
        /// <param name="moObjectToRemove"></param>
        private void RemoveObject(MapObject moObjectToRemove)
        {
            iarrMap[(int)MapLayer.Objects, moObjectToRemove.BasePoint.x, moObjectToRemove.BasePoint.y] = null;

            if (moObjectToRemove.Area != null)
            {
                foreach (MapPoint mpDirection in moObjectToRemove.Area)
                {
                    iarrMap[(int)MapLayer.Objects, moObjectToRemove.BasePoint.x + mpDirection.x, moObjectToRemove.BasePoint.y + mpDirection.y] = null;
                }
            }
        }


        /// <summary>
        /// removes an object from object map layer 
        /// </summary>
        /// <param name="moObjectToRemove"></param>
        private void RemoveObjectInLayer2(MapObject moObjectToRemove)
        {
            iarrMap[(int)MapLayer.Objects2, moObjectToRemove.BasePoint.x, moObjectToRemove.BasePoint.y] = null;

            if (moObjectToRemove.Area != null)
            {
                foreach (MapPoint mpDirection in moObjectToRemove.Area)
                {
                    iarrMap[(int)MapLayer.Objects2, moObjectToRemove.BasePoint.x + mpDirection.x, moObjectToRemove.BasePoint.y + mpDirection.y] = null;
                }
            }
        }


        /// <summary>
        /// checks if two zones are phisically adjacent 
        /// by numbering their adjacent zone squares
        /// also add any adjacent point to an array list and returns it
        /// </summary>
        /// <param name="iFirstZoneIndex"></param>
        /// <param name="iSecondZoneIndex"></param>
        /// <returns>if the list is size 0 then no adjacencies</returns>
        private ArrayList GetPhisicallyAdjacentPoints(int iFirstZoneIndex, int iSecondZoneIndex)
        {
            ArrayList arrmpAdjPointList = new ArrayList();
            int iAdjacenciesCount ;

            //count all straight adjacencies
            //no check for straigth because voronoi algorithem does not allow adjacent zones to be connected not in a straight way
            for (int i = 1; i < (int)eMapSize; i++)
            {
                for (int j = 1; j < (int)eMapSize ; j++)
                {
                    //if you get a cell that withing source zone count all this sqaure connection points to checked zone
                    if ((int)this.iarrMap[(int)MapLayer.Zones, i, j] == iFirstZoneIndex)
                    {
                        //array of points which define a round trip around a sqaure to check all its adjacencies 
                        //MapPoint[] arrmp =  { 
                        //    new MapPoint(0,1) , new MapPoint (1,1) , new MapPoint (1,0),
                        //    new MapPoint(1,-1) , new MapPoint (0,-1) , new MapPoint(-1,-1),
                        //    new MapPoint( -1, 0) , new MapPoint(-1,1) 
                        //};
                        iAdjacenciesCount = 0;
                        foreach (MapPoint mpItem in mpRoundTripAllDirections)
                        {

                            //check outing boundaries

                            if (IsInsideMapLimit(i + mpItem.x, j + mpItem.y))
                            {
                                //if one of the surrounding sqaures belongs to questioned zone then raise counter
                                if ((int)this.iarrMap[(int)MapLayer.Zones, i + mpItem.x, j + mpItem.y] == iSecondZoneIndex)
                                {

                                    int iWidth = Math.Max(iarrThicknessList[(int)iarrMap[(int)MapLayer.Zones, i, j] - 1],
                                                          iarrThicknessList[(int)iarrMap[(int)MapLayer.Zones, i + mpItem.x, j + mpItem.y] - 1]);

                                    //dwp на двойную толщину границ вокруг не должно быть "третьей" зоны
                                    if (!((i + iWidth * 2 > (int)eMapSize - 1) || (j + iWidth * 2 > (int)eMapSize - 1) || (i - iWidth * 2 < 0) || (j - iWidth * 2 < 0)))
                                        if (!(((int)this.iarrMap[(int)MapLayer.Zones, i + iWidth * 2, j + iWidth * 2] != iFirstZoneIndex &&
                                               (int)this.iarrMap[(int)MapLayer.Zones, i + iWidth * 2, j + iWidth * 2] != iSecondZoneIndex) ||
                                              ((int)this.iarrMap[(int)MapLayer.Zones, i - iWidth * 2, j - iWidth * 2] != iFirstZoneIndex &&
                                               (int)this.iarrMap[(int)MapLayer.Zones, i - iWidth * 2, j - iWidth * 2] != iSecondZoneIndex) ||
                                              ((int)this.iarrMap[(int)MapLayer.Zones, i + iWidth * 2, j - iWidth * 2] != iFirstZoneIndex &&
                                               (int)this.iarrMap[(int)MapLayer.Zones, i + iWidth * 2, j - iWidth * 2] != iSecondZoneIndex) ||
                                              ((int)this.iarrMap[(int)MapLayer.Zones, i - iWidth * 2, j + iWidth * 2] != iFirstZoneIndex &&
                                               (int)this.iarrMap[(int)MapLayer.Zones, i - iWidth * 2, j + iWidth * 2] != iSecondZoneIndex) ||
                                              ((int)this.iarrMap[(int)MapLayer.Zones, i, j - iWidth * 2] != iFirstZoneIndex &&
                                               (int)this.iarrMap[(int)MapLayer.Zones, i, j - iWidth * 2] != iSecondZoneIndex) ||
                                              ((int)this.iarrMap[(int)MapLayer.Zones, i - iWidth * 2, j] != iFirstZoneIndex &&
                                               (int)this.iarrMap[(int)MapLayer.Zones, i - iWidth * 2, j] != iSecondZoneIndex) ||
                                              ((int)this.iarrMap[(int)MapLayer.Zones, i, j + iWidth * 2] != iFirstZoneIndex &&
                                               (int)this.iarrMap[(int)MapLayer.Zones, i, j + iWidth * 2] != iSecondZoneIndex) ||
                                              ((int)this.iarrMap[(int)MapLayer.Zones, i + iWidth * 2, j] != iFirstZoneIndex &&
                                               (int)this.iarrMap[(int)MapLayer.Zones, i + iWidth * 2, j] != iSecondZoneIndex)) 
                                      )

                                    iAdjacenciesCount++;
                                    //if (iAdjacenciesCount >= I_ADJACENCIES_FACTOR)
                                    //    return true;
                                }
                            }
                        }
                        if (iAdjacenciesCount >= 3)
                        {
                            MapPoint mpNewAdjPoint = new MapPoint(i, j);
                            arrmpAdjPointList.Add(mpNewAdjPoint);
                        }
                    }
                }
            }
            return arrmpAdjPointList;
        }

        /// <summary>
        /// place two garrisons with guards
        /// </summary>
        /// <param name="iFirstZoneIndex"></param>
        /// <param name="iSecondZoneIndex"></param>
        /// <param name="iConnectionIndex"></param>
        private void PlaceGarrisons(MapPoint iFirst, MapPoint iSecond, int iDir, int iConnection)
        {
            ObjectsReader obrdGetPortal = new ObjectsReader("OtherObjects.xml");
            MapObject mpobGarrison = obrdGetPortal.GetRandomObjectByType(eObjectType.Forpost); //dwp гарнизон со стенами
            MapObject mpobjGuard2 = new MapObject(iSecond, null, "Random", eObjectType.Guard, null,
                                        /* dwp. отрицательная сила указывает, что охрана не будет расти на карте */
                                        -int.Parse(thSelectedTemplate.GetConnectionGuardValue(iConnection))); 
            mpobGarrison.iValue  = int.Parse(thSelectedTemplate.GetConnectionGuardValue(iConnection));
            int AntiDir = iDir < 4 ? iDir + 4 : iDir - 4;
            switch (iDir)
            {   /* внешний гарнизон смотрит "от ворот" */
                case 0: mpobjGuard2.Rotation  = "6.28319"; /* вниз */
                        mpobGarrison.Rotation = "6.28319"; 
                        break;
                case 4: mpobjGuard2.Rotation  = "3.14159"; /* вверх */
                        mpobGarrison.Rotation = "6.28319"; 
                        break;
                case 2: mpobjGuard2.Rotation  = "1.5708";  /* вправо */
                        mpobGarrison.Rotation = "4.71239"; 
                        break;
                case 6: mpobjGuard2.Rotation  = "4.71239"; /* влево */
                        mpobGarrison.Rotation = "4.71239"; 
                        break;
                case 1: mpobjGuard2.Rotation = "0.7854";  /* вправо-вниз */
                        mpobGarrison.Rotation = "3.92699";
                        break;
                case 5: mpobjGuard2.Rotation = "3.92699"; /* влево-вверх */
                        mpobGarrison.Rotation = "3.92699";
                        break;
                case 3: mpobjGuard2.Rotation   = "2.3562"; /* вправо-вверх */
                        mpobGarrison.Rotation  = "2.3562";
                        break;
                case 7: mpobjGuard2.Rotation   = "5.49779";  /* влево-вниз */
                        mpobGarrison.Rotation  = "2.3562";
                        break;
                default: break;
            }
        
            //dwp. ручная установка гарнизона
            iarrMap[(int)MapLayer.Objects, iFirst.x, iFirst.y] = mpobGarrison;
            iarrMap[(int)MapLayer.Objects, iSecond.x, iSecond.y] = mpobjGuard2;

            for (int i = 0; i < mpRoundTripAllDirections.Length; i++)
              {
                if(iFirst.x + mpRoundTripAllDirections[i].x != iSecond.x ||
                   iFirst.y + mpRoundTripAllDirections[i].y != iSecond.y)
                   iarrMap[(int)MapLayer.Blocks, iFirst.x + mpRoundTripAllDirections[i].x, iFirst.y + mpRoundTripAllDirections[i].y] = eBlockStatus.Done;
              }
        }

        /// <summary>
        /// place a pathway of two way portal between 2 unadjacent zones
        /// </summary>
        /// <param name="iFirstZoneIndex"></param>
        /// <param name="iSecondZoneIndex"></param>
        private void PlaceTwoWayPortalAndGuards(int iFirstZoneIndex, int iSecondZoneIndex ,int iConnectionIndex)
        {
            ObjectsReader obrdGetPortal = new ObjectsReader("OtherObjects.xml");
            MapObject mpobTwoWayPortal = obrdGetPortal.GetObjectByName(eObjectType.Monolith_Two_Way.ToString());
            MapPoint mpFirstPortalPoint, mpSecondPortalPoint;

            if (thSelectedTemplate.iarrLinkToStartingZone[iConnectionIndex] &&
                thSelectedTemplate.iarrCountLinkedStartingZone[iFirstZoneIndex - 1] > 1) 
            { // dwp этот портал требует искусственного удаления от других подобных порталов в этой зоне
                mpFirstPortalPoint = new MapPoint(-1,-1);
                for (int i = 0; i < 4; i++) 
                {
                    if (mparrMaxDistancePointsInZones[i, iFirstZoneIndex - 1] != null)
                    {
                       mpFirstPortalPoint = GetFreePointInZoneAroundPoint(iFirstZoneIndex, mpobTwoWayPortal, mparrMaxDistancePointsInZones[i, iFirstZoneIndex - 1]);
                       mparrMaxDistancePointsInZones[i, iFirstZoneIndex - 1] = null;
                       break;
                    }
                }
                if (mpFirstPortalPoint.x == -1 || mpFirstPortalPoint.y == -1) 
                   mpFirstPortalPoint = GetFreeFittingPointInZone(iFirstZoneIndex, mpobTwoWayPortal);
            }
            else mpFirstPortalPoint = GetFreeFittingPointInZone(iFirstZoneIndex, mpobTwoWayPortal);

            if (thSelectedTemplate.iarrLinkToStartingZone[iConnectionIndex] &&
                thSelectedTemplate.iarrCountLinkedStartingZone[iSecondZoneIndex - 1] > 1) 
            { // dwp этот портал требует искусственного удаления от других подобных порталов в этой зоне
                mpSecondPortalPoint = new MapPoint(-1, -1);
                for (int i = 0; i < 4; i++)
                {
                    if (mparrMaxDistancePointsInZones[i, iSecondZoneIndex - 1] != null)
                    {
                        mpSecondPortalPoint = GetFreePointInZoneAroundPoint(iSecondZoneIndex, mpobTwoWayPortal, mparrMaxDistancePointsInZones[i, iSecondZoneIndex - 1]);
                        mparrMaxDistancePointsInZones[i, iSecondZoneIndex - 1] = null;
                        break;
                    }
                }
                if (mpSecondPortalPoint.x == -1 || mpSecondPortalPoint.y == -1)
                    mpSecondPortalPoint = GetFreeFittingPointInZone(iSecondZoneIndex, mpobTwoWayPortal);
            }
            else mpSecondPortalPoint = GetFreeFittingPointInZone(iSecondZoneIndex, mpobTwoWayPortal);

            //add both points to opening points
            AccessPoints[iFirstZoneIndex-1, (int)ePointLayer.OpenningPoints].Add(mpFirstPortalPoint);
            AccessPoints[iSecondZoneIndex-1, (int)ePointLayer.OpenningPoints].Add(mpSecondPortalPoint);

            string strPortalsGroupId = ObjectSpacificPropertiesHelper.GetNextTwoWayPortalGroupId().ToString();
            mpobTwoWayPortal.BasePoint = mpFirstPortalPoint;
            mpobTwoWayPortal.strName = "portal_" + strPortalsGroupId + "_1"; // for Dyrman
            
            mpobTwoWayPortal.ObjectSpacificProperties.Add(eObjectsSpacificProperties.TwoWayPortalGroupID.ToString(), strPortalsGroupId);
            mpobTwoWayPortal.Type = eObjectType.Monolith_Two_Way;
            if (!PlaceObject(mpobTwoWayPortal, iFirstZoneIndex))
            {
#if (DEBUG) 
                dumpBlockMaskToFileErgo();
#endif 
                throw new Exception("Bad zone. No room for placing TwoWays Portals at Zone #" + iFirstZoneIndex.ToString() + Environment.NewLine + 
                                    "Try again or check your template.");
            }
            //set guard value
            mpobTwoWayPortal.iValue = int.Parse(thSelectedTemplate.GetConnectionGuardValue(iConnectionIndex));
            PlaceObjectGuard(mpobTwoWayPortal, iFirstZoneIndex);

            mpobTwoWayPortal = obrdGetPortal.GetObjectByName(eObjectType.Monolith_Two_Way.ToString()); //get new instance
            mpobTwoWayPortal.BasePoint = mpSecondPortalPoint;
            mpobTwoWayPortal.strName = "portal_" + strPortalsGroupId + "_2"; // for Dyrman

            mpobTwoWayPortal.ObjectSpacificProperties.Add(eObjectsSpacificProperties.TwoWayPortalGroupID.ToString(), strPortalsGroupId);
            mpobTwoWayPortal.Type = eObjectType.Monolith_Two_Way;

            if (!PlaceObject(mpobTwoWayPortal, iSecondZoneIndex))
            {
#if (DEBUG) 
             dumpBlockMaskToFileErgo();
#endif
                throw new Exception("Bad zone. No room for placing TwoWays Portals at Zone #" + iSecondZoneIndex.ToString() + Environment.NewLine +
                                    "Try again or check your template.");
            }
            //set guard value
            mpobTwoWayPortal.iValue = int.Parse(thSelectedTemplate.GetConnectionGuardValue(iConnectionIndex));
            PlaceObjectGuard(mpobTwoWayPortal, iSecondZoneIndex);
        }


        private bool PlaceObjectGuard(MapObject mpobGuardedObject, int iZoneIndex)
        {

            if (mpobGuardedObject.iValue != 0)
            {
                //todo:add access points to all objects and remove constant 0,-1 point
                MapPoint mpAccessPoint;
                if (mpobGuardedObject.AccessPoint != null)
                    mpAccessPoint = mpobGuardedObject.Type == eObjectType.Monolith_Two_Way ? new MapPoint(0, -1) : mpobGuardedObject.AccessPoint;
                else
                    mpAccessPoint = new MapPoint(0, -1);
                //tranform point by object direction
                mpAccessPoint = TransformPointDirection(mpAccessPoint, mpobGuardedObject.Direction);

                //MapPoint mpGuardPoint = new MapPoint(mpobGuardedObject.BasePoint.x, mpobGuardedObject.BasePoint.y );
                MapObject mpobjGuard = new MapObject(mpobGuardedObject.BasePoint + mpAccessPoint, null, "Random", eObjectType.Guard, null, mpobGuardedObject.iValue);
                if (!PlaceObject(mpobjGuard, iZoneIndex))
                    throw new Exception("Placing object's guard out of Map!");
                return true;
            }
            else
                return false;
        }

        private MapPoint TransformPointDirection(MapPoint mpAccessPoint, ObjectDirection objectDirection)
        {
            switch (objectDirection)
            {
                case ObjectDirection.Down:
                    return mpAccessPoint;
                    //break;
                case ObjectDirection.Right:
                    return new MapPoint(mpAccessPoint.y * -1, mpAccessPoint.x);
                    //break;
                case ObjectDirection.Up:
                    return new MapPoint(mpAccessPoint.x * -1, mpAccessPoint.y * - 1);
                    //break;
                case ObjectDirection.Left:
                    return new MapPoint(mpAccessPoint.y ,  mpAccessPoint.x * -1 );
                    //break;
                default:
                    return mpAccessPoint;
                    //break;
            }
            //return mpAccessPoint;
        }

        private System.Collections.ArrayList GetALLFreeFittingPointsInZone(int iZoneIndex, MapObject moObjectToBePlaced)
        {
            System.Collections.ArrayList mparrFreeZonePoints = GetFreeZonePointList(iZoneIndex);

            for (int i = 0; i < mparrFreeZonePoints.Count; i++)
            {
                moObjectToBePlaced.BasePoint = (MapPoint)mparrFreeZonePoints[i];
                //remove all points that can't be placed in
                if (!CanPlaceObjectWithDistanceCheck(moObjectToBePlaced, iZoneIndex))
                {
                    mparrFreeZonePoints.Remove(mparrFreeZonePoints[i]);
                    i--; //redo current index because after remove it just turned into next object in list
                }
            }
            return mparrFreeZonePoints;
        }

        /// <summary>
        /// поиск возможной точки ближайшей к заданной
        /// </summary>
        /// <param name="iZoneIndex"></param>
        /// <param name="moObjectToBePlaced"></param>
        /// <param name="nearPoint"></param>
        /// <returns></returns>
        private MapPoint GetFreePointInZoneAroundPoint(int iZoneIndex, MapObject moObjectToBePlaced, MapPoint nearPoint)
        {
            System.Collections.ArrayList mparrFreeZonePoints = GetFreeZonePointList(iZoneIndex);

            for (int i = 0; i < mparrFreeZonePoints.Count; i++)
            {
                moObjectToBePlaced.BasePoint = (MapPoint)mparrFreeZonePoints[i];
                //remove all points that can't be placed in
                if (!CanPlaceObject(moObjectToBePlaced))
                {
                    mparrFreeZonePoints.Remove(mparrFreeZonePoints[i]);
                    i--; //redo current index because after remove it just turned into next object in list
                }
            }

            if (mparrFreeZonePoints.Count == 0)
            { //if no free points return a point signaling no more points available
#if (DEBUG) //{
                LostObjectsCounter.DumpObject(moObjectToBePlaced, iZoneIndex);
#endif //DEBUG }
                return new MapPoint(-1, -1);
            }
            //dwp выбираем ближайщую к заданной из всех возможных точек 
            double mindistance = float.MaxValue, distance;
            int nearest = -1;

            for (int i = 0; i < mparrFreeZonePoints.Count; i++)
            {
                distance = (MapPoint)mparrFreeZonePoints[i] - nearPoint;
                if (distance < mindistance)
                {
                    mindistance = distance;
                    nearest = i;
                }
            }
            if (nearest != -1) return (MapPoint)mparrFreeZonePoints[nearest];
            else return new MapPoint(-1, -1);

        }

        /// <summary>
        /// Gets a free point inside a zone based on object size
        /// </summary>
        /// <param name="iZoneIndex"></param>
        /// <param name="moObjectToBePlaced"></param>
        /// <returns></returns>
        private MapPoint GetFreeFittingPointInZone(int iZoneIndex, MapObject moObjectToBePlaced)
        {
            System.Collections.ArrayList mparrFreeZonePoints = GetFreeZonePointList(iZoneIndex);

            for (int i = 0; i < mparrFreeZonePoints.Count; i++)
            {
                moObjectToBePlaced.BasePoint = (MapPoint) mparrFreeZonePoints[i];
                //remove all points that can't be placed in
                if (!CanPlaceObjectWithDistanceCheck(moObjectToBePlaced, iZoneIndex))
                {
                    mparrFreeZonePoints.Remove(mparrFreeZonePoints[i]);
                    i--; //redo current index because after remove it just turned into next object in list
                }                
            }

            if (mparrFreeZonePoints.Count == 0) { //if no free points return a point signaling no more points available
#if (DEBUG) //{
                LostObjectsCounter.DumpObject(moObjectToBePlaced, iZoneIndex);
#endif //DEBUG }
                return new MapPoint(-1, -1);
            }

            //pick a random point from list and return it
            //#if ( DEBUG )
            //    Random Randomizer.rnd = new Random(1);
            //#else
            //    Random Randomizer.rnd = new Random();
            //#endif

            int iRandomIndex = Randomizer.rnd.Next(mparrFreeZonePoints.Count);            
            //dwp!!! cтрашный баг отловил. проверка шла поодному направлению а ставился обьект в другом направлении
            return (MapPoint)mparrFreeZonePoints[iRandomIndex];
        }


        /// <summary>
        /// Gets a free point inside a zone based on object size
        /// </summary>
        /// <param name="iZoneIndex"></param>
        /// <param name="moObjectToBePlaced"></param>
        /// <returns></returns>
        private MapPoint GetFittingPointInZoneAdjacentToPointInDirection(int iZoneIndex,MapObject moBaseObject , MapObject moObjectToBePlaced)
        {
            System.Collections.ArrayList mparrFreeZonePoints = GetFreeZonePointList(iZoneIndex);



            for (int i = 0; i < mparrFreeZonePoints.Count; i++)
            {
                moObjectToBePlaced.BasePoint = (MapPoint)mparrFreeZonePoints[i];
                //remove all points that can't be placed in
                if (!CanPlaceTightObject(moObjectToBePlaced))
                {
                    mparrFreeZonePoints.Remove(mparrFreeZonePoints[i]);
                    i--; //redo current index because after remove it just turned into next object in list
                }
            }


            //among points left remove all points that are not in requested direction
            for (int i = 0; i < mparrFreeZonePoints.Count; i++)
            {
                MapPoint mpPotentialPoint = (MapPoint)mparrFreeZonePoints[i];
                //remove all points that can't be placed in
                if (mpPotentialPoint.IsPointInDirectionCompareToPoint(moBaseObject.BasePoint, moBaseObject.Direction))
                {
                    mparrFreeZonePoints.Remove(mparrFreeZonePoints[i]);
                    i--; //redo current index because after remove it just turned into next object in list
                }
            }


            //among points left remove all points that are objects are not adjacent to each other
            for (int i = 0; i < mparrFreeZonePoints.Count; i++)
            {
                MapPoint mpPotentialPoint = (MapPoint)mparrFreeZonePoints[i];
                moObjectToBePlaced.BasePoint = mpPotentialPoint;
                //remove all points that can't be placed in
                if ( ! DoObjectsOverLap ( moBaseObject , moObjectToBePlaced ) )
                {
                    mparrFreeZonePoints.Remove(mparrFreeZonePoints[i]);
                    i--; //redo current index because after remove it just turned into next object in list
                }
            }


            if (mparrFreeZonePoints.Count == 0) //if no free points return a point signaling no more points available
                return new MapPoint(-1, -1);

            //pick a random point from list and return it
            //#if ( DEBUG )
            //                Random Randomizer.rnd = new Random(1);
            //#else
            //            Random Randomizer.rnd = new Random();
            //#endif

            int iRandomIndex = Randomizer.rnd.Next(mparrFreeZonePoints.Count);       

            return (MapPoint)mparrFreeZonePoints[iRandomIndex];
        }

        private bool DoObjectsOverLap(MapObject moBaseObject, MapObject moObjectToBePlaced)
        {
            //get list of pathway points for base object
            ArrayList arrBasePathWaysPoints = new ArrayList();

            foreach ( MapPoint mpPathWay in moBaseObject.PathWays )
            {
                //get point via direction
                MapPoint newPathWay = TransformPointDirection(mpPathWay, moBaseObject.Direction);
                arrBasePathWaysPoints.Add(moBaseObject.BasePoint + newPathWay);
            }

            //get list of pathway points for target object
            ArrayList arrTargetAreaPoints = new ArrayList();

            if (moObjectToBePlaced.Area != null)
            {
                foreach (MapPoint mpArea in moObjectToBePlaced.Area)
                {
                    //get point via direction
                    MapPoint mpnewArea = TransformPointDirection(mpArea, (ObjectDirection)iarrMap[(int)MapLayer.Directions, moObjectToBePlaced.BasePoint.x, moObjectToBePlaced.BasePoint.y]);
                    arrTargetAreaPoints.Add(moObjectToBePlaced.BasePoint + mpnewArea);
                }
            }
            arrTargetAreaPoints.Add(moObjectToBePlaced.BasePoint);

            //now compare two lists if any 1 point is in common between them

            foreach (MapPoint mpBasePoint in arrBasePathWaysPoints)
            {
                if (arrTargetAreaPoints.Contains(mpBasePoint))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// gets a list of free points for a spacific zone
        /// </summary>
        /// <param name="iZoneIndex"></param>
        /// <returns></returns>
        private System.Collections.ArrayList GetFreeZonePointList(int iZoneIndex)
        {
            System.Collections.ArrayList mparrFreeZonePointList = new System.Collections.ArrayList();
            for ( int i = 1 ; i < (int)eMapSize  ; i++)
                for (int j = 1; j < (int)eMapSize ; j++)
                {
                    if ((int) iarrMap[(int)MapLayer.Zones, i, j] == iZoneIndex)//if point is in spacified zone
                    {
                        if (IsObjectFreePoint(i, j))
                        {
                            mparrFreeZonePointList.Add(new MapPoint(i, j));
                        }
                    }
                }
            return (mparrFreeZonePointList);

        }


        ///// <summary>
        ///// gets a list of free points for a spacific zone
        ///// </summary>
        ///// <param name="iZoneIndex"></param>
        ///// <returns></returns>
        //private System.Collections.ArrayList GetFreeZonePointList(int iZoneIndex)
        //{
        //    System.Collections.ArrayList mparrFreeZonePointList = new System.Collections.ArrayList();
        //    for (int i = 1; i < (int)eMapSize; i++)
        //        for (int j = 1; j < (int)eMapSize; j++)
        //        {
        //            if ((int)iarrMap[(int)MapLayer.Zones, i, j] == iZoneIndex)//if point is in spacified zone
        //            {
        //                if (IsObjectFreePoint(i, j))
        //                {
        //                    mparrFreeZonePointList.Add(new MapPoint(i, j));
        //                }
        //            }
        //        }
        //    return (mparrFreeZonePointList);

        //}

        /// <summary>
        /// checks if a point is free from objects ( or other )
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private bool IsObjectFreePoint(int x, int y)
        {
            eBlockStatus BlockStatus = (eBlockStatus) iarrMap[(int)MapLayer.Blocks, x, y];

            //only free or maybe block points considered as free
            if ( BlockStatus == eBlockStatus.Free || BlockStatus == eBlockStatus.MaybeBlock || BlockStatus == eBlockStatus.Preserved)
                return ( iarrMap[(int)MapLayer.Objects, x, y] == null ); //check if current point is free
            else
                return false;

        }


        public void PopulateTowns()
        {
            for (int i = 0; i < thSelectedTemplate.ZoneNumber ; i++)
            {
                PopulateTownInZone(i);
            }
        }


        public void PopulateMines()
        {
            for (int i = 0; i < thSelectedTemplate.ZoneNumber; i++)
            {
                PopulateMinesInZone(i);
            }
        }




        public float CalculateZoneDensity(int iZoneIndex)
        {

            int iZoneFilledPoints = 0;
            int iZoneTotalSize = 0;

            //calculate how many points are filled in zone
            for (int i = 0; i < (int)eMapSize; i++)
            {
                for (int j = 0; j < (int)eMapSize; j++)
                {
                    if ((int)iarrMap[(int)MapLayer.Zones, i, j] == iZoneIndex)
                    {
                        iZoneTotalSize++;
                        //if point is an object or area related then add it to zone fill area
                        if (iarrMap[(int)MapLayer.Objects, i, j] != null)
                        {
                            iZoneFilledPoints++;
                        }
                    }

                }
            }
            return (float) iZoneFilledPoints / iZoneTotalSize;
        }

        public int CalculateZoneSize(int iZoneIndex)
        {
            int iZoneTotalSize = 0;

            //calculate how many points are filled in zone
            for (int i = 0; i < (int)eMapSize; i++)
            {
                for (int j = 0; j < (int)eMapSize; j++)
                {
                    if ((int)iarrMap[(int)MapLayer.Zones, i, j] == iZoneIndex)
                    {
                        iZoneTotalSize++;
                    }

                }
            }
            return  iZoneTotalSize;
        }

        public int CalculateZoneFilled(int iZoneIndex)
        {

            int iZoneFilledPoints = 0;

            //calculate how many points are filled in zone
            for (int i = 0; i < (int)eMapSize; i++)
            {
                for (int j = 0; j < (int)eMapSize; j++)
                {
                    if ((int)iarrMap[(int)MapLayer.Zones, i, j] == iZoneIndex)
                    {
                        //if point is an object or area related then add it to zone fill area
                        if (iarrMap[(int)MapLayer.Objects, i, j] != null)
                        {
                            iZoneFilledPoints++;
                        }
                    }

                }
            }
            return iZoneFilledPoints;
        }

        //public int GetZoneSize(int iZoneIndex)
        //{


        //    int iZoneTotalSize = 0;

        //    for (int i = 0; i < (int)eMapSize; i++)
        //    {
        //        for (int j = 0; j < (int)eMapSize; j++)
        //        {
        //            if (iarrMap[(int)MapLayer.Zones, i, j] == iZoneIndex)
        //            {
        //                iZoneTotalSize++;

        //            }

        //        }
        //    }
        //    return iZoneTotalSize;
        //}

        public void PopulateBlocksInsideZones()
        {
            for (int i = 0; i < thSelectedTemplate.ZoneNumber; i++)
            {
                PopulateBlocksInsideZone(i+1);
            }
        }

        private void PopulateBlocksInsideZone(int iZoneIndex)
        {
            //draw a random block density (20% - 30%)
            //#if (DEBUG)
            //     Random Randomizer.rnd = new Random(3);
            //#else
            //     Random Randomizer.rnd = new Random();
            //#endif
            //random size of all blocks objects
            double dRandomBlockDensity = (double) Randomizer.rnd.Next(30,35)/100;
            int iTotalBlockStackSize = 0;

            //draw an initial random block size 
            int iBlockStackLimit =  0;
            int iRandomBlockObjectsNumber = 0;
            int iBlockObjectsCounter = 0;
            ObjectsReader obrdBlocksReader = new ObjectsReader( "Blocks.xml" );
            
            MapObject moBlock ;
            MapPoint mpStackPoint;
            MapObject moPrevObject = new MapObject(null,null,string.Empty);

            int iTriesCounter = 12;

            do
            {
                
                //pick a random free point and draw a random size of each block ,
                //then place object until that size is reached or general size is reached if size is reached draw a new random point
                //2 conditions for blocks drawned - also draw object number (up to 8 objects)

                //first time will always get in here
                //if number limit is reached or total size limit is reached pick a new point and randomize again picks
                if (iBlockObjectsCounter == iRandomBlockObjectsNumber || iTotalBlockStackSize >= iBlockStackLimit)
                {
                    moBlock = obrdBlocksReader.GetRandomObjectByCategory(eObjectCategory.Block.ToString());
                    mpStackPoint = GetFreeFittingPointInZone(iZoneIndex, moBlock);



                    if (mpStackPoint.x != -1)
                    {
                        iTriesCounter = 12;
                        iRandomBlockObjectsNumber = Randomizer.rnd.Next(1, 9);
                        iBlockStackLimit = Convert.ToInt32(GetZoneSize(iZoneIndex) * dRandomBlockDensity / Randomizer.rnd.Next(14, 20)); //zone size * Density limit (*0.2-0.3) / random number for 5-15 block stacks
                        //reset counter and size of block(new block starting)
                        iBlockObjectsCounter = 1;
                        iTotalBlockStackSize = 0;

                        //save reference to prev object
                        moPrevObject = moBlock;
                    }
                    else
                    {
                        iTriesCounter--;

                        if (iTriesCounter == 0)
                            break;
                    }
                }
                else
                {
                    moBlock = obrdBlocksReader.GetRandomObjectByCategory(eObjectCategory.Block.ToString());
                    mpStackPoint = GetFittingPointInZoneAdjacentToPointInDirection(iZoneIndex,moPrevObject ,moBlock);



                    if (mpStackPoint.x != -1)
                    {
                        iTriesCounter = 12;
                        iBlockObjectsCounter++;
                        //save reference to prev objectif
                        moPrevObject = moBlock;

                    }
                    else
                    {
                        iTriesCounter--;

                        if (iTriesCounter == 0)
                        {
                            iBlockObjectsCounter = iRandomBlockObjectsNumber;
                        }
                    }
                    

                }


                if ( mpStackPoint.x != -1 )
                {

                    moBlock.BasePoint = mpStackPoint;
                    if (PlaceTightObject(moBlock))
                    {
                       // iBlockObjectsCounter++;
                        iTotalBlockStackSize+= moBlock.ObjectSize;
                       
                    }
                }
            } while (CalculateZoneDensity(iZoneIndex) < dRandomBlockDensity);


        }

        private int GetZoneSize(int iZoneIndex)
        {
            int iZoneTotalSize = 0;

            //calculate how many points are filled in zone
            for (int i = 0; i < (int)eMapSize; i++)
            {
                for (int j = 0; j < (int)eMapSize; j++)
                {
                    if ((int)iarrMap[(int)MapLayer.Zones, i, j] == iZoneIndex)
                    {
                        iZoneTotalSize++;
                    }

                }
            }
            return iZoneTotalSize;
        }

        public void PopulateMinesInZone(int iZoneIndex)
        {


           bool boolLink = Convert.ToBoolean(thSelectedTemplate.GetLinkAttribute(iZoneIndex, "Mines"));
           double AppearChance = 0;
           if (boolLink && thSelectedTemplate.getZoneProperty(iZoneIndex, eTemplateElements.IsStartingZone.ToString()) != "True")
           {
               XmlNode xndNodeTemplate;
               if (thSelectedTemplate.xSelectedObjectSets[iZoneIndex] == null)
               { // выбираем сет и в зависимости от него ставим города или нет
                   XmlElement[] xObjectSets = new XmlElement[3];
                   for (int i = 0; i < 3; i++)
                   {
                       xObjectSets[i] = thSelectedTemplate.GetObjectsSet(iZoneIndex, i + 1);
                   }
                   double dRandomObjectSetPointer = Randomizer.rnd.NextDouble();
                   double dSum = 0;
                   int iRandomObjectSetIndex = -1;

                   do
                   {
                       iRandomObjectSetIndex++;
                       //add number until sum of appear chance reaches random number (up to 1.0) to generate a random effect
                       dSum += Convert.ToDouble(xObjectSets[iRandomObjectSetIndex].Attributes["Appear_Chance"].Value, System.Globalization.CultureInfo.InvariantCulture);
                   } while (dSum < dRandomObjectSetPointer);
                   //dwp сет выбрали и запомнили
                   thSelectedTemplate.xSelectedObjectSets[iZoneIndex] = xObjectSets[iRandomObjectSetIndex];

                   ObjectsReader obrdReader = new ObjectsReader(xObjectSets[iRandomObjectSetIndex]);
                   xndNodeTemplate = obrdReader.GetObjectsData().SelectSingleNode(".//Object[@Name='Mines']");
               }
               else
               {
                   ObjectsReader obrdReader = new ObjectsReader(thSelectedTemplate.xSelectedObjectSets[iZoneIndex]);
                   xndNodeTemplate = obrdReader.GetObjectsData().SelectSingleNode(".//Object[@Name='Mines']");
               }
               try
                {
               AppearChance = Convert.ToDouble(xndNodeTemplate.Attributes[eObjectData.Chance.ToString()].Value, System.Globalization.CultureInfo.InvariantCulture);
                }
               catch
               {
               AppearChance = 0;
               }
           }
            


           if(thSelectedTemplate.getZoneProperty(iZoneIndex, eTemplateElements.IsStartingZone.ToString()) == "True" || !boolLink || AppearChance > 0)
           {

            int[] iarrMinesNumber = this.thSelectedTemplate.GetMinesData(iZoneIndex);
            string[] strarrMinesValue = this.thSelectedTemplate.GetAdditionalMinesData(iZoneIndex, "Value");
            string[] strarrMinesChance = this.thSelectedTemplate.GetAdditionalMinesData(iZoneIndex, "Chance");

            //go over all mines (but last which is not supported yet and abandoned which require special care
            for (int i = 0; i < iarrMinesNumber.Length - 1; i++)
            {
                //if any mines defined by user
                if (iarrMinesNumber[i] > 0)
                {
                    //loop as number as requested mines and place them in zone
                    for (int j = 0; j < iarrMinesNumber[i]; j++)
                    {
                        double RandomChance = Randomizer.rnd.NextDouble();

                        //if random chance is smaller then hit! 
                        if (RandomChance <= double.Parse(strarrMinesChance[i]))
                        {
                            //place the requested mine
                            ObjectsReader obrdMines = new ObjectsReader("Mines.xml");
                            MapObject mpobjMine = obrdMines.GetObjectByName(Enum.GetNames(typeof(eMines))[i]);
                            MapPoint mpMinePoint = null;
                            //if its first wood/ore in a starting zone then make sure its in a minimum distance of the starting town
                            if (i <= 1) //if its woord or ore
                            {
                                if (iZoneIndex + 1 == thSelectedTemplate.getStartingZoneIndex(1) ||
                                    iZoneIndex + 1 == thSelectedTemplate.getStartingZoneIndex(2) ||
                                    iZoneIndex + 1 == thSelectedTemplate.getStartingZoneIndex(3) ||
                                    iZoneIndex + 1 == thSelectedTemplate.getStartingZoneIndex(4)) //if its a starting zone-
                                {
                                    //get starting town point
                                    MapPoint mpStartTown = (MapPoint)AccessPoints[iZoneIndex, (int)ePointLayer.StartingTownsPoints][0];
                                    System.Collections.ArrayList AllFreePoinsForPlaceMine = GetALLFreeFittingPointsInZone(iZoneIndex + 1, mpobjMine);
                                    int iRandomPointIndex = Randomizer.rnd.Next(AllFreePoinsForPlaceMine.Count);

                                    while (AllFreePoinsForPlaceMine.Count > 0 &&
                                           (MapPoint)AllFreePoinsForPlaceMine[iRandomPointIndex] - mpStartTown > 30)
                                    {

                                        AllFreePoinsForPlaceMine.Remove(AllFreePoinsForPlaceMine[iRandomPointIndex]);
                                        iRandomPointIndex = Randomizer.rnd.Next(AllFreePoinsForPlaceMine.Count);
                                    }
                                    if (AllFreePoinsForPlaceMine.Count == 0)
                                        throw new Exception("No room for placing mines at Starting Zone!");
                                    else mpMinePoint = (MapPoint)AllFreePoinsForPlaceMine[iRandomPointIndex];
                                }
                                else mpMinePoint = GetFreeFittingPointInZone(iZoneIndex + 1, mpobjMine);
                            }
                            else mpMinePoint = GetFreeFittingPointInZone(iZoneIndex + 1, mpobjMine);

                            mpobjMine.BasePoint = mpMinePoint;
                            if (!PlaceObject(mpobjMine, iZoneIndex + 1))
                            {
                                //dumpBlockMaskToFileErgo();
                                throw new Exception("Not enough space for placing mines at Zone #" + (iZoneIndex + 1).ToString());
                            }

                            //place mines guard
                            //this.PlaceObjectGuard(mpobjMine);

                            MapPoint mpAccess = mpobjMine.BasePoint + TransformPointDirection(mpobjMine.AccessPoint, mpobjMine.Direction);


                            //add the mine point for opening points list
                            AccessPoints[iZoneIndex, (int)ePointLayer.MinePoints].Add(mpAccess);

                            if (i != (int)eMines.Abandoned_Mine)
                            { //no guard for abondoned mine
                                MapObject mpobjGuard = new MapObject(mpAccess, null, "Random", eObjectType.Guard, null, int.Parse(strarrMinesValue[i]));
                                //mpobjGuard.ObjectSpacificProperties.Add("Value", thSelectedTemplate.GetConnectionGuardValue(iConnectionIndex));
                                if (!PlaceObject(mpobjGuard, iZoneIndex + 1))
                                    throw new Exception("Not enough space for placing mines guards at Zone #" + (iZoneIndex + 1).ToString());

                                //place random 0,1 or 2 additional mine resources
                                double dRandomAdjacentResources = Randomizer.rnd.NextDouble();

                                if (dRandomAdjacentResources < 0.5)
                                {
                                    //place first resource
                                    ObjectsReader obrdResources = new ObjectsReader("Resources.xml");
                                    MapObject mpResource1 = obrdResources.GetObjectByName(Enum.GetNames(typeof(eResources))[i]);
                                    mpResource1.BasePoint = mpAccess + TransformPointDirection(new MapPoint(1, 0), mpobjMine.Direction);
                                    PlaceObject(mpResource1, iZoneIndex + 1);
                                }

                                dRandomAdjacentResources = Randomizer.rnd.NextDouble();

                                if (dRandomAdjacentResources < 0.5)
                                {
                                    //place second resource
                                    ObjectsReader obrdResources = new ObjectsReader("Resources.xml");
                                    MapObject mpResource1 = obrdResources.GetObjectByName(Enum.GetNames(typeof(eResources))[i]);
                                    mpResource1.BasePoint = mpAccess + TransformPointDirection(new MapPoint(-1, 0), mpobjMine.Direction);
                                    PlaceObject(mpResource1, iZoneIndex + 1);
                                }
                            }

                            //also go around guard point and mark place as done if is free 
                            MapPoint mpAroundGuard;
                            for (int x = 0; x < mpRoundTripAllDirections.Length; x++)
                            {
                                //
                                mpAroundGuard = mpAccess + mpRoundTripAllDirections[x];
                                if (iarrMap[(int)MapLayer.Objects, mpAroundGuard.x, mpAroundGuard.y] == null)
                                    iarrMap[(int)MapLayer.Objects, mpAroundGuard.x, mpAroundGuard.y] = "AroundGuard";
                            }
                        }
                    }
                }
            }
          }
        }


        internal const int ITOWNNUMBER = 3;

        public void PopulateTownInZone(int iZoneIndex)
        {
            bool boolLink = Convert.ToBoolean(thSelectedTemplate.GetLinkAttribute(iZoneIndex, "Towns"));
            double AppearChance = 0;
            if (boolLink && thSelectedTemplate.getZoneProperty(iZoneIndex, eTemplateElements.IsStartingZone.ToString()) != "True")
            { // выбираем сет и в зависимости от него ставим города или нет
                XmlElement[] xObjectSets = new XmlElement[3];
                for (int i = 0; i < 3; i++)
                {
                    xObjectSets[i] = thSelectedTemplate.GetObjectsSet(iZoneIndex, i + 1);
              }
              double dRandomObjectSetPointer = Randomizer.rnd.NextDouble();
              double dSum = 0;
              int iRandomObjectSetIndex = -1;

              do {
                    iRandomObjectSetIndex++;
                    //add number until sum of appear chance reaches random number (up to 1.0) to generate a random effect
                    dSum += Convert.ToDouble(xObjectSets[iRandomObjectSetIndex].Attributes["Appear_Chance"].Value, System.Globalization.CultureInfo.InvariantCulture);
              } while (dSum < dRandomObjectSetPointer);
              //dwp сет выбрали и запомнили
              thSelectedTemplate.xSelectedObjectSets[iZoneIndex] = xObjectSets[iRandomObjectSetIndex];

              ObjectsReader obrdReader = new ObjectsReader(xObjectSets[iRandomObjectSetIndex]);
              XmlNode xndNodeTemplate = obrdReader.GetObjectsData().SelectSingleNode(".//Object[@Name='Towns']");

              try
              {
                  AppearChance = Convert.ToDouble(xndNodeTemplate.Attributes[eObjectData.Chance.ToString()].Value, System.Globalization.CultureInfo.InvariantCulture);
              }
              catch
              {
                  AppearChance = 0;
              }
            }

            if (thSelectedTemplate.getZoneProperty(iZoneIndex, eTemplateElements.IsStartingZone.ToString()) == "True" || !boolLink || AppearChance > 0)
            for (int i = 1; i < ITOWNNUMBER + 1; i++)
            {
                string strTownType = thSelectedTemplate.GetTownsAttributes(iZoneIndex , i , eTown.Type.ToString());

                eTownType TownType = (eTownType) Enum.Parse(typeof(eTownType), strTownType);
                bool bFirstStartingTown = true;

                //if none then no town and no need to create town object
                if (TownType != eTownType.None)
                {
                    ObjectsReader obrdTownRead = new ObjectsReader("OtherObjects.xml");
                    MapObject mpobjTown = obrdTownRead.GetObjectByName("RandomTown");
                    ObjectSpacificPropertiesHelper.AddTownProperties(mpobjTown, thSelectedTemplate , i ,iZoneIndex);
                    MapPoint mpTownPoint = GetFreeFittingPointInZone(iZoneIndex + 1, mpobjTown);
                    //add point to access point
                    //if its the first starting town add it to starting town

                    mpobjTown.BasePoint = mpTownPoint;
                    if (!PlaceObject(mpobjTown, iZoneIndex + 1))
                        throw new Exception("Bad zone. No room for placing town at Zone #" + (iZoneIndex + 1).ToString() + Environment.NewLine + 
                                            "Try again or check your template.");


                    //get the access point (and even further to get to preserved
                    MapPoint mpPreserved = mpobjTown.BasePoint + TransformPointDirection(mpobjTown.AccessPoint, mpobjTown.Direction) + TransformPointDirection(new MapPoint(0, -2), mpobjTown.Direction) ;

                    if ((eBlockStatus)iarrMap[(int)MapLayer.Blocks, mpPreserved.x, mpPreserved.y] != eBlockStatus.Preserved)
                    {
                        MakeThroughFromTownToPresereved(mpPreserved);
                    }


                    if (mpobjTown.ObjectSpacificProperties[eTown.IsStartingTown.ToString()] == "True" && bFirstStartingTown)
                    {
                        bFirstStartingTown = false;
                        AccessPoints[iZoneIndex, (int)ePointLayer.StartingTownsPoints].Add(mpPreserved);
                    }
                    else AccessPoints[iZoneIndex, (int)ePointLayer.OtherTownsPoints].Add(mpPreserved);
                }
            }
        }


        /// <summary>
        /// this method was made to aid roads algorithem and creates the shortest path from the first open town point
        /// to the preserved area
        /// </summary>
        /// <param name="mpPreserved"></param>
        private void MakeThroughFromTownToPresereved(MapPoint mpPreserved)
        {
            //count in 4 directions and find closest way


            //check all directions and find closest distance to preserved point
            int[] iDistance = new int[mpRoundTrip.Length];

            //first check in first zone point , - look for closest preserved point in same zone


            //find minimum distance
            //only go in 4 directions
            int iMinDistanceIndex1 = -1;
            int iMinDistance = int.MaxValue;

            for (int i = 0; i < mpRoundTrip.Length; i++)
            {
                iDistance[i] = CountPreservedDistance(mpPreserved.x, mpPreserved.y, i * 2);
                if (iDistance[i] < iMinDistance)
                {
                    iMinDistance = iDistance[i];
                    iMinDistanceIndex1 = i*2;
                }
            }

            if (iMinDistanceIndex1 == -1)
                throw new Exception("Can't create road");

            MarkPreservedInDirection(mpPreserved.x, mpPreserved.y, iMinDistanceIndex1);

        }



        /// <summary>
        /// goes over each done adjacent and if its a maybefree turns it into must be free
        /// </summary>
        internal void TransformDoneAdjacentToPreserved()
        {
            for (int i = 0; i < (int)eMapSize; i++)
            {
                for (int j = 0; j < (int)eMapSize; j++)
                {
                    if ((eBlockStatus)iarrMap[(int)MapLayer.Blocks, i, j] == eBlockStatus.Done)
                    {
                        //go around and if its a maybe change it to must be free
                        foreach (MapPoint mpDirection in mpRoundTripAllDirections)
                        {
                            if (IsInsideMapLimit(i + mpDirection.x, j + mpDirection.y))
                            {
                                if ((eBlockStatus)iarrMap[(int)MapLayer.Blocks, i + mpDirection.x, j + mpDirection.y] == eBlockStatus.Free)
                                {
                                    iarrMap[(int)MapLayer.Blocks, i + mpDirection.x, j + mpDirection.y] = eBlockStatus.Preserved;
                                }

                            }
                        }

                    }
                }
            }
        }

        /// <summary>
        /// goes over each Free and turns it into block by chance of 0.65
        /// </summary>
        internal void TransformFreeToBlockByRandomChance()
        {
            for (int i = 0; i < (int)eMapSize; i++)
            {
                for (int j = 0; j < (int)eMapSize; j++)
                {
                    if ((eBlockStatus)iarrMap[(int)MapLayer.Blocks, i, j] == eBlockStatus.Free)
                    {
                        if (Randomizer.rnd.NextDouble() < 0.5)
                            iarrMap[(int)MapLayer.Blocks, i, j] = eBlockStatus.MaybeBlock;
                    }
                }
            }
        }


        public static readonly int[] MIL_POST_TIER_CHANCES = {
            35, 35, 20, 10
        };

        internal void PopulateDwellings()
        {
            DwellingGenerator dw_gen = new DwellingGenerator(GetDwellingNumber());
            if (dw_gen.DwellNumber == 0) {
                return;
            }
            dw_gen.DwellStatus = (eDwellingStatus)Settings.Default.DwellingStatus;
            dw_gen.Run(); //generate dwellings

            ObjectsReader obrdDwellingRead = new ObjectsReader("OtherObjects.xml");

            for (int iStartingZoneIndex = 1; iStartingZoneIndex <= thSelectedTemplate.IPLAYERNUMBER; iStartingZoneIndex++)
            {
                //populate for first zone
                int iPlayerZoneIndex = thSelectedTemplate.getStartingZoneIndex(iStartingZoneIndex);

                //get which player is in that zone
                string strPlayerId = thSelectedTemplate.getPlayerIdPerZone(iPlayerZoneIndex - 1);
                int    intPlayerId = int.Parse(strPlayerId.Replace("PLAYER_",""));

                int iSelectedZoneFaction = -1;
                //get faction selection for that zone            
                iSelectedZoneFaction = thSelectedTemplate.iarrSelectedPlayersFactions[intPlayerId - 1];

                //go over all dwellings
                for (int i = 0; i < dw_gen.DwellNumber; i++)
                {
                    //if its top level produce a military post object with selected levels
                    if (dw_gen.IsTop(i))
                    {
                        int numDwelling = 0;
                        string indexDwelling = "";
                        string strDwellingMask = string.Empty;

                        for (int j = 3; j < 7; j++)
                        {
                            numDwelling += dw_gen.Mask(i, j);
                            strDwellingMask = strDwellingMask + dw_gen.Mask(i, j).ToString();
                            if (dw_gen.Mask(i, j) == 1) indexDwelling = (j + 1).ToString();
                        }

                        //get name of selected faction
                        string strFactionName = Enum.GetName(typeof(eTownType), iSelectedZoneFaction);
                        MapObject mpobjDwelling;
                        if (numDwelling > 1 || Settings.Default.NoNewHighDwelling) indexDwelling = "";
                        mpobjDwelling = obrdDwellingRead.GetObjectByName(strFactionName + indexDwelling + "Dwelling");
                        mpobjDwelling.ObjectSpacificProperties.Add("DwellingArray", strDwellingMask);
                        //type is high dwelling and needs rendering..
                        MapPoint mpDwellingPoint = GetFreeFittingPointInZone(iPlayerZoneIndex, mpobjDwelling);

                        mpobjDwelling.BasePoint = mpDwellingPoint;
                        if (!PlaceObject(mpobjDwelling, iPlayerZoneIndex)) throw new Exception("Can't place dwelling");
                        MarkPathForZone(mpobjDwelling.BasePoint, (MapPoint)AccessPoints[iPlayerZoneIndex,0][0]);
                    }
                    else 
                    {
                        //if its low level then procude a random dwelling set it as selected level

                        MapObject mpobjDwelling = obrdDwellingRead.GetObjectByName("RandomDwelling");
                        //also add player assosiation
                        mpobjDwelling.ObjectSpacificProperties.Add("PlayerID", strPlayerId);
                        for (int j = 0; j < 3; j++)
                        {
                            if (dw_gen.Mask(i, j) == 1)
                                mpobjDwelling.ObjectSpacificProperties.Add("DwellingLevel", (j + 1).ToString());
                        }

                        //type is low dwelling and needs rendering..
                        MapPoint mpDwellingPoint = GetFreeFittingPointInZone(iPlayerZoneIndex, mpobjDwelling);
                        mpobjDwelling.BasePoint = mpDwellingPoint;
                        if (!PlaceObject(mpobjDwelling, iPlayerZoneIndex)) throw new Exception("Can't place dwelling");
                        MarkPathForZone(mpobjDwelling.BasePoint, (MapPoint)AccessPoints[iPlayerZoneIndex,0][0]);
                    }
                    #if (DEBUG)
                    dumpBlockMaskToFileErgo();
                    #endif
                }
            }
        }

        internal void PopulateDwellingsInZones()
        {
            for (int i = 0; i < thSelectedTemplate.ZoneNumber; i++) {
                PopulateDwellingsInZone(i);
            }
        }

        public static readonly int MPOST_BASE_CHANCE = 5; //Dwp мульти вероятность 5% с версии 1.6.6.
        private static readonly double[] DWELLINGS_BASE_CHANCES = { 18, 18, 18, 18, 18, 5, 5};
        private static readonly double[] DWELLINGS_BASE_CHANCES_OLD = { 33, 33, 34, 0, 0, 0, 0 };

        private class DwellingGenerator
        {
            private int mpost_chance_; //military post base chance
            private double[] dwell_chances_; //base chances for dwellings
            private int[] m_chances_; //base chances for dwellings

            private eDwellingStatus dwell_status_ = (eDwellingStatus)Settings.Default.DwellingStatus;

            public int DwellNumber { get { return number_; } set { number_ = value; } }
            public eDwellingStatus DwellStatus { get { return dwell_status_; } set { dwell_status_ = value; } }
            public bool IsTop(int i) { return i >= 0 && i < number_ ? is_top_[i] : false; }
            public int Mask(int i, int j) { return i >= 0 && i < number_ && j >= 0 && j < 7 ? mask_[i, j] : 0; }

            private int number_; //dwelling number
            private bool[] is_top_; //mpost?
            private int[,] mask_;
            
            public DwellingGenerator(int num)
            {
                number_ = num;
                is_top_ = new bool[number_];
                mask_ = new int[number_, 7];
                m_chances_ = new int[4];
                dwell_chances_ = new double[7];
                mpost_chance_ = ((dwell_status_ == eDwellingStatus.Extended) || (dwell_status_ == eDwellingStatus.Standard)) ? 
                                0 : (Settings.Default.NoNewHighDwelling ? 25 : MPOST_BASE_CHANCE);
                /*dwp. при генерации старых без новых высоких двеллингов, оставляем вероятность 25% */
                for (int i = 0; i < 7; i++)
                {
                    dwell_chances_[i] = Settings.Default.NoNewHighDwelling || (dwell_status_ == eDwellingStatus.Extended) || (dwell_status_ == eDwellingStatus.Standard)
                                           ? DWELLINGS_BASE_CHANCES_OLD[i] : DWELLINGS_BASE_CHANCES[i];
                }
                if (dwell_status_ == eDwellingStatus.Standard)
                {
                    dwell_chances_[2] = 0; //dwp для стандарта убираем 3й двеллинг
                }
                for (int i = 0; i < 4; i++)
                {
                    m_chances_[i] = MIL_POST_TIER_CHANCES[i];
                }
            }

            public void Run()
            {
                for (int i = 0; i < number_; i++) {
                    if (Randomizer.rnd.Next(100) < mpost_chance_) {
                        is_top_[i] = true;
                        mpost_chance_ = 0;      // dwp выпал мульти значит
                        dwell_chances_[5] = 0;  // .....
                        dwell_chances_[6] = 0;  // 6-7 уровни и сам мульти в ноль.

                    }
                    else {
                        is_top_[i] = false;
                    }
                    if (is_top_[i]) {
                        int sum = 0, rmaxval = 100; // dwp количество высоких двеллингов
                        //this dwelling will include lvl 4-7 tiers foreach lvl randomize if it exist or not
                        do
                        {
                            for (int j = 3; j < 7; j++)
                            {
                                if (mask_[i, j] == 0) 
                                /* 1.7.2 цикл должен срабатывать только для неиспользованых уровней 
                                   иначе зациклится */
                                {   //GETS 0 OR 1
                                    //iarrDwellingMask[i, j] = Randomizer.rnd.Next(2);
                                    //dmitrik: chances for tiers 4-7: 50,50,25,25
                                    //dwp: шансы изменены на 35 35 20 10
                                    mask_[i, j] = Randomizer.rnd.Next(rmaxval) < m_chances_[j - 3] ? 1 : 0;
                                    sum += mask_[i, j];
                                }
                            }
                        } while (sum < (Settings.Default.NoNewHighDwelling ? 1 : 2));
                        for (int j = 3; j < 7; j++) /* понижаем вдвое вероятность хай левелов */
                        {
                            dwell_chances_[j] = dwell_chances_[j] * 0.5;
                        }
                    }
                    else {
                        //only 1 lvl 1-3 tier
                        //int iRandomTierLevel = Randomizer.rnd.Next(3);
                        double total_chances = 0;

                        for (int j = 0; j < 7; j++)
                        {
                            total_chances += dwell_chances_[j];
                        }
                        int lvl = Randomizer.Choice (dwell_chances_,total_chances);
                        //mark dwelling level
                        mask_[i, lvl] = 1;
                        if (lvl == 5 || lvl == 6) // dwp выпал 6,7 уровень
                        {
                            mpost_chance_ = 0;      // .....
                            dwell_chances_[5] = 0;  // .....
                            dwell_chances_[6] = 0;  // 6-7 уровни и мульти в ноль.
                        }
                        if (lvl > 2)
                        {
                            is_top_[i] = true;
                            for (int j = 3; j < 7; j++) /* понижаем вдвое вероятность хай левелов */
                            {
                                dwell_chances_[j] = dwell_chances_[j] * 0.5;
                            }
                        }
                        else //half chances for next dwell the same level as current
                        { if (dwell_status_ == eDwellingStatus.Random)
                            {
                                Randomizer.ShiftChance(dwell_chances_, lvl, 0.5);
                            }
                            else
                            {
                                Randomizer.ShiftChance(dwell_chances_, lvl, 0);
                                //Randomizer.SwapChances(dwell_chances_, lvl, i + 1);
                            }
                        }
                    }
                }
            }
        }

        internal void PopulateDwellingsInZone(int iZoneIndex)
        {
            if (iZoneIndex < thSelectedTemplate.IPLAYERNUMBER) { //not for starting zones
                return;
            }
            DwellingGenerator dw_gen = new DwellingGenerator(thSelectedTemplate.GetRandomDwellingNumberInZone(iZoneIndex));
            if (dw_gen.DwellNumber == 0) {
                return;
            }
           
            dw_gen.Run(); //generate dwellings
            ObjectsReader obrdDwellingRead = new ObjectsReader("OtherObjects.xml");

            //go over all dwellings
            for (int i = 0; i < dw_gen.DwellNumber; i++) {
                //if its top level produce a military post object with selected levels
                MapObject dwelling = null;
                if (dw_gen.IsTop(i)) {
                    int faction = Randomizer.rnd.Next(2, 10); //get random faction
                    //get name of selected faction
                    string faction_name = Enum.GetName(typeof(eTownType), faction);
                    int numDwelling = 0;
                    string indexDwelling = "";
                    string mask = string.Empty;

                    for (int j = 3; j < 7; j++)
                    {
                        numDwelling += dw_gen.Mask(i, j);
                        if (dw_gen.Mask(i, j) == 1) indexDwelling = (j + 1).ToString();
                        mask = mask + dw_gen.Mask(i, j).ToString();
                    }
                    if (numDwelling > 1 ||Settings.Default.NoNewHighDwelling) indexDwelling = "";
                    dwelling = obrdDwellingRead.GetObjectByName(faction_name + indexDwelling + "Dwelling");
                    //for high level dwelling need a string of 0 or 1 that says if a certain level is in dwelling
                    dwelling.ObjectSpacificProperties.Add("DwellingArray", mask);
                    //type is high dwelling and needs rendering..
                }
                else {
                    //if its low level then procude a random dwelling set it as selected level

                    dwelling = obrdDwellingRead.GetObjectByName("RandomDwelling");
                    //also add player assosiation
                    dwelling.ObjectSpacificProperties.Add("PlayerID", "PLAYER_NONE");
                    for (int j = 0; j < 3; j++) {
                        if (dw_gen.Mask(i, j) == 1)
                            dwelling.ObjectSpacificProperties.Add("DwellingLevel", (j + 1).ToString());
                    }
                }
                MapPoint dwelling_point = GetFreeFittingPointInZone(iZoneIndex + 1, dwelling);
                dwelling.BasePoint = dwelling_point;
                if (!PlaceObject(dwelling, iZoneIndex + 1))
                    throw new Exception("Can't place dwelling");
                MarkPathForZone(dwelling.BasePoint, (MapPoint)AccessPoints[iZoneIndex, 0][0]);
#if (DEBUG)
                dumpBlockMaskToFileErgo();
#endif


            }
        }

        private int GetDwellingNumber()
        {
            switch (Settings.Default.DwellingStatus) {
                case (int)eDwellingStatus.Standard: //number: 2, lvl: 1,2 
                    return 2;
                case (int)eDwellingStatus.Extended: //number: 3, lvl: 1-3
                    return 3;
                case (int)eDwellingStatus.Random: //number: 1-3, lvl: 1-7 
                default:
                    return Randomizer.rnd.Next(1, 4);
            }
        }

        private int GetMPostBaseChance()
        {
            switch (Settings.Default.DwellingStatus) {
                case (int)eDwellingStatus.Standard: //number: 2, lvl: 1,2 
                case (int)eDwellingStatus.Extended: //number: 3, lvl: 1-3
                    return 0;
                case (int)eDwellingStatus.Random: //number: 1-3, lvl: 1-7 
                default:
                    return MPOST_BASE_CHANCE;
            }
        }

        private double[] GetDwellingsBaseChance()
        {
            switch (Settings.Default.DwellingStatus) {
                case (int)eDwellingStatus.Standard: //number: 2, lvl: 1,2 
                case (int)eDwellingStatus.Extended: //number: 3, lvl: 1-3
                    return new double[] { 100, 0, 0, 0, 0, 0, 0 };
                case (int)eDwellingStatus.Random: //number: 1-3, lvl: 1-7 
                default:
                    return DWELLINGS_BASE_CHANCES;
            }
        }
    }
}
