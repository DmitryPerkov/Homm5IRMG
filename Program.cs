using System;
//using System.Diagnostics;
//using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Homm5RMG
{
    static class Program
    {
        /*        [DllImport("kernel32", SetLastError = true)]
                private static extern bool AttachConsole(int dwProcessId);
                        [DllImport("user32.dll")]
                static extern IntPtr GetForegroundWindow();

                [DllImport("user32.dll", SetLastError = true)]
                static extern uint GetWindowThreadProcessId(IntPtr hWnd, out int lpdwProcessId);
        */
        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        static extern bool FreeConsole();

        public static frmMain frmMainMenu;
        public static frmTemplateEditor frmTemplateEdit;
        public static frmMines frmSetMines;
        public static frmTowns frmSetTowns;
        public static frmObjectsPick frmObjectsSelect;
        public static frmPickGuard frmGuards;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;
            System.Threading.Thread.CurrentThread.CurrentUICulture = System.Globalization.CultureInfo.InvariantCulture;
            frmMainMenu = new frmMain();
            frmTemplateEdit = new frmTemplateEditor();
            frmSetMines = new frmMines();
            frmSetTowns = new frmTowns();
            frmObjectsSelect = new frmObjectsPick();
            frmSplash fSplash = new frmSplash();
            frmGuards = new frmPickGuard();
            //Application.Run(frmMainMenu);

            //MessageBox.Show(args[0]);
            if (args.Length > 0)
            {
                if(args[0]=="/?" || args.Length < 5) { 
                //IntPtr ptr = GetForegroundWindow();
                //int u;
                //GetWindowThreadProcessId(ptr, out u);
                //Process process = Process.GetProcessById(u);
                //AttachConsole(process.Id);

                System.Console.WriteLine("Homm5RMG Тип_генерации PL1=Раса_игрока_1[(Exc(исключаемые расы через запятую))]");
                System.Console.WriteLine("                       PL2=Раса_игрока_2[(Exc(исключаемые расы через запятую))]");
                System.Console.WriteLine("                      [PL3=Раса_игрока_3[(Exc(исключаемые расы через запятую))]]");
                System.Console.WriteLine("                      [PL4=Раса_игрока_4[(Exc(исключаемые расы через запятую))]]");
                System.Console.WriteLine("         Template=Полное_имя_файла_шаблона | DirTemplate = Полное_имя_каталога_шаблона");
                System.Console.WriteLine("         Path=Папка_сохранения_карты");
                System.Console.WriteLine("         [Dwelling=Standart|Random|Extented]");
                System.Console.WriteLine("         [Syze=Размер]");
                System.Console.WriteLine("         [Name=Имя_для_карты]");
                System.Console.WriteLine("         [HaosWeek=1 | 0]");
                System.Console.WriteLine("         [Diff=1 | 0]");
                System.Console.WriteLine("Тип генерации: 1 = 1 vs 1");
                System.Console.WriteLine("               2 = 1 vs 1 vs 1");
                System.Console.WriteLine("               3 = 1 vs 1 vs 1 vs 1");
                System.Console.WriteLine("               4 = 1 + 1 vs 1 + 1(парная)");
                System.Console.WriteLine("Раса_игрока_X: 0 - random");
                System.Console.WriteLine("               1 - Haven");
                System.Console.WriteLine("               2 - Sylvan");
                System.Console.WriteLine("               3 - dungeon");
                System.Console.WriteLine("               4 - inferno");
                System.Console.WriteLine("               5 - fortress");
                System.Console.WriteLine("               6 - stronghold");
                System.Console.WriteLine("               7 - Academy");
                System.Console.WriteLine("               8 - Necro");
                System.Console.WriteLine("Exc(исключаемые расы через запятую):Только для Расы Random можно указать исключаемые расы(1-8)");
                System.Console.WriteLine("Template = путь с шаблоном");
                System.Console.WriteLine("DirTemplate = путь с папкой шаблонов");
                System.Console.WriteLine("Размер: Medium");
                System.Console.WriteLine("        Small");
                System.Console.WriteLine("        Tiny");
                System.Console.WriteLine("        Impossible");
                System.Console.WriteLine("        Huge         default");
                System.Console.WriteLine("        ExtraLarge");
                System.Console.WriteLine("        Large");
                System.Console.WriteLine("Dwelling по умолчанию Random");
                System.Console.WriteLine("Name - по умолчанию имя карты генерируется");
                System.Console.WriteLine("HaosWeek по умолчанию - 0=нет");
                System.Console.WriteLine("Diff Differnt Faction, по умолчанию - 1=да");
                System.Console.WriteLine("");
                System.Console.WriteLine("Примеры: Homm5RMG 1 PL1=1 PL2=7 Template=c:\\hmm\\irmg\\hugetemplate\\mask.irt Path=c:\\hmm\\maps Name=moz_vs_dwp1");
                System.Console.WriteLine("         Homm5RMG 1 PL1=0 PL2=0 DirTemplate=c:\\hmm\\irmg\\hugetemplate Path=c:\\hmm\\maps Name=moz_vs_dwp2");
                System.Console.WriteLine("         Homm5RMG 1 PL1=0 PL2=0 DirTemplate=c:\\hmm\\irmg\\hugetemplate Path=c:\\hmm\\maps Name=moz_vs_dwp2 Diff=0");
                System.Console.WriteLine("         Homm5RMG 1 PL1=0(Exc(6, 8)) PL2=0(Exc(3, 8)) DirTemplate=c:\\hmm\\irmg\\hugetemplate Path=c:\\hmm\\maps Name=moz_vs_dwp3");
                System.Console.WriteLine("         Homm5RMG 1 PL1=1 PL2=0(Exc(3, 8)) DirTemplate=c:\\hmm\\irmg\\hugetemplate Path=c:\\hmm\\maps");
                    return;
                }
                // переменные для разбора строки параметров
                string tmpstr1 = "", tmpstr2 = "", tmpstr3 = "0", tmpstr4 = "0",
                       tmpstr5 = "", tmpstr6 = "", tmpstr7 = "", tmpstr8 = "", 
                       tmpstr9 = "Huge", tmpstr10 = "Random", tmpstr11 = "0";
                string[] list1, list2, list3, list4; //списки Exc в командной строке
                int[] exc1 = new int[] { 0, 0, 0, 0, 0, 0, 0, 0 }, exc2= new int[] { 0, 0, 0, 0, 0, 0, 0, 0 }, // преобразованые исключения рас для генерации
                      exc3 = new int[] { 0, 0, 0, 0, 0, 0, 0, 0 }, exc4= new int[] { 0, 0, 0, 0, 0, 0, 0, 0 };
                int delta=0;
                bool set1=false,set2=true,set3=true,set4=false;

                if (args[0]=="4"||args[0]=="3")
                {
                    tmpstr4 = args[4].Split('=')[0];
                    tmpstr3 = args[3].Split('=')[0];
                    delta = 2;
                }
                if (args[0] == "2")
                {
                    tmpstr3 = args[3].Split('=')[0];
                    delta = 1;
                }
                tmpstr1 = args[1].Split('=')[0];
                tmpstr2 = args[2].Split('=')[0];
                if ((args[0] != "1") & (args[0] != "2") & (args[0] != "3") & (args[0] != "4")) { System.Console.WriteLine("Неверный вызов, используйте /? для получения справки.");
                    return;
                }
                if (string.Compare(tmpstr1, "PL1") == 0)
                {
                    tmpstr1 = args[1].Split('=')[1];
                    if (tmpstr1.IndexOf("Exc(") >= 0)
                    {
                        list1 = tmpstr1.Substring(tmpstr1.IndexOf("Exc(")+4).Replace(")","").Split(',');
                        for(int i = 0; i < list1.Length; i++)
                        {
                            exc1[Convert.ToInt32(list1[i])-1] = 1;
                        }
                        tmpstr1 = tmpstr1.Split('(')[0];
                        if (tmpstr1 != "0") { System.Console.WriteLine("Неверный вызов, используйте /? для получения справки."); return; }
                    }
                }
                else
                {
                    System.Console.WriteLine("Неверный вызов, используйте /? для получения справки.");
                    return;
                }
                if (string.Compare(tmpstr2, "PL2") == 0) {
                    tmpstr2 = args[2].Split('=')[1];
                    if (tmpstr2.IndexOf("Exc(") >= 0)
                    {
                        list2 = tmpstr2.Substring(tmpstr2.IndexOf("Exc(") + 4).Replace(")", "").Split(',');
                        for (int i = 0; i < list2.Length; i++)
                        {
                            exc2[Convert.ToInt32(list2[i]) - 1] = 1;
                        }
                        tmpstr2 = tmpstr2.Split('(')[0];
                        if (tmpstr2 != "0") { System.Console.WriteLine("Неверный вызов, используйте /? для получения справки."); return; }
                    }
                }
                if (string.Compare(tmpstr3, "PL3") == 0){
                    tmpstr3 = args[3].Split('=')[1];
                    if (tmpstr3.IndexOf("Exc(") >= 0)
                    {
                        list3 = tmpstr3.Substring(tmpstr3.IndexOf("Exc(") + 4).Replace(")", "").Split(',');
                        for (int i = 0; i < list3.Length; i++)
                        {
                            exc3[Convert.ToInt32(list3[i]) - 1] = 1;
                        }
                        tmpstr3 = tmpstr3.Split('(')[0];
                        if (tmpstr3 != "0") { System.Console.WriteLine("Неверный вызов, используйте /? для получения справки."); return; }
                    }
                }
                if (string.Compare(tmpstr4, "PL4") == 0){
                    tmpstr4 = args[4].Split('=')[1];
                    if (tmpstr4.IndexOf("Exc(") >= 0)
                    {
                        list4 = tmpstr4.Substring(tmpstr3.IndexOf("Exc(") + 4).Replace(")", "").Split(',');
                        for (int i = 0; i < list4.Length; i++)
                        {
                            exc4[Convert.ToInt32(list4[i]) - 1] = 1;
                        }
                        tmpstr4 = tmpstr4.Split('(')[0];
                        if (tmpstr4 != "0") { System.Console.WriteLine("Неверный вызов, используйте /? для получения справки."); return; }
                    }
                }
                switch (args[3+delta].Split('=')[0]) { 
                    case "Template":
                        tmpstr5 = args[3 + delta].Split('=')[1];
                        set1 = false;
                        break;
                    case "DirTemplate":
                        tmpstr5 = args[3 + delta].Split('=')[1];
                        set1 = true;
                        break;
                    default:
                        System.Console.WriteLine("Неверный вызов, используйте /? для получения справки.");
                        return;    
                }
                if (string.Compare(args[4 + delta].Split('=')[0],"Path") == 0)
                    tmpstr7 = args[4 + delta].Split('=')[1];
                else
                {
                    System.Console.WriteLine("Неверный вызов, используйте /? для получения справки.");
                    return;
                }

                // перебираем оставщиеся необязательные параметры командной строки
                for (int i = 0; i < args.Length - delta - 5; i++)
                    {
                        switch (args[5 + delta + i].Split('=')[0])
                        {
                            case "Dwelling":
                                tmpstr10 = args[5 + delta + i].Split('=')[1];
                                break;
                            case "Syze":
                                tmpstr9 = args[5 + delta + i].Split('=')[1];
                                break;
                            case "Name":
                                tmpstr6 = args[5 + delta + i].Split('=')[1];
                                set2 = false;
                                break;
                            case "HaosWeek":
                                set4 = Convert.ToInt32(args[5 + delta + i].Split('=')[1]) == 1;
                                break;
                            case "Diff":
                                set3 = Convert.ToInt32(args[5 + delta + i].Split('=')[1]) != 1;
                                break;
                            case "RandomSeed":
                                tmpstr11 = args[5 + delta + i].Split('=')[1];
                                break;
                        default:
                                System.Console.WriteLine("Неверный вызов, используйте /? для получения справки.");
                                return;
                        }
                    }
                // запуск генерации без графики
                try
                {
                    frmMainMenu.Console_DoWork(Convert.ToInt32(args[0]),
                                               Convert.ToInt32(tmpstr1), exc1, Convert.ToInt32(tmpstr2), exc2, 
                                               Convert.ToInt32(tmpstr3), exc3, Convert.ToInt32(tmpstr4), exc4,
                                               set1, tmpstr5, set2, tmpstr6, tmpstr7, set3, set4, tmpstr9, tmpstr10, tmpstr11);
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine(ex.Message);
                    System.Console.WriteLine("Неверный вызов, используйте /? для получения справки.");
                }
            }
            if(args.Length == 0) {
                FreeConsole();
                Application.Run(fSplash);
            }
        }
    }
}