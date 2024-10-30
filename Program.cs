using System.Reflection.Metadata;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if(app.Environment.IsDevelopment()){
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


List<Category> categories = new List<Category>();



app.MapGet("/",()=> Results.Ok(new {message = "Api Working Fine" }));

app.MapGet("/api/categories",([FromQuery] string searchValue="")=>{
    if(!string.IsNullOrEmpty(searchValue)){
        var searchedCategory = categories.Where(c=>!string.IsNullOrEmpty(c.Name) && c.Name.Contains(searchValue,StringComparison.OrdinalIgnoreCase)).ToList();
        return Results.Ok(searchedCategory);
    }
    return Results.Ok(categories);
});



app.MapPost("/api/categories",([FromBody] Category categoryData)=>{
    
    if(string.IsNullOrEmpty(categoryData.Name)){
        return Results.BadRequest("Category name is required");
    }

    if(!string.IsNullOrEmpty(categoryData.Name)){
        if(categoryData.Name.Length<2)
            return Results.BadRequest("category name should be longger than 2");
    }

    var newCategory = new Category{
        CategoryId = Guid.NewGuid(),
        Name = categoryData.Name,
        Description = categoryData.Description,
        CreatedAt = DateTime.UtcNow,
    };
    categories.Add(newCategory);
    return Results.Created($"/api/categories/{newCategory.CategoryId}",newCategory);  
});



app.MapPut("/api/categories/{categoryId:guid}",(Guid categoryId, [FromBody] Category categoryData)=>{
    var foundCategory = categories.FirstOrDefault(category => category.CategoryId == categoryId);
    
    if(foundCategory == null){
        return Results.NotFound("Category with this id does not exist");
    }
    if(categoryData == null){
        return Results.BadRequest("category data is missing");
    }

    if(!string.IsNullOrWhiteSpace(categoryData.Name)){
        if(categoryData.Name.Length>2)
            foundCategory.Name = categoryData.Name;
        else return Results.BadRequest("category name should be longger than 2");
    }
    
    if(!string.IsNullOrWhiteSpace(categoryData.Name)){
        foundCategory.Description = categoryData.Description;
    }
    
    return Results.Ok();
});



app.MapDelete("/api/categories/{categoryId:guid}",(Guid categoryId)=>{
    var foundCategory = categories.FirstOrDefault(category => category.CategoryId == categoryId);
    
    if(foundCategory == null){
        return Results.NotFound("Category with this id does not exist");
    }

    categories.Remove(foundCategory);
    return Results.NoContent();
});



app.Run();


public record Category{
    public Guid CategoryId {get; set;}
    public String Name {get; set;}
    public String Description {get; set;} = string.Empty;
    public DateTime CreatedAt {get; set;}
};
