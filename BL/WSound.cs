using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace Homm5RMG.BL
{
    public class WSounds
    {
        [DllImport("WinMM.dll")]
        public static extern bool PlaySound(string fname, int Mod, int flag);
        // these are the SoundFlags we are using here, check mmsystem.h for more    
        public int SND_ASYNC = 0x0001;
        // play asynchronously    
        public int SND_FILENAME = 0x00020000; // use file name    
        public int SND_PURGE = 0x0040;     // purge non-static events     
        public void Play(string fname, int SoundFlags)
        {
            PlaySound(fname, 0, SoundFlags);
        }

        public void StopPlay()
        {
            PlaySound(null, 0, SND_PURGE);
        }
    }
}
