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

using biz.dfch.CS.Playground.Entity.Framework.Example.Model;
using biz.dfch.CS.Playground.Entity.Framework.Example.Tests.Factory;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace biz.dfch.CS.Playground.Entity.Framework.Example.Tests.Model
{
    [TestClass]
    public class OrderItemTest
    {
        [TestMethod]
        public void GetPriceReturnsCalculatedPriceOfOrderItem()
        {
            OrderItem orderItem = TestFactory.CreateOrderItemWithCatalogItem(2, 5.25m);

            Assert.AreEqual(10.5m, orderItem.GetPrice());
        }
    }
}
