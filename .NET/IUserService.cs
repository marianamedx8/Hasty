using Sabio.Models.Domain.Users;
using Sabio.Models.Requests.Users;
using Sabio.Models;
using System.Threading.Tasks;
using Sabio.Models.Domain;
using System.Data;
using System.Collections.Generic;

namespace Sabio.Services
{
    public interface IUserService
    {
        int Create(object userModel);
        Task<bool> LogInAsync(string email, string password);
        Task<bool> LogInTest(string email, string password, int id, string[] roles = null);
        UserProfile GetCurrentUserById(int id);
        User GetUserById(int id);
        Paged<User> GetAllPaginated(int pageIndex, int pageSize);
        UserAuth GetAuthData(string email);
        string GetAuthByEmail(string email);
        bool HasProfile(int id);
        void AddToken(int userId, string token, int tokenType);
        void UpdateCurrentUser(UserUpdateRequest model, int userId);
        void UpdateUserConfirm(string email, string token);
        void UpdateUserStatus(int id, int statusId);
        void UpdatePassword(ChangePasswordRequest model);
        int Create(UserAddRequest model);
        AdminDash GetAllData();
    }
}