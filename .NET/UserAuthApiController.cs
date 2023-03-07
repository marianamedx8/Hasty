using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Sabio.Models;
using Sabio.Models.Domain.Users;
using Sabio.Models.Requests.Users;
using Sabio.Services;
using Sabio.Services.Interfaces;
using Sabio.Web.Controllers;
using Sabio.Web.Models.Responses;
using Sabio.Web.StartUp;
using System;
using Sabio.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using Sabio.Models.Requests;
using Stripe;
using NuGet.ContentModel;
using System.Collections.Generic;
using Sabio.Models.Domain.ListingReservations;

namespace Sabio.Web.Api.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserAuthApiController : BaseApiController
    {
        private IUserService _service = null;
        private IAuthenticationService<int> _authService = null;
        private IEmailService _emailService = null;

        public UserAuthApiController(IUserService service
         , IEmailService emailService
        , ILogger<UserAuthApiController> logger
        , IAuthenticationService<int> authService) : base(logger)
        {
            _service = service;
            _authService = authService;
            _emailService = emailService;
        }

        [HttpGet("profile")]
        public ActionResult<ItemResponse<UserProfile>> GetUserDetails()
        {
            int iCode = 200;
            BaseResponse response = null;
            try
            {
                UserProfile aUser = _service.GetCurrentUserById(_authService.GetCurrentUserId());

                if (aUser == null)
                {
                    iCode = 404;
                    response = new ErrorResponse("User Not Found");
                }
                else
                {
                    response = new ItemResponse<UserProfile> { Item = aUser };
                }
            }
            catch (Exception ex)
            {
                iCode = 500;
                base.Logger.LogError(ex.ToString());
                response = new ErrorResponse($"Generic Error:${ex.Message}");
            }
            return StatusCode(iCode, response);
        }

        [HttpGet("{id:int}")]
        public ActionResult<ItemResponse<User>> GetUserById(int id)
        {
            int iCode = 200;
            BaseResponse response = null;
            try
            {
                User aUser = _service.GetUserById(id);
                if (aUser == null)
                {
                    iCode = 404;
                    response = new ErrorResponse("User Not Found");
                }
                else
                {
                    response = new ItemResponse<User> { Item = aUser };
                }
            }
            catch (Exception ex)
            {
                iCode = 500;
                base.Logger.LogError(ex.ToString());
                response = new ErrorResponse($"Generic Error:${ex.Message}");
            }
            return StatusCode(iCode, response);
        }

        [HttpGet("{email}")]
        public ActionResult<ItemResponse<UserAuth>> GetAuthData(string email)
        {
            int iCode = 200;
            BaseResponse response = null;
            try
            {
                UserAuth aUser = _service.GetAuthData(email);

                if (aUser == null)
                {
                    iCode = 404;
                    response = new ErrorResponse("Email Not Found");
                }
                else
                {
                    response = new ItemResponse<UserAuth> { Item = aUser };
                }
            }
            catch (Exception ex)
            {
                iCode = 500;
                base.Logger.LogError(ex.ToString());
                response = new ErrorResponse($"Generic Error:${ex.Message}");
            }
            return StatusCode(iCode, response);
        }

        [HttpGet("paginate")]
        public ActionResult<ItemResponse<Paged<User>>> GetAllPaginated(int pageIndex, int pageSize)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {

                Paged<User> paged = _service.GetAllPaginated(pageIndex, pageSize);
                if (paged == null)
                {
                    code = 404;
                    response = new ErrorResponse("Record Not Found");
                }
                else
                {
                    response = new ItemResponse<Paged<User>>() { Item = paged };

                }
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
                base.Logger.LogError(ex.ToString());
            }
            return StatusCode(code, response);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult<ItemResponse<int>> Create(UserAddRequest model)
        {
            ObjectResult result = null;

            try
            {
                int id = _service.Create(model);
                string token = Guid.NewGuid().ToString();
                int tokenTypeid = (int)TokenType.NewUser;
                _service.AddToken(id, token, tokenTypeid);
                _emailService.SendRegisterConfirmation(model, token);
                ItemResponse<int> response = new ItemResponse<int>() { Item = id };
                result = Created201(response);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                ErrorResponse response = new ErrorResponse(ex.Message);
                result = StatusCode(500, response);
            }
            return result;
        }

        [HttpPut("update")]
        public ActionResult<SuccessResponse> UpdateUser(UserUpdateRequest model)
        {
            int code = 200;
            BaseResponse response = null;
            try
            {
                int userId = _authService.GetCurrentUserId();
                _service.UpdateCurrentUser(model, userId);
                response = new SuccessResponse();
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
            }
            return StatusCode(code, response);
        }

        [HttpPut("confirm")]
        [AllowAnonymous]
        public ActionResult<SuccessResponse> UpdateUserConfirm(string email, string token)
        {
            int iCode = 200;
            BaseResponse response = null;
            try
            {
                _service.UpdateUserConfirm(email, token);
                response = new SuccessResponse();
            }
            catch (Exception ex)
            {
                iCode = 500;
                response = new ErrorResponse(ex.Message);
            }
            return StatusCode(iCode, response);
        }

        [HttpPut("status/{id:int}")]
        public ActionResult<SuccessResponse> UpdateUserStatus(int id, int statusId)
        {
            int iCode = 200;
            BaseResponse response = null;
            try
            {
                _service.UpdateUserStatus(id, statusId);
                response = new SuccessResponse();
            }
            catch (Exception ex)
            {
                iCode = 500;
                response = new ErrorResponse(ex.Message);
            }
            return StatusCode(iCode, response);
        }

        [HttpGet("profile/check/{id:int}")]
        public ActionResult<ItemResponse<bool>>CheckProfile(int id)
        {
            int iCode = 200;
            BaseResponse response = null;

            try
            {
                bool? hasProfile = _service.HasProfile(id);
                if (hasProfile == null)
                {
                    iCode = 404;
                    response = new ErrorResponse("Requested Id not found.");
                }
                else
                {
                response = new ItemResponse<bool?>() { Item = hasProfile };
                }
            }
            catch (Exception ex)
            {
                iCode = 500;
                Logger.LogError(ex.ToString());
                response = new ErrorResponse(ex.Message);
            }
            return StatusCode(iCode, response);
        }

        [HttpGet("pass/{email}")]
        public ActionResult<ItemResponse<string>> GetAuthByEmail(string email)
        {
            int iCode = 200;
            BaseResponse response = null;
            try
            {
                string pw = _service.GetAuthByEmail(email);

                if (pw == null)
                {
                    iCode = 404;
                    response = new ErrorResponse("Email Not Found");
                }
                else
                {
                    response = new ItemResponse<string> { Item = pw };
                }
            }
            catch (Exception ex)
            {
                iCode = 500;
                base.Logger.LogError(ex.ToString());
                response = new ErrorResponse($"Generic Error:${ex.Message}");
            }
            return StatusCode(iCode, response);
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public ActionResult<SuccessResponse> Login(UserLoginRequest model)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                bool isValidCreds = _service.LogInAsync(model.Email, model.Password).Result;

                if (isValidCreds == true)
                {
                    response = new SuccessResponse();
                }
                else
                {
                    code = 401;
                    response = new ErrorResponse("Invalid Email or Password.");
                }
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
            }
            return StatusCode(code, response);
        }

        [HttpGet("logout")]
        public async Task<ActionResult<SuccessResponse>> LogoutAsync()
        {
            ObjectResult result = null;
            try
            {
                await _authService.LogOutAsync();
                SuccessResponse response = new SuccessResponse();
                result = StatusCode(200, response);
            }
            catch (Exception ex)
            {

                base.Logger.LogError(ex.ToString());
                ErrorResponse response = new ErrorResponse($"Generic Error: {ex.Message}");
                result = StatusCode(500, response);
            }

            return result;
        }

        [HttpGet("current")]
        public ActionResult<ItemResponse<IUserAuthData>> GetCurrentUser()
        {
            int iCode = 200;
            BaseResponse response = null;
            try
            {
                IUserAuthData currentUser= _authService.GetCurrentUser();

                if (currentUser == null)
                {
                    iCode = 404;
                    response = new ErrorResponse("User Not Found");
                }
                else
                {
                    response = new ItemResponse<IUserAuthData> { Item = currentUser };
                }
            }
            catch (Exception ex)
            {
                iCode = 500;
                base.Logger.LogError(ex.ToString());
                response = new ErrorResponse($"Generic Error:${ex.Message}");
            }
            return StatusCode(iCode, response);
        }

        [HttpPut("reset")]
        [AllowAnonymous]
        public ActionResult<SuccessResponse> ForgotPassword(UserAuth model)
        {
            int iCode = 200;
            BaseResponse response = null;
            try
            {
                UserAuth user = _service.GetAuthData(model.Email);
                if (user == null)
                {
                    iCode = 404;
                    response = new ErrorResponse("Email Not Found");
                }
                else
                {
                    string token = Guid.NewGuid().ToString();
                    int tokenTypeid = (int)TokenType.ResetPassword;
                    _service.AddToken(user.Id, token, tokenTypeid);
                    _emailService.SendResetPassword(model, token);
                    response = new SuccessResponse();
                }
            }
            catch (Exception ex)
            {
                iCode = 500;
                base.Logger.LogError(ex.ToString());
                response = new ErrorResponse($"Generic Error:${ex.Message}");
            }
            return StatusCode(iCode, response);
        }

        [HttpPut("changepassword")]
        [AllowAnonymous]
        public ActionResult<SuccessResponse> ChangePassword(ChangePasswordRequest model)
        {
            int iCode = 200;
            BaseResponse response = null;
            try
            {
                _service.UpdatePassword(model);
                response = new SuccessResponse();
            }
            catch (Exception ex)
            {
                iCode = 500;
                response = new ErrorResponse(ex.Message);
            }
            return StatusCode(iCode, response);
        }

        [HttpGet()]
        public ActionResult<ItemResponse<AdminDash>> GetAllData()
        {
            int code = 200;
            BaseResponse response = null;
            try
            {
                AdminDash dash = _service.GetAllData();
                if (dash == null)
                {
                    code = 404;
                    response = new ErrorResponse("Data Not Found");
                }
                else
                {
                    response = new ItemResponse<AdminDash> { Item = dash };
                }
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
            }
            return StatusCode(code, response);
        }
    }
}