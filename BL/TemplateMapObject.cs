using System;
using System.Collections.Generic;
using System.Text;

namespace Homm5RMG
{

    /// <summary>
    /// holds gui related template object data
    /// </summary>
    class TemplateMapObject
    {
        public string Name; //objects name
        public string Value; //guard strength
        public string Chance; //chance to appear in zone
        public string MaxNumber = ""; //max number for this object in zone

        public TemplateMapObject(string strName)
        {
            Name = strName;
        }

        public override string ToString()
        {
            return Name; 
            //return base.ToString();
        }

        //this is overridden for the compare of map points
        public override bool Equals(object obj)
        {
            if (obj is TemplateMapObject)
                return (this.Name == ((TemplateMapObject)obj).Name);
            else
                return false;
        }

    }
}
