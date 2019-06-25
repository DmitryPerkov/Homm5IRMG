using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using Homm5RMG.BL;
using System.Reflection;

namespace Homm5RMG
{
    //public enum eObjectRotation 
    //{
    //    Down =6.28319 ,
    //    Right = 1.5708,
    //    Up = 3.14159,
    //    Left = 4.71239
    //}

    class ObjectsWriter
    {
        public static string[] strDirections =  { "6.28319"  , //down
                                                  "1.5708" ,  //right
                                                  "3.14159" , //up
                                                  "4.71239" };// left

        private const string OBJECTSSOURCEFILENAME = "Objects.xml";
        private string strEmptyMap = MapCreator.MAPS_INITIAL_DIR + MapCreator.strMapName + "\\Map.xdb";
        private string strEmptyMapTag = MapCreator.MAPS_INITIAL_DIR + MapCreator.strMapName + "\\map-tag.xdb";
        private const string SAVEMAPNAMEANDPATH = @"\Maps\Multiplayer\";
        private const string MAPXDB = @"\map.xdb";
        private const string MAPTAGXDB = @"\map-tag.xdb";
        private int countAltar ;
        private int countCartographer;
        public static string SUCCSESS = "Success";
        public XmlDocument xdocMap;
        public XmlDocument xdocMapDescription;
        public XmlNode xndObjects;
        public XmlNode xndPlayers;

        public ObjectsWriter()
        {
            #region Get Objects Node In Empty Map
            xdocMap = new XmlDocument();
            xdocMap.Load(strEmptyMap);
            xdocMapDescription = new XmlDocument();
            xdocMapDescription.Load(strEmptyMapTag);
            xndObjects = xdocMap.SelectSingleNode("//objects");
            xndPlayers = xdocMap.SelectSingleNode("//players");
            countAltar = 0;
            countCartographer = 0;

            #endregion
        }


        public void SetMoonCalendar()
        {
            XmlElement xMoons = (XmlElement) xdocMap.SelectSingleNode("//MoonCalendar");
            xMoons.SetAttribute("href", @".(MoonCalendar)/IRMGChaosWeeks.xdb#xpointer(/MoonCalendar)");

        }

        public void InitPlayersParams(int nump)
        {
            for(int i = 0; i < nump; i++) { //dwp активация игроков
                xndPlayers.ChildNodes[i].SelectSingleNode(".//ActivePlayer").FirstChild.Value = "true";
            }
            if (Program.frmMainMenu.RadioButton3.Checked) // dwp включаем альянсы
            {
                xdocMap.SelectSingleNode(".//CustomTeams").FirstChild.Value = "true";
                //dwp это команда №2
                xndPlayers.ChildNodes[1].SelectSingleNode(".//Team").FirstChild.Value = "1";
                xndPlayers.ChildNodes[2].SelectSingleNode(".//Team").FirstChild.Value = "1";
                xdocMapDescription.SelectSingleNode(".//teams").ChildNodes[0].FirstChild.Value = "2";
                xdocMapDescription.SelectSingleNode(".//teams").ChildNodes[1].FirstChild.Value = "2";
            }
            if (Program.frmMainMenu.RadioButton2.Checked) // dwp активируем третьего игрока в описании
                xdocMapDescription.SelectSingleNode(".//teams").AppendChild(xdocMapDescription.SelectSingleNode(".//teams").FirstChild.Clone());
            if (Program.frmMainMenu.RadioButton4.Checked) // dwp активируем третьего и четвертого игрока в описании 
            {
                xdocMapDescription.SelectSingleNode(".//teams").AppendChild(xdocMapDescription.SelectSingleNode(".//teams").FirstChild.Clone());
                xdocMapDescription.SelectSingleNode(".//teams").AppendChild(xdocMapDescription.SelectSingleNode(".//teams").FirstChild.Clone());
            }
        }

        /// <summary>
        /// Set the coordinates for a given object and append it to the map file
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="xndNewChild">object xml node containing item element that will be added</param>
        /// dwp присваиваем имя объета на карте. для работы скриптов
        public void SetObjectAtPosAndGenerateID(int x, int y, XmlNode xndNewChild , string strObjRotation, string ObjName)
        {
            XmlNode xndName = xndNewChild.SelectSingleNode(".//Name");
            XmlNode xndPos = xndNewChild.SelectSingleNode(".//Pos");
            XmlNode xndItem = xndNewChild.SelectSingleNode(".//Item");
            XmlNode xndRot = xndNewChild.SelectSingleNode(".//Rot");

            //set object direction
            xndRot.InnerText = strObjRotation;
            
            #region set objects name
            //dwp. присваиваем имена, где необходимо
            if (ObjName.Contains("portal"))
            {
                xndName.InnerText = ObjName;
            }
            else
            {
                switch (ObjName)
                {    /* начало блока для TE 6.5.7 */
                    case "Dragon_Utopia":
                        xndName.InnerText = ""; 
                        break;
                    case "SunkenTemple":
                        xndName.InnerText = "";  
                        break;
                    case "MagiVault":
                        xndName.InnerText = "";  
                        break;  
                    case "Pyramid":
                        xndName.InnerText = "";  
                        break;
                    case "Bank_naga_temple":
                        xndName.InnerText = "";  
                        break;
                    case "Elemantal_Stockpile":
                        xndName.InnerText = "";  
                        break;
                    case "Bank_demolish":
                        xndName.InnerText = "";  
                        break;
                    case "Bank_unkempt":
                        xndName.InnerText = "";  
                        break;
                    case "TreantThicket":
                        xndName.InnerText = "";  
                        break;
                    case "DwarvenTreasury":
                        xndName.InnerText = "";  
                        break;
                    case "Crypt":
                        xndName.InnerText = "";  
                        break;
                    case "GargoyleStonevault":
                        xndName.InnerText = "";  
                        break;
                    case "WitchBank":
                        xndName.InnerText = "";  
                        break;
                    /* конец блока для TE 6.5.7 */

                    case "Hut_Of_Magi1":
                        xndName.InnerText = ObjName;
                        break;
                    case "Hut_Of_Magi2":
                        xndName.InnerText = ObjName;
                        break;
                    case "Hut_Of_Magi3":
                        xndName.InnerText = ObjName;
                        break;
                    case "GarrisonEvil":
                        xndName.InnerText = ObjName;
                        break;
                    case "GarrisonOrcish":
                        xndName.InnerText = ObjName;
                        break;
                    case "GarrisonAntiMagic":
                        xndName.InnerText = ObjName;
                        break;
                    case "GarrisonGood":
                        xndName.InnerText = ObjName;
                        break;
                    case "GarrisonDwarven":
                        xndName.InnerText = ObjName;
                        break;
                    case "Cartographer":
                        xndName.InnerText = "custom_xray_" + countCartographer.ToString();
                        countCartographer++;
                        break;
                    case "SacrificeAltar":
                        xndName.InnerText = "custom_altar_" + countAltar.ToString();
                        countAltar++;
                        break;
                }
            }
            #endregion
            //set "random" or "next" ID for object
            ((XmlElement)xndItem).SetAttribute("id", ItemIDGenerator.GetNextID());
            //set pos
            xndPos.FirstChild.InnerText = x.ToString() ;
            xndPos.ChildNodes[1].InnerText = y.ToString();
            xndObjects.AppendChild(xdocMap.ImportNode(xndNewChild.FirstChild, true));
        }

        /// <summary>
        /// saves map.xdb file under the map directory name if not exist its created.
        /// </summary>
        /// <param name="strMapName"></param>
        public void SaveMap(string strMapName)
        {
            try
            {
                xdocMap.Save(Assembly.GetExecutingAssembly().CodeBase.Substring(0,Assembly.GetExecutingAssembly().CodeBase.Length - 12).Substring(8) + SAVEMAPNAMEANDPATH + strMapName + MAPXDB);
                xdocMapDescription.Save(Assembly.GetExecutingAssembly().CodeBase.Substring(0, Assembly.GetExecutingAssembly().CodeBase.Length - 12).Substring(8) + SAVEMAPNAMEANDPATH + strMapName + MAPTAGXDB);
            }
            catch (Exception)
            {
                Directory.CreateDirectory(Assembly.GetExecutingAssembly().CodeBase.Substring(0,Assembly.GetExecutingAssembly().CodeBase.Length - 12).Substring(8) + SAVEMAPNAMEANDPATH + strMapName);
                xdocMap.Save(Assembly.GetExecutingAssembly().CodeBase.Substring(0, Assembly.GetExecutingAssembly().CodeBase.Length - 12).Substring(8) + SAVEMAPNAMEANDPATH + strMapName + MAPXDB);
                xdocMapDescription.Save(Assembly.GetExecutingAssembly().CodeBase.Substring(0, Assembly.GetExecutingAssembly().CodeBase.Length - 12).Substring(8) + SAVEMAPNAMEANDPATH + strMapName + MAPTAGXDB);
            }
        }

        internal void RemoveITSpell()
        {
            XmlNode xSpellIds =  xdocMap.DocumentElement.SelectSingleNode("./spellIDs");
            
            ////select the IT spell node
            XmlNode xSubItemIT = xSpellIds.SelectSingleNode(".//Item[.='SPELL_DIMENSION_DOOR']");
            //xSubItemIT.RemoveAll();
            //xdocMap.RemoveChild(xSubItemIT);
            //xSubItemIT.RemoveAll();
            xSpellIds.RemoveChild(xSubItemIT);
        }
    }
}
