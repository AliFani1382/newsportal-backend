using NewsPortal.Domain.Common;

namespace NewsPortal.Domain.Entities;

public class City : BaseEntity
{
    public required string Name { get; set; } 
    public required string Slug { get; set; } 
};
