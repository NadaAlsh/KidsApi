using KidsWebMvc.API;
using Microsoft.AspNetCore.Authentication.Cookies;

//var builder = WebApplication.CreateBuilder(args);

////Add services to the container.
//builder.Services.AddControllersWithViews();
//builder.Services.AddHttpClient<KidsApiClient>(client =>
//{
//  client.BaseAddress = new Uri(builder.Services.Configuration["ApiBaseUrl"]);

//});
//builder.Services.AddHttpContextAccessor();

//var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (!app.Environment.IsDevelopment())
//{
//    app.UseExceptionHandler("/BaitiApp/Error");
//    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
//    app.UseHsts();
//}

//app.UseHttpsRedirection();
//app.UseStaticFiles();

//app.UseRouting();

//app.UseAuthorization();

//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=BaitiApp}/{action=Login}/{id?}");

//app.Run();







var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();





// Configure HttpClient with base address
builder.Services.AddHttpClient<KidsApiClient>(client =>
{
  client.BaseAddress = new Uri(builder.Configuration["ApiBaseUrl"]);
});

// Add IHttpContextAccessor
builder.Services.AddHttpContextAccessor();

// Configure session
builder.Services.AddSession(options =>
{
  options.IdleTimeout = TimeSpan.FromMinutes(30);
  options.Cookie.HttpOnly = true;
  options.Cookie.IsEssential = true;
});

// Configure authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
      options.LoginPath = "/Account/Login";
    });

// Add CORS policy
builder.Services.AddCors(options =>
{
  options.AddPolicy("AllowAllOrigins",
      policyBuilder => policyBuilder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
  app.UseExceptionHandler("/Home/Error");
  app.UseHsts();
}
app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseCors("AllowAllOrigins");



app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=BaitiApp}/{action=Login}/{id?}");

app.Run();




