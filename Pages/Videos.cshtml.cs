using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EvgeshaXT.Pages;

public class VideosModel : PageModel
{
    public List<VideoCard> VideoCards { get; set; } = [];
    public List<DirectoryCard> DirectoryCards { get; set; } = [];
    public void OnGet()
    {
        foreach (var file in Directory.GetFiles("wwwroot/Videos", "*.mp4", SearchOption.AllDirectories))
        {
            var fileInfo = new FileInfo(file);
            VideoCards.Add(new VideoCard
            {
                Src = "/" + file.Replace("wwwroot/", ""),
                Title = Path.GetFileNameWithoutExtension(fileInfo.Name),
                DirectoryName = fileInfo.Directory!.Name,
                DateTime = fileInfo.LastWriteTime
            });
            
            VideoCards = [.. VideoCards.OrderByDescending(v => v.DateTime).Take(6)];
        }

        foreach (var dir in Directory.GetDirectories("wwwroot/Videos", "*", SearchOption.TopDirectoryOnly))
        {
            DirectoryInfo? dirInfo = new(dir);
            string imagePath = Path.Combine("/Videos/" + dirInfo.Name + ".png");
            string? imageUrl = imagePath.Replace("\\", "/");

            DirectoryCards.Add(new DirectoryCard
            {
                ImageUrl = imageUrl,
                Name = dirInfo.Name
            });
        }
    }

    public class VideoCard
    {
        public string Src { get; set; } = "";
        public string Title { get; set; } = "";
        public string DirectoryName { get; set; } = "";
        public DateTime DateTime { get; set; }
    }

    public class DirectoryCard
    {
        public string ImageUrl { get; set; } = "";
        public string Name { get; set; } = "";
    }
}