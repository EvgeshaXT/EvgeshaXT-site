using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EvgeshaXT.Pages.Ru;

public class VideosModel : PageModel
{
    public List<VideoManager.VideoCard> VideoCards { get; set; } = [];
    public List<VideoManager.DirectoryCard> DirectoryCards { get; set; } = [];
    public void OnGet()
    {
        VideoCards = VideoManager.GetLastVideos();
        DirectoryCards = VideoManager.GetDirectoryCards();
    }
}