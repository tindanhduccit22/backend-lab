using final.Data;
using Microsoft.EntityFrameworkCore;
using final.Controllers;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container. 
builder.Services.AddControllers();

builder.Services.AddMvc();
builder.Services.AddAuthorization();



builder.Services.AddDbContext<AppDbContext>(
options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();
if (app.Environment.IsDevelopment())
{
}
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();