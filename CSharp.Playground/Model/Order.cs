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
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Database.Model
{
    public class Order
    {
        [Key]
        public Int32 Id { get; set; }
        [Required]
        public String CreatedBy { get; set; }
        [Required]
        public DateTimeOffset OrderDate { get; set; }
        public String LastModifiedBy { get; set; }
        public DateTimeOffset LastModifiedAt { get; set; }
        public String Status { get; set; }

        // DFCHECK separate order number necessary?
        [Required]
        public Int32 OrderNumber { get; set; }
        [Required]
        public String UserId { get; set; }
        public Address BillingAddress { get; set; }
        [MaxLength(3)]
        public String Currency { get; set; }
        [Required]
        public Boolean TaxesIncluded { get; set; }

        // DFCHECK Transaction data necessary?
        public virtual List<OrderItem> OrderItems { get; set; }


        public Decimal GetTotalOrderItemsPrice()
        {
            Decimal totalOrderItemsPrice = 0;
            foreach (var orderItem in OrderItems)
            {
                totalOrderItemsPrice += orderItem.GetPrice();
            }
            return totalOrderItemsPrice;
        }

        public Decimal GetTotalPrice()
        {
            return GetTotalOrderItemsPrice() + GetTotalTax();
        }

        public Decimal GetTotalTax()
        {
            // DFCHECK Check how to calculate taxes (Several taxes per orderItem possible?)
            Decimal totalTax = 0;
            foreach (var orderItem in OrderItems)
            {
                if (orderItem.CatalogItem.Taxable)
                {
                    totalTax += orderItem.GetPrice() * orderItem.CatalogItem.TaxRate;
                }
            }
            return totalTax;
        }
    }
}
