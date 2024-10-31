using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ecommerce_web_api.DTOs;
using ecommerce_web_api.Models;
using Microsoft.AspNetCore.Mvc;

namespace ecommerce_web_api.Controllers
{
    [ApiController]
    [Route("/api/categories")]
    public class CategoryController:ControllerBase
    {
        private static List<Category> categories = new List<Category>();
        
        
        [HttpGet]
        public IActionResult GetCategories([FromQuery] string searchValue=""){
            // if(!string.IsNullOrEmpty(searchValue)){
            // var searchedCategory = categories.Where(c=>!string.IsNullOrEmpty(c.Name) && c.Name.Contains(searchValue,StringComparison.OrdinalIgnoreCase)).ToList();
            //     return Ok(searchedCategory);
            // }

            var categoryList = categories.Select(c=> new CategoryReadDto{
                CategoryId = c.CategoryId,
                Name = c.Name,
                Description = c.Description,
                CreatedAt = c.CreatedAt
            }).ToList();
            return Ok(categoryList);
        }


        [HttpPost]
        public IActionResult CreateCategories([FromBody] CategoryCreateDto categoryData){
            

            var newCategory = new Category{
                CategoryId = Guid.NewGuid(),
                Name = categoryData.Name,
                Description = categoryData.Description,
                CreatedAt = DateTime.UtcNow,
            };
            categories.Add(newCategory);

            var categoryReadDto =  new CategoryReadDto{
                CategoryId = newCategory.CategoryId,
                Name = newCategory.Name,
                Description = newCategory.Description,
                CreatedAt = newCategory.CreatedAt
            };
            return Created($"/api/categories/{newCategory.CategoryId}",categoryReadDto);  

        }


        [HttpPut("{categoryId:guid}")]
        public IActionResult UpdateCategoryById(Guid categoryId, [FromBody] CategoryUpdateDto categoryData){
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