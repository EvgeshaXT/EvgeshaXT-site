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
    string path = context.Request.Path.Value ?? "";

    string culture = "";
    string selectedCulture = context.Request.Query["culture"].ToString();

    if (selectedCulture == "")
    {
        string cookieCulture = context.Request.Cookies["culture"] ?? "";

        if (cookieCulture == "ru" || cookieCulture == "en") { culture = cookieCulture; }
        else culture = GetCultureFromRequest(context);

        CultureInfo cultureInfo = new(culture);
        CultureInfo.CurrentCulture = cultureInfo;
        CultureInfo.CurrentUICulture = cultureInfo;

        bool isRuPath = path.StartsWith("/ru", StringComparison.OrdinalIgnoreCase);
        bool isRuCulture = culture == "ru";

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
    else
    {
        if (selectedCulture == "ru" || selectedCulture == "en")
        {
            culture = selectedCulture;
            SaveCultureToCookie(context, culture);
        }
        else culture = GetCultureFromRequest(context);

        CultureInfo cultureInfo = new(culture);
        CultureInfo.CurrentCulture = cultureInfo;
        CultureInfo.CurrentUICulture = cultureInfo;

        string newPath = culture == "ru" ? GetRuPath(path) : GetEnPath(path);
        context.Response.Redirect(newPath);

        return;
    }

    await next();
});

app.MapRazorPages();

VideoManager.Initialize();

app.Run();

string GetCultureFromRequest(HttpContext context)
{
    string culture = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;

    if (culture == "ru" || culture == "be" || culture == "uk") culture = "ru";
    else culture = "en";

    SaveCultureToCookie(context, culture);
    return culture;
}

void SaveCultureToCookie(HttpContext context, string culture)
{
    CookieOptions options = new()
    {
        MaxAge = TimeSpan.FromDays(400),
        HttpOnly = true,
        IsEssential = true
    };

    context.Response.Cookies.Append("culture", culture, options);
}

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