using System;
using System.Collections.Generic;
using System.Text;

namespace Homm5RMG
{
    class VoronoiPointExistException : Exception
    {
        public VoronoiPointExistException(string strErrorMessage)
            : base(strErrorMessage)
        {
        }
    }
}
