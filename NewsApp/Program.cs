using Microsoft.EntityFrameworkCore;
using NewsApp.Repositories.Abstract;
using NewsApp.Models.Domain;
using NewsApp.Repositories.Abstract;
using NewsApp.Repositories.Implementation;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using System.Reflection;
using Microsoft.AspNetCore.Identity;
using System.Configuration;
using NewsApp.Models.DTO;

var builder = WebApplication.CreateBuilder(args);



// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddScoped<INewsService, NewsService>();
builder.Services.AddScoped<IUserAuthenticationService, UserAuthenticationService>();
builder.Services.AddScoped<ICommentService, CommentService>();
builder.Services.AddScoped<ApplicationUser>();

builder.Services.AddTransient<NewsCommentViewModel, NewsCommentViewModel>();


builder.Services.AddDbContext<DatabaseContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("conn")));



// for identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<DatabaseContext>()
    .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(op => op.LoginPath = "/UserAuthentication/Login");

// this is for getting login user information
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

app.Use(async (context, next) =>
{
    await next();

    if (context.Response.StatusCode == 404)
    {
        context.Request.Path = "/Home/Index";
        await next();
    }
});

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();



app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");




app.Run();
