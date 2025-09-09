<<<<<<< HEAD
﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AZ_DemoCosmosDB.Models
{
    public class Product
    {
        [JsonProperty("id")]
        public string? Id { get; set; }

        [JsonProperty("category")]
        public string? Category { get; set; }

        public string? Name { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public bool Clearance { get; set; }
    }
}
=======
﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AZ_DemoCosmosDB.Models
{
    public class Product
    {
        [JsonProperty("id")]
        public string? Id { get; set; }

        [JsonProperty("category")]
        public string? Category { get; set; }

        public string? Name { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public bool Clearance { get; set; }
    }
}
>>>>>>> e0a126255fa52286222261f3e7132fcc5ff91e1a
