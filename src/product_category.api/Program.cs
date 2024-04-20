using product_category.api.Apis;
using product_category.api.Application;
using product_category.api.Filters;

var builder = WebApplication.CreateBuilder(args);

builder.AddApplication();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGroup("/api/v1/product-category").AddFluentValidationFilter().WithTags("Product-Category API").MapProductCategoryApi();

app.Run();