using MarksManagementSystem.Data;
using MarksManagementSystem.Data.Repositories.Classes;
using MarksManagementSystem.Data.Repositories.Interfaces;
using MarksManagementSystem.Helpers;
using MarksManagementSystem.Services.Classes;
using MarksManagementSystem.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddDbContext<MarksManagementContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MarksManagementSystem")));

// Dependency injection for repositories
builder.Services.AddScoped<ICourseRepository, CourseRepository>();
builder.Services.AddScoped<ITutorRepository, TutorRepository>();
builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped<ICourseTutorRepository, CourseTutorRepository>();
builder.Services.AddScoped<ICourseStudentRepository, CourseStudentRepository>();
builder.Services.AddScoped<IPasswordCreator, PasswordCreator>();

// Dependency injection for services
builder.Services.AddScoped<IViewAllTutorsService, ViewAllTutorsService>();
builder.Services.AddScoped<IAddTutorService, AddTutorService>();
builder.Services.AddScoped<IAddStudentService, AddStudentService>();
builder.Services.AddScoped<IViewAllStudentsService, ViewAllStudentsService>();  
builder.Services.AddScoped<IAddCourseService, AddCourseService>();
builder.Services.AddScoped<ICourseStudentManagementService, CourseStudentManagementService>();
builder.Services.AddScoped<IEditCourseService, EditCourseService>();
builder.Services.AddScoped<IViewAllCoursesService, ViewAllCoursesService>();
builder.Services.AddScoped<IViewCourseService, ViewCourseService>();
builder.Services.AddScoped<ILoginService, LoginService>();
builder.Services.AddScoped<IChangePasswordService, ChangePasswordService>();






//Configure Authorization and Authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.SlidingExpiration = true;
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Forbidden/";
        options.Cookie.IsEssential = true;
        options.Cookie.SameSite = SameSiteMode.Strict;
        options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
        options.Cookie.MaxAge = TimeSpan.FromMinutes(30);
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy =>
    {
        policy.RequireClaim("Role", "Admin");
    });

    options.AddPolicy("Tutor", policy =>
    {
        policy.RequireClaim("UserType", "Tutor");
    });
});



var app = builder.Build();

await EnsureDbCreated(app.Services, app.Logger);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();

async Task EnsureDbCreated(IServiceProvider services, ILogger logger)
{
    using var db = services.CreateScope()
        .ServiceProvider.GetRequiredService<MarksManagementContext>();
    await db.Database.MigrateAsync();
}
