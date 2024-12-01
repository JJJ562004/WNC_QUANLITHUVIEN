using Microsoft.EntityFrameworkCore;
using BaoCaoCuoiKi_QuanLyThuVien.Models;
using BaoCaoCuoiKi_QuanLyThuVien.Data;
using Microsoft.OpenApi.Models;

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
    // Define a Swagger document for each group
    options.SwaggerDoc("Books", new OpenApiInfo
    {
        Title = "Library Management API - Books",
        Version = "v1",
        Description = "API for managing books"
    });

    options.SwaggerDoc("Students", new OpenApiInfo
    {
        Title = "Library Management API - Students",
        Version = "v1",
        Description = "API for managing students"
    });

    options.SwaggerDoc("Staffs", new OpenApiInfo
    {
        Title = "Library Management API - Staffs",
        Version = "v1",
        Description = "API for managing staffs"
    });

    options.SwaggerDoc("BorrowingRecordsAPI", new OpenApiInfo
    {
        Title = "Library Management API - Borrowing Records",
        Version = "v1",
        Description = "API for managing borrowing Records"
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
        c.SwaggerEndpoint("/swagger/Books/swagger.json", "Books API");
        c.SwaggerEndpoint("/swagger/Students/swagger.json", "Students API");
        c.SwaggerEndpoint("/swagger/Staffs/swagger.json", "Staffs API");
        c.SwaggerEndpoint("/swagger/BorrowingRecordsAPI/swagger.json", "Borrowing Records API");
        c.RoutePrefix = string.Empty; // Optional: Set Swagger UI at the root
    });
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

// Map controllers
app.MapControllers();

app.Run();
