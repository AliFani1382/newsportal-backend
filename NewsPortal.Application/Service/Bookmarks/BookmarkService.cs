using NewsPortal.Application.Common;
using NewsPortal.Application.Common.Interfaces;
using NewsPortal.Application.DTOs.Bookmarks;
using NewsPortal.Application.DTOs.News;
using NewsPortal.Application.Interfaces;
using NewsPortal.Application.Repositories;

namespace NewsPortal.Application.Service.Bookmarks;

public sealed class BookmarkService : IBookmarkService
{
    private readonly IBookmarkRepository _bookmarkRepository;
    private readonly INewsRepository _newsRepository;
    private readonly IUnitOfWork _unitOfWork;

    public BookmarkService(
        IBookmarkRepository bookmarkRepository,
        INewsRepository newsRepository,
        IUnitOfWork unitOfWork)
    {
        _bookmarkRepository = bookmarkRepository;
        _newsRepository = newsRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ApiResponse<IReadOnlyList<NewsDto>>> GetMyBookmarksAsync(
        int userId)
    {
        var bookmarks =
            await _bookmarkRepository.GetByUserIdAsync(userId);

        var result =
            bookmarks
                .Select(b => new NewsDto
                {
                    Id = b.News.Id,
                    Title = b.News.Title,
                    Slug = b.News.Slug,
                    Content = b.News.Content,
                    PublicationDate = b.News.PublicationDate,
                    ImagePath = b.News.ImagePath,
                    Status = b.News.Status.ToString(),
                    CityId = b.News.CityId,
                    CategoryId = b.News.CategoryId,
                    WriterId = b.News.WriterId
                })
                .ToList();

        return ApiResponse<IReadOnlyList<NewsDto>>
            .Success(result);
    }

    public async Task<ApiResponse<bool>> AddAsync(
        int newsId,
        int userId)
    {
        var news =
            await _newsRepository.GetByIdAsync(newsId);

        if (news is null)
        {
            return ApiResponse<bool>.Failure(
                ApiErrorCode.NotFound,
                "خبر مورد نظر پیدا نشد");
        }

        var existingBookmark =
            await _bookmarkRepository.GetByUserAndNewsAsync(
                userId,
                newsId);

        if (existingBookmark is not null)
        {
            return ApiResponse<bool>.Success(
                true,
                "خبر قبلاً نشان شده است");
        }

        var bookmark = new Domain.Entities.Bookmark(
            newsId,
            userId);

        await _bookmarkRepository.AddAsync(bookmark);

        await _unitOfWork.CommitAsync();

        return ApiResponse<bool>.Success(
            true,
            "خبر با موفقیت نشان شد");
    }

    public async Task<ApiResponse<bool>> RemoveAsync(
        int newsId,
        int userId)
    {
        var bookmark =
            await _bookmarkRepository.GetByUserAndNewsAsync(
                userId,
                newsId);

        if (bookmark is null)
        {
            return ApiResponse<bool>.Success(
                true,
                "خبر در نشان‌شده‌ها وجود ندارد");
        }

        _bookmarkRepository.Remove(bookmark);

        await _unitOfWork.CommitAsync();

        return ApiResponse<bool>.Success(
            true,
            "نشان خبر حذف شد");
    }

    public async Task<ApiResponse<BookmarkStatusDto>> GetStatusAsync(
        int newsId,
        int userId)
    {
        var news =
            await _newsRepository.GetByIdAsync(newsId);

        if (news is null)
        {
            return ApiResponse<BookmarkStatusDto>.Failure(
                ApiErrorCode.NotFound,
                "خبر مورد نظر پیدا نشد");
        }

        var bookmark =
            await _bookmarkRepository.GetByUserAndNewsAsync(
                userId,
                newsId);

        return ApiResponse<BookmarkStatusDto>.Success(
            new BookmarkStatusDto
            {
                IsBookmarked = bookmark is not null
            });
    }
}