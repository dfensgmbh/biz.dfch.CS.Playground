/**
 * Copyright 2018 d-fens GmbH
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using System.Threading;
using MyWpfUserControlLibrary;

namespace MyConsoleApp
{
    class Program
    {
        //[STAThread]
        static void Main(string[] args)
        {
            Console.WriteLine("Creating thread that creates and shows WPF window ...");

            var worker = new Worker();

            var myThread = new Thread(worker.CreateAndShowMyWindow);
            myThread.SetApartmentState(ApartmentState.STA);

            Console.WriteLine("Starting thread that creates and shows WPF window ...");
            myThread.Start();

            Console.WriteLine("Join thread that shows WPF window ...");
            myThread.Join();

            Console.WriteLine("Exit Main method");
        }
    }

    public class Worker
    {
        /**
         * Creating and showing a WPF Window, which is defined in a WPF user control library, from another non WPF executable
         */
        public void CreateAndShowMyWindow()
        {
            //CustomApplication app = new CustomApplication();
            //app.Run();

            var myWindow = new MyWindow();
            myWindow.ShowDialog();
        }
    }
}
