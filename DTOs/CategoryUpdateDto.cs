using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ecommerce_web_api.DTOs
{
    public class CategoryUpdateDto
    {
        
        [StringLength(100,MinimumLength =2,ErrorMessage ="category name must be between 2 to 100 characters")]
        public string Name {get; set;}

        [StringLength(500,ErrorMessage ="Category description cannot exceed 500 characters")]
        public string Description {get;set;} = string.Empty;
   
    }
}