using Microsoft.Win32;
using System;
using System.Media;
using System.Windows.Forms;
using YYProject.AdvancedDeletion;

namespace DeletionTool
{
    static class Program
    {
        #region Doubts
        /*[DllImport("winmm.dll", EntryPoint = "sndPlaySound")]
          [return: MarshalAs(UnmanagedType.Bool)]
          private static extern Boolean SndPlaySound(String pszSound, Int32 fuSound);
          private const Int32 SND_SYSTEM = 0x00200000;
          
          SndPlaySound("EmptyRecycleBin", SND_SYSTEM );  //The sound that was played is wrong, but I don't know why……
        */
        #endregion



        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main(String[] args)
        {
            using (System.Threading.Mutex mutex = new System.Threading.Mutex(true, "790586ef-8565-4f70-9aa6-639b9c6c25db", out var createNew))
            {
                if (createNew)
                {
                    if (args.Length > 0)
                    {
                        var path = args[0];
                        if (AdvancedDeletion.IsExists(path, out var hasDot))
                        {
                            AdvancedDeletion.Delete(path);
                            var sound = new SoundPlayer(Registry.CurrentUser.OpenSubKey(@"AppEvents\Schemes\Apps\Explorer\EmptyRecycleBin\.Current", false).GetValue(null) as String);
                            sound.PlaySync();
                        }
                        return;
                    }
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(new FormMain());
                }
                else
                {
                    Environment.Exit(1);//It's not required but can send system a non-normal exit signal……
                }
            }
        }
    }
}
