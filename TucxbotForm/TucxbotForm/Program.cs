using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TucxbotForm
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
#if DEBUG
            NativeMethods.AllocConsole();
            Console.WriteLine("Debug Console");
#endif
            
            Application.Run(new MainForm());
            
#if DEBUG
            NativeMethods.FreeConsole();
#endif
        }
    }
}
