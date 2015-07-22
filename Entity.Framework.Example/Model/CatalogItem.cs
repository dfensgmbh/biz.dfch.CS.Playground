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

namespace biz.dfch.CS.Playground.Entity.Framework.Example.Model
{
    public class CatalogItem
    {
        [Key]        
        public Int32 Id { get; set; }
        // DFCHECK check if the following two properties are necessary (Maybe embedded in TOSCA?)
        [Required]
        public String Name { get; set; }
        public String Description { get; set; }
        [Required]
        public String Version { get; set; }
        [Required]
        public String CreatedBy { get; set; }
        [Required]
        public DateTimeOffset CreatedAt { get; set; }
        public String LastModifiedBy { get; set; }
        public DateTimeOffset LastModifiedAt { get; set; }
        public String Status { get; set; }

        public DateTimeOffset ValidFrom { get; set; }
        public DateTimeOffset ValidTo { get; set; }

        // DFCHECK check, if price could be simple or if it is a price per unit or even more complex
        public Decimal Price { get; set; }
        public Boolean Taxable { get; set; }
        public Decimal TaxRate { get; set; }

        [Required]
        public String ToscaDefinition { get; set; }

        // DFCHECK check, if nesting catalog items is needed (Maybe a list of catalogItems?)
        public virtual CatalogItem catalogItem { get; set; }
    }
}
