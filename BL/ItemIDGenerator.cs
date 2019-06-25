using System;
using System.Collections.Generic;
using System.Text;

namespace Homm5RMG
{
    class ItemIDGenerator
    {
        public static int CurrentItemID = 1000000000;
        public static readonly string STRITEMPREFIX = "item_";

        public static string GetNextID()
        {
            CurrentItemID++;
            return (STRITEMPREFIX + CurrentItemID.ToString() ) ;
        }
    }
}
