using NewsPortal.Application.Common;
using NewsPortal.Application.DTOs.Newsletter;

namespace NewsPortal.Application.Interfaces;

public interface INewsletterService
{
    Task<ApiResponse<bool>> SubscribeAsync(
        SubscribeNewsletterDto dto);

    Task<ApiResponse<bool>> UnsubscribeAsync(
        UnsubscribeNewsletterDto dto);
}