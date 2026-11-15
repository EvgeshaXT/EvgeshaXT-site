using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EvgeshaXT.Pages.Ru.Videos;

public class DesktopModel : PageModel
{
    public List<VideoManager.VideoCard> VideoCards { get; set; } = [];
    public void OnGet() => VideoCards = VideoManager.GetVideoCards("Desktop");
}