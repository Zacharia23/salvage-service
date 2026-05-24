using SalvageCore.DTOs.User;
using SalvageCore.DTOs.User.Request;
using SalvageCore.DTOs.User.Response;
using SalvageCore.Helpers;
using SalvageCore.Models;

namespace SalvageCore.Interface;

public interface IUserRepository
{
    public Task<ServiceResponse<ICollection<UserListResponse>>> FetchUsers();
    public Task<ServiceResponse<UserProfileResponse>> FetchUserDetails(Guid userId);
    public Task<ServiceResponse<SystemUser>> RegisterUser(UserRequest user);
    Task<bool> UserExists(Guid userId);
}