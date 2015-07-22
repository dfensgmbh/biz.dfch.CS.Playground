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
using System.ComponentModel.DataAnnotations;

namespace biz.dfch.CS.Playground.Entity.Framework.Example.Model
{
    public class Address
    {
        [Key]
        public Int32 Id { get; set; }

        [Required]
        public String Name { get; set; }
        [Required]
        public String LastName { get; set; }
        [Required]
        public String Address1 { get; set; }
        public String Address2 { get; set; }
        [MaxLength(5)]
        [Required]
        public String ZIP { get; set; }
        [Required]
        public String City { get; set; }
        public String Company { get; set; }
        [Required]
        public String Country { get; set; }
        [Required]
        public String Phone { get; set; }
    }
}
