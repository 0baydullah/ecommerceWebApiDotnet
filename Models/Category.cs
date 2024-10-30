using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ecommerce_web_api.Models
{
    public class Category{
        public Guid CategoryId {get; set;}
        public String Name {get; set;}
        public String Description {get; set;} = string.Empty;
        public DateTime CreatedAt {get; set;}
    }
}