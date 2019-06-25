using System;
using System.Collections.Generic;
using System.Text;

namespace Homm5RMG.Testing
{
    public static class LostObjectsCounter
    {
        public static void DumpObject(MapObject mo, int zone)
        {
            try {
                System.IO.StreamWriter s = System.IO.File.AppendText("LostObjects.txt");
                s.WriteLine("zone= \"" + zone.ToString() + "\"; name=\"" + mo.strName + "\"; x=\"" + mo.BasePoint.x + "\"; y=\"" + mo.BasePoint.y + "\"");
                s.Flush();
                s.Close();
                s.Dispose();
            }
            catch (Exception e) {
                throw new Exception("error writing lost objects file", e);
            }
        }
    }
}
