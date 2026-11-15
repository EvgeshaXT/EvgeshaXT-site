using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EvgeshaXT.Pages.Videos;

public class OsuModel : PageModel
{
    public List<VideoManager.VideoCard> VideoCards { get; set; } = [];
    public void OnGet() => VideoCards = VideoManager.GetVideoCards("osu!");
}