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
using System.Linq;
﻿using Database.Context;
﻿using Database.Model;

namespace Database
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var db = new OrderingContext())
            {
                // Create and save a new Catalog
                Console.Write("Enter a name for a new Catalog: ");
                var name = Console.ReadLine();

                var catalog = new Catalog
                {
                    Name = name,
                    CreatedBy = "testuser",
                    CreatedAt = DateTimeOffset.Now
                };
                db.Catalogs.Add(catalog);
                db.SaveChanges();

                // Display all Catalogs from the database 
                var query = from b in db.Catalogs
                            orderby b.Name
                            select b;

                Console.WriteLine("All catalogs stored in the database:");
                foreach (var item in query)
                {
                    Console.WriteLine(item.Name);
                }

                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
            }
        } 
    }
}
