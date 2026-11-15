using System.Globalization;
using Microsoft.AspNetCore.Localization;

WebApplicationBuilder? builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorPages().AddViewLocalization();
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

WebApplication? app = builder.Build();

app.UseStaticFiles();

var supportedCultures = new[]
{
    new CultureInfo("en"),
    new CultureInfo("ru")
};
app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("en"),
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures
});

app.Use(async (context, next) =>
{
    string selectedCulture = context.Request.Query["culture"].ToString();
    string path = context.Request.Path.Value ?? "";

    if (selectedCulture != "")
    {
        CookieOptions options = new()
        {
            MaxAge = TimeSpan.FromDays(400),
            HttpOnly = true,
            IsEssential = true
        };

        context.Response.Cookies.Append("culture", selectedCulture, options);

        CultureInfo cultureInfo = new(selectedCulture);
        CultureInfo.CurrentCulture = cultureInfo;
        CultureInfo.CurrentUICulture = cultureInfo;

        string newPath = selectedCulture == "ru" ? GetRuPath(path) : GetEnPath(path);
        context.Response.Redirect(newPath);

        return;
    }
    else
    {
        string cookieCulture = context.Request.Cookies["culture"] ?? "";
        string culture = cookieCulture != "" ? cookieCulture : CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;

        CultureInfo cultureInfo = new(culture);
        CultureInfo.CurrentCulture = cultureInfo;
        CultureInfo.CurrentUICulture = cultureInfo;

        bool isRuPath = path.StartsWith("/ru", StringComparison.OrdinalIgnoreCase);
        bool isRuCulture = culture == "ru" || culture == "be" || culture == "uk";

        if (isRuCulture && !isRuPath)
        {
            context.Response.Redirect(GetRuPath(path));
            return;
        }
        else if (!isRuCulture && isRuPath)
        {
            context.Response.Redirect(GetEnPath(path));
            return;
        }
    }

    await next();
});

app.MapRazorPages();

VideoManager.Initialize();

app.Run();

string GetRuPath(string path)
{
    if (path.StartsWith("/ru", StringComparison.OrdinalIgnoreCase)) return path;
    return "/ru" + path;
}
string GetEnPath(string path)
{
    string newPath = path.Replace("/ru", "");
    
    if (newPath == "") return "/";
    return newPath;
}