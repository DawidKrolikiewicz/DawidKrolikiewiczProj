using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;

using DawidKrolikiewiczProj.Data;
using DawidKrolikiewiczProj.Areas.Identity.Data;




/*  ----------------------  Builder  ----------------------  */
var builder = WebApplication.CreateBuilder(args);



/*  ---------------------  Services  ----------------------  */
builder.Services.AddDbContext<DawidKrolikiewiczProjContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DawidKrolikiewiczProjContext") ??
    throw new InvalidOperationException("Connection string 'DawidKrolikiewiczProjContext' not found.")));

// [Identity]
builder.Services.AddDbContext<DawidKrolikiewiczProjContextIdentity>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DawidKrolikiewiczProjContextIdentityConnection") ??
    throw new InvalidOperationException("Connection string 'DawidKrolikiewiczProjContextIdentityConnection' not found.")));

// [Identity]
builder.Services.AddDefaultIdentity<DawidKrolikiewiczProjUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<DawidKrolikiewiczProjContextIdentity>();


/*  ----------  Add services to the container.  -----------  */
builder.Services.AddControllersWithViews();

// [Identity]
builder.Services.AddRazorPages();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
});

/*  --------------------  App.Build()  --------------------  */
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days.
    // You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();
