using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyWpfUserControlLibrary;

namespace MyConsoleApp
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            RunApplication();
        }

        /**
         * Starting a WPF Window, which is in a WPF user control library, from another non WPF executable
         */
        private static void RunApplication()
        {
            //CustomApplication app = new CustomApplication();
            //app.Run();

            var myWindow = new MyWindow();
            myWindow.ShowDialog();
        }
    }
}
