﻿/**
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
