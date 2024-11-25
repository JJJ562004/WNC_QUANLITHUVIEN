using Microsoft.EntityFrameworkCore;
using BaoCaoCuoiKi_QuanLyThuVien.Models;
using BaoCaoCuoiKi_QuanLyThuVien.Data;

var builder = WebApplication.CreateBuilder(args);

// Database Configuration
builder.Services.AddDbContext<LibraryDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("QuanLyThuVien")));

// Add services to the container
builder.Services.AddControllersWithViews();

// Enable CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

// Configure JSON options
builder.Services.AddControllers()
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
            options.JsonSerializerOptions.MaxDepth = 64;
        });

// Configure Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Library Management API",
        Version = "v1",
        Description = "API documentation for the Library Management System",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "Pham Thanh Hieu",
            Email = "your-email@example.com"
        }
    });
});

var app = builder.Build();

// Use CORS policy
app.UseCors("AllowAll");

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Library Management API v1");
        c.RoutePrefix = string.Empty; // Set Swagger UI at the app's root (optional)
    });
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

// Map controllers
app.MapControllers();

app.Run();
