using Microsoft.AspNetCore.Authentication.Cookies;
using WebApplicationLab5.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// ��������� ����� ���������� � ���������������
builder.Services.AddControllersWithViews();

// ��������� HttpClient ��� Auth0Service
builder.Services.AddHttpClient<IAuth0Service, Auth0Service>();

// ��������� �������������� � ������������� ���
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = "Auth0Cookie";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
        options.LoginPath = "/Auth/Login";
        options.LogoutPath = "/Auth/Logout";
    });

// ��������� �����������
builder.Services.AddAuthorization();

var app = builder.Build();

// ������������ ������� ������� HTTP-������
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// ��������� �������������� �� ����������� �� �������
app.UseAuthentication(); // ������� ���� ����� UseAuthorization()
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
