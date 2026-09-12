using NewsPortal.Application.Common;
using NewsPortal.Application.Common.Interfaces;
using NewsPortal.Application.DTOs.Newsletter;
using NewsPortal.Application.Interfaces;
using NewsPortal.Application.Repositories;

namespace NewsPortal.Application.Service.Newsletter;

public sealed class NewsletterService : INewsletterService
{
    private readonly INewsletterSubscriberRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public NewsletterService(
        INewsletterSubscriberRepository repository,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ApiResponse<bool>> SubscribeAsync(
        SubscribeNewsletterDto dto)
    {
        var subscriber =
            await _repository.GetByEmailAsync(dto.Email);

        if (subscriber is not null)
        {
            if (!subscriber.IsActive)
            {
                subscriber.Resubscribe();

                await _unitOfWork.CommitAsync();

                return ApiResponse<bool>.Success(
                    true,
                    "عضویت شما در خبرنامه دوباره فعال شد.");
            }

            return ApiResponse<bool>.Success(
                true,
                "این ایمیل قبلاً در خبرنامه عضو شده است.");
        }

        var newSubscriber =
            new NewsPortal.Domain.Entities.NewsletterSubscriber(
                dto.Email);

        await _repository.AddAsync(newSubscriber);

        await _unitOfWork.CommitAsync();

        return ApiResponse<bool>.Success(
            true,
            "عضویت شما در خبرنامه با موفقیت انجام شد.");
    }

    public async Task<ApiResponse<bool>> UnsubscribeAsync(
        UnsubscribeNewsletterDto dto)
    {
        var subscriber =
            await _repository.GetByEmailAsync(dto.Email);

        if (subscriber is null)
        {
            return ApiResponse<bool>.Success(
                true,
                "اگر این ایمیل عضو خبرنامه باشد، عضویت آن لغو خواهد شد.");
        }

        if (!subscriber.IsActive)
        {
            return ApiResponse<bool>.Success(
                true,
                "عضویت این ایمیل قبلاً لغو شده است.");
        }

        subscriber.Unsubscribe();

        await _unitOfWork.CommitAsync();

        return ApiResponse<bool>.Success(
            true,
            "عضویت شما در خبرنامه با موفقیت لغو شد.");
    }
}