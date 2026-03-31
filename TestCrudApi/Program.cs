using Microsoft.EntityFrameworkCore;
using TestCrudApi.Models;
using TestCrudApi.Services;

var builder = WebApplication.CreateBuilder(args);

var CorsPolicy = "TestCrudCors";

builder.Services.AddCors(op =>
    op.AddPolicy(CorsPolicy,
        build =>
        {
            build.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
        })
);
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddDbContext<TestCrudContext>(options =>
    options.UseSqlServer("ConnectionString"));
builder.Services.AddDataProtection();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("TestCrudCors");
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
