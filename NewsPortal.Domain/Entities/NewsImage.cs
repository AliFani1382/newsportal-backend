using NewsPortal.Domain.Common;

namespace NewsPortal.Domain.Entities;

public class NewsImage : BaseEntity
{
    public int NewsId { get; private set; }

    public string ImagePath { get; private set; } = null!;

    public int DisplayOrder { get; private set; }

    public News News { get; private set; } = null!;

    private NewsImage()
    {
    }

    public NewsImage(
        int newsId,
        string imagePath,
        int displayOrder)
    {
        NewsId = newsId;
        ImagePath = imagePath;
        DisplayOrder = displayOrder;
        CreatedDate = DateTime.UtcNow;
    }
}