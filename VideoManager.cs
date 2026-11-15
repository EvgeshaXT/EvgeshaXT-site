public static class VideoManager
{
    public static List<VideoCard> VideoCards { get; set; } = [];
    public static List<DirectoryCard> DirectoryCards { get; set; } = [];
    public static void Initialize()
    {
        VideoCardsInitialize();
        DirectoryCardsInitialize();
    }

    public static void VideoCardsInitialize()
    {
        VideoCards.Clear();
        foreach (string? file in Directory.GetFiles("wwwroot/Videos", "*.mp4", SearchOption.AllDirectories))
        {
            FileInfo? fileInfo = new(file);
            VideoCards.Add(new VideoCard
            {
                Src = "/" + file.Replace("wwwroot/", ""),
                Title = Path.GetFileNameWithoutExtension(fileInfo.Name),
                DirectoryName = fileInfo.Directory!.Name,
                DateTime = fileInfo.LastWriteTime
            });
        }
        VideoCards = [.. VideoCards.OrderByDescending(v => v.DateTime)];
    }

    public static void DirectoryCardsInitialize()
    {
        DirectoryCards.Clear();
        foreach (string? dir in Directory.GetDirectories("wwwroot/Videos", "*", SearchOption.TopDirectoryOnly))
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

    public static List<VideoCard> GetLastVideos()
    {
        VideoCardsInitialize();
        return [.. VideoCards.Take(6)];
    }

    public static List<VideoCard> GetVideoCards(string directoryName)
    {
        VideoCardsInitialize();
        return [.. VideoCards.Where(v => v.DirectoryName == directoryName)];
    }
    public static List<DirectoryCard> GetDirectoryCards()
    {
        DirectoryCardsInitialize();
        return [.. DirectoryCards];
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