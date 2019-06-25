using System;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;

namespace Homm5RMG.BL
{
    class MapCreator
    {
        public static string strMapName = string.Empty;
        public static string MAPS_INITIAL_DIR = Assembly.GetExecutingAssembly().CodeBase.Substring(0,Assembly.GetExecutingAssembly().CodeBase.Length - 12).Substring(8) + @"Maps\Multiplayer\";

        public static void CreateTempMapFiles(MapSize SizeToCreate ,string strNameOfMap, Homm5RMG.Properties.Settings settings, int [] SelectedFactions, int[][] ExcludedPlayersFactions)
        {
            DeleteTempMapFiles();
            strMapName =strNameOfMap;

            string strTargetTemp = MAPS_INITIAL_DIR + strMapName;

            Directory.CreateDirectory(strTargetTemp);
            DirectoryInfo dirinfEmptyMapFiles = new DirectoryInfo(Assembly.GetExecutingAssembly().CodeBase.Substring(0,Assembly.GetExecutingAssembly().CodeBase.Length - 12).Substring(8) + "Data\\Basic Empty Map Files\\" + SizeToCreate.ToString());
            
            CopyToTarget(dirinfEmptyMapFiles, "" , strTargetTemp);

            SetMapName();
            SetMapParameters(settings, SelectedFactions, ExcludedPlayersFactions);
        }

        private static void CopyToTarget(DirectoryInfo dirinfCurrDir, string strRelativePath , string strTargetTemp)
        {
            foreach (DirectoryInfo dirinfSubDir in dirinfCurrDir.GetDirectories())
            {
                Directory.CreateDirectory (strTargetTemp +  strRelativePath  + "\\" + dirinfSubDir.Name);
                CopyToTarget(dirinfSubDir, strRelativePath + "\\" + dirinfSubDir.Name, strTargetTemp);
            }


            foreach (FileInfo fiMapFile in dirinfCurrDir.GetFiles())
            {
                fiMapFile.CopyTo(strTargetTemp + strRelativePath + "\\" + fiMapFile.Name);
            }
        }

        public static void SetMapName()
        {
            //open text file name.txt and insert name into it
            System.IO.StreamWriter fsName = new StreamWriter(MAPS_INITIAL_DIR + "\\" + strMapName + "\\name.txt",true,Encoding.Unicode);
            fsName.Write( "<color=7777FF>" + strMapName);
            fsName.Close();
        }

        public static void SetMapParameters(Homm5RMG.Properties.Settings settings,int [] SelectedFactions, int[][] ExcludedPlayersFactions)
        {
            System.IO.StreamWriter file = new StreamWriter(MAPS_INITIAL_DIR + "\\" + strMapName + "\\description.txt", true, Encoding.Unicode);
            file.Write(get_map_description(settings, SelectedFactions, ExcludedPlayersFactions));
            file.Close();
        }

        private static string get_map_description(Homm5RMG.Properties.Settings settings, int[] SelectedFactions, int[][] ExcludedPlayersFactions )
        {
            StringBuilder desc = new StringBuilder();
            String[] excludedrace = new string[4];
            for (int pl = 0; pl < (Program.frmMainMenu.RadioButton3.Checked||Program.frmMainMenu.RadioButton4.Checked ? 4:
                                   Program.frmMainMenu.RadioButton2.Checked?3:2); pl++)
            {
                if (SelectedFactions[pl] == 1)
                {
                    for (int fac = 0; fac < 8; fac++)
                        if (ExcludedPlayersFactions[pl][fac] == 1)
                            excludedrace[pl] = String.Concat(excludedrace[pl], excludedrace[pl] == null ? Enum.GetNames(typeof(eTownType))[fac + 2] : "," + Enum.GetNames(typeof(eTownType))[fac + 2]);
                    if (excludedrace[pl] != null) excludedrace[pl] = String.Concat("(Excluding:", excludedrace[pl], ")");
                }
            }

            try {
                System.IO.FileInfo f = new System.IO.FileInfo(settings.DefaultTemplateFile);
                desc.AppendFormat(Program.frmMainMenu.RadioButton3.Checked ? ("{0}{4} + {3}{7} vs {1}{5} + {2}{6}  ") :
                                  ("{0}{4} vs {1}{5}" +
                                  ((Program.frmMainMenu.RadioButton2.Checked || Program.frmMainMenu.RadioButton4.Checked) ? " vs {2}{6}" : "") +
                                  (Program.frmMainMenu.RadioButton4.Checked ? " vs {3}{7}" : "")) + ", ",
                                  Enum.GetNames(typeof(eTownType))[SelectedFactions[0]],
                                  Enum.GetNames(typeof(eTownType))[SelectedFactions[1]],
                                  Enum.GetNames(typeof(eTownType))[SelectedFactions[2]],
                                  Enum.GetNames(typeof(eTownType))[SelectedFactions[3]],
                                  excludedrace[0], excludedrace[1], excludedrace[2], excludedrace[3]);
                desc.AppendFormat("template: {0}, ", settings.RandomTemplate ? "random(" + frmMain.templatelist + ")" :
                                    f.Name.Split('.')[0] + "(T:" + f.LastWriteTime + ",S:" + f.Length + ")");
                string[] sizes = Enum.GetNames(typeof(MapSize));
                desc.AppendFormat("size: {0}, ", sizes[settings.SelectedSizeIndex]);
                desc.AppendFormat("start zone dwellings: {0}, ", Enum.GetNames(typeof(eDwellingStatus))[settings.DwellingStatus]);
                desc.AppendFormat("random seed: {0}, ", settings.RandomSeed);
                desc.AppendFormat("IRMG : {0}, ", frmMain.STR_VERSION);
                desc.AppendFormat("date: {0}", DateTime.Now);
            }
            catch (Exception) {
                return "";
            }

            return desc.ToString();
        }

        internal static void ZipTempMapFiles(string strDestMapNameAndPath)
        {
            ICSharpCode.SharpZipLib.Zip.ZipFile NewMapZipped = ICSharpCode.SharpZipLib.Zip.ZipFile.Create(strDestMapNameAndPath);

            NewMapZipped.BeginUpdate();

            ZipFilesInDir ( new DirectoryInfo( MAPS_INITIAL_DIR ) , NewMapZipped);

            //NewMapZipped.Add(@"Maps\Multiplayer\"+"\\"+);
            //NewMapZipped.Add(@"Maps\Multiplayer\TestMap\GroundTerrain.bin");
            NewMapZipped.CommitUpdate();
            NewMapZipped.Close();
        }

        private static void ZipFilesInDir(DirectoryInfo dirinfDirToZip, ICSharpCode.SharpZipLib.Zip.ZipFile NewMapZipped)
        {
            foreach (DirectoryInfo dirinfSubFolder in dirinfDirToZip.GetDirectories())
            {
                ZipFilesInDir(dirinfSubFolder, NewMapZipped);
            }

            foreach (FileInfo finfMapFile in dirinfDirToZip.GetFiles())
            {
                NewMapZipped.Add( finfMapFile.FullName.Remove( 0 , finfMapFile.FullName.IndexOf("\\Maps")+1) );
            }
        }


        public static void RemoveReadOnly(DirectoryInfo dirinfMapsDir)
        {

           // File.SetAttributes(dirinfMapsDir.FullName, FileAttributes.Normal);


            ////remove all files read only
            //foreach (FileInfo finfMapFile in dirinfMapsDir.GetFiles())
            //{
            //    finfMapFile.Attributes = FileAttributes.Normal;
            //}


            //do it in all sub dirs recursivly
            foreach (DirectoryInfo dirinfSub in dirinfMapsDir.GetDirectories())
            {
                RemoveReadOnly(dirinfSub);
            }

            //remove current read only
            dirinfMapsDir.Attributes = dirinfMapsDir.Attributes ^ FileAttributes.ReadOnly;

        }

        internal static void DeleteTempMapFiles()
        {
                
                if (Directory.Exists(Assembly.GetExecutingAssembly().CodeBase.Substring(0,Assembly.GetExecutingAssembly().CodeBase.Length - 12).Substring(8) + "Maps"))
                {
                    try
                    {
                        //RemoveReadOnly(new DirectoryInfo("Maps"));
                        //new DirectoryInfo("Maps").Attributes = FileAttributes.Directory;
                        Directory.Delete(Assembly.GetExecutingAssembly().CodeBase.Substring(0,Assembly.GetExecutingAssembly().CodeBase.Length - 12).Substring(8) + "Maps", true);

                    }
                    catch (Exception)
                    {
                    }
                }
        }


        internal static bool CheckMapExistance(string strMapDirectory, string strMapName)
        {
            string strFullName = strMapName + ".h5m";

            DirectoryInfo dirinfMapDir = new DirectoryInfo(strMapDirectory);

            foreach (FileInfo fiMapDirFile in dirinfMapDir.GetFiles())
            {
                if (fiMapDirFile.Name == strFullName)
                    return true;

            }

            return false;
        }
    }
}
