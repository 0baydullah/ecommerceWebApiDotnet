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
        

        [HttpGet("{categoryId:guid}")]
        public IActionResult GetCategoriesById(Guid categoryId){
           
            var foundCategory = categories.FirstOrDefault(c=>c.CategoryId == categoryId);
            if(foundCategory == null){
                return NotFound(ApiResponse<object>.ErrorResponse(new List<string>{"Category is not found with this id"},404,"validation failed"));
            }
            var categoryReadDto =new CategoryReadDto{
                CategoryId = foundCategory.CategoryId,
                Name = foundCategory.Name,
                Description = foundCategory.Description,
                CreatedAt = foundCategory.CreatedAt
            };
            return Ok(ApiResponse<CategoryReadDto>.SuccessResponse(categoryReadDto,200,"Category returned successfully"));
        }

        
        [HttpGet]
        public IActionResult GetCategories([FromQuery] string searchValue=""){
           

            var categoryList = categories.Select(c=> new CategoryReadDto{
                CategoryId = c.CategoryId,
                Name = c.Name,
                Description = c.Description,
                CreatedAt = c.CreatedAt
            }).ToList();
            return Ok(ApiResponse<List<CategoryReadDto>>.SuccessResponse(categoryList,200,"Categorires returned successfully"));
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
            return Created(nameof(GetCategoriesById),ApiResponse<CategoryReadDto>.SuccessResponse(categoryReadDto,201,"Categorires created successfully"));  

        }


        [HttpPut("{categoryId:guid}")]
        public IActionResult UpdateCategoryById(Guid categoryId, [FromBody] CategoryUpdateDto categoryData){
            var foundCategory = categories.FirstOrDefault(category => category.CategoryId == categoryId);
    
            if(foundCategory == null){
                return NotFound(ApiResponse<object>.ErrorResponse(new List<string>{"Category is not found with this id"},404,"validation failed"));
            }
            if(categoryData == null){
                return BadRequest("category data is missing");
            }

            
            foundCategory.Name = categoryData.Name;
            foundCategory.Description = categoryData.Description;
            
            
            return Ok(ApiResponse<object>.SuccessResponse(null,204,"Categorires updated successfully"));
        }

        
        
        [HttpDelete("{categoryId:guid}")]
        public IActionResult DeleteCategories(Guid categoryId){
            var foundCategory = categories.FirstOrDefault(category => category.CategoryId == categoryId);
    
            if(foundCategory == null){
                return NotFound(ApiResponse<object>.ErrorResponse(new List<string>{"Category is not found with this id"},404,"validation failed"));
            }

            categories.Remove(foundCategory);
            return Ok(ApiResponse<object>.SuccessResponse(null,204,"Categorires deleted successfully"));
        }
    }
}