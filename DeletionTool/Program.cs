using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DeletionTool
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            using (System.Threading.Mutex mutex = new System.Threading.Mutex(true, "790586ef-8565-4f70-9aa6-639b9c6c25db", out var createNew))
            {
                if (createNew)
                {
                    Application.Run(new FormMain());
                }
                else
                {
                    Environment.Exit(1);
                }
            }

        }
    }
}
