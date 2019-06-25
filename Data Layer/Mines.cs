using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Homm5RMG
{
    class Mines
    {
        private const string MINESSOURCEFILENAME = "Mines.xml";
        public XmlDocument xdocMines;


        public Mines()
        {
            xdocMines = new XmlDocument();
            xdocMines.Load(MINESSOURCEFILENAME);
        }


        /// <summary>
        /// get Mines node
        /// </summary>
        /// <returns></returns>
        public XmlElement GetMinesData()
        {
            return xdocMines.DocumentElement;
        }


        public XmlNode GetMineXMLByType(eMines MineType)
        {
            return null;
        }
    }
}
