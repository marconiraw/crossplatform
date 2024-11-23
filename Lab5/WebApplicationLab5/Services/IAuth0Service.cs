using WebApplicationLab5.Models;
using System.Threading.Tasks;

namespace WebApplicationLab5.Services
{
	public interface IAuth0Service
	{
		Task<bool> RegisterUserAsync(UserModel user);
		Task<(string AccessToken, string Username, string Fullname, string Phone, string Email)> LoginUserAsync(string email, string password);
	}
}
