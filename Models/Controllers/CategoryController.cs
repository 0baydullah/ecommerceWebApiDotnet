using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ecommerce_web_api.Models.Controllers
{
    [ApiController]
    [Route("/api/categories")]
    public class CategoryController:ControllerBase
    {
        private static List<Category> categories = new List<Category>();
        
        
        [HttpGet]
        public IActionResult GetCategories([FromQuery] string searchValue=""){
            if(!string.IsNullOrEmpty(searchValue)){
            var searchedCategory = categories.Where(c=>!string.IsNullOrEmpty(c.Name) && c.Name.Contains(searchValue,StringComparison.OrdinalIgnoreCase)).ToList();
                return Ok(searchedCategory);
            }
            return Ok(categories);
        }


        [HttpPost]
        public IActionResult CreateCategories([FromBody] Category categoryData){
            if(string.IsNullOrEmpty(categoryData.Name)){
                return BadRequest("Category name is required");
            }

            if(!string.IsNullOrEmpty(categoryData.Name)){
                if(categoryData.Name.Length<2)
                    return BadRequest("category name should be longger than 2");
            }

            var newCategory = new Category{
                CategoryId = Guid.NewGuid(),
                Name = categoryData.Name,
                Description = categoryData.Description,
                CreatedAt = DateTime.UtcNow,
            };
            categories.Add(newCategory);
            return Created($"/api/categories/{newCategory.CategoryId}",newCategory);  

        }


        [HttpPut("{categoryId:guid}")]
        public IActionResult UpdateCategoryById(Guid categoryId, [FromBody] Category categoryData){
            var foundCategory = categories.FirstOrDefault(category => category.CategoryId == categoryId);
    
            if(foundCategory == null){
                return NotFound("Category with this id does not exist");
            }
            if(categoryData == null){
                return BadRequest("category data is missing");
            }

            if(!string.IsNullOrWhiteSpace(categoryData.Name)){
                if(categoryData.Name.Length>2)
                    foundCategory.Name = categoryData.Name;
                else return BadRequest("category name should be longger than 2");
            }
            
            if(!string.IsNullOrWhiteSpace(categoryData.Name)){
                foundCategory.Description = categoryData.Description;
            }
            
            return Ok();
        }

        
        
        [HttpDelete("{categoryId:guid}")]
        public IActionResult DeleteCategories(Guid categoryId){
            var foundCategory = categories.FirstOrDefault(category => category.CategoryId == categoryId);
    
            if(foundCategory == null){
                return NotFound("Category with this id does not exist");
            }

            categories.Remove(foundCategory);
            return NoContent();
        }
    }
}