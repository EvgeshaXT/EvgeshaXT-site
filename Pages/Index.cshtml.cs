using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EvgeshaXT.Pages;

public class IndexModel : PageModel
{
    public string DayPartMessage { get; private set; } = "";

    public void OnGet()
    {
        DateTime time = DateTime.Now;

        if (time.Hour >= 22 || time.Hour < 6) DayPartMessage = "🌃 Good night..~";
        else if (time.Hour >= 6 && time.Hour < 12) DayPartMessage = "🌅 Good morning!";
        else if (time.Hour >= 12 && time.Hour < 17) DayPartMessage = "🌞 Hi! UwU";
        else if (time.Hour >= 17 && time.Hour < 22) DayPartMessage = "🌆 Good evening..~";
    }
}
