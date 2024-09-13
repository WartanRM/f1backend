using System.Threading.Tasks;
using F1Backend.Models;

namespace F1Backend.Services
{
    public interface IAuthService
    {
        Task<User> RegisterAsync(RegisterDTO registerdto);
        Task<string> LoginAsync(LoginDTO logindto); 
    }
}
