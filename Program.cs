using System.Reflection.Metadata;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

builder.Services.Configure<ApiBehaviorOptions>(options=>{
    options.InvalidModelStateResponseFactory = context=>{
        var errors = context.ModelState
                .Where(e=>e.Value.Errors.Count>0)
                .Select(e=> new {
                    Field = e.Key,
                    Errors = e.Value.Errors.Select(x=>x.ErrorMessage).ToArray()
                }).ToList();

             //   var errorString = string.Join(";",errors.Select(e=>$"{e.Field} : {string.Join(",",e.Message)}"));

                return new BadRequestObjectResult(new {
                    Message = "Validation Failed",
                    Errors = errors
                });
    };
});

var app = builder.Build();

if(app.Environment.IsDevelopment()){
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.MapGet("/",()=> Results.Ok(new {message = "Api Working Fine" }));

app.MapControllers();

app.Run();



