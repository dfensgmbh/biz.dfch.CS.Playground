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
﻿using System;
using System.Collections.Generic;
﻿using System.Diagnostics;
﻿using System.Linq;
using System.Text;
using System.Threading.Tasks;
﻿using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Entity.Framework.Example.Tests
{
    [TestClass]
    public class ListOfDictionaryTest
    {
        [TestMethod]
        public void GetDictionaryValueFromListOfDictionaries()
        {
            IList<IDictionary<String, Object>> data = new List<IDictionary<String, Object>>();
            for (int i = 0; i < 3; i++)
            {
                var test = new Dictionary<String, Object>
                {
                    { "apiMessageId", "aaa" + i % 4 },
                    { "bbb", "bbb" + i % 4 },
                    { "ccc", "ccc" + i % 4 },
                    { "ddd", "ddd" + i % 4 },
                    { "eee", "eee" + i % 4 },
                    { "fff", "fff" + i % 4 },
                    { "ggg", "ggg" + i % 4 },
                    { "hhh", "hhh" + i % 4 },
                    { "iii", "iii" + i % 4 }
                };
                data.Add(test);
            }
            var x = data.SelectMany(m => m)
                .FirstOrDefault(m => m.Key == "apiMessageId");
            
            Assert.AreEqual("aaa0", x.Value);
        }
    }
}
