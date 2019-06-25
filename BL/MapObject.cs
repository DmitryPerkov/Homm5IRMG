using System;
using System.Collections.Generic;
using System.Text;

namespace Homm5RMG
{
    public class MapObject : IComparable    
    {
        public MapPoint BasePoint;
        public MapPoint[] Area;
        public string strName;
        public Dictionary<string, string> ObjectSpacificProperties;
        public eObjectType Type = eObjectType.General;
        public MapPoint[] PathWays;
        public int iValue=0;
        public MapPoint AccessPoint;
        public bool bShouldBeGuarded;
        public string Rotation = ObjectsWriter.strDirections[(int)ObjectDirection.Down];
        public int    max_number_ = 0; //max number of object in zone
        //dwp! обьект хранит не только максимальное количество , 
        //dwp! но и текущее количество размещенных обьектов и вероятность размещения.
        public double probability_ = 0; //dwp вероятность появления обьекта
        public int current_number = 0;   //dwp текущее количество обьектов в зоне

       // public ObjectDirection Direction = ObjectDirection.Down;

        private ObjectDirection objdirDirection = ObjectDirection.Down;



        public int ObjectSize
        {
            get
            {
                if (Area != null)
                {
                    return Area.Length + 1;
                }
                else
                    return 1; 
            }
        }
	

        public ObjectDirection Direction
        {
            get { return objdirDirection; }
            set 
            {
                if (this.objdirDirection != value)
                {
                    //update rotation 
                    int iRotationIndex = -1;
                    //find current rotation
                    for (int i = 0; i < 4; i++)
                    {
                        if (ObjectsWriter.strDirections[i] == Rotation)
                            iRotationIndex = i;
                    }

                    int iDifference = (int)value - (int)objdirDirection;
                    Rotation = ObjectsWriter.strDirections[AddDirectionIndex(iRotationIndex, iDifference)];

                    objdirDirection = value;
                }
            }
        }

        //adds in rotation the current index to index added 
        private int AddDirectionIndex(int iInitialIndex, int iAddedIndexDifference)
        {
            int iSum = iInitialIndex + iAddedIndexDifference;
            //adjust in rotation
            if (iSum < 0)
                iSum +=4;
            else
                if (iSum > 3)
                    iSum -= 4;
            return iSum;
        }
	



        public MapObject(MapPoint pBasePoint, MapPoint[] pArea , string strObjectName)
        {
            BasePoint = pBasePoint;
            Area = pArea;
            strName = strObjectName;
            ObjectSpacificProperties = new Dictionary<string, string>();
        }


        public MapObject(MapPoint pBasePoint, MapPoint[] pArea, string strObjectName , eObjectType eType)
        {
            BasePoint = pBasePoint;
            Area = pArea;
            strName = strObjectName;
            ObjectSpacificProperties = new Dictionary<string, string>();
            this.Type = eType;
        }

        public MapObject(MapPoint pBasePoint, MapPoint[] pArea, string strObjectName, eObjectType eType, MapPoint[] pPathWays)
        {
            PathWays = pPathWays;
            BasePoint = pBasePoint;
            Area = pArea;
            strName = strObjectName;
            ObjectSpacificProperties = new Dictionary<string, string>();
            this.Type = eType;
        }

        public MapObject(MapPoint pBasePoint, MapPoint[] pArea, string strObjectName, eObjectType eType, MapPoint[] pPathWays , int iObjectValue)
        {
            PathWays = pPathWays;
            BasePoint = pBasePoint;
            Area = pArea;
            strName = strObjectName;
            ObjectSpacificProperties = new Dictionary<string, string>();
            this.Type = eType;
            iValue = iObjectValue;
        }

        public MapObject(MapPoint pBasePoint, MapPoint[] pArea, string strObjectName, eObjectType eType, MapPoint[] pPathWays, int iObjectValue , MapPoint mpAccessPoint)
        {
            PathWays = pPathWays;
            BasePoint = pBasePoint;
            Area = pArea;
            strName = strObjectName;
            ObjectSpacificProperties = new Dictionary<string, string>();
            this.Type = eType;
            iValue = iObjectValue;
            AccessPoint = mpAccessPoint;
        }

        public MapObject(MapPoint pBasePoint, MapPoint[] pArea, string strObjectName, eObjectType eType, MapPoint[] pPathWays, int iObjectValue, MapPoint mpAccessPoint, string strRotation, bool bShouldGuarded)
        {
            PathWays = pPathWays;
            BasePoint = pBasePoint;
            Area = pArea;
            strName = strObjectName;
            ObjectSpacificProperties = new Dictionary<string, string>();
            this.Type = eType;
            iValue = iObjectValue;
            AccessPoint = mpAccessPoint;
            Rotation = strRotation;
            bShouldBeGuarded = bShouldGuarded;

        }

        public MapObject(MapPoint pBasePoint, MapPoint[] pArea, string strObjectName, eObjectType eType, MapPoint[] pPathWays, int iObjectValue, MapPoint mpAccessPoint, string strRotation ,ObjectDirection objDir , bool bShouldGuarded)
        {
            PathWays = pPathWays;
            BasePoint = pBasePoint;
            Area = pArea;
            strName = strObjectName;
            ObjectSpacificProperties = new Dictionary<string, string>();
            this.Type = eType;
            iValue = iObjectValue;
            AccessPoint = mpAccessPoint;
            Rotation = strRotation;
            this.objdirDirection = objDir;
            bShouldBeGuarded = bShouldGuarded;
        }

        //dwp добавлена вероятность
        public MapObject(MapPoint pBasePoint, MapPoint[] pArea, string strObjectName, eObjectType eType, MapPoint[] pPathWays, int iObjectValue, MapPoint mpAccessPoint, string strRotation, ObjectDirection objDir, bool bShouldGuarded, int max_number, double probability)
        {
            PathWays = pPathWays;
            BasePoint = pBasePoint;
            Area = pArea;
            strName = strObjectName;
            ObjectSpacificProperties = new Dictionary<string, string>();
            this.Type = eType;
            iValue = iObjectValue;
            AccessPoint = mpAccessPoint;
            Rotation = strRotation;
            this.objdirDirection = objDir;
            bShouldBeGuarded = bShouldGuarded;
            max_number_ = max_number;
            //dwp инициализация вероятности и текущего количества.
            probability_ = probability;
            current_number = 0;
        }

        #region IComparable Members

        public int CompareTo(object obj)
        {
            // dwp! для сотрировки обьектов в порядке возрастания вероятности потом размера 
            // return BasePoint.y -((MapObject)obj).BasePoint.y ; //старый вариант.
            return probability_ != ((MapObject)obj).probability_ ?(int)((probability_ - ((MapObject)obj).probability_) * 1000) : ObjectSize - ((MapObject)obj).ObjectSize;
        }

        #endregion

        internal MapObject Clone()
        {
            return new MapObject(this.BasePoint, 
                                 this.Area, 
                                 this.strName, 
                                 this.Type, 
                                 this.PathWays, 
                                 this.iValue, 
                                 this.AccessPoint,
                                 this.Rotation,
                                 this.objdirDirection, 
                                 bShouldBeGuarded);
        }
    }
}
