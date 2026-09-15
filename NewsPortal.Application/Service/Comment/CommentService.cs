using NewsPortal.Application.Common;
using NewsPortal.Application.Common.Interfaces;
using NewsPortal.Application.DTOs.Comments;
using NewsPortal.Application.Interfaces;
using NewsPortal.Application.Repositories;
using NewsPortal.Application.Service.Notifications;
using NewsPortal.Domain.Entities;

namespace NewsPortal.Application.Service.Comments;

public sealed class CommentService : ICommentService
{
    private readonly ICommentRepository _commentRepository;
    private readonly INewsRepository _newsRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly INotificationService _notificationService;
    public CommentService(
        ICommentRepository commentRepository,
        INewsRepository newsRepository,
        IUnitOfWork unitOfWork,
        INotificationService notificationService)
    {
        _commentRepository = commentRepository;
        _newsRepository = newsRepository;
        _unitOfWork = unitOfWork;
        _notificationService = notificationService;
    }

    public async Task<ApiResponse<IReadOnlyList<CommentDto>>> GetByNewsIdAsync(
        int newsId,
        bool isAdmin)
    {
        var news =
            await _newsRepository.GetByIdAsync(newsId);

        if (news is null)
        {
            return ApiResponse<IReadOnlyList<CommentDto>>.Failure(
                ApiErrorCode.NotFound,
                "خبر مورد نظر پیدا نشد");
        }

        var comments =
            await _commentRepository.GetByNewsIdAsync(
                newsId,
                isAdmin);

        var result =
            comments
                .Select(MapToDto)
                .ToList();

        return ApiResponse<IReadOnlyList<CommentDto>>
            .Success(result);
    }

    public async Task<ApiResponse<CommentDto>> CreateAsync(
       int newsId,
       CreateCommentDto dto,
       int userId)
    {
        var news =
            await _newsRepository.GetByIdAsync(newsId);

        if (news is null)
        {
            return ApiResponse<CommentDto>.Failure(
                ApiErrorCode.NotFound,
                "خبر مورد نظر پیدا نشد");
        }

        var content = dto.Content.Trim();

        var comment = new Comment(
            newsId,
            userId,
            content);

        await _commentRepository.AddAsync(comment);

        await _unitOfWork.CommitAsync();

        if (news.WriterId != userId)
        {
            await _notificationService.CreateAsync(
                news.WriterId,
                "نظر جدید برای خبر شما",
                $"یک نظر جدید برای خبر «{news.Title}» ثبت شد.",
                $"/news/{news.Slug}");
        }

        var createdComment =
            await _commentRepository.GetByIdWithUserAsync(
                comment.Id);

        if (createdComment is null)
        {
            return ApiResponse<CommentDto>.Failure(
                ApiErrorCode.NotFound,
                "نظر ایجاد شده پیدا نشد");
        }

        return ApiResponse<CommentDto>.Success(
            MapToDto(createdComment),
            "نظر شما ثبت شد و پس از تأیید نمایش داده خواهد شد");
    }

    public async Task<ApiResponse<IReadOnlyList<CommentDto>>> GetPendingAsync()
    {
        var comments =
            await _commentRepository.GetPendingAsync();

        var result =
            comments
                .Select(MapToDto)
                .ToList();

        return ApiResponse<IReadOnlyList<CommentDto>>
            .Success(result);
    }

    public async Task<ApiResponse<bool>> ApproveAsync(
        int id)
    {
        var comment =
            await _commentRepository.GetByIdWithUserAsync(id);

        if (comment is null)
        {
            return ApiResponse<bool>.Failure(
                ApiErrorCode.NotFound,
                "نظر مورد نظر پیدا نشد");
        }

        comment.Approve();

        await _unitOfWork.CommitAsync();

        await _notificationService.CreateAsync(
    comment.UserId,
    "نظر شما تأیید شد",
    $"نظر شما برای خبر «{comment.News.Title}» تأیید شد.",
    $"/news/{comment.News.Slug}");

        return ApiResponse<bool>.Success(
            true,
            "نظر با موفقیت تأیید شد");
    }

    public async Task<ApiResponse<bool>> RejectAsync(
        int id)
    {
        var comment =
            await _commentRepository.GetByIdWithUserAsync(id);

        if (comment is null)
        {
            return ApiResponse<bool>.Failure(
                ApiErrorCode.NotFound,
                "نظر مورد نظر پیدا نشد");
        }

        comment.Reject();

        await _unitOfWork.CommitAsync();

        await _notificationService.CreateAsync(
    comment.UserId,
    "نظر شما رد شد",
    $"نظر شما برای خبر «{comment.News.Title}» رد شد.",
    $"/news/{comment.News.Slug}");

        return ApiResponse<bool>.Success(
            true,
            "نظر با موفقیت رد شد");

        return ApiResponse<bool>.Success(
            true,
            "نظر با موفقیت رد شد");
    }

    public async Task<ApiResponse<bool>> DeleteAsync(
        int id,
        int userId,
        bool isAdmin)
    {
        var comment =
            await _commentRepository.GetByIdWithUserAsync(id);

        if (comment is null)
        {
            return ApiResponse<bool>.Failure(
                ApiErrorCode.NotFound,
                "نظر مورد نظر پیدا نشد");
        }

        if (!isAdmin && comment.UserId != userId)
        {
            return ApiResponse<bool>.Failure(
                ApiErrorCode.Forbidden,
                "شما اجازه حذف این نظر را ندارید");
        }

        _commentRepository.Remove(comment);

        await _unitOfWork.CommitAsync();

        return ApiResponse<bool>.Success(
            true,
            "نظر با موفقیت حذف شد");
    }

    private static CommentDto MapToDto(
        Comment comment)
    {
        return new CommentDto
        {
            Id = comment.Id,
            NewsId = comment.NewsId,
            UserId = comment.UserId,
            UserName = comment.User?.Username ?? string.Empty,
            Content = comment.Content,
            Status = comment.Status.ToString(),
            CreatedAt = comment.CreatedDate
        };
    }
}