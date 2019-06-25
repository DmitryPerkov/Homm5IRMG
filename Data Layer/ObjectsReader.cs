using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Collections;
using Homm5RMG.BL;
using System.Reflection;

namespace Homm5RMG
{
    #region Object Read Enums

    public enum eObjectCategory
    {
        Block
    }
    public enum eObjectType
    {
        Barrier,
        Monolith_Two_Way ,
        Guard ,
        General ,
        GuardedTreasure ,
        Town , 
        Treasure ,
        Artifacts ,
        Mine ,
        Block ,
        Complex ,
        HighDwelling ,
        LowDwelling,
        Garrison,
        Forpost /*,
        GuardWithoutGrow*/
    }

    enum eObjectData
    {
        Chance,
        Value,
        Area,
        MaxNumber

    }
    #endregion


    /// <summary>
    /// this class handles all reading and gui editing of objects
    /// </summary>
    class ObjectsReader
    {
        private string OBJECTSSOURCEFILENAME = Assembly.GetExecutingAssembly().CodeBase.Substring(0,Assembly.GetExecutingAssembly().CodeBase.Length - 12).Substring(8) + "Data/Objects.xml";
        public XmlDocument xdocObjects;
        public XmlElement xObjects;

        public  ObjectsReader()
        {
            xdocObjects = new XmlDocument();
            xdocObjects.Load(OBJECTSSOURCEFILENAME);
           // SetObjectsChanceAndValue(this.GetObjectsData());
        }


        public void SetObjectsPower(XmlElement xObjectsRoot)
        {
            XmlNodeList xndlstObjects = xObjectsRoot["BattleObjects"].SelectNodes(".//Object");

            foreach (XmlNode xndObject in xndlstObjects)
            {
                 ((XmlElement)xndObject).SetAttribute("Chance", "1.0");
                ((XmlElement)xndObject).SetAttribute("Value", "1000");
            }

            xdocObjects.Save(OBJECTSSOURCEFILENAME);
        }

        private void SetObjectsChanceAndValue(XmlElement xObjectsRoot)
        {
            XmlNodeList xndlstObjects = xObjectsRoot.SelectNodes( ".//Object" );

            foreach (XmlNode xndObject in xndlstObjects)
            {
               // ((XmlElement)xndObject).SetAttribute("Chance", "1.0");
                ((XmlElement)xndObject).SetAttribute("Value", "1000");
            }
        }

        public ObjectsReader(XmlElement xTemplateObjects)
        {
            xObjects = xTemplateObjects;
            
        }

        public ObjectsReader(string strFilePath)
        {
            strFilePath = Assembly.GetExecutingAssembly().CodeBase.Substring(0,Assembly.GetExecutingAssembly().CodeBase.Length - 12).Substring(8) + "Data/" + strFilePath;
            OBJECTSSOURCEFILENAME = strFilePath;
            xdocObjects = new XmlDocument();
            xdocObjects.Load( strFilePath);
           // SetObjectsChanceAndValue(this.GetObjectsData());

        }

        /// <summary>
        /// get objects node
        /// </summary>
        /// <returns></returns>
        public XmlElement GetObjectsData()
        {
            if (xdocObjects != null)
                return xdocObjects.DocumentElement;
            else
                return xObjects;
        }

        /// <summary>
        /// A little helping method to fix items will be put inside object tags 
        /// </summary>
        public void FixToObjects()
        {
          //  XmlDocument xdocObjects = new XmlDocument();
         //   xdocObjects.Load(OBJECTSSOURCEFILENAME);

            foreach (XmlNode xndGroups in xdocObjects.DocumentElement)
            {
                if (xndGroups.Name == eTerrain.Conquest.ToString())
                {
                    for (int i = 0; i < xndGroups.ChildNodes.Count; i++)
                    {
                        XmlNode xndItem = xndGroups.ChildNodes[i];
                        if (xndItem.Name == "Item")
                        {
                            XmlElement xelObject = xdocObjects.CreateElement("Object");
                            //xelObject.SetAttribute( "Name" ,xndItem.
                            xndGroups.ReplaceChild((XmlNode)xelObject, xndItem);
                            XmlNode xndObject = xelObject.AppendChild(xndItem);

                        }
                    }
                }
            }
            xdocObjects.Save(OBJECTSSOURCEFILENAME);
        }

        /// <summary>
        /// A little helping method to add size
        /// </summary>
        public void AddSizeObjects()
        {
            const int SIZE = 6; 
            XmlDocument xdocObjects = new XmlDocument();
            xdocObjects.Load(OBJECTSSOURCEFILENAME);

            XmlDocument xdocSizeObjects = new XmlDocument();
            xdocSizeObjects.Load("SizeObjects.xml");

            foreach (XmlNode xndItem in xdocSizeObjects.DocumentElement)
            {
             //   for (int i = 0; i < xndGroups.ChildNodes.Count; i++)
               // {
                 //   XmlNode xndItem = xndGroups.ChildNodes[i];
                    XmlNode xndItemInObjectsFile = xdocObjects.SelectSingleNode("//Item[@id='" + xndItem.Attributes["id"].Value + "']");
                //XmlNode xndItemInObjectsFile = xdocObjects.SelectNodes("//Item[@id=" + xndItem.Attributes["id"].Value + "]")[0];

                    ((XmlElement)xndItemInObjectsFile.ParentNode).SetAttribute("Size", SIZE.ToString());
                    //if (xndItem.Name == "Item")
                    //{
                    //    XmlElement xelObject = xdocObjects.CreateElement("Object");
                    //    //xelObject.SetAttribute( "Name" ,xndItem.
                    //    xndGroups.ReplaceChild((XmlNode)xelObject, xndItem);
                    //    XmlNode xndObject = xelObject.AppendChild(xndItem);

                    //}
               // }
            }
            xdocObjects.Save(OBJECTSSOURCEFILENAME);
        }


        public void AddArea()
        {


            string strArea = "";
            XmlDocument xdocObjects = new XmlDocument();
            xdocObjects.Load(OBJECTSSOURCEFILENAME);


            foreach (XmlNode xndGroups in xdocObjects.DocumentElement)
            {
                for (int i = 0; i < xndGroups.ChildNodes.Count; i++)
                {
                    XmlNode xndItem = xndGroups.ChildNodes[i];
                    ((XmlElement)xndItem).SetAttribute("Area", strArea);
                    //if (xndItem.A == "Item")
                    //{
                    //}
                }
            }
            xdocObjects.Save(OBJECTSSOURCEFILENAME);
        }


        public void AddType(eObjectType eType)
        {

            //XmlDocument xdocObjects = new XmlDocument();
            //xdocObjects.Load(OBJECTSSOURCEFILENAME);


            foreach (XmlNode xndGroups in xdocObjects.DocumentElement)
            {
                for (int i = 0; i < xndGroups.ChildNodes.Count; i++)
                {
                    if (xndGroups.Name == eTerrain.Conquest.ToString())
                    {
                        XmlNode xndItem = xndGroups.ChildNodes[i];
                        ((XmlElement)xndItem).SetAttribute("Type", eType.ToString());
                        //if (xndItem.A == "Item")
                        //{
                        //}
                    }
                }
            }
            xdocObjects.Save(OBJECTSSOURCEFILENAME);
        }

        /// <summary>
        /// A little helping method to fix name propertys
        /// </summary>
        public void AddNamesToObjects()
        {
            //XmlDocument xdocObjects = new XmlDocument();
            //xdocObjects.Load(OBJECTSSOURCEFILENAME);


            foreach (XmlNode xndGroups in xdocObjects.DocumentElement)
            {
                if (xndGroups.Name == eTerrain.Conquest.ToString())
                {
                    for (int i = 0; i < xndGroups.ChildNodes.Count; i++)
                    {
                        XmlNode xndItem = xndGroups.ChildNodes[i];
                        XmlNode xndPath = ((XmlElement)xndItem).GetElementsByTagName("Shared")[0];
                        string strName = xndPath.Attributes["href"].Value;
                        strName = strName.Substring("/MapObjects/".Length, strName.IndexOf('.') - "/MapObjects/".Length);

                        if (strName.IndexOf('/') > 0)
                        {
                            strName = strName.Split('/')[strName.Split('/').Length - 1];
                        }
                        ((XmlElement)xndItem).SetAttribute("Name", strName);


                        //if (xndItem.A == "Item")
                        //{
                        //}
                    }
                }
            }
            xdocObjects.Save(OBJECTSSOURCEFILENAME);



        }

        /// <summary>
        /// retrieves a object from object list by name
        /// </summary>
        /// <returns>the object</returns>
        public MapObject GetObjectByName(string strName)
        {
            XmlNode xndMapObject = this.GetObjectsData().SelectSingleNode("//Object[@Name='" + strName + "']");

            return (ConvertXMLNodeToMapObject(xndMapObject));

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="strName"></param>
        /// <returns></returns>
        public MapObject GetRandomObjectByType(eObjectType objType)
        {
            XmlNodeList xndMapObjects = this.GetObjectsData().SelectNodes(".//Object[@Type='" + objType.ToString() + "']");

            int iRandomIndex = Randomizer.rnd.Next(xndMapObjects.Count);

            return (ConvertXMLNodeToMapObject(xndMapObjects[iRandomIndex]));

            //return null; 

        }



        public MapObject GetRandomObject()
        {
            //draw a random object set 
            //#if (DEBUG)
            //     Random Randomizer.rnd = new Random(1);
            //#else
                // Random Randomizer.rnd = new Random();   
            //#endif
          
            //get all objects  ,  get a random index and pick a random object
            XmlNodeList xndlstAllSetObjects = this.GetObjectsData().SelectNodes(".//Object");
            XmlNode xndRandomObject;
            double dObjectAppearChance = 0;

            do
            {
                int iRandomObjectIndex = Randomizer.rnd.Next(xndlstAllSetObjects.Count);
                xndRandomObject = xndlstAllSetObjects[iRandomObjectIndex];
                dObjectAppearChance = Convert.ToDouble(xndRandomObject.Attributes[eObjectData.Chance.ToString()].Value, System.Globalization.CultureInfo.InvariantCulture);

            }while ( dObjectAppearChance <= Randomizer.rnd.NextDouble() );

            return (ConvertXMLNodeToMapObject(xndRandomObject));


        }

        public MapObject GetRandomObjectByCategory(eTerrain eSelectedCategory)
        {
            ////draw a random object set 
            //#if (DEBUG)
            //     Random Randomizer.rnd = new Random(1);
            //#else
            //Random Randomizer.rnd = new Random();
            //#endif

            //get all objects  ,  get a random index and pick a random object
            XmlNodeList xndlstAllSetObjects = this.GetObjectsData().SelectSingleNode(".//" + eSelectedCategory.ToString()).SelectNodes(".//Object");
            XmlNode xndRandomObject;

            int iRandomObjectIndex = Randomizer.rnd.Next(xndlstAllSetObjects.Count);
            xndRandomObject = xndlstAllSetObjects[iRandomObjectIndex];

            return (ConvertXMLNodeToMapObject(xndRandomObject));


        }


        /// <summary>
        /// returns all objects in a required category
        /// </summary>
        /// <param name="eSelectedCategory"></param>
        /// <returns></returns>
        public ArrayList GetAllObjectsByCategory(eTerrain eSelectedCategory)
        {
            ArrayList arrAllCategoryObjects = new ArrayList();
            ////draw a random object set 
            //#if (DEBUG)
            //     Random Randomizer.rnd = new Random(1);
            //#else
            //Random Randomizer.rnd = new Random();
            //#endif

            //get all objects  ,  get a random index and pick a random object
            XmlNodeList xndlstAllSetObjects = this.GetObjectsData().SelectSingleNode(".//" + eSelectedCategory.ToString()).SelectNodes(".//Object");
            //XmlNode xndRandomObject;


            foreach (XmlNode xndCategoryObject in xndlstAllSetObjects)
            {
                arrAllCategoryObjects.Add(ConvertXMLNodeToMapObject(xndCategoryObject));

            }


            return (arrAllCategoryObjects);

        }



        public MapObject GetRandomObjectByCategory(string strCategory)
        {
            ////draw a random object set 
            //#if (DEBUG)
            //     Random Randomizer.rnd = new Random(1);
            //#else
            //Random Randomizer.rnd = new Random();
            //#endif

            //get all objects  ,  get a random index and pick a random object
            XmlNodeList xndlstAllSetObjects = this.GetObjectsData().SelectSingleNode(".//" + strCategory).SelectNodes(".//Object");
            XmlNode xndRandomObject;

            int iRandomObjectIndex = Randomizer.rnd.Next(xndlstAllSetObjects.Count);
            xndRandomObject = xndlstAllSetObjects[iRandomObjectIndex];

            return (ConvertXMLNodeToMapObject(xndRandomObject));


        }


        /// <summary>
        /// converts an xml node to a map object
        /// pathways was an old idea that currently is not needed
        /// </summary>
        /// <param name="xndMapObject">the xml to convert</param>
        /// <returns>object as a map object</returns>
        public MapObject ConvertXMLNodeToMapObject(XmlNode xndMapObject)
        {
            eObjectType ObjectType = eObjectType.General;
            try
            {
                ObjectType = (eObjectType) Enum.Parse(typeof (eObjectType ), xndMapObject.Attributes["Type"].Value );
            }
            catch (Exception)
            {
            }


            string strRotation = ObjectsWriter.strDirections[(int)ObjectDirection.Down];
            try
            {
                strRotation = xndMapObject.Attributes["InitialRotation"].Value;
            }
            catch (Exception)
            {
            }


            bool bShouldBeGuarded = true;
            try
            {
                bShouldBeGuarded = bool.Parse( xndMapObject.Attributes["ShouldBeGuarded"].Value);
            }
            catch (Exception)
            {
            }

            MapPoint mpDummy = new MapPoint(-1, -1);
            MapPoint[] mpArea;
            ArrayList arrArea;

            string strAreaValues;
            string[] strPointValues;
            //dwp добавлена вероятность
            string strValue, strMaxNumber, strProbability;

            try
            {
                strValue = int.Parse(xndMapObject.Attributes["Value"].Value).ToString();
            }
            catch (Exception)
            {
                strValue = "0";
            }

            try
            {
                strMaxNumber = int.Parse(xndMapObject.Attributes["MaxNumber"].Value).ToString();
            }
            catch (Exception)
            {
                strMaxNumber = "0";
            }

            //dwp добавлена вероятность
            try
            {
                strProbability = double.Parse(xndMapObject.Attributes["Chance"].Value).ToString();
            }
            catch (Exception)
            {
                strProbability = "0";
            }

            try
            {
                strAreaValues = xndMapObject.Attributes["Area"].Value;
                strPointValues = strAreaValues.Split(';');
                arrArea = new ArrayList();
            }
            catch (Exception)
            {
               // mpArea = null;
                strPointValues = null;
                arrArea = null;
            }

            if (strPointValues != null) {
                //int iAreaIndex = 0;

                foreach (string strPoint in strPointValues) {
                    string[] strarrPoint = strPoint.Split(',');

                    arrArea.Add(new MapPoint(Convert.ToInt32(strarrPoint[0]), Convert.ToInt32(strarrPoint[1])));
                }
            }

            MapPoint[] mpPathWays;
    
            //convert from arraylist to array
            if (arrArea != null)
            {
                mpArea = new MapPoint[arrArea.Count];

                for (int i = 0; i < arrArea.Count; i++)
                {
                    mpArea[i] = (MapPoint)arrArea[i];
                }
            }
            else
                mpArea = null;


            //convert from arraylist to array
            //if (arraylistPathWays.Count > 0)
            //{
            //    mpPathWays = new MapPoint[arraylistPathWays.Count];

            //    for (int i = 0; i < arraylistPathWays.Count; i++)
            //    {
            //        mpPathWays[i] = (MapPoint)arraylistPathWays[i];
            //    }

            //}
            //else
            //    if (ObjectType != eObjectType.Barrier )
            //        mpPathWays = mpDirections;
            //    else
                    mpPathWays = null;


            string strAccessPoint=  string.Empty;
            try
            {
                strAccessPoint = xndMapObject.Attributes["AccessPoint"].Value;
                
            }
            catch
            {
            }

            MapPoint mpAccessPoint = null;

            if (strAccessPoint != string.Empty) {
                mpAccessPoint = new MapPoint(int.Parse(strAccessPoint.Split(',')[0]), int.Parse(strAccessPoint.Split(',')[1]));
            }
            else
                mpAccessPoint = new MapPoint(0, -1);

            //dwp добавлена вероятность
            return (new MapObject(mpDummy, mpArea, xndMapObject.Attributes["Name"].Value, ObjectType, mpPathWays, int.Parse(strValue), mpAccessPoint, strRotation, ObjectDirection.Down, bShouldBeGuarded, int.Parse(strMaxNumber), double.Parse(strProbability)));
          
        }

        /// <summary>
        /// update an object data based on gui data
        /// </summary>
        /// <param name="tmoObjectUpdateData">relevant object data</param>
        public void UpdateObject(TemplateMapObject tmoObjectUpdateData)
        {
            XmlNode xndObjectToUpdate = this.GetObjectsData().SelectSingleNode("//Object[@Name='" + tmoObjectUpdateData.Name + "']");

            if (xndObjectToUpdate != null)
            {
                ((XmlElement)xndObjectToUpdate).SetAttribute(eObjectData.Chance.ToString(), tmoObjectUpdateData.Chance.ToString());
                ((XmlElement)xndObjectToUpdate).SetAttribute(eObjectData.Value.ToString(), tmoObjectUpdateData.Value.ToString());
                ((XmlElement)xndObjectToUpdate).SetAttribute(eObjectData.MaxNumber.ToString(), tmoObjectUpdateData.MaxNumber.ToString());
            }
        }

        /// <summary>
        /// update an object data based on gui data
        /// </summary>
        /// <param name="tmoObjectUpdateData">relevant object data</param>
        public void UpdateObjectWithZeroChance(TemplateMapObject tmoObjectUpdateData)
        {
            XmlNode xndObjectToUpdate = this.GetObjectsData().SelectSingleNode("//Object[@Name='" + tmoObjectUpdateData.Name + "']");

            if (xndObjectToUpdate != null)
            {
                ((XmlElement)xndObjectToUpdate).SetAttribute(eObjectData.Chance.ToString(), "0.0");
                ((XmlElement)xndObjectToUpdate).SetAttribute(eObjectData.Value.ToString(), tmoObjectUpdateData.Value.ToString());
                ((XmlElement)xndObjectToUpdate).SetAttribute(eObjectData.MaxNumber.ToString(), "");
            }
        } 

        //internal MapObject GetRandomObject(int iZoneIndex)
        //{
  

        //    //get all objects  ,  get a random index and pick a random object
        //    XmlNodeList xndlstAllSetObjects = xObjectSets[iRandomObjectSetIndex].SelectNodes(".//Object");
        //    int iRandomObjectIndex = Randomizer.rnd.Next(xndlstAllSetObjects.Count);

        //    XmlNode xndRandomObject = xndlstAllSetObjects[iRandomObjectIndex];
        //}




    }
}
