using Microsoft.EntityFrameworkCore;
using SampleDatabase004;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options => options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

var connection = builder.Configuration
    .GetConnectionString("SampleDatabaseSecret");

builder.Services.AddDbContext<Db2netr004Context>(options => options.UseSqlServer(connection));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.MapGet("/products/{id}", (Db2netr004Context context, int id) =>
{
    Product product = context.Products.Where(product => product.ProductId == id).Include(product => product.ProductCategory).Include(product => product.ProductModel).SingleOrDefault();
    return product;
})
.WithOpenApi();

app.Run();

