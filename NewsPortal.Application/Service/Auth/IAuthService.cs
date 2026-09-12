using NewsPortal.Application.Common;
using NewsPortal.Application.DTOs.Auth;


namespace NewsPortal.Application.Service.Auth
{
    public interface IAuthService
    {

        Task<ApiResponse<AuthResponseDto>> RegisterAsync(
            RegisterDto dto);

        Task<ApiResponse<AuthResponseDto>> LoginAsync(
           LoginDto dto);
       
    }
}
