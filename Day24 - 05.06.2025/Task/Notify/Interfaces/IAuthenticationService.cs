using Notify.Models.DTO;
namespace Notify.Interfaces;

public interface IAuthenticationService
{
     public Task<UserLoginResponse> Login(UserLoginRequest user);

}