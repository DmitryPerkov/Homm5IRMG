using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Text.RegularExpressions;
using Homm5RMG.BL;
using Homm5RMG.Properties;
using System.Collections;

namespace Homm5RMG
{
    #region Template Enums
    enum eFactions
    {

    }

    enum eMines
    {
        Sawmill = 0,
        Ore_Pit,
        Gold_Mine,
        Sulfur_Dune,        
        Crystal_Cavern,
        Gem_Pond,
        Alchemist_Lab ,
        Abandoned_Mine ,
        Player_Related
    }

    enum eResources
    {
        Wood = 0,
        Ore,
        Gold,
        Sulfur,
        Crystal,
        Gems,
        Mercury
    }
    enum eTown
    {     
        Type = 0,     
        BlackSmith,
        Tavern,
        MageGuildLevel,
        IsStartingTown,
        TownSpecialStracture,
        TownWalls,
        IncomeLevel,
        DwellingLevel,
        DwellingsUpgrades,
        ResourceLevel
    }

    public enum eTownType
    {
        None = 0,
        Random,
        Haven,
        Sylvan,
        Dungeon,
        Inferno,
        Fortress,
        Orcs,
        Academy,
        Necropolis        
    }

    enum eTownXMLType
    {
        RandomTown = 1,
        Heaven ,
        Preserve,
        Dungeon,
        Inferno,
        Fortress,
        Orc_Stronghold,
        Academy,
        Necromancy
    }

    enum eTownXMLArmyHREFType
    {
        Random = 1,
        Haven,
        Preserve,
        Dungeon,
        Inferno,
        Dwarf,
        Stronghold,
        Academy,
        Necropolis,
        Neutral
    }

    enum eTownSpecXMLHREFType
    {
        Random = 1,
        Haven,
        Preserve,
        Dungeon,
        Inferno,
        Fortress,
        Stronghold,
        Academy,
        Necropolis,
        Neutral
    }

    enum eWalls
    {
        None = 0 ,
        Fort ,
        Citadel ,
        Castle 
    }

    enum eIncome
    {
        VillageHall = 0,
        TownHall,
        CityHall,
        Capitol
    }

    enum eResource
    {
        None = 0,
        MarketPlace ,
        ResourceSile
    }

    enum eMonsterStrength
    {
        Weak = -1 ,        
        Avarage = 0 ,
        Strong = 1 ,
        Scary  = 2, 
        Impossible = 3
    }

    enum eTerrain
    {
        Grass = 0,
        Dirt ,
        Sand ,
        Lava ,
        Snow ,
        Subterrain,
        Orcish ,
        Conquest ,
        Native

    }

    enum eTemplateElements
    {
        Mines = 0 ,
        Towns ,
        MonsterStrength,
        TerrainType ,
        IsStartingTown ,
        SizePrecentage ,
        ObjectsSet ,
        Guards ,
        Zone ,
        Connects_To,
        IsStartingZone,
        Thickness

    }
    #endregion


    /// <summary>
    /// this class deals with saving a template vs the gui for it
    /// </summary>
    public class TemplateHandler
    {
        #region class Verbs
        public XmlDocument xdocTemplateFile;
        public string strFileName;
        private int iZoneCount;
        private XmlElement xZones;
        private XmlElement xConnections;
        private int iConnectionCount;
        public string[] strarrPlayerIds;
        public int[] iarrSelectedPlayersFactions;
        public int[] SaveiarrSelectedPlayersFactions;
        public int[][] iarrExcludedPlayersFactions;
        public int[] iarrTerrainTable;
        public bool[] iarrLinkToStartingZone;
        public int[] iarrCountLinkedStartingZone;
        public XmlElement[] xSelectedObjectSets;        
        #endregion

        #region Class Constants
        public static readonly int IMINENUMBER = 9;        
        private const string STRZONESNUMBER = "Number";
        //private const string[3] TOWN = { "Type", "BlackSmith", "Tavern" };
        private const string TRUE = "True";
        private const string FALSE = "False";
        #endregion


        public TemplateHandler()
        {
            //init vlaue will be changed if template exist
            iZoneCount = 0;
            iConnectionCount = 0;
            //strFileName = strFilenameAndPath;
            xdocTemplateFile = new XmlDocument();

            string strFilenameAndPath = "Temp.irt";
            //if file is not found, create a new xml file
            XmlTextWriter xmlWriter = new XmlTextWriter(strFilenameAndPath, System.Text.Encoding.UTF8);
            xmlWriter.Formatting = Formatting.Indented;
            xmlWriter.WriteProcessingInstruction("xml", "version='1.0' encoding='UTF-8'");
            xmlWriter.WriteStartElement("IRMG");

            #region example info - remove later
            //If WriteProcessingInstruction is used as above,
            //Do not use WriteEndElement() here
            //xmlWriter.WriteEndElement();
            //it will cause the <Root></Root> to be <Root />
            #endregion
            //create the basic elements in the template file
            xmlWriter.Close();
            xdocTemplateFile.Load(strFilenameAndPath);

            XmlNode root = xdocTemplateFile.DocumentElement;
            xConnections = xdocTemplateFile.CreateElement("Connections");
            xZones = xdocTemplateFile.CreateElement("Zones");
            //XmlText textNode = xdocTemplateFile.CreateTextNode("hello");
            //textNode.Value = "hello, world";
            root.AppendChild(xConnections);
            root.AppendChild(xZones);
            xZones.SetAttribute(STRZONESNUMBER, "0");
            xConnections.SetAttribute(STRZONESNUMBER, "0");
            //childNode2.AppendChild(textNode);
            this.AddNewZone();
            this.AddNewConnection();
        }


        /// <summary>
        /// ctor - initilize all elements in template
        /// checks if file exist ,if not create it from scratch
        /// </summary>
        /// <param name="strFilenameAndPath">name and path of template file</param>
        public TemplateHandler(string strFilenameAndPath)
        {
            //init vlaue will be changed if template exist
            iZoneCount = 0;
            iConnectionCount = 0;
            strFileName = strFilenameAndPath;
            xdocTemplateFile = new XmlDocument();
            try
            {
                //if file does not exist and io exception will be thrown
                xdocTemplateFile.Load(strFilenameAndPath);
                xZones =(XmlElement) xdocTemplateFile.GetElementsByTagName("Zones")[0];
                xConnections = (XmlElement)xdocTemplateFile.GetElementsByTagName("Connections")[0];
                iZoneCount = Convert.ToInt32( xZones.Attributes[STRZONESNUMBER].Value );
                iConnectionCount = Convert.ToInt32(xConnections.Attributes[STRZONESNUMBER].Value); 
            }
            catch(System.IO.FileNotFoundException)
            {
                //if file is not found, create a new xml file
                XmlTextWriter xmlWriter = new XmlTextWriter(strFilenameAndPath, System.Text.Encoding.UTF8);
                xmlWriter.Formatting = Formatting.Indented;
                xmlWriter.WriteProcessingInstruction("xml", "version='1.0' encoding='UTF-8'");
                xmlWriter.WriteStartElement("IRMG");

                #region example info - remove later
                //If WriteProcessingInstruction is used as above,
                //Do not use WriteEndElement() here
                //xmlWriter.WriteEndElement();
                //it will cause the <Root></Root> to be <Root />
                #endregion
                //create the basic elements in the template file
                xmlWriter.Close();
                xdocTemplateFile.Load(strFilenameAndPath);

                XmlNode root = xdocTemplateFile.DocumentElement;
                xConnections = xdocTemplateFile.CreateElement("Connections");
                xZones = xdocTemplateFile.CreateElement("Zones");
                //XmlText textNode = xdocTemplateFile.CreateTextNode("hello");
                //textNode.Value = "hello, world";
                root.AppendChild(xConnections);
                root.AppendChild(xZones);
                xZones.SetAttribute(STRZONESNUMBER, "0");
                xConnections.SetAttribute(STRZONESNUMBER, "0");
                //childNode2.AppendChild(textNode);
                this.AddNewZone();
                this.AddNewConnection();
                //textNode.Value = "replacing hello world";
                //xdocTemplateFile.Save(strFilenameAndPath);
            }

            catch (XmlException)
            {
                
                //if file is not found, create a new xml file
                XmlTextWriter xmlWriter = new XmlTextWriter(strFilenameAndPath, System.Text.Encoding.UTF8);
                xmlWriter.Formatting = Formatting.Indented;
                xmlWriter.WriteProcessingInstruction("xml", "version='1.0' encoding='UTF-8'");
                xmlWriter.WriteStartElement("IRMG");
                //If WriteProcessingInstruction is used as above,
                //Do not use WriteEndElement() here
                //xmlWriter.WriteEndElement();
                //it will cause the <Root></Root> to be <Root />
                xmlWriter.Close();
                xdocTemplateFile.Load(strFilenameAndPath);

                XmlNode root = xdocTemplateFile.DocumentElement;
                xConnections = xdocTemplateFile.CreateElement("Connections");
                xZones = xdocTemplateFile.CreateElement("Zones");
                //XmlText textNode = xdocTemplateFile.CreateTextNode("hello");
                //textNode.Value = "hello, world"; idan is stupid!

                root.AppendChild(xConnections);
                root.AppendChild(xZones);
                xZones.SetAttribute(STRZONESNUMBER, "0");
                xConnections.SetAttribute(STRZONESNUMBER, "0");
               // childNode2.AppendChild(textNode);
                this.AddNewZone();
                this.AddNewConnection();
                //textNode.Value = "replacing hello world";
                xdocTemplateFile.Save(strFilenameAndPath);
            }

            

        }

        //returns number of zones in the template
        public int ZoneNumber
        {
            get
            {
                return Convert.ToInt32(xZones.Attributes[STRZONESNUMBER].Value);

            }
            set
            {

            }
        }

        //returns number of Connections in the template
        public int ConnectionNumber
        {
            get
            {
                return Convert.ToInt32(xConnections.Attributes[STRZONESNUMBER].Value);

            }
            set
            {
                
            }
        }

        /// <summary>
        /// saves template to set filename
        /// </summary>
        public void SaveTemplateToFile()
        {
            xdocTemplateFile.Save(strFileName);
        }

        /// <summary>
        /// saves template to a spacific filename and change current filename
        /// </summary>
        /// <param name="strFilenameAndPath">filename</param>
        public void SaveTemplateToFile ( string strFilenameAndPath )
        {
            strFileName = strFilenameAndPath;
            xdocTemplateFile.Save(strFilenameAndPath);
        }



        /// <summary>
        /// adds all elements in a new zone (with default values)
        /// </summary>
        public void AddNewZone()
        {

            iZoneCount++;
            XmlElement xZone = xdocTemplateFile.CreateElement("Zone" + iZoneCount.ToString());
            xZones.AppendChild(xZone);
            xZones.SetAttribute(STRZONESNUMBER, iZoneCount.ToString());

            //add all elements with their default values
            #region Add Mines Element Inside Zone
            XmlElement xMines = xdocTemplateFile.CreateElement("Mines");
            SetMinesProperty(new int[IMINENUMBER], xMines);            
            xZone.AppendChild(xMines);
            XmlElement xMinesData = xdocTemplateFile.CreateElement("MinesData");
            string[] strarrMineValues = new string[IMINENUMBER] ;
            for (int i = 0; i < IMINENUMBER; i++)
            {
                strarrMineValues[i] = "1000";
            }
            SetMinesProperty(strarrMineValues, xMinesData, "Value");

            string[] strarrMineChance = new string[IMINENUMBER];
            for (int i = 0; i < IMINENUMBER; i++)
            {
                strarrMineChance[i] = "1.0";
            }
            SetMinesProperty(strarrMineChance, xMinesData, "Chance");

            //set special attribute of player related mine
            xMinesData.SetAttribute(eMines.Player_Related + "ID", "1");
            xZone.AppendChild(xMinesData);
            #endregion

            #region Add Towns Element Inside Zone
            XmlElement xTowns = xdocTemplateFile.CreateElement("Towns");
            XmlElement xTown1 = CreateTown(1);
            XmlElement xTown2 = CreateTown(2);
            XmlElement xTown3 = CreateTown(3);
            xTowns.AppendChild(xTown1);
            xTowns.AppendChild(xTown2);
            xTowns.AppendChild(xTown3);
            xZone.AppendChild(xTowns);
            #endregion

            #region Add Monster Strength Element Inside Zone
            XmlElement xMonsterStrength = xdocTemplateFile.CreateElement(eTemplateElements.MonsterStrength.ToString());
            xMonsterStrength.InnerXml = eMonsterStrength.Avarage.ToString();
            xZone.AppendChild(xMonsterStrength);
            #endregion

            #region Add Terrain Element Inside Zone
            XmlElement xTerrain = xdocTemplateFile.CreateElement(eTemplateElements.TerrainType.ToString());
            xTerrain.InnerXml = eTerrain.Grass.ToString();
            xZone.AppendChild(xTerrain);
            #endregion

            #region Add Starting Zone Element Inside Zone
            XmlElement xStartingZone = xdocTemplateFile.CreateElement(eTemplateElements.IsStartingZone.ToString());
            xStartingZone.InnerXml = FALSE;
            xZone.AppendChild(xStartingZone);
            #endregion

            #region Add Size Precentage Element Inside Zone
            XmlElement xSizePrecentage = xdocTemplateFile.CreateElement(eTemplateElements.SizePrecentage.ToString());
            XmlElement xThickness = xdocTemplateFile.CreateElement(eTemplateElements.Thickness.ToString());
            xSizePrecentage.InnerXml = "25%";
            xThickness.InnerText = "7";
            xZone.AppendChild(xSizePrecentage);
            xZone.AppendChild(xThickness);
            #endregion

            #region Add Objects set 1 Element Inside Zone
            ObjectsReader obrdObjectSet = new ObjectsReader();
            

            XmlElement xObjectSet1 = xdocTemplateFile.CreateElement(eTemplateElements.ObjectsSet.ToString() + "1");
            xObjectSet1.SetAttribute("Appear_Chance", "0.2");
            xObjectSet1.AppendChild( this.xdocTemplateFile.ImportNode( obrdObjectSet.GetObjectsData() ,true ) );
            xZone.AppendChild(xObjectSet1);
            
            #endregion

            #region Add Objects set 2 Element Inside Zone
            //ObjectsReader obrdObjectSet = new ObjectsReader();


            XmlElement xObjectSet2 = xdocTemplateFile.CreateElement(eTemplateElements.ObjectsSet.ToString() + "2");
            xObjectSet2.SetAttribute("Appear_Chance", "0.4");
            xObjectSet2.AppendChild(this.xdocTemplateFile.ImportNode(obrdObjectSet.GetObjectsData(), true));
            xZone.AppendChild(xObjectSet2);

            #endregion

            #region Add Objects set 32 Element Inside Zone
            //ObjectsReader obrdObjectSet = new ObjectsReader();


            XmlElement xObjectSet3 = xdocTemplateFile.CreateElement(eTemplateElements.ObjectsSet.ToString() + "3");
            xObjectSet3.SetAttribute("Appear_Chance", "0.4");
            xObjectSet3.AppendChild(this.xdocTemplateFile.ImportNode(obrdObjectSet.GetObjectsData(), true));
            xZone.AppendChild(xObjectSet3);

            #endregion
        }


        /// <summary>
        /// Adds a new connection
        /// </summary>
        public void AddNewConnection()
        {
           // xConnections = (XmlElement)xdocTemplateFile.GetElementsByTagName("Connections")[0];
            iConnectionCount++;
            xConnections.SetAttribute(STRZONESNUMBER, iConnectionCount.ToString());

            XmlElement xConnection = xdocTemplateFile.CreateElement("Connection" + iConnectionCount.ToString());

            xConnection.SetAttribute("Zone", "1");
            xConnection.SetAttribute("Connects_To", "2");
            xConnections.AppendChild(xConnection);


            #region Handle Connection Guards
            //create guards element
            XmlElement xGuards = xdocTemplateFile.CreateElement("Guards");

            xGuards.SetAttribute("Value", "10000");
            xGuards.SetAttribute("Custom", "False");

            //add default guard


            xConnection.AppendChild(xGuards);
            

            #endregion
        }

        /// <summary>
        /// Update an existing connection
        /// </summary>
        public void AddUpdateConnection(int iConnectionIndex)
        {
            XmlElement xConnections = (XmlElement)xdocTemplateFile.GetElementsByTagName("Connections")[0];

            XmlElement xConnection = xdocTemplateFile.CreateElement("Connection");
        }

        /// <summary>
        /// Replaces the objects node to the template in the right object set place and zone
        /// </summary>
        /// <param name="xndObjectsList">objects node</param>
        /// <param name="iZoneIndex">index of zone</param>
        /// <param name="strObjectSetNumber">number of the object set</param>
        public void UpdateObjectSetProperty(XmlNode xndObjectsList, int iZoneIndex, int iObjectSetNumber)
        {
            XmlNode xndObjectSet = xZones.ChildNodes[iZoneIndex].SelectSingleNode("./ObjectsSet" + iObjectSetNumber);
            xndObjectSet.RemoveChild(xndObjectSet.FirstChild);
            xndObjectSet.AppendChild(xdocTemplateFile.ImportNode(xndObjectsList, true)); 
        
        }

        /// <summary>
        /// creates a default town based on index
        /// </summary>
        /// <param name="iTownIndex">Index of town</param>
        /// <returns>node created</returns>
        XmlElement CreateTown(int iTownIndex)
        {
            XmlElement xTown = xdocTemplateFile.CreateElement("Town" + iTownIndex.ToString());
            xTown.SetAttribute(eTown.Type.ToString(), eTownType.None.ToString());
            xTown.SetAttribute(eTown.BlackSmith.ToString(), TRUE);
            xTown.SetAttribute(eTown.Tavern.ToString(), TRUE);
            xTown.SetAttribute(eTown.IsStartingTown.ToString(), FALSE);
            xTown.SetAttribute(eTown.MageGuildLevel.ToString(), "1");
            xTown.SetAttribute(eTown.TownSpecialStracture.ToString(), FALSE);
            xTown.SetAttribute(eTown.TownWalls.ToString(), eWalls.None.ToString());
            xTown.SetAttribute(eTown.IncomeLevel.ToString(), eIncome.TownHall.ToString());
            xTown.SetAttribute(eTown.DwellingLevel.ToString(), "2");
            xTown.SetAttribute(eTown.DwellingsUpgrades.ToString(), TRUE);
            xTown.SetAttribute(eTown.ResourceLevel.ToString(), eResource.MarketPlace.ToString());

            return xTown;

        }


        /// <summary>
        /// Set all template attributes for a town in spacific zone
        /// </summary>
        /// <param name="iZoneIndex">index for zone</param>
        /// <param name="iTownNumber">Number of town (103)</param>
        /// <param name="strTownAttribute">The attribute to update</param>
        /// <param name="strValue">The attribute value</param>
        public void SetTownsAttributes(int iZoneIndex, int iTownNumber, string strTownAttribute, string strValue)
        {
            XmlElement xTown = (XmlElement) xZones.ChildNodes[iZoneIndex].SelectSingleNode(".//Town" + iTownNumber);
            xTown.SetAttribute(strTownAttribute, strValue);
            //((XmlElement)xndZoneProperty).GetElementsByTagName(strZoneProperty);
        }
        //dwp связь герерации города и/или шахты с выбором сета обьектов
        public void SetLinkAttribute(int iZoneIndex, string LinkedPage, string strValue)
        {
            XmlElement xLink = (XmlElement)xZones.ChildNodes[iZoneIndex].SelectSingleNode(".//" + LinkedPage);
            xLink.SetAttribute("LinkWithObjectSet", strValue);
        }

        public string GetLinkAttribute(int iZoneIndex, string LinkedPage)
        {
            XmlElement xLink = (XmlElement)xZones.ChildNodes[iZoneIndex].SelectSingleNode(".//" + LinkedPage);
            string res;
            if (xLink != null) res = xLink.GetAttribute("LinkWithObjectSet");
            else res = "false";
            if (res == "") return "false";
            else return res;
        }

        /// <summary>
        /// Get a Town Attribute based on its name 
        /// </summary>
        /// <param name="iZoneIndex">zone index number</param>
        /// <param name="iTownNumber">1-3 towns  -its number</param>
        /// <param name="strTownAttribute"></param>
        /// <returns></returns>
        public string GetTownsAttributes(int iZoneIndex, int iTownNumber, string strTownAttribute)
        {
            XmlElement xTown = (XmlElement)xZones.ChildNodes[iZoneIndex].SelectSingleNode(".//Town" + iTownNumber.ToString());
            //xTown.SetAttribute(strTownAttribute, strValue);
            return xTown.GetAttribute(strTownAttribute);
            //((XmlElement)xndZoneProperty).GetElementsByTagName(strZoneProperty);
        }


        /// <summary>
        /// sets the mines data
        /// </summary>
        /// <param name="iMines">array of mine values based on order in enum</param>
        /// <param name="xMines">the element of mines</param>
        public void SetMinesProperty (int[] iMines , XmlElement xMines)
        {
            xMines.SetAttribute(eMines.Sawmill.ToString(), iMines[(int)eMines.Sawmill].ToString());
            xMines.SetAttribute(eMines.Ore_Pit.ToString(), iMines[(int)eMines.Ore_Pit].ToString());
            xMines.SetAttribute(eMines.Gold_Mine.ToString(), iMines[(int)eMines.Gold_Mine].ToString());
            xMines.SetAttribute(eMines.Sulfur_Dune.ToString(), iMines[(int)eMines.Sulfur_Dune].ToString());
            xMines.SetAttribute(eMines.Crystal_Cavern.ToString(), iMines[(int)eMines.Crystal_Cavern].ToString());
            xMines.SetAttribute(eMines.Gem_Pond.ToString(), iMines[(int)eMines.Gem_Pond].ToString());
            xMines.SetAttribute(eMines.Alchemist_Lab.ToString(), iMines[(int)eMines.Alchemist_Lab].ToString());
            xMines.SetAttribute(eMines.Abandoned_Mine.ToString() , iMines[(int)eMines.Abandoned_Mine].ToString());
            xMines.SetAttribute(eMines.Player_Related.ToString(), iMines[(int)eMines.Player_Related].ToString());
                        
        }

        /// <summary>
        /// sets the mines data
        /// </summary>
        /// <param name="iMines">array of mine values based on order in enum</param>
        /// <param name="xMines">the element of mines</param>
        public void SetMinesProperty(object[] iMinesData, XmlElement xMines ,string strSubCategory)
        {
            xMines.SetAttribute(eMines.Sawmill.ToString() +  strSubCategory, iMinesData[(int)eMines.Sawmill].ToString());
            xMines.SetAttribute(eMines.Ore_Pit.ToString() + strSubCategory, iMinesData[(int)eMines.Ore_Pit].ToString());
            xMines.SetAttribute(eMines.Gold_Mine.ToString() + strSubCategory, iMinesData[(int)eMines.Gold_Mine].ToString());
            xMines.SetAttribute(eMines.Sulfur_Dune.ToString() + strSubCategory, iMinesData[(int)eMines.Sulfur_Dune].ToString());
            xMines.SetAttribute(eMines.Crystal_Cavern.ToString() + strSubCategory, iMinesData[(int)eMines.Crystal_Cavern].ToString());
            xMines.SetAttribute(eMines.Gem_Pond.ToString() + strSubCategory, iMinesData[(int)eMines.Gem_Pond].ToString());
            xMines.SetAttribute(eMines.Alchemist_Lab.ToString() + strSubCategory, iMinesData[(int)eMines.Alchemist_Lab].ToString());
            xMines.SetAttribute(eMines.Abandoned_Mine.ToString() + strSubCategory, iMinesData[(int)eMines.Abandoned_Mine].ToString());
            xMines.SetAttribute(eMines.Player_Related.ToString() + strSubCategory, iMinesData[(int)eMines.Player_Related].ToString());

        }

        /// <summary>
        /// updates a zone property
        /// </summary>
        /// <param name="iZoneIndex">the zone name</param>
        /// <param name="strZoneProperty">the propery to update</param>
        /// <param name="strValue">property value</param>
        public void UpdateZoneProperty(int iZoneIndex, string strZoneProperty, string strValue)
        {
            try
            {
                XmlNode xndZoneProperty = xZones.ChildNodes[iZoneIndex].SelectSingleNode("./" + strZoneProperty);
                xndZoneProperty.InnerText = strValue;
            }
            catch
            {
                XmlElement xName = xdocTemplateFile.CreateElement(strZoneProperty);
                xName.InnerText = strValue;
                xZones.ChildNodes[iZoneIndex].AppendChild(xName);
            }
        }


        /// <summary>
        /// updates a zone Attribute
        /// </summary>
        /// <param name="iZoneIndex">the zone name</param>
        /// <param name="strZoneProperty">the Property to update</param>
        /// <param name="strValue">property value</param>
        public void UpdateZoneAttribute(int iZoneIndex, string strZoneProperty, string strValue , string strAttributeName)
        {
            XmlNode xndZoneProperty = xZones.ChildNodes[iZoneIndex].SelectSingleNode("./" + strZoneProperty);
            ((XmlElement)xndZoneProperty).SetAttribute (strAttributeName ,strValue );
            //((XmlElement)xndZoneProperty).GetElementsByTagName(strZoneProperty);
        }

        /// <summary>
        /// get a property from the template
        /// </summary>
        /// <param name="iZoneIndex">the zone index</param>
        /// <param name="strZoneProperty">which property to get</param>
        /// <returns>its value</returns>
        public string getZoneProperty(int iZoneIndex, string strZoneProperty)
        {
            try
            {
                XmlNode xndZoneProperty = xZones.ChildNodes[iZoneIndex].SelectSingleNode("./" + strZoneProperty);
                return (xndZoneProperty.InnerText);
            }
            catch
            {
                //dwp. это возврат толщины зоны по умолчанию равен 7. может отсутсвовать в старых шаблонах
                return ("7");
            }
            //((XmlElement)xndZoneProperty).GetElementsByTagName(strZoneProperty);
        }

        /// <summary>
        /// get a Connection property from the template
        /// </summary>
        /// <param name="iConnectionIndex">the Connection index</param>
        /// <param name="strConnectionProperty">which property to get</param>
        /// <returns>its value</returns>
        public string getConnectionProperty(int iConnectionIndex, string strConnectionProperty)
        {
            return (xConnections.ChildNodes[iConnectionIndex].Attributes[strConnectionProperty].Value);

        }

        /// <summary>
        ///// get a Connection property from the template
        ///// </summary>
        ///// <param name="iConnectionIndex">the Connection index</param>
        ///// <param name="strConnectionProperty">which property to get</param>
        ///// <returns>its value</returns>
        //public string getConnectionGuardValue(int iConnectionIndex)
        //{
        //    return (xConnections.ChildNodes[iConnectionIndex].FirstChild.Attributes["Value"].Value);

        //}
        /// <summary>
        /// removes Last zone from tmeplate
        /// </summary>
        public void DeleteZone()
        {
            xZones.RemoveChild(xZones.LastChild);
            iZoneCount--;
            xZones.SetAttribute(STRZONESNUMBER, iZoneCount.ToString());

        }

        /// <summary>
        /// removes Last Connection from tmeplate
        /// </summary>
        public void DeleteConnection()
        {
            xConnections.RemoveChild(xConnections.LastChild);
            iConnectionCount--;
            xConnections.SetAttribute(STRZONESNUMBER, iConnectionCount.ToString());

        }


        /// <summary>
        /// Saves Mines data in template based on zone number
        /// </summary>
        /// <param name="iZoneIndex">zone number</param>
        /// <param name="iMinesData">array of mines data</param>
        public void SetMinesData(int iZoneIndex, int[] iMinesData)
        {
            XmlElement xMinesAtIndex = (XmlElement) xZones.ChildNodes[iZoneIndex].SelectSingleNode("./Mines");
            xMinesAtIndex.SetAttribute(eMines.Sawmill.ToString(), iMinesData[(int)eMines.Sawmill].ToString());
            xMinesAtIndex.SetAttribute(eMines.Ore_Pit.ToString(), iMinesData[(int)eMines.Ore_Pit].ToString());
            xMinesAtIndex.SetAttribute(eMines.Gold_Mine.ToString(), iMinesData[(int)eMines.Gold_Mine].ToString());
            xMinesAtIndex.SetAttribute(eMines.Sulfur_Dune.ToString(), iMinesData[(int)eMines.Sulfur_Dune].ToString());
            xMinesAtIndex.SetAttribute(eMines.Gem_Pond.ToString(), iMinesData[(int)eMines.Gem_Pond].ToString());
            xMinesAtIndex.SetAttribute(eMines.Crystal_Cavern.ToString(), iMinesData[(int)eMines.Crystal_Cavern].ToString());
            xMinesAtIndex.SetAttribute(eMines.Alchemist_Lab.ToString(), iMinesData[(int)eMines.Alchemist_Lab].ToString());
            xMinesAtIndex.SetAttribute(eMines.Abandoned_Mine.ToString(), iMinesData[(int)eMines.Abandoned_Mine].ToString());
            xMinesAtIndex.SetAttribute(eMines.Player_Related.ToString(), iMinesData[(int)eMines.Player_Related].ToString());

            
        }


        /// <summary>
        /// Saves Mines data in template based on zone number
        /// </summary>
        /// <param name="iZoneIndex">zone number</param>
        /// <param name="iMinesData">array of mines data</param>
        public void SetMinesAdditionalData(int iZoneIndex, int[] iMinesData , string strAdditionalKind)
        {
            XmlElement xMinesAtIndex = (XmlElement)xZones.ChildNodes[iZoneIndex].SelectSingleNode("./MinesData");
            xMinesAtIndex.SetAttribute(eMines.Sawmill.ToString() + strAdditionalKind, iMinesData[(int)eMines.Sawmill].ToString());
            xMinesAtIndex.SetAttribute(eMines.Ore_Pit.ToString() + strAdditionalKind, iMinesData[(int)eMines.Ore_Pit].ToString());
            xMinesAtIndex.SetAttribute(eMines.Gold_Mine.ToString() + strAdditionalKind, iMinesData[(int)eMines.Gold_Mine].ToString());
            xMinesAtIndex.SetAttribute(eMines.Sulfur_Dune.ToString() + strAdditionalKind, iMinesData[(int)eMines.Sulfur_Dune].ToString());
            xMinesAtIndex.SetAttribute(eMines.Gem_Pond.ToString() + strAdditionalKind, iMinesData[(int)eMines.Gem_Pond].ToString());
            xMinesAtIndex.SetAttribute(eMines.Crystal_Cavern.ToString() + strAdditionalKind, iMinesData[(int)eMines.Crystal_Cavern].ToString());
            xMinesAtIndex.SetAttribute(eMines.Alchemist_Lab.ToString() + strAdditionalKind, iMinesData[(int)eMines.Alchemist_Lab].ToString());
            xMinesAtIndex.SetAttribute(eMines.Abandoned_Mine.ToString() + strAdditionalKind, iMinesData[(int)eMines.Abandoned_Mine].ToString());
            xMinesAtIndex.SetAttribute(eMines.Player_Related.ToString() + strAdditionalKind, iMinesData[(int)eMines.Player_Related].ToString());
        }

        public void SetMinesAdditionalData(int iZoneIndex, string[] strMinesData, string strAdditionalKind)
        {
            XmlElement xMinesAtIndex = (XmlElement)xZones.ChildNodes[iZoneIndex].SelectSingleNode("./MinesData");
            xMinesAtIndex.SetAttribute(eMines.Sawmill.ToString() + strAdditionalKind, strMinesData[(int)eMines.Sawmill]);
            xMinesAtIndex.SetAttribute(eMines.Ore_Pit.ToString() + strAdditionalKind, strMinesData[(int)eMines.Ore_Pit]);
            xMinesAtIndex.SetAttribute(eMines.Gold_Mine.ToString() + strAdditionalKind, strMinesData[(int)eMines.Gold_Mine]);
            xMinesAtIndex.SetAttribute(eMines.Sulfur_Dune.ToString() + strAdditionalKind, strMinesData[(int)eMines.Sulfur_Dune]);
            xMinesAtIndex.SetAttribute(eMines.Gem_Pond.ToString() + strAdditionalKind, strMinesData[(int)eMines.Gem_Pond]);
            xMinesAtIndex.SetAttribute(eMines.Crystal_Cavern.ToString() + strAdditionalKind, strMinesData[(int)eMines.Crystal_Cavern]);
            xMinesAtIndex.SetAttribute(eMines.Alchemist_Lab.ToString() + strAdditionalKind, strMinesData[(int)eMines.Alchemist_Lab]);
            xMinesAtIndex.SetAttribute(eMines.Abandoned_Mine.ToString() + strAdditionalKind, strMinesData[(int)eMines.Abandoned_Mine]);
            xMinesAtIndex.SetAttribute(eMines.Player_Related.ToString() + strAdditionalKind, strMinesData[(int)eMines.Player_Related]);
        }

        /// <summary>
        /// Saves Mines data in template based on zone number
        /// </summary>
        public void SetMinesPlayerIdData(int iZoneIndex, int iValue)
        {
            XmlElement xMinesAtIndex = (XmlElement)xZones.ChildNodes[iZoneIndex].SelectSingleNode("./MinesData");
            xMinesAtIndex.SetAttribute( eMines.Player_Related+"ID" , iValue.ToString() );
        }

        /// <summary>
        /// Saves Mines data in template based on zone number
        /// </summary>
        /// <param name="iZoneIndex">zone number</param>
        /// <param name="iMinesData">array of mines data</param>
        public string GetMinesPlayerIdData(int iZoneIndex)
        {
            return xZones.ChildNodes[iZoneIndex].SelectSingleNode("./MinesData").Attributes[eMines.Player_Related + "ID"].Value;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="iZoneIndex"></param>
        /// <param name="strSetNumber"></param>
        /// <param name="tmoObjectToAdd"></param>
        private void AddObjectToZoneAndObjectSet(int iZoneIndex, string strSetNumber, TemplateMapObject tmoObjectToAdd)
        {
            ObjectsReader obReadObject = new ObjectsReader();
            XmlNode xndObjectSet = xZones.ChildNodes[iZoneIndex].SelectSingleNode(".//" + eTemplateElements.ObjectsSet.ToString() + strSetNumber); 
            //obReadObject.GetObjectsData.
            
        }

        public int[] GetZonePrecentageList()
        {
            int[] PrecentageList = new int[this.iZoneCount];

            for (int i = 0; i < this.iZoneCount; i++)
            {
                PrecentageList[i] = int.Parse( getZoneProperty(i, eTemplateElements.SizePrecentage.ToString()).Trim('%'));
            }
            return PrecentageList;
        }

        public int[] GetZoneBordersThickness()
        {
            int[] ThicknessList = new int[this.iZoneCount];

            for (int i = 0; i < this.iZoneCount; i++)
            {
                ThicknessList[i] = int.Parse(getZoneProperty(i, eTemplateElements.Thickness.ToString()));
            }
            return ThicknessList;
        }


        /// <summary>
        /// Gets Data Of Mines
        /// </summary>
        /// <param name="iZoneIndex"></param>
        public int[] GetMinesData(int iZoneIndex)
        {
            //get mines node at current index
            XmlNode xndMinesAtIndex = xZones.ChildNodes[iZoneIndex].SelectSingleNode("./Mines");
            int[] iarrMines = new int[IMINENUMBER];
            #region Set Mines values
            iarrMines[(int)eMines.Sawmill] = Convert.ToInt32( xndMinesAtIndex.Attributes.GetNamedItem(eMines.Sawmill.ToString()).Value);
            iarrMines[(int)eMines.Ore_Pit] = Convert.ToInt32(xndMinesAtIndex.Attributes.GetNamedItem(eMines.Ore_Pit.ToString()).Value);
            iarrMines[(int)eMines.Gold_Mine] = Convert.ToInt32(xndMinesAtIndex.Attributes.GetNamedItem(eMines.Gold_Mine.ToString()).Value);
            iarrMines[(int)eMines.Sulfur_Dune] = Convert.ToInt32(xndMinesAtIndex.Attributes.GetNamedItem(eMines.Sulfur_Dune.ToString()).Value);
            iarrMines[(int)eMines.Gem_Pond] = Convert.ToInt32(xndMinesAtIndex.Attributes.GetNamedItem(eMines.Gem_Pond.ToString()).Value);
            iarrMines[(int)eMines.Crystal_Cavern] = Convert.ToInt32(xndMinesAtIndex.Attributes.GetNamedItem(eMines.Crystal_Cavern.ToString()).Value);
            iarrMines[(int)eMines.Alchemist_Lab] = Convert.ToInt32(xndMinesAtIndex.Attributes.GetNamedItem(eMines.Alchemist_Lab.ToString()).Value);
            iarrMines[(int)eMines.Abandoned_Mine] = Convert.ToInt32(xndMinesAtIndex.Attributes.GetNamedItem(eMines.Abandoned_Mine.ToString()).Value);
            iarrMines[(int)eMines.Player_Related] = Convert.ToInt32(xndMinesAtIndex.Attributes.GetNamedItem(eMines.Player_Related.ToString()).Value);   
            #endregion

            return iarrMines;
        }

        /// <summary>
        /// Gets Data Of Mines
        /// </summary>
        /// <param name="iZoneIndex"></param>
        public string[] GetAdditionalMinesData(int iZoneIndex, string strAdditionalKind)
        {
            //get mines node at current index
            XmlNode xndMinesAtIndex = xZones.ChildNodes[iZoneIndex].SelectSingleNode("./MinesData");
            string[] strarrMines = new string[IMINENUMBER];
            #region Set Mines values
            strarrMines[(int)eMines.Sawmill] = xndMinesAtIndex.Attributes.GetNamedItem(eMines.Sawmill.ToString() + strAdditionalKind).Value;
            strarrMines[(int)eMines.Ore_Pit] = xndMinesAtIndex.Attributes.GetNamedItem(eMines.Ore_Pit.ToString() + strAdditionalKind).Value;
            strarrMines[(int)eMines.Gold_Mine] = xndMinesAtIndex.Attributes.GetNamedItem(eMines.Gold_Mine.ToString() + strAdditionalKind).Value;
            strarrMines[(int)eMines.Sulfur_Dune] = xndMinesAtIndex.Attributes.GetNamedItem(eMines.Sulfur_Dune.ToString() + strAdditionalKind).Value;
            strarrMines[(int)eMines.Gem_Pond] = xndMinesAtIndex.Attributes.GetNamedItem(eMines.Gem_Pond.ToString() + strAdditionalKind).Value;
            strarrMines[(int)eMines.Crystal_Cavern] = xndMinesAtIndex.Attributes.GetNamedItem(eMines.Crystal_Cavern.ToString() + strAdditionalKind).Value;
            strarrMines[(int)eMines.Alchemist_Lab] = xndMinesAtIndex.Attributes.GetNamedItem(eMines.Alchemist_Lab.ToString() + strAdditionalKind).Value;
            strarrMines[(int)eMines.Abandoned_Mine] = xndMinesAtIndex.Attributes.GetNamedItem(eMines.Abandoned_Mine.ToString() + strAdditionalKind).Value;
            strarrMines[(int)eMines.Abandoned_Mine] = xndMinesAtIndex.Attributes.GetNamedItem(eMines.Abandoned_Mine.ToString() + strAdditionalKind).Value;
            strarrMines[(int)eMines.Player_Related] = xndMinesAtIndex.Attributes.GetNamedItem(eMines.Player_Related.ToString() + strAdditionalKind).Value;
            #endregion

            return strarrMines;
        }



        /// <summary>
        /// translate the connection to a list of int in first row is first zone and in 2nd the second
        /// </summary>
        /// <returns>list of connection by zone index number</returns>
        public int[,] GetConnectionList()
        {
                                //rows,//connection count
            int[,] iarrConnectionList = new int[2, ConnectionNumber];
            int iCount = 0;
            foreach (XmlNode xndItem in xConnections)
            {
                //string test = xndItem.Attributes.GetNamedItem("Connects_To").Value;
                //string test2 = xndItem.Attributes.GetNamedItem("Zone").Value;
               iarrConnectionList[0, iCount] = Convert.ToInt32(xndItem.Attributes.GetNamedItem("Zone").Value);
               iarrConnectionList[1, iCount] = Convert.ToInt32(xndItem.Attributes.GetNamedItem("Connects_To").Value);
               iCount++;

            }
            return iarrConnectionList;
        }



        /// <summary>
        /// import guard xml for a connection point (import is needed since xml is generated from a different xml (guards.xml ) 
        /// </summary>
        /// <param name="xGuardXML"></param>
        /// <param name="iConnectionIndex"></param>
        public void ImportGuardToConnection(XmlElement xGuardXML, int iConnectionIndex)
        {

            XmlNode xndConnectionNode = xConnections.ChildNodes[iConnectionIndex];

            xndConnectionNode.RemoveChild(xndConnectionNode.FirstChild);
            xndConnectionNode.AppendChild(xdocTemplateFile.ImportNode(xGuardXML, true));
           // xndConnectionNode.ReplaceChild(xndConnectionNode.FirstChild, xdocTemplateFile.ImportNode(xGuardXML , true));
                
        }

      

        /// <summary>
        /// Gets the index zone for the starting zone index 
        /// </summary>
        /// <param name="iIndex">Which starting zone index to bring (first second ect )</param>
        /// <returns>starting zone index </returns>
        public int getStartingZoneIndex(int iIndex)
        {
            int iCountStartingZones =0 ;

            foreach (XmlNode xndZone in xZones.ChildNodes)
            {
                //if its a starting zone
                if (xndZone.SelectSingleNode(".//" + eTemplateElements.IsStartingZone.ToString()).InnerText == "True")
                {
                    iCountStartingZones++;

                    if (iIndex == iCountStartingZones)
                    {
                        return Convert.ToInt32 ( xndZone.Name.Substring(xndZone.Name.Length - 1) );
                    }
                }

                
                    
            }


            return -1;
        }



        /// <summary>
        /// returns an object set for a zone and object set number
        /// </summary>
        /// <param name="iZoneIndex">origine zone for object set</param>
        /// <param name="iObjectSet">object set number 1-3</param>
        /// <returns>element of object set</returns>
        internal XmlElement GetObjectsSet(int iZoneIndex, int iObjectSet)
        {
             return (XmlElement) xZones.ChildNodes[iZoneIndex].SelectSingleNode("./ObjectsSet" + iObjectSet.ToString());  
        }



        
        /// <summary>
        /// Updates a Connection Attribute
        /// </summary>
        /// <param name="iConnectionNumber">connection num</param>
        /// <param name="strAttributeName">the attribute to set</param>
        /// <param name="strValue">value to be set</param>
        internal void UpdateConnectionAttribute(int iConnectionNumber, string strAttributeName, string strValue)
        {
            ((XmlElement)xConnections.ChildNodes[iConnectionNumber]).SetAttribute(strAttributeName, strValue);
        }

        #region Zones Generation Section

        /// <summary>
        /// order is determined to maximise the chance for connected zones to be phisically adjacet
        /// </summary>
        /// <returns>array of zone indicies to determine the order in which to evaluate zones , Trying to put connected zones adjacent to each other</returns>
        internal int[] GenerateEvaluationOrderList()
        {
            int[] iarrEvaluationOrderList = new int[this.ZoneNumber];
            int[,] iarrConnectionList = GetConnectionList();

            for (int j = 0; j < IPLAYERNUMBER; j++)
            {
                iarrEvaluationOrderList[j] = getStartingZoneIndex(j + 1);
            }

            int iNextZone1 = iarrEvaluationOrderList[IPLAYERNUMBER - 2];
            int iNextZone2 = iarrEvaluationOrderList[IPLAYERNUMBER - 1];
            int i = IPLAYERNUMBER;

            do
            {
                iNextZone1 = GetNextUnassaignedZone(iarrConnectionList, iNextZone1, iarrEvaluationOrderList);
                if (iNextZone1 != -1)
                {
                    iarrEvaluationOrderList[i] = iNextZone1;
                    i++;
                }

                iNextZone2 = GetNextUnassaignedZone(iarrConnectionList, iNextZone2, iarrEvaluationOrderList);
                if (iNextZone2 != -1)
                {
                    iarrEvaluationOrderList[i] = iNextZone2;
                    i++;
                }
            } while (iNextZone1 != -1 || iNextZone2 != -1);


            for (int j = 0; j < this.ZoneNumber; j++)
            {
                if (!IsAssaigned(iarrEvaluationOrderList, j + 1))
                {
                    iarrEvaluationOrderList[i] = j + 1;
                    i++;
                }
            }

            return iarrEvaluationOrderList;

        }


        /// <summary>
        /// checks if zone index appears inside the order list array
        /// </summary>
        /// <param name="iarrEvaluationOrderList"></param>
        /// <param name="iQuestionedZone">The zone needs to be checked</param>
        /// <returns>if zone has been assaigned in order or not</returns>
        private bool IsAssaigned(int[] iarrEvaluationOrderList, int iQuestionedZone)
        {
            for (int i = 0; i < iarrEvaluationOrderList.Length; i++)
            {
                if (iarrEvaluationOrderList[i] == iQuestionedZone)
                    return true;

            }
            return false;
        }


        /// <summary>
        /// Get the next unassaigned zone by searching a zone connected to previuse zone 
        /// </summary>
        /// <param name="iarrConnectionList"></param>
        /// <param name="IPreviuseInOrder"></param>
        /// <returns></returns>/// 
        private int GetNextUnassaignedZone(int[,] iarrConnectionList, int IPreviuseInOrder, int[] iarrEvaluationOrderList)
        {
            for (int i = 0; i < iarrConnectionList.Length / 2; i++)
            {
                if (iarrConnectionList[0, i] == IPreviuseInOrder)
                    if (!this.IsAssaigned(iarrEvaluationOrderList, iarrConnectionList[1, i]))
                        return iarrConnectionList[1, i];
                if (iarrConnectionList[1, i] == IPreviuseInOrder)
                    if (!this.IsAssaigned(iarrEvaluationOrderList, iarrConnectionList[0, i]))
                        return iarrConnectionList[0, i];
            }
            return -1;
        }

        #endregion

        internal XmlNode GetConnectionGuardXML(int i)
        {
            //throw new Exception("The method or operation is not implemented.");
            return null;
        }

        internal XmlNode GetZoneXML(int iZoneIndex)
        {
            
            return xZones.ChildNodes[iZoneIndex-1];
        }

        /// <summary>
        /// returns value for spacific connection
        /// </summary>
        /// <param name="iConnectionIndex">connection index</param>
        /// <returns>returns value for spacific connection</returns>
        public string GetConnectionGuardValue(int iConnectionIndex)
        {
            return  xConnections.ChildNodes[iConnectionIndex]["Guards"].Attributes["Value"].Value;
        }

        public const int IOBJECTSETS = 3;

        internal MapObject GetRandomObjectInZone(int iZoneIndex)
        {
            //get all object sets for current zone
            XmlElement[] xObjectSets = new XmlElement[IOBJECTSETS];

            for (int i = 0; i < IOBJECTSETS; i++)
            {
                xObjectSets[i] = GetObjectsSet(iZoneIndex, i+1);
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
            ObjectsReader obrdReader = new ObjectsReader(xObjectSets[iRandomObjectSetIndex]);
            return obrdReader.GetRandomObject();
        }

//dwp выбор сета, если он не был выбран при генерации городов или шахт
//построения списка всех возможных обьектов в выбранном сете.

        internal System.Collections.ArrayList GetALLObjectInZone(int iZoneIndex)
        {
            System.Collections.ArrayList arrALLObjectInZone = new System.Collections.ArrayList();
            ObjectsReader obrdReader;

            if (xSelectedObjectSets[iZoneIndex] == null)
            {
                //get all object sets for current zone
                XmlElement[] xObjectSets = new XmlElement[IOBJECTSETS];
                for (int i = 0; i < IOBJECTSETS; i++)
                {
                    xObjectSets[i] = GetObjectsSet(iZoneIndex, i + 1);
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
                obrdReader = new ObjectsReader(xObjectSets[iRandomObjectSetIndex]);
            }
            else obrdReader = new ObjectsReader(xSelectedObjectSets[iZoneIndex]);

            XmlNodeList xndlstAllSetObjects = obrdReader.GetObjectsData().SelectNodes(".//Object");
            double dObjectAppearChance = 0;
            int iMaxAmount = 0;
            string oName,oType;
            
            for(int i = 0; i < xndlstAllSetObjects.Count; i++ ) {
                //dwp. так как менанизм плотности отключен, с нулевым количеством обьекты не рассматриваются
                try
                {
                    dObjectAppearChance = Convert.ToDouble(xndlstAllSetObjects[i].Attributes[eObjectData.Chance.ToString()].Value, System.Globalization.CultureInfo.InvariantCulture);
                    iMaxAmount = int.Parse(xndlstAllSetObjects[i].Attributes["MaxNumber"].Value);
                    oName = xndlstAllSetObjects[i].Attributes["Name"].Value;
                }
                catch
                {
                    iMaxAmount = 0;
                    dObjectAppearChance = 0;
                    oName = "";
                }
                try
                {
                    oType = xndlstAllSetObjects[i].Attributes["Type"].Value;
                }
                catch
                {
                    oType = "";
                }

                if (dObjectAppearChance > 0 && iMaxAmount > 0 && oName != "Mines" && oName != "Towns" &&
                    (Settings.Default.NewBankGenerating || oType != "new"))
                    arrALLObjectInZone.Add(obrdReader.ConvertXMLNodeToMapObject(xndlstAllSetObjects[i]));
            }
            arrALLObjectInZone.Sort();
            arrALLObjectInZone.Reverse();
            return arrALLObjectInZone;
        }

        internal int GetRandomDwellingNumberInZone(int iZoneIndex)
        {
            //get all object sets for current zone
            XmlElement[] xObjectSets = new XmlElement[IOBJECTSETS];

            for (int i = 0; i < IOBJECTSETS; i++) {
                xObjectSets[i] = GetObjectsSet(iZoneIndex, i + 1);
            }

            double dRandomObjectSetPointer = Randomizer.rnd.NextDouble();
            double dSum = 0;
            int iRandomObjectSetIndex = -1;

            do {
                iRandomObjectSetIndex++;
                //add number until sum of appear chance reaches random number (up to 1.0) to generate a random effect
                dSum += Convert.ToDouble(xObjectSets[iRandomObjectSetIndex].Attributes["Appear_Chance"].Value, System.Globalization.CultureInfo.InvariantCulture);
            } while (dSum < dRandomObjectSetPointer);

            int dw_num = 0;
            try {
                string str = xObjectSets[iRandomObjectSetIndex].Attributes["Dwelling_Number"].Value;
                Regex r = new Regex(@"(?<min>\d{1})-(?<max>\d{1})");
                Match m = r.Match(str);
                if (m.Success) {
                    dw_num = Randomizer.rnd.Next(Convert.ToInt32(m.Result("${min}")),
                                                 Convert.ToInt32(m.Result("${max}")));
                    if (dw_num > 5) {
                        dw_num = 5; //that's enough
                    }
                }
            }
            catch (Exception) {
            }
            return dw_num;
        }

        internal string TemplateValidationCheck()
        {
            string strErrorString = string.Empty;
            string strInfo = string.Empty;
            
            try
            {
                //if no zones
                if (this.ZoneNumber <= 1)
                    strErrorString += "Numer of zones is less or equal to one" + Environment.NewLine;
                //if no connections
                if (this.ConnectionNumber <= 0)
                    strErrorString += "Numer of connections is less or equal to zero." + Environment.NewLine;

                int n = StartingZonesCount();
                //dwp. Проверяем наличие необходимого количества стартовых зон
                if (Program.frmMainMenu.RadioButton1.Checked) 
                { //dwp два игрока
                    IPLAYERNUMBER = 2;
                    if (n < 2)
                        strErrorString += "Insufficient number of starting zones, Must be 2 starting zones or more." + Environment.NewLine;
                }
                else if (Program.frmMainMenu.RadioButton2.Checked)
                { //dwp три игрока
                    IPLAYERNUMBER = 3;
                    if (n < 3)
                        strErrorString += "Insufficient number of starting zones, Must be 3 starting zones or more." + Environment.NewLine;
                }
                else {//dwp четыре игрока
                    IPLAYERNUMBER = 4;
                    if (n < 4)
                        strErrorString += "Insufficient number of starting zones, Must be 4 starting zones." + Environment.NewLine;
                }

                //check if both starting zones got atleast 1 starting town in each.
                if (!DoTownsExistInStartingZone())
                    strErrorString += "One or all of starting zones don't have towns, Towns are required for a player" + Environment.NewLine;

                //check zones size total precentage
                if (TotalSizePercentage() != 100)
                    strErrorString += "The total size percentage of zones is not equal to 100%" + Environment.NewLine;

                //check total chance for 3 sets in each of the zones
                for (int i = 0; i < ZoneNumber; i++)
                {
                    if (CheckObjectSetsTotalChance(i) != 1.0)
                    {
                        strErrorString += string.Format("The total object sets chances for zone number {0} does not equal 1.0 " + Environment.NewLine, i + 1);
                    }
                }

                if ( !CheckConnectionZones() )
                    strErrorString += "Connection definition is wrong , An unexisting zone appears in connection list - Current template range is 1 to " + iZoneCount.ToString() + Environment.NewLine;

                //check all zones are connected and there is a path that leads from first player zone to second
                if (!CheckConnections())
                    strErrorString += "Connection definition is wrong , Not all zones connected or no path from first starting zone to second" + Environment.NewLine;

                return strErrorString;
            }
            catch (Exception)
            {
                strErrorString += "A general template error occoured , Please recheck the template ";
                return strErrorString;
            }

        }

        private bool CheckConnectionZones()
        {
            int[,] iarrConnectionList = GetConnectionList();

            for (int i = 0; i < iConnectionCount; i++)
            {
                if (iarrConnectionList[0, i] <= 0 || iarrConnectionList[0, i] > iZoneCount || iarrConnectionList[1, i] <= 0 || iarrConnectionList[1, i] > iZoneCount)
                    return false;
            }

            return true;
        }


        /// <summary>
        /// check total chance of all object sets inside a zone
        /// </summary>
        /// <param name="iZoneIndex">/index of the zone to check</param>
        /// <returns>Total chance</returns>
        private double CheckObjectSetsTotalChance(int iZoneIndex)
        {
            double dTotalChance = 0;
            for (int i = 1; i < 4; i++)
            {

                XmlElement xObjectSet = GetObjectsSet(iZoneIndex, i);
                dTotalChance += Convert.ToDouble(xObjectSet.Attributes["Appear_Chance"].Value);//,System.Globalization.CultureInfo.InvariantCulture);
                //System.Windows.Forms.MessageBox.Show(xObjectSet.Attributes["Appear_Chance"].Value);
            }
            return dTotalChance;
        }

        private int TotalSizePercentage()
        {
            int iTotalPercentageSum = 0;
            for (int i = 0; i < ZoneNumber; i++)
            {
                iTotalPercentageSum += int.Parse(getZoneProperty(i, eTemplateElements.SizePrecentage.ToString()).Trim('%')); 
            }

            return iTotalPercentageSum;
        }

        private bool DoTownsExistInStartingZone()
        {
            //int iCountStartingZones = 0;
            foreach (XmlNode xndZone in xZones.ChildNodes)
            {
                //iCountStartingZones = 0;
                //if its a starting zone
                if (xndZone.SelectSingleNode(".//" + eTemplateElements.IsStartingZone.ToString()).InnerText == "True")
                {
                    int iTownCount = 0;
                    //CountNumber Of Towns
                    for (int i = 1; i < 4; i++)
                    {
                        XmlNode xndTowni=  xndZone.SelectSingleNode(".//Town" + i.ToString());
                        if (xndTowni.Attributes["Type"].Value != eTownType.None.ToString() && xndTowni.Attributes["IsStartingTown"].Value == "True" )
                        {
                            iTownCount++;
                        }

                    }
                    if ( iTownCount == 0 )
                        return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Count how many starting zones are there
        /// </summary>
        /// <returns></returns>
        public int StartingZonesCount()
        {
            int iCountStartingZones = 0;
            //int iZoneIndex=0;

            for (int i = 0; i < ZoneNumber; i++)
			{
                //if its a starting zone
                if ( getZoneProperty(i,eTemplateElements.IsStartingZone.ToString()) == "True" )
                {
                  iCountStartingZones++;
                }
            }
            return iCountStartingZones;
        }
        

        private bool CheckConnections()
        {

            //check that each zone has access to all other zones
            int[,] iarrConnectionList = GetConnectionList();

            for (int i = 1; i < iZoneCount+1; i++)
            {
                for (int j = 1; j < iZoneCount+1; j++)
                {
                    if (i != j)
                    {
                        bool[] bZoneWalk = new bool[iZoneCount];
                        //maybe need to set all array to false
                        if (!CheckZoneAccess(i, j, iarrConnectionList, bZoneWalk))
                        {
                            return false;            
                        }
                    }
                }                                    
            }
            return true;
        }

        private bool CheckZoneAccess(int iSource, int iDestination, int[,] iarrConnectionList ,bool[] bZoneWalk)
        {            
            //mark source as traveled 
            bZoneWalk[iSource-1] = true;
            //walk recursevly from one zone to another until reached and don't repeat same zone on the way,if other zone not reached then error
            //stop condition
            if (iSource == iDestination)
                return true;

            //find all directions for current zone
            for (int i = 0; i < iConnectionCount; i++)
            {
                if (iarrConnectionList[0, i] == iSource)
                {
                    if ( ! bZoneWalk[ iarrConnectionList[1,i]-1 ] )
                    {
                    //go check in this direction
                        if (CheckZoneAccess(iarrConnectionList[1, i], iDestination, iarrConnectionList, bZoneWalk))
                            return true;
                    }

                }
                if (iarrConnectionList[1, i] == iSource)
                {
                    if (!bZoneWalk[iarrConnectionList[0, i]-1])
                    {
                        //go check in this direction
                        if (CheckZoneAccess(iarrConnectionList[0, i], iDestination, iarrConnectionList, bZoneWalk))
                            return true;
                    }
                }
            }

            return false;

        }


        //todoImplementMethod with a random player id
        internal string getPlayerIdPerZone(int iZoneIndex)
        {
            return strarrPlayerIds[iZoneIndex];
        }

        //todoImplementMethod with a random player id
        internal void GenerateRandomIdPerZone()
        {
            //goes over all zones and if its a starting zone randomizes the id , all other zones gets player none
            strarrPlayerIds = new string[this.ZoneNumber];
            //set all as none by default
            for (int i = 0; i < strarrPlayerIds.Length; i++)
            {
                strarrPlayerIds[i] = "PLAYER_NONE";
            }
            //randomize result
            int iRandomInitialPlayer = Randomizer.rnd.Next(1, IPLAYERNUMBER + 1), currP = 0;

            //goes over all zones , if is starting then align it as random result
            for (int i = 0; i < strarrPlayerIds.Length; i++)
            {
                XmlNode xndIsStartingZone = xZones.ChildNodes[i].SelectSingleNode(".//" + eTemplateElements.IsStartingZone );
                if (bool.Parse(xndIsStartingZone.InnerText) && currP < IPLAYERNUMBER)
                {
                    strarrPlayerIds[i] = "PLAYER_" + iRandomInitialPlayer.ToString();
                    iRandomInitialPlayer = NextPlayerRotation(iRandomInitialPlayer);
                    currP++;
                }
                if (currP >= IPLAYERNUMBER) break;
            }
        }

        public int IPLAYERNUMBER = 2;

        private int NextPlayerRotation(int iPrevNumber)
        {
            iPrevNumber++;
            if (iPrevNumber > IPLAYERNUMBER)
                iPrevNumber = 1;
            return iPrevNumber;
        }

        private bool isEqualStartingZones(ArrayList L1,ArrayList L2)
        {
            if (L1.Count != L2.Count) {
                return false;
            }

            for (int i = 1; i < L1.Count; i++)
            {
                if (!L2.Contains(L1[i]))
                {
                    return false;
                }
            }
            return true;
        }

        internal eTerrain GetZoneTypeForZone(int iZoneIndex)
        {
            string strZoneType;
            //if its a neutral zone then for default always return grass
            if (iZoneIndex == -2)
                return eTerrain.Grass;
            strZoneType = getZoneProperty(iZoneIndex, eTemplateElements.TerrainType.ToString());

            return (eTerrain) Enum.Parse(typeof( eTerrain), strZoneType);
        }

        internal void CopyZone(int iSourceZoneIndex, int iDestination)
        {
            xZones.ChildNodes[iDestination].RemoveAll();
            foreach (XmlNode xndZoneChild in xZones.ChildNodes[iSourceZoneIndex])
            {
                this.xZones.ChildNodes[iDestination].AppendChild(xndZoneChild.Clone());
            }
        }

        /// <summary>
        /// returns the monster strength for a spacific zone
        /// </summary>
        /// <param name="iZoneIndex">zone index</param>
        /// <returns>returns the monster strength for a spacific zone</returns>
        internal string GetZoneStrength(int iZoneIndex)
        {
            XmlNode xndZoneProperty = xZones.ChildNodes[iZoneIndex].SelectSingleNode("./" + eTemplateElements.MonsterStrength.ToString());
            return (xndZoneProperty.InnerText);
        }

        /// <summary>
        /// вычислим зоны и соединения, которые соеденяют стратовые зоны
        /// </summary>
        internal void CalcLinkedZones() /* !!! */
        {

            iarrLinkToStartingZone = new bool[iConnectionCount];
            iarrCountLinkedStartingZone = new int[ZoneNumber]; // сохраняем количество линков до стартовых зон

            // запоминаем соединенные стартовые зоны
            ArrayList[] iarrSavedLinkedStartingZone  = new ArrayList[ZoneNumber]; 
            int[] iarrCountLinkedStartingZone2 = new int[ZoneNumber]; // для фиксации третьего прохода 
            int[,] iarrConnectionList = GetConnectionList();

                for (int i = 0; i < ZoneNumber; i++ )
                { iarrSavedLinkedStartingZone[i] = new ArrayList();
                }

                /* dwp взвешиваем соединения по мере удаления от стартовой зоны до глубины 2 */
                /* первый проход - глубина 0 */
                for (int i = 0; i < iConnectionCount; i++)
                {
                    if (getZoneProperty(iarrConnectionList[0, i] - 1, eTemplateElements.IsStartingZone.ToString()) == "True")
                    {
                        iarrLinkToStartingZone[i] = true;
                        if (!iarrSavedLinkedStartingZone[iarrConnectionList[1, i] - 1].Contains(iarrConnectionList[0, i] - 1))
                        {
                            iarrCountLinkedStartingZone[iarrConnectionList[1, i] - 1]++;
                            iarrSavedLinkedStartingZone[iarrConnectionList[1, i] - 1].Add(iarrConnectionList[0, i] - 1);
                        }
                    }
                    if (getZoneProperty(iarrConnectionList[1, i] - 1, eTemplateElements.IsStartingZone.ToString()) == "True")
                    {
                        iarrLinkToStartingZone[i] = true;
                        if (!iarrSavedLinkedStartingZone[iarrConnectionList[0, i] - 1].Contains(iarrConnectionList[1, i] - 1))
                        {
                            iarrCountLinkedStartingZone[iarrConnectionList[0, i] - 1]++;
                            iarrSavedLinkedStartingZone[iarrConnectionList[0, i] - 1].Add(iarrConnectionList[1, i] - 1);
                        }
                    }
                }

            /* второй проход - глубина 1 */
            for (int i = 0; i < iConnectionCount; i++)
            {
                if (!iarrLinkToStartingZone[i] &&
                     iarrCountLinkedStartingZone[iarrConnectionList[0, i] - 1] > 0 &&
                     iarrCountLinkedStartingZone[iarrConnectionList[1, i] - 1] > 0 &&
                     !isEqualStartingZones(iarrSavedLinkedStartingZone[iarrConnectionList[0, i] - 1],
                                            iarrSavedLinkedStartingZone[iarrConnectionList[1, i] - 1])
                     )
                {
                    if (iarrCountLinkedStartingZone[iarrConnectionList[0, i] - 1] >
                        iarrCountLinkedStartingZone[iarrConnectionList[1, i] - 1])
                    {
                       iarrCountLinkedStartingZone[iarrConnectionList[1, i] - 1]++;

                    }
                    else if (iarrCountLinkedStartingZone[iarrConnectionList[0, i] - 1] <
                             iarrCountLinkedStartingZone[iarrConnectionList[1, i] - 1])
                    {
                       iarrCountLinkedStartingZone[iarrConnectionList[0, i] - 1]++;
                    }
                    else
                    {
                       iarrCountLinkedStartingZone[iarrConnectionList[0, i] - 1]++;
                       iarrCountLinkedStartingZone[iarrConnectionList[1, i] - 1]++;
                    }
 
                    iarrLinkToStartingZone[i] = true;
                }
                    
            }
            /* третий проход ч.1 - глубина 2 - подсчет линков*/
            for (int i = 0; i < iConnectionCount; i++)
            {
                if (!iarrLinkToStartingZone[i] &&
                     iarrCountLinkedStartingZone[iarrConnectionList[0, i] - 1] == 0 &&
                     iarrCountLinkedStartingZone[iarrConnectionList[1, i] - 1] > 0
                    ) 
                {
                    for (int j = 0; j < iarrSavedLinkedStartingZone[iarrConnectionList[1, i] - 1].Count; j++)
                    {
                      if (!iarrSavedLinkedStartingZone[iarrConnectionList[0, i] - 1].Contains(iarrSavedLinkedStartingZone[iarrConnectionList[1, i] - 1][j]))
                          iarrSavedLinkedStartingZone[iarrConnectionList[0, i] - 1].Add(iarrSavedLinkedStartingZone[iarrConnectionList[1, i] - 1][j]);
                    }
                    iarrCountLinkedStartingZone2[iarrConnectionList[0, i] - 1] = iarrSavedLinkedStartingZone[iarrConnectionList[0, i] - 1].Count;
                }
                if (!iarrLinkToStartingZone[i] &&
                     iarrCountLinkedStartingZone[iarrConnectionList[0, i] - 1] > 0 &&
                     iarrCountLinkedStartingZone[iarrConnectionList[1, i] - 1] == 0
                    )
                {
                    for (int j = 0; j < iarrSavedLinkedStartingZone[iarrConnectionList[0, i] - 1].Count; j++)
                    {
                     if (!iarrSavedLinkedStartingZone[iarrConnectionList[1, i] - 1].Contains(iarrSavedLinkedStartingZone[iarrConnectionList[0, i] - 1][j]))       
                      iarrSavedLinkedStartingZone[iarrConnectionList[1, i] - 1].Add(iarrSavedLinkedStartingZone[iarrConnectionList[0, i] - 1][j]);
                    }
                    iarrCountLinkedStartingZone2[iarrConnectionList[1, i] - 1] = iarrSavedLinkedStartingZone[iarrConnectionList[1, i] - 1].Count;
                }

            }
            /* третий проход ч.2 - глубина 2 - фиксация линков */
            for (int i = 0; i < iConnectionCount; i++)
            {
                if (iarrCountLinkedStartingZone2[iarrConnectionList[0, i] - 1] > 1)
                {
                    iarrLinkToStartingZone[i] = true;
                    iarrCountLinkedStartingZone[iarrConnectionList[0, i] - 1] = iarrCountLinkedStartingZone2[iarrConnectionList[0, i] - 1];
                }
                if (iarrCountLinkedStartingZone2[iarrConnectionList[1, i] - 1] > 1)
                {
                    iarrLinkToStartingZone[i] = true;
                    iarrCountLinkedStartingZone[iarrConnectionList[1, i] - 1] = iarrCountLinkedStartingZone2[iarrConnectionList[1, i] - 1];
                }
            }

        }

        /// <summary>
        /// returns an array that defines a terrain for each zone 
        /// </summary>
        /// <returns>returns an array that defines a terrain for each zone </returns>
        internal void SetTerrainTable()
        {
            iarrTerrainTable = new int[ZoneNumber];

            for (int i = 0; i < iarrTerrainTable.Length; i++)
            {
                eTerrain eTerrainType = (eTerrain) Enum.Parse( typeof(eTerrain) , getZoneProperty(i,eTemplateElements.TerrainType.ToString()));
                if ( eTerrainType == eTerrain.Native )
                {
                    iarrTerrainTable[i] = GetFactionNativeTerrain(i);
                }
                else
                    iarrTerrainTable[i] =(int)eTerrainType  ;
            }
            //return (iarrTerrainTable);
        }

        /// <summary>
        /// returns a native terrain for a zone, if none defined then returns a random result
        /// </summary>
        /// <param name="iZoneIndex">zone index</param>
        /// <returns>native terrain for zone</returns>
        private int GetFactionNativeTerrain(int iZoneIndex)
        {
            //get the player id
            string strPlayerID = getPlayerIdPerZone(iZoneIndex);
            int iTerrainNumber;
            switch (strPlayerID)
            {
                case "PLAYER_NONE": 
                        //if random player or its a none player then randomize a native zone
                        iTerrainNumber = Enum.GetNames(typeof(eTerrain)).Length - 1;
                         
                        //maybe here needs to save faction(terrain) pick for dwellings use.. 
                        //or maybe needs to randomize it earlier and get it here

                        //return a random terrain
                        return Randomizer.rnd.Next(iTerrainNumber);
                    
                    //break;
                case "PLAYER_1":
                    //get faction for player 1
                    return ReverseFactionSelectionToTerrain ( iarrSelectedPlayersFactions[0] );
                    //break;
                case "PLAYER_2":
                    //get faction for player 2
                    return ReverseFactionSelectionToTerrain ( iarrSelectedPlayersFactions[1]);
                    //break;
                case "PLAYER_3":
                    //get faction for player 3
                    return ReverseFactionSelectionToTerrain( iarrSelectedPlayersFactions[2]);
                    //break;
                case "PLAYER_4":
                    //get faction for player 4
                    return ReverseFactionSelectionToTerrain(iarrSelectedPlayersFactions[3]);
                    //break;
                default:
                    break;
            }

                //if so get player faction choice terrain

            //if random player or its a none player then randomize a native zone
            iTerrainNumber = Enum.GetNames(typeof(eTerrain)).Length - 1;
            //return a random terrain
            return Randomizer.rnd.Next(iTerrainNumber);



        }

        private int ReverseFactionSelectionToTerrain(int iFactionSelection)
        {

            switch (iFactionSelection)
            {
                case (int)eTownType.Haven:
                    return (int)eTerrain.Grass;
                    //break;
                case (int)eTownType.Sylvan:
                    return (int)eTerrain.Grass;
                    //break;
                case (int)eTownType.Dungeon:
                    return (int)eTerrain.Subterrain;
                    //break;
                case (int)eTownType.Inferno:
                    return (int)eTerrain.Lava;
                    //break;
                case (int)eTownType.Fortress:
                    return (int)eTerrain.Snow;
                    //break;
                case (int)eTownType.Orcs:
                    return (int)eTerrain.Orcish;
                    //break;
                case (int)eTownType.Academy:
                    return (int)eTerrain.Sand;
                    //break;
                case (int)eTownType.Necropolis:
                    return (int)eTerrain.Dirt;
                    //break;
                default:
                    throw new Exception("Error In Terrain Assignment");
                    //break;
            }
        }

        internal void RemoveObjectWith0Chance()
        {
            for (int iZoneIndex = 0; iZoneIndex < iZoneCount; iZoneIndex++)
            {


                //get all object sets for current zone
                XmlElement[] xObjectSets = new XmlElement[IOBJECTSETS];

                for (int i = 0; i < IOBJECTSETS; i++)
                {
                    xObjectSets[i] = GetObjectsSet(iZoneIndex, i + 1);
                }

               
                //in each object set remove objects with 0 chance
                for (int i = 0; i < IOBJECTSETS; i++)
                {
                    XmlNodeList nodes = xObjectSets[i].SelectNodes(".//Object[@Chance='0.0']");
                    for (int j = 0; j < nodes.Count; j++)
                    {
                        nodes[j].ParentNode.RemoveChild(nodes[j]);
                    }
                    //for (int j = 0; j < xObjectSets[i].ChildNodes.Count; j++)
                    //{
                    //    if (xObjectSets[i].ChildNodes[j].Attributes["Chance"].Value == "0.0")
                    //    {
                    //        xObjectSets[i].RemoveChild(xObjectSets[i].ChildNodes[j]);
                    //        j--;
                    //    }

                    //}
                }
            }
        }

        /// <summary>
        /// checks if any player set a random town,If so then pick a random faction for it
        /// </summary>
        internal void SetPlayerFactions(bool different_factions)
        {
           bool flcontinue; 
           iarrSelectedPlayersFactions[0] = SaveiarrSelectedPlayersFactions[0];
           iarrSelectedPlayersFactions[1] = SaveiarrSelectedPlayersFactions[1];
           iarrSelectedPlayersFactions[2] = SaveiarrSelectedPlayersFactions[2];
           iarrSelectedPlayersFactions[3] = SaveiarrSelectedPlayersFactions[3];
            
           //dwp цикл, в котором присваиваются расы игрокам
           for (int pl = 0; pl < IPLAYERNUMBER; pl++) {
             if (iarrSelectedPlayersFactions[pl] == (int)eTownType.Random)
             {
               //randomize selection
               //dmitrik: use Different Factions option
               //dwp адаптировал алгоритм dmitrik-а для четырех игроков
               do
               {
                   //dwp генерится рандомная раса, из незапрещенных в выборе
                   do {
                       iarrSelectedPlayersFactions[pl] = Randomizer.rnd.Next(2, 10);
                   } while (iarrExcludedPlayersFactions[pl][iarrSelectedPlayersFactions[pl]-2] == 1);

                   flcontinue = false;
                   if (different_factions)
                   {
                       for (int j = 0; j < IPLAYERNUMBER; j++)
                       {   //dwp если альянсы, то проверка на различие делается только внутри альянса
                           if (((j != pl && !Program.frmMainMenu.RadioButton3.Checked) ||
                               (Program.frmMainMenu.RadioButton3.Checked && (j + pl) == 3)) &&
                               iarrSelectedPlayersFactions[j] == iarrSelectedPlayersFactions[pl])
                           {
                               flcontinue = true;
                               break;
                           }
                       }
                   }
               } while (flcontinue);
             }
           }
           //dwp теперь, по идентификаторам игроков в зонах, устанавливаем на городах присвоенные им расы
           for (int i = 0; i < this.ZoneNumber; i++)
           {
               string pl_id = getPlayerIdPerZone(i).Replace("PLAYER_", "");
               int pl;
               try
               {
                   pl = int.Parse(pl_id);
               }
               catch
               {
                   pl = 0;
               }
               if (pl != 0)
               {
                   for (int j = 1; j < 4; j++)
                   {
                       if (GetTownsAttributes(i, j, eTown.IsStartingTown.ToString()) == "True")
                           SetTownsAttributes(i, j, eTown.Type.ToString(), Enum.GetNames(typeof(eTownType))[iarrSelectedPlayersFactions[pl - 1]]);
                   }
               }
           } 
           Settings.Default.FactionRed    = iarrSelectedPlayersFactions[0];
           Settings.Default.FactionBlue   = iarrSelectedPlayersFactions[1];
           Settings.Default.FactionGreen  = iarrSelectedPlayersFactions[2];
           Settings.Default.FactionYellow = iarrSelectedPlayersFactions[3];
        }
    }
}
