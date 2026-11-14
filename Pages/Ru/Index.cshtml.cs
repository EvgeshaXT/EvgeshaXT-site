using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EvgeshaXT.Pages.Ru;

public class IndexModel : PageModel
{
    public string DayPartMessage { get; private set; } = "";

    public void OnGet()
    {
        DateTime time = DateTime.Now;

        if (time.Hour >= 22 || time.Hour < 6) DayPartMessage = "🌃 Доброй ночи..~";
        else if (time.Hour >= 6 && time.Hour < 12) DayPartMessage = "🌅 Доброе утро!";
        else if (time.Hour >= 12 && time.Hour < 17) DayPartMessage = "🌞 Добрый день UwU";
        else if (time.Hour >= 17 && time.Hour < 22) DayPartMessage = "🌆 Доброго вечера..~";
    }
}
