using NewsPortal.Domain.Common;
using System.Collections.Generic;

namespace NewsPortal.Domain.Entities;

public class Category : BaseEntity
{
    public required string Name { get; set; }
    public required string Slug { get; set; }

    public ICollection<News> News { get; set; } = [];
}
