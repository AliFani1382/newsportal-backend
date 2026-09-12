using NewsPortal.Application.Common;
using NewsPortal.Application.Common.Interfaces;
using NewsPortal.Application.DTOs.Reactions;
using NewsPortal.Application.Interfaces;
using NewsPortal.Application.Repositories;
using NewsPortal.Domain.Entities;
using NewsPortal.Domain.Enums;

namespace NewsPortal.Application.Service.Reactions;

public sealed class ReactionService : IReactionService
{
    private readonly INewsReactionRepository _reactionRepository;
    private readonly INewsRepository _newsRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ReactionService(
        INewsReactionRepository reactionRepository,
        INewsRepository newsRepository,
        IUnitOfWork unitOfWork)
    {
        _reactionRepository = reactionRepository;
        _newsRepository = newsRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ApiResponse<ReactionDto>> GetReactionAsync(
        int newsId,
        int userId)
    {
        var news =
            await _newsRepository.GetByIdAsync(newsId);

        if (news is null)
        {
            return ApiResponse<ReactionDto>.Failure(
                ApiErrorCode.NotFound,
                "خبر مورد نظر پیدا نشد");
        }

        var reaction =
            await _reactionRepository.GetByUserAndNewsAsync(
                userId,
                newsId);

        var likeCount =
            await _reactionRepository.CountByNewsAndTypeAsync(
                newsId,
                ReactionType.Like);

        var dislikeCount =
            await _reactionRepository.CountByNewsAndTypeAsync(
                newsId,
                ReactionType.Dislike);

        return ApiResponse<ReactionDto>.Success(
            new ReactionDto
            {
                LikeCount = likeCount,
                DislikeCount = dislikeCount,
                MyReaction = reaction?.Type.ToString()
            });
    }

    public async Task<ApiResponse<ReactionDto>> SetReactionAsync(
        int newsId,
        int userId,
        string type)
    {
        var news =
            await _newsRepository.GetByIdAsync(newsId);

        if (news is null)
        {
            return ApiResponse<ReactionDto>.Failure(
                ApiErrorCode.NotFound,
                "خبر مورد نظر پیدا نشد");
        }

        if (!Enum.TryParse<ReactionType>(
                type,
                true,
                out var reactionType))
        {
            return ApiResponse<ReactionDto>.Failure(
                ApiErrorCode.ValidationError,
                "نوع واکنش نامعتبر است");
        }

        var existingReaction =
            await _reactionRepository.GetByUserAndNewsAsync(
                userId,
                newsId);

        if (existingReaction is not null)
        {
            if (existingReaction.Type == reactionType)
            {
                _reactionRepository.Remove(
                    existingReaction);
            }
            else
            {
                existingReaction.ChangeType(
                    reactionType);
            }
        }
        else
        {
            var reaction = new NewsReaction(
                newsId,
                userId,
                reactionType);

            await _reactionRepository.AddAsync(
                reaction);
        }

        await _unitOfWork.CommitAsync();

        return await GetReactionAsync(
            newsId,
            userId);
    }
}