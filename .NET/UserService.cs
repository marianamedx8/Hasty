using Sabio.Data;
using Sabio.Data.Providers;
using Sabio.Models;
using Sabio.Models.Domain;
using Sabio.Services.Interfaces;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Reflection.PortableExecutable;
using Sabio.Models.Domain.Users;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Collections.Generic;
using Sabio.Models.Requests.Users;
using System.Reflection;
using Stripe;

namespace Sabio.Services
{
    public class UserService : IUserService, IBaseUserMapper
    {
        private IAuthenticationService<int> _authenticationService;
        private IDataProvider _dataProvider;

        public UserService(IAuthenticationService<int> authSerice, IDataProvider dataProvider)
        {
            _authenticationService = authSerice;
            _dataProvider = dataProvider;
        }

        public async Task<bool> LogInAsync(string email, string password)
        {
            bool isSuccessful = false;

            IUserAuthData response = Get(email, password);

            if (response != null)
            {
                await _authenticationService.LogInAsync(response);
                isSuccessful = true;
            }

            return isSuccessful;
        }

        public async Task<bool> LogInTest(string email, string password, int id, string[] roles = null)
        {
            bool isSuccessful = false;
            var testRoles = new[] { "User", "Super", "Content Manager" };

            var allRoles = roles == null ? testRoles : testRoles.Concat(roles);

            IUserAuthData response = new UserBase
            {
                Id = id
                ,
                Name = email
                ,
                Roles = allRoles
                ,
                TenantId = "Acme Corp UId"
            };

            Claim fullName = new Claim("CustomClaim", "Sabio Bootcamp");
            await _authenticationService.LogInAsync(response, new Claim[] { fullName });

            return isSuccessful;
        }

        public int Create(object userModel)
        {
            //make sure the password column can hold long enough string. put it to 100 to be safe

            int userId = 0;
            string password = "Get from user model when you have a concreate class";
            string salt = BCrypt.BCryptHelper.GenerateSalt();
            string hashedPassword = BCrypt.BCryptHelper.HashPassword(password, "");

            //DB provider call to create user and get us a user id

            //be sure to store both salt and passwordHash
            //DO NOT STORE the original password value that the user passed us

            return userId;
        }

        /// <summary>
        /// Gets the Data call to get a give user
        /// </summary>
        /// <param name="email"></param>
        /// <param name="passwordHash"></param>
        /// <returns></returns>
        private IUserAuthData Get(string email, string password)
        {
            string passwordFromDb = "";
            UserBase user = null;
            UserAuth userFromDB = null;
            string procName = "[dbo].[Users_Select_AuthData]";
            _dataProvider.ExecuteCmd(procName, delegate (SqlParameterCollection parameterCollection)
            {
                parameterCollection.AddWithValue("@Email", email);
            }, delegate (IDataReader reader, short set)
            {
                userFromDB = new UserAuth();
                int startingIndex = 0;
                userFromDB.Id = reader.GetSafeInt32(startingIndex++);
                userFromDB.Email = reader.GetSafeString(startingIndex++);
                passwordFromDb = reader.GetSafeString(startingIndex++);
                userFromDB.Roles = reader.DeserializeObject<List<LookUp>>(startingIndex++);

                bool isValidCredentials = BCrypt.BCryptHelper.CheckPassword(password, passwordFromDb);
                if (isValidCredentials)
                {
                    user = new UserBase();
                    user.Id = userFromDB.Id;
                    user.Name = userFromDB.Email;
                    if (userFromDB.Roles != null)
                    {
                        user.Roles = userFromDB.Roles.Select(roles => roles.Name).ToArray();
                    }
                    user.TenantId = "HastyId123";
                }
            });
            return user;
        }

        public UserProfile GetCurrentUserById(int id)
        {
            string procName = "[dbo].[Users_SelectByCurrentUser]";
            UserProfile aUser = null;

            _dataProvider.ExecuteCmd(procName, delegate (SqlParameterCollection parameterCollection)
            {
                parameterCollection.AddWithValue("Id", id);
            }, delegate (IDataReader reader, short set)
            {
                int startingIndex = 0;
                aUser = new UserProfile();
                aUser.Id = reader.GetSafeInt32(startingIndex++);
                aUser.FirstName = reader.GetSafeString(startingIndex++);
                aUser.LastName = reader.GetSafeString(startingIndex++);
                aUser.Mi = reader.GetSafeString(startingIndex++);
                aUser.AvatarUrl = reader.GetSafeString(startingIndex++);
                aUser.Email = reader.GetSafeString(startingIndex++);
                aUser.Roles = reader.DeserializeObject<List<LookUp>>(startingIndex++);
            });
            return aUser;
        }
        public User GetUserById(int id)
        {
            string procName = "[dbo].[Users_SelectById]";
            User aUser = null;
            _dataProvider.ExecuteCmd(procName, delegate (SqlParameterCollection parameterCollection)
            {
                parameterCollection.AddWithValue("Id", id);
            }, delegate (IDataReader reader, short set)
            {
                int startingIndex = 0;
                aUser = new User();
                aUser.Id = reader.GetSafeInt32(startingIndex++);
                aUser.FirstName = reader.GetSafeString(startingIndex++);
                aUser.LastName = reader.GetSafeString(startingIndex++);
                aUser.Mi = reader.GetSafeString(startingIndex++);
                aUser.AvatarUrl = reader.GetSafeString(startingIndex++);
                aUser.Email = reader.GetSafeString(startingIndex++);
                aUser.isConfirmed = reader.GetSafeBool(startingIndex++);
                aUser.StatusId = reader.GetSafeInt32(startingIndex++);
                aUser.DateCreated = reader.GetSafeDateTime(startingIndex++);
                aUser.DateModified = reader.GetSafeDateTime(startingIndex++);
            });
            return aUser;
        }

        public Paged<User> GetAllPaginated(int pageIndex, int pageSize)
        {
            Paged<User> pagedList = null;
            List<User> list = null;
            int totalCount = 0;
            string procName = "[dbo].[Users_SelectAll_Pagination]";

            _dataProvider.ExecuteCmd(procName, (param) =>
            {
                param.AddWithValue("@PageIndex", pageIndex);
                param.AddWithValue("@PageSize", pageSize);
            }, (reader, recordSetIndex) =>
            {
                int startingIndex = 0;
                User aUser = MapSingleUser(reader, ref startingIndex);
                if (totalCount == 0)
                {
                    totalCount = reader.GetSafeInt32(startingIndex++);
                }

                if (list == null)
                {
                    list = new List<User>();
                }
                list.Add(aUser);
            });
            if (list != null)
            {
                pagedList = new Paged<User>(list, pageIndex, pageSize, totalCount);
            }
            return pagedList;
        }

        public UserAuth GetAuthData(string email)
        {
            string procName = "[dbo].[Users_Select_AuthData]";
            UserAuth aUser = null;

            _dataProvider.ExecuteCmd(procName, delegate (SqlParameterCollection parameterCollection)
            {
                parameterCollection.AddWithValue("email", email);
            }, delegate (IDataReader reader, short set)
            {
                int startingIndex = 0;
                aUser = new UserAuth();
                aUser.Id = reader.GetSafeInt32(startingIndex++);
                aUser.Email = reader.GetSafeString(startingIndex++);
                aUser.Password = reader.GetSafeString(startingIndex++);
                aUser.Roles = reader.DeserializeObject<List<LookUp>>(startingIndex++);
            });
            return aUser;
        }

        public string GetAuthByEmail(string email)
        {
            string procName = "[dbo].[Users_SelectPass_ByEmail]";
            string pw = null;

            _dataProvider.ExecuteCmd(procName, delegate (SqlParameterCollection parameterCollection)
            {
                parameterCollection.AddWithValue("email", email);
            }, delegate (IDataReader reader, short set)
            {
                int startingIndex = 0;
                pw = reader.GetSafeString(startingIndex++);
            });
            return pw;
        }

        public void UpdateCurrentUser(UserUpdateRequest model, int userId)
        {
            string procName = "[dbo].[Users_Update]";
            _dataProvider.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection col)
            {
                col.AddWithValue("@FirstName", model.FirstName);
                col.AddWithValue("@LastName", model.LastName);
                col.AddWithValue("@Mi", model.Mi);
                col.AddWithValue("@AvatarUrl", model.AvatarUrl);
                col.AddWithValue("@Id", userId);
            }, returnParameters: null);
        }

        public void UpdateUserConfirm(string email, string token)
        {
            string procName = "[dbo].[Users_UpdateConfirm]";
            _dataProvider.ExecuteNonQuery(procName
                , inputParamMapper: delegate (SqlParameterCollection col)
                {
                    col.AddWithValue("@Email", email);
                    col.AddWithValue("@Token", token);
                }, returnParameters: null);
        }

        public void UpdateUserStatus(int id, int statusId)
        {
            string procName = "[dbo].[Users_UpdateStatus]";
            _dataProvider.ExecuteNonQuery(procName
                , inputParamMapper: delegate (SqlParameterCollection col)
                {
                    col.AddWithValue("@Id", id);
                    col.AddWithValue("@StatusId", statusId);
                }, returnParameters: null);
        }
        public bool HasProfile(int id)
        {
            bool hasProfile = false;
            string procName = "[dbo].[Users_HasProfile]";

            _dataProvider.ExecuteNonQuery(procName
            , inputParamMapper: delegate (SqlParameterCollection col)
              {
                  col.AddWithValue("@Id", id);

                  SqlParameter hasProfileOut = new SqlParameter("@HasProfile", SqlDbType.Bit);
                  hasProfileOut.Direction = ParameterDirection.Output;

                  col.Add(hasProfileOut);
              }, returnParameters: delegate (SqlParameterCollection returnCollection)
                        {
                            object oHasProfile = returnCollection["@HasProfile"].Value;
                            bool.TryParse(oHasProfile.ToString(), out hasProfile);
                        });
            return hasProfile;
        }

        public void AddToken(int userId, string token, int tokenType)
        {
            string procName = "[dbo].[UserTokens_Insert]";
            _dataProvider.ExecuteNonQuery(procName, delegate (SqlParameterCollection col)
            {
                col.AddWithValue("@Token", token);
                col.AddWithValue("@UserId", userId);
                col.AddWithValue("@TokenType", tokenType);
            });
        }

        public void UpdatePassword(ChangePasswordRequest model)
        {
            string password = model.Password;
            string salt = BCrypt.BCryptHelper.GenerateSalt();
            string hashedPassword = BCrypt.BCryptHelper.HashPassword(password, salt);
            model.Password = hashedPassword;
            string procName = "[dbo].[Users_UpdatePassword]";
            _dataProvider.ExecuteNonQuery(procName
                , inputParamMapper: delegate (SqlParameterCollection col)
                {
                    col.AddWithValue("@Email", model.Email);
                    col.AddWithValue("@Token", model.Token);
                    col.AddWithValue("@Password", model.Password);
                    col.AddWithValue("@PasswordConfirm", model.PasswordConfirm);
                }, returnParameters: null);
        }

        public int Create(UserAddRequest model)
        {
            int id = 0;
            string password = model.Password;
            string salt = BCrypt.BCryptHelper.GenerateSalt();
            string hashedPassword = BCrypt.BCryptHelper.HashPassword(password, salt);
            model.Password = hashedPassword;
            string procName = "[dbo].[Users_Insert_WithRole]";
            _dataProvider.ExecuteNonQuery(procName
                , inputParamMapper: delegate (SqlParameterCollection col)
                {
                    AddCommonParams(model, col);

                    SqlParameter idOut = new SqlParameter("@Id", SqlDbType.Int);
                    idOut.Direction = ParameterDirection.Output;

                    col.Add(idOut);
                }, returnParameters: delegate (SqlParameterCollection returnCollection)
                {
                    object oId = returnCollection["@Id"].Value;
                    int.TryParse(oId.ToString(), out id);
                });
            return id;
        }

        public AdminDash GetAllData()
        {
            AdminDash adminDash = null;
            string procName = "[dbo].[StatusTypes_TotalCount_Each]";

            _dataProvider.ExecuteCmd(procName, inputParamMapper: null,
                singleRecordMapper: delegate (IDataReader reader, short set)
                {
                    int startingIndex = 0;
                    if (adminDash == null)
                    {
                        adminDash = new AdminDash();
                    }
                    if (set == 0)
                    {
                        adminDash.UserCount = reader.GetSafeInt32(startingIndex++);
                    }
                    else if (set == 1)
                    {
                        UserStatus status = new UserStatus();

                        status.TotalCount = reader.GetSafeInt32(startingIndex++);
                        status.Name = reader.GetSafeString(startingIndex++);

                        if (adminDash.Status == null)
                        {
                            adminDash.Status = new List<UserStatus>();
                        }
                        adminDash.Status.Add(status);
                    }
                    else if (set == 2)
                    {
                        adminDash.ActiveSubscriptions = reader.GetSafeInt32(startingIndex++);
                    }
                    else if (set == 3)
                    {
                        Subs subs = new Subs();

                        subs.subsByPlan = reader.GetSafeInt32(startingIndex++);
                        subs.Name = reader.GetSafeString(startingIndex++);
                        subs.incomeBySub = reader.GetSafeInt32(startingIndex++);
                        subs.Amount = reader.GetSafeInt32(startingIndex++);

                        if (adminDash.Income == null)
                        {
                            adminDash.Income = new List<Subs>();
                        }
                        adminDash.Income.Add(subs);
                    }
                    else if(set == 4)
                    {
                        SubsPlans week = new SubsPlans();

                        week.Count = reader.GetSafeInt32(startingIndex++);
                        week.Amount = reader.GetSafeInt32(startingIndex++);
                        if (adminDash.ByWeek == null)
                        {
                            adminDash.ByWeek = new List<SubsPlans>();
                        }
                        adminDash.ByWeek.Add(week);
                    }
                    else if (set == 5)
                    {
                        SubsPlans year = new SubsPlans();

                        year.Count = reader.GetSafeInt32(startingIndex++);
                        year.Amount = reader.GetSafeInt32(startingIndex++);
                        if (adminDash.ByYear == null)
                        {
                            adminDash.ByYear = new List<SubsPlans>();
                        }
                        adminDash.ByYear.Add(year);
                    }
                    else if (set == 6)
                    {
                        LookUp month = new LookUp();

                        month.Name = reader.GetSafeString(startingIndex++);
                        month.Id = reader.GetSafeInt32(startingIndex++);
                        if (adminDash.ByMonth == null)
                        {
                            adminDash.ByMonth = new List<LookUp>();
                        }
                        adminDash.ByMonth.Add(month);
                    }
                });
            return adminDash;
        }

        private static void AddCommonParams(UserAddRequest model, SqlParameterCollection col)
        {
            col.AddWithValue("@Email", model.Email);
            col.AddWithValue("@FirstName", model.FirstName);
            col.AddWithValue("@LastName", model.LastName);
            col.AddWithValue("@Mi", model.Mi);
            col.AddWithValue("@AvatarUrl", model.AvatarUrl);
            col.AddWithValue("@Password", model.Password);
            col.AddWithValue("@PasswordConfirm", model.PasswordConfirm);
            col.AddWithValue("@StatusId", model.StatusId);
            col.AddWithValue("@RoleId", model.RoleId);
        }
        private User MapSingleUser(IDataReader reader, ref int startingIndex)
        {
            User aUser = new User();
            aUser.Id = reader.GetSafeInt32(startingIndex++);
            aUser.FirstName = reader.GetSafeString(startingIndex++);
            aUser.LastName = reader.GetSafeString(startingIndex++);
            aUser.Mi = reader.GetSafeString(startingIndex++);
            aUser.AvatarUrl = reader.GetSafeString(startingIndex++);
            aUser.Email = reader.GetSafeString(startingIndex++);
            aUser.isConfirmed = reader.GetSafeBool(startingIndex++);
            aUser.StatusId = reader.GetSafeInt32(startingIndex++);
            aUser.DateCreated = reader.GetSafeDateTime(startingIndex++);
            aUser.DateModified = reader.GetSafeDateTime(startingIndex++);
            return aUser;
        }
        public BaseUser MapBaseUser(IDataReader reader, ref int startingIndex)
        {
            BaseUser aUser = new BaseUser();
            aUser.Id = reader.GetSafeInt32(startingIndex++);
            aUser.FirstName = reader.GetSafeString(startingIndex++);
            aUser.LastName = reader.GetSafeString(startingIndex++);
            aUser.Mi = reader.GetSafeString(startingIndex++);
            aUser.AvatarUrl = reader.GetSafeString(startingIndex++);
            return aUser;
        }
    }
}