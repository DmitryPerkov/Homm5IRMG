using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Homm5RMG.Properties;

namespace Homm5RMG
{

    enum eObjectsSpacificProperties
    {
        TwoWayPortalGroupID,
        GroupID
    }

    enum eTownUpgradeXMLValues
    {
        BLD_UPG_NONE = 0,
        BLD_UPG_1 ,
        BLD_UPG_2 ,
        BLD_UPG_3 ,
        BLD_UPG_4 ,
        BLD_UPG_5
    }


    class ObjectSpacificPropertiesHelper
    {
        public static int TwoWayPortalGroupID = -1;

        public static int GetNextTwoWayPortalGroupId()
        {
            TwoWayPortalGroupID++;
            return TwoWayPortalGroupID;
        }

        public static void AddTownProperties(MapObject mpobjTown, TemplateHandler thTemplate ,int iTownNumber ,int iZoneIndex)
        {
            string[] strPropertiesNames = Enum.GetNames(typeof(eTown));

            foreach (string strProperty in strPropertiesNames)
            {
                //add property
                mpobjTown.ObjectSpacificProperties.Add(strProperty, thTemplate.GetTownsAttributes(iZoneIndex, iTownNumber, strProperty));
            }
            //todo: Add Player Assaign
            string strPlayerID = thTemplate.getPlayerIdPerZone(iZoneIndex);

            mpobjTown.ObjectSpacificProperties.Add("PlayerId", strPlayerID);
        }


        internal static XmlNode GenerateGarrisonXML(MapObject mpobjObject, XmlNode xndGarrison, Guards guardsGenerator,eMonsterStrength iStrength)
        {

            //set guards for garrison
            XmlNode xndGuard = guardsGenerator.GetGarrisonGuards((int)(mpobjObject.iValue * double.Parse(Settings.Default.MonsterFactor)), 
                                                                 false, 
                                                                 iStrength,
                                                                 xndGarrison.SelectSingleNode(".//armySlots"));
            return xndGarrison;
        }

        internal static XmlNode GenerateTownXML(MapObject mpobjObject , XmlNode xndTown, Guards guardsGenerator )//, string strPlayerId)
        {
            //ObjectsReader obrdGetTown = new ObjectsReader();
            //XmlNode xndTown = obrdGetTown.GetObjectByName("Town");

            //set blacksmith
            string strBlackSmith = mpobjObject.ObjectSpacificProperties[eTown.BlackSmith.ToString()];

            if (bool.Parse(strBlackSmith))
            {
                SetTownXMLAttribute(xndTown, eTown.BlackSmith.ToString(), 1);
            }
            else
            {
                SetTownXMLAttribute(xndTown, eTown.BlackSmith.ToString(), 0);
            }

            //set dwelling level and upgrade
            string strDwellingLevel = mpobjObject.ObjectSpacificProperties[eTown.DwellingLevel.ToString()];
            string strDwellingsUpgrades = mpobjObject.ObjectSpacificProperties[eTown.DwellingsUpgrades.ToString()];

            int iUpgradeLevel =  Convert.ToInt32( (bool.Parse( strDwellingsUpgrades ) ) );
            iUpgradeLevel++;

            for (int i = 1; i <= int.Parse(strDwellingLevel); i++)
            {
                SetTownXMLAttribute(xndTown, "DWELLING_" + i.ToString(), iUpgradeLevel);
            }

            //set income level
            string strIncomeLevel = mpobjObject.ObjectSpacificProperties[eTown.IncomeLevel.ToString()];

            SetTownXMLAttribute(xndTown, "TOWN_HALL", (int)Enum.Parse(typeof(eIncome), strIncomeLevel));

            //set player
            string strIsStarting = mpobjObject.ObjectSpacificProperties[eTown.IsStartingTown.ToString()];

            //if it needs player assignment assaign it the needed player
            if (bool.Parse(strIsStarting))
            {
                ////todo:unmark and add playerid
                xndTown.SelectSingleNode(".//PlayerID").InnerText = mpobjObject.ObjectSpacificProperties["PlayerId"];

                ////set type according to user selected town
                //string strType = mpobjObject.ObjectSpacificProperties[eTown.Type.ToString()];
                ////eTownXMLType.
                //xndTown.SelectSingleNode(".//Shared");

                //for a starting zone the template's town settings is not important..
                //only first screen will determine town settings and currently
                //its only random.

                string strType = mpobjObject.ObjectSpacificProperties[eTown.Type.ToString()];


                XmlNode xndSharedType = xndTown.SelectSingleNode(".//Shared");
                string strHref = xndSharedType.Attributes["href"].Value;

                string strName = strHref.Substring("/MapObjects/".Length, strHref.LastIndexOf('.') - "/MapObjects/".Length);
                if (strName.IndexOf('/') > 0)
                {
                    strName = strName.Split('/')[1];
                }

                if (strType == eTownType.Random.ToString())
                    strHref = strHref.Replace(strName, Enum.Parse(typeof(eTownXMLType), ((int)Enum.Parse(typeof(eTownType), strType)).ToString()).ToString());
                else
                    strHref = strHref.Replace(strName, Enum.Parse(typeof(eTownXMLType), ((int)Enum.Parse(typeof(eTownType), strType)).ToString()).ToString() + ".(AdvMapTownShared)");


                ((XmlElement)xndSharedType).SetAttribute("href", strHref);

                //set town specialization
                TownSpecBuilder ts_builder = new TownSpecBuilder(guardsGenerator.TSParser);
                string spec = ts_builder.GetRandomSpecialization(EnumConvert.TownTypeFromString(strType));
                XmlNode xndSpec = xndTown.SelectSingleNode(".//Specialization");
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("/GameMechanics/TownSpecialization/{0}/Random/{1}#xpointer(/TownSpecialization)",
                    ((eTownSpecXMLHREFType)EnumConvert.TownTypeFromString(strType)).ToString(),
                    spec);
                ((XmlElement)xndSpec).SetAttribute("href", sb.ToString());
            }
            else
            {
                //set type according to template definition

                //set type according to user selected town
                string strType = mpobjObject.ObjectSpacificProperties[eTown.Type.ToString()];
                

                XmlNode xndSharedType = xndTown.SelectSingleNode(".//Shared");
                string strHref = xndSharedType.Attributes["href"].Value;

                string strName = strHref.Substring("/MapObjects/".Length, strHref.LastIndexOf('.') - "/MapObjects/".Length);
                if (strName.IndexOf('/') > 0)
                {
                    strName = strName.Split('/')[1];
                }
                
                if ( strType == eTownType.Random.ToString() )
                    strHref = strHref.Replace(strName, Enum.Parse(typeof(eTownXMLType), ((int)Enum.Parse(typeof(eTownType), strType)).ToString()).ToString() );
                else
                    strHref = strHref.Replace(strName, Enum.Parse ( typeof ( eTownXMLType) , ((int) Enum.Parse( typeof ( eTownType) ,strType )).ToString() ).ToString() + ".(AdvMapTownShared)");

                ((XmlElement) xndSharedType).SetAttribute ("href",strHref);

                //set guards for neutral town
                TownGarrisonBuilder tgbuilder = new TownGarrisonBuilder(guardsGenerator.TGParser);
                TownGarrison tg = tgbuilder.GetRandomTownGarrison();
                if (tg != null) {
                    XmlNode xndArmySlots = xndTown.SelectSingleNode(".//armySlots");
                    foreach (XmlNode n in tg.Node.ChildNodes) {
                        xndArmySlots.AppendChild(xndArmySlots.OwnerDocument.ImportNode(n, true));
                    }
                }
            }

            //set mage guild level
            string strMageGuildLevel = mpobjObject.ObjectSpacificProperties[eTown.MageGuildLevel.ToString()];

            SetTownXMLAttribute(xndTown, "MAGIC_GUILD", int.Parse(strMageGuildLevel));

            
            //set mage guild level
            string strResourceLevel = mpobjObject.ObjectSpacificProperties[eTown.ResourceLevel.ToString()];
            
            SetTownXMLAttribute(xndTown, "MARKETPLACE", (int)Enum.Parse(typeof(eResource) , strResourceLevel));

            //set blacksmith
            string strTavern = mpobjObject.ObjectSpacificProperties[eTown.Tavern.ToString()];

            if (bool.Parse(strTavern))
            {
                SetTownXMLAttribute(xndTown, eTown.Tavern.ToString(), 1);
            }
            else
            {
                SetTownXMLAttribute(xndTown, eTown.Tavern.ToString(), 0);
            }


            //set Town Walls
            string strTownWalls = mpobjObject.ObjectSpacificProperties[eTown.TownWalls.ToString()];

            SetTownXMLAttribute(xndTown, "FORT", (int)Enum.Parse(typeof(eWalls), strTownWalls));

            return xndTown;

        }

        internal static void SetTownXMLAttribute(XmlNode xndTown, string strAttribute, int SetLevel)
        {
            XmlNode xndInitialUpgradeNode = xndTown.SelectSingleNode(".//Item[Type='" + "TB_" + strAttribute.ToUpper() + "']").ChildNodes[1];
            xndInitialUpgradeNode.InnerText = Enum.GetName(typeof(eTownUpgradeXMLValues), SetLevel);
        }

        internal static void RenderComplexObject(ObjectsWriter obwrTransformer, MapObject moComplexObject , XmlElement xOtherObjects)
        {
            XmlNode xndPos,xndRot;
            int iXPos;
            int iYPos;

            //get complex object xml reference in other objects xml
            XmlNode xndComplexObject = xOtherObjects.SelectSingleNode("//ComplexObject[@Name='" + moComplexObject.strName + "']");
            foreach (XmlNode xndObject in xndComplexObject.ChildNodes)
            {
                //get relative x and y for object
                xndPos = xndObject.SelectSingleNode(".//Pos");
                iXPos = int.Parse( xndPos.FirstChild.InnerText);
                iYPos = int.Parse(xndPos.ChildNodes[1].InnerText);
                xndRot = xndObject.SelectSingleNode(".//Rot");
                MapPoint mpRelativeObjectPoint = TransformPointDirection(new MapPoint(iXPos, iYPos), moComplexObject.Direction);
                MapObject mpSubObject = new MapObject(mpRelativeObjectPoint, null, moComplexObject.strName);
                mpSubObject.Rotation = xndRot.InnerText;
                mpSubObject.Direction = moComplexObject.Direction;
                obwrTransformer.SetObjectAtPosAndGenerateID(mpRelativeObjectPoint.x + moComplexObject.BasePoint.x, mpRelativeObjectPoint.y + moComplexObject.BasePoint.y, xndObject.Clone(), mpSubObject.Rotation, moComplexObject.strName);
            }
            

        }

        internal static MapPoint TransformPointDirection(MapPoint mpAccessPoint, ObjectDirection objectDirection)
        {
            switch (objectDirection)
            {
                case ObjectDirection.Down:
                    return mpAccessPoint;
                    break;
                case ObjectDirection.Right:
                    return new MapPoint(mpAccessPoint.y * -1, mpAccessPoint.x);
                    break;
                case ObjectDirection.Up:
                    return new MapPoint(mpAccessPoint.x * -1, mpAccessPoint.y * -1);
                    break;
                case ObjectDirection.Left:
                    return new MapPoint(mpAccessPoint.y, mpAccessPoint.x * -1);
                    break;
                default:
                    return mpAccessPoint;
                    break;
            }
            return mpAccessPoint;

        }


        /// <summary>
        /// goes over the properties spacific to high dwellings (if 4-7 tier are present) and set them in xml
        /// </summary>
        /// <param name="xndHighDwelling"></param>
        /// <param name="mpDwelling"></param>
        /// <returns></returns>
        internal static XmlNode ProcessHighDwellingObject(XmlNode xndHighDwelling, MapObject mpDwelling)
        {
            string strDwellingArray = mpDwelling.ObjectSpacificProperties["DwellingArray"];
            XmlNode xndTiers = xndHighDwelling.SelectSingleNode(".//creaturesEnabled");

            if (xndTiers.HasChildNodes) {
            for (int i = 0; i < 4; i++)
            {
                xndTiers.ChildNodes[i].InnerText = strDwellingArray[i].ToString();
            }
            }
                       

            return xndHighDwelling;

        }

        internal static XmlNode ProcessLowDwellingObject(XmlNode xndLowDwelling, MapObject mpDwelling)
        {
            //get player assosiation and set it
            string strPlayerID = mpDwelling.ObjectSpacificProperties["PlayerID"];
            
            //get player id node
            XmlNode xndPlayerLink = xndLowDwelling.SelectSingleNode(".//LinkToPlayer");
            xndPlayerLink.InnerText = strPlayerID;

            if (strPlayerID == "PLAYER_NONE") {
                XmlNode node = xndLowDwelling.SelectSingleNode(".//RndSource");
                node.InnerText = "RND_NONE";
            }

            //set level 


            XmlNode xndShared = xndLowDwelling.SelectSingleNode(".//Shared");

            string strHref = xndShared.Attributes["href"].Value;

            string strDwellingLevelNumber = mpDwelling.ObjectSpacificProperties["DwellingLevel"];
            ((XmlElement)xndShared).SetAttribute("href", strHref.Replace("1", strDwellingLevelNumber));

            return xndLowDwelling;
        }

        internal static XmlNode SetTwoWayPortalId(XmlNode xndTwoWayPortal , string strPortalGroupId)
        {
            XmlNode xndGroupId = xndTwoWayPortal.SelectSingleNode(".//" + eObjectsSpacificProperties.GroupID.ToString());
            xndGroupId.InnerText = strPortalGroupId;

            return xndTwoWayPortal;
        }
    }
}
