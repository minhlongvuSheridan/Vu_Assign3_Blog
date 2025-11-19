using Microsoft.EntityFrameworkCore;
using Vu_Assign3_Blog.Core.Interfaces;
using Vu_Assign3_Blog.Infrastructure.Data;
using Vu_Assign3_Blog.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers().AddNewtonsoftJson();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContext<Assign3DbContext>(options =>
options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddScoped<IPostRepository, PostRepository>();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
app.MapOpenApi();
}
app.UseAuthorization();
app.MapControllers();
app.Run();


