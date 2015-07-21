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
﻿using System.ComponentModel.DataAnnotations;

namespace Database.Model
{
    public class OrderItem
    {
        [Key]
        public Int32 Id { get; set; }
        // DFCHECK CreatedBy and CreatedAt for OrderItem needed?
        [Required]
        public String CreatedBy { get; set; }
        [Required]
        public DateTimeOffset CreatedAt { get; set; }
        public String LastModifiedBy { get; set; }
        public DateTimeOffset LastModifiedAt { get; set; }
        public String Status { get; set; }

        [Required]
        public Int32 Quantity { get; set; }

        public virtual CatalogItem CatalogItem { get; set; }

        public Decimal GetPrice()
        {
            return CatalogItem.Price * Quantity;
        }
    }
}
