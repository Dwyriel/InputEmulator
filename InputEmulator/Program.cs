using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InputEmulator
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MainWindow());
            }
            catch (Exception e)
            {
                string errorText = "Something went wrong, make sure the program is outside of " + '"' + "Program Files" + '"' + " folder and its subfolders or run the program as an Administrator.";
                ErrorLogger.ShowErrorTextWithExceptionMessage(errorText, e);
            }
        }
    }
}
