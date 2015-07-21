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
﻿using Database.Model;

namespace CSharp.Playground.Tests.Model
{
    public class TestFactory
    {
        public static OrderItem CreateOrderItemWithCatalogItem(Int32 quantity, Decimal catalogItemPrice)
        {
            OrderItem orderItem = new OrderItem();
            orderItem.Quantity = quantity;
            CatalogItem catalogItem = new CatalogItem();
            catalogItem.Price = catalogItemPrice;
            orderItem.CatalogItem = catalogItem;
            return orderItem;
        }

        public static OrderItem CreateOrderItemWithCatalogItem(Int32 quantity, Decimal catalogItemPrice, Boolean taxable, Decimal taxRate)
        {
            OrderItem orderItem = CreateOrderItemWithCatalogItem(quantity, catalogItemPrice);
            orderItem.CatalogItem.Taxable = taxable;
            orderItem.CatalogItem.TaxRate = taxRate;
            return orderItem;
        }
    }
}
