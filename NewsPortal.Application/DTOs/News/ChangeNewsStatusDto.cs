using NewsPortal.Domain.Enums;

namespace NewsPortal.Application.DTOs.News;

public class ChangeNewsStatusDto
{
    public NewsStatus Status { get; set; }
}
