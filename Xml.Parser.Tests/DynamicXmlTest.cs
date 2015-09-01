/**
 * Copyright 2015 Marc Rufer, d-fens GmbH
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
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Xml.Parser.Tests
{
    [TestClass]
    public class DynamicXmlTest
    {
        [TestMethod]
        public void DynamicDeserializationTest()
        {
            dynamic result = DynamicXml.Parse(CreateSampleXml());
            var id = result.Student[0].ID;
            Assert.AreEqual("100", result.Student[0].ID);
            Assert.AreEqual("Lily", result.Student[1].Name);
        }

        private String CreateSampleXml()
        {
            return@"<Students>
                <Student ID=""100"">
                    <Name>Paul</Name>
                    <Mark>90</Mark>
                </Student>
                <Student>
                    <Name>Lilly</Name>
                    <Mark>80</Mark>
                </Student>
            </Students>";
        }
    }
}
