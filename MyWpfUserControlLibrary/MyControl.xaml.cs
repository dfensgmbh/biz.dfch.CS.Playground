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
using System.Windows;
using System.Windows.Controls;
using EA;

namespace MyWpfUserControlLibrary
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class MyControl : UserControl
    {
        public MyControl()
        {
            InitializeComponent();
        }

        private void MyButton_Click(object sender, RoutedEventArgs e)
        {
            var pathToEaRepo = "C:\\src\\biz.dfch.PS.EnterpriseArchitect.Scripts\\src\\SampleModel.eapx";
            var eaRepo = new Repository();

            eaRepo.OpenFile(pathToEaRepo);

            MyTextBox.Text = eaRepo.ConnectionString;

            eaRepo.CloseFile();
            eaRepo.Exit();
        }
    }
}
