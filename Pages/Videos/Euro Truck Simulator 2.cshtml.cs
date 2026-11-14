using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EvgeshaXT.Pages.Videos;

public class EuroTruckSimulator2Model : PageModel
{
    public List<VideoCard> VideoCards { get; set; } = [];
    public void OnGet()
    {
        foreach (var file in Directory.GetFiles("wwwroot/Videos/Euro Truck Simulator 2", "*.mp4", SearchOption.AllDirectories))
        {
            var fileInfo = new FileInfo(file);
            VideoCards.Add(new VideoCard
            {
                Src = "/" + file.Replace("wwwroot/", ""),
                Title = Path.GetFileNameWithoutExtension(fileInfo.Name),
                DirectoryName = fileInfo.Directory!.Name,
                DateTime = fileInfo.LastWriteTime
            });
            
            VideoCards = [.. VideoCards.OrderByDescending(v => v.DateTime)];
        }
    }

    public class VideoCard
    {
        public string Src { get; set; } = "";
        public string Title { get; set; } = "";
        public string DirectoryName { get; set; } = "";
        public DateTime DateTime { get; set; }
    }
}