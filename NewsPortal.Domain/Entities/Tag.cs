using NewsPortal.Domain.Common;
using System.Collections.Generic;

namespace NewsPortal.Domain.Entities;

public class Tag : BaseEntity
{
    public required string Name { get; set; }

    public required string Slug { get; set; }

    public ICollection<NewsTag> NewsTags { get; set; } = [];
}