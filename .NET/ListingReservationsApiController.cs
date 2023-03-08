using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Hasty.Services.Interfaces;
using Hasty.Services;
using Hasty.Web.Controllers;
using Hasty.Web.Models.Responses;
using System.Collections.Generic;
using System;
using Hasty.Models.Domain.ListingReservations;
using Hasty.Models;
using Hasty.Models.Requests.ListingReservations;
using System.Data.SqlClient;
using Hasty.Models.Domain.Listings;
using Hasty.Models.Domain;

namespace Hasty.Web.Api.Controllers
{
    [Route("api/listings/reservations")]
    [ApiController]
    public class ListingReservationsApiController : BaseApiController
    {
        private IListingReservationsService _service = null;
        private IAuthenticationService<int> _authService = null;

        public ListingReservationsApiController(IListingReservationsService service
        , ILogger<ListingReservationsApiController> logger
        , IAuthenticationService<int> authService) : base(logger)
        {
            _service = service;
            _authService = authService;
        }
        
        [HttpGet("paginate")]
        public ActionResult<ItemResponse<Paged<ListingReservation>>> GetAllPaginated(int pageIndex, int pageSize)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                Paged<ListingReservation> page = _service.GetAllPaginated(pageIndex, pageSize);

                if (page == null)
                {
                    code = 404;
                    response = new ErrorResponse("App Resource not found.");
                }
                else
                {
                    response = new ItemResponse<Paged<ListingReservation>> { Item = page };
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
        
        [HttpGet("createdby/{userId:int}")]
        public ActionResult<ItemResponse<Paged<ListingReservation>>> GetByCreatedByPaginated(int pageIndex, int pageSize, int userId)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                Paged<ListingReservation> page = _service.GetByCreatedByPaginated(pageIndex, pageSize, userId);

                if (page == null)
                {
                    code = 404;
                    response = new ErrorResponse("App Resource not found.");
                }
                else
                {
                    response = new ItemResponse<Paged<ListingReservation>> { Item = page };
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

        [HttpGet("{id:int}")]
        public ActionResult<ItemResponse<ListingReservation>> GetById(int id)
        {

            int iCode = 200;
            BaseResponse response = null;

            try
            {
                ListingReservation aListing = _service.GetById(id);


                if (aListing == null)
                {
                    iCode = 404;
                    response = new ErrorResponse("Application Resource not found");
                }
                else
                {
                    response = new ItemResponse<ListingReservation> { Item = aListing };
                }
            }

            catch (Exception ex)
            {
                iCode = 500;
                base.Logger.LogError(ex.ToString());
                response = new ErrorResponse($"Generic Error: {ex.Message}");

            }

            return StatusCode(iCode, response);

        }

        [HttpPost]
        public ActionResult<ItemResponse<int>> Add(ListingReservationAddRequest request)
        {
            ObjectResult result = null;
            try
            {
                int id = _service.Add(request);
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
        
        [HttpPut("{id:int}")]
        public ActionResult<ItemResponse<int>> Update(ListingReservationAddRequest model, int id)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                _service.Update(model, id);

                response = new SuccessResponse();
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
            }

            return StatusCode(code, response);
        }

        [HttpPut("status/listing/{listingid:int}")]
        public ActionResult<ItemResponse<int>> UpdateStatusByListingId(ListingReservationUpdateStatus model, int listingid)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                _service.UpdateStatusByListingId(model, listingid);

                response = new SuccessResponse();
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
            }

            return StatusCode(code, response);
        }

        [HttpPut("status/{id:int}")]
        public ActionResult<ItemResponse<int>> UpdateStatusById(ListingReservationUpdateStatus model, int id)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                _service.UpdateStatusById(model, id);

                response = new SuccessResponse();
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
            }

            return StatusCode(code, response);
        }

        [HttpGet("paginate/owner")]
        public ActionResult<ItemResponse<Paged<ListingReservation>>> GetByOwnerPaginated(int pageIndex, int pageSize)
        {
            ActionResult result = null;

            try
            {
                int userId = _authService.GetCurrentUserId();
                Paged<ListingReservation> paged = _service.GetByOwnerPaginated(pageIndex, pageSize, userId);

                if (paged == null)
                {
                    result = NotFound404(new ErrorResponse("Records Not Found"));
                }
                else
                {
                    ItemResponse<Paged<ListingReservation>> response = new ItemResponse<Paged<ListingReservation>>() { Item = paged };
                    result = Ok200(response);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                result = StatusCode(500, new ErrorResponse(ex.Message.ToString()));
            }

            return result;
        }

        [HttpGet("paginate/date")]
        public ActionResult<ItemResponse<Paged<ListingReservation>>> GetByDateByOwner(int pageIndex, int pageSize, string selectDate)
        {
            ActionResult result = null;

            try
            {
                int userId = _authService.GetCurrentUserId();
                Paged<ListingReservation> paged = _service.GetByDateByOwner(pageIndex, pageSize, selectDate, userId);

                if (paged == null)
                {
                    result = NotFound404(new ErrorResponse("Records Not Found"));
                }
                else
                {
                    ItemResponse<Paged<ListingReservation>> response = new ItemResponse<Paged<ListingReservation>>() { Item = paged };
                    result = Ok200(response);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                result = StatusCode(500, new ErrorResponse(ex.Message.ToString()));
            }

            return result;
        }
        
        [HttpGet("popular")]
        public ActionResult<ItemResponse<PopularListingReservation>> GetPopularByOwner()
        {
            int code = 200;
            BaseResponse response = null;
            try
            {
                int userId = _authService.GetCurrentUserId();
                List<PopularListingReservation> list = _service.GetPopularByOwner(userId);
                if (list == null)
                {
                    code = 404;
                    response = new ErrorResponse("Reservation Not Found");
                }
                else
                {
                    response = new ItemsResponse<PopularListingReservation> { Items = list };
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
