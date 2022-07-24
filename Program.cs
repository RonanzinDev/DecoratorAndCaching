using DecoratorAndCaching.Stores;
using DecoratorAndCaching.Stores.Caching;
using Microsoft.OpenApi.Models;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddMemoryCache();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo()
    {
        Version = "v1",
        Title = "Cache Strategy",
        Description = "Swagger Surface",
        Contact = new OpenApiContact()
        {
            Name = "Gabriel Ronan",
            Email = "morellianogm@gmail.com",
        }
    });
});

builder.Services.AddScoped<ICarStore, CarStore>();
EnableDecorator(builder.Services);
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

void EnableDecorator(IServiceCollection services)
{
    services.AddScoped<CarStore>();
    services.AddScoped<ICarStore, CarCachingDecorator<CarStore>>();
}