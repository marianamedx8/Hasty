using Sabio.Data.Providers;
using Sabio.Models.Domain.ListingReservations;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using Sabio.Data;
using Sabio.Models.Domain.Users;
using Sabio.Models;
using Sabio.Models.Requests.ListingReservations;
using System.Reflection;
using Sabio.Services.Interfaces;
using sib_api_v3_sdk.Model;
using Sabio.Models.Domain;
using Sabio.Models.Domain.Listings;
using Google.Apis.AnalyticsReporting.v4.Data;

namespace Sabio.Services
{
    public class ListingReservationsService : IListingReservationsService
    {
        IDataProvider _data = null;
        static IBaseUserMapper _baseUserMapper = null;
        static IListingService _listingService = null;
        static ILocationMapper _locationMapper = null;

        public ListingReservationsService(IDataProvider data, IBaseUserMapper baseUserMapper, IListingService listingService, ILocationMapper locationMapper)
        {
            _data = data;
            _baseUserMapper = baseUserMapper;
            _listingService = listingService;
            _locationMapper = locationMapper;
        }
        public int Add(ListingReservationAddRequest model)
        {
            int id = 0;

            string procName = "[dbo].[ListingReservations_Insert]";
            _data.ExecuteNonQuery(procName,
                inputParamMapper: delegate (SqlParameterCollection coll)
                {
                    AddCommonParams(model, coll);

                    SqlParameter idOut = new SqlParameter("@Id", SqlDbType.Int);
                    idOut.Direction = ParameterDirection.Output;
                    coll.Add(idOut);
                },

                returnParameters: delegate (SqlParameterCollection returnCollection)
                {
                    object oldId = returnCollection["@Id"].Value;
                    int.TryParse(oldId.ToString(), out id);
                }
            );
            return id;
        }
        public Paged<ListingReservation> GetAllPaginated(int pageIndex, int pageSize)
        {
            Paged<ListingReservation> pagedList = null;
            List<ListingReservation> list = null;
            int totalCount = 0;
            string procName = "dbo.ListingReservations_SelectAll_PaginationV2";
            _data.ExecuteCmd(procName, inputParamMapper: delegate (SqlParameterCollection col)
            {
                col.AddWithValue("@PageIndex", pageIndex);
                col.AddWithValue("@PageSize", pageSize);
            },
            singleRecordMapper: delegate (IDataReader reader, short set)
            {
                int startingIndex = 0;
                ListingReservation aReservation = MapSingleListingReservation(reader, ref startingIndex);
                if (totalCount == 0)
                {
                    totalCount = reader.GetSafeInt32(startingIndex++);
                }
                if (list == null)
                {
                    list = new List<ListingReservation>();
                }
                list.Add(aReservation);
            });
            if (list != null)
            {
                pagedList = new Paged<ListingReservation>(list, pageIndex, pageSize, totalCount);
            }
            return pagedList;

        }      
        public Paged<ListingReservation> GetByCreatedByPaginated(int pageIndex, int pageSize, int userid)
        {
            Paged<ListingReservation> pagedList = null;
            List<ListingReservation> list = null;
            int totalCount = 0;

            _data.ExecuteCmd(
                "dbo.ListingReservations_SelectBy_CreatedBy_Paginated",
                (param) =>

                {
                    param.AddWithValue("@PageIndex", pageIndex);
                    param.AddWithValue("@PageSize", pageSize);
                    param.AddWithValue("@UserId", userid);
                },
                (reader, recordSetIndex) =>
                {
                    int startingIndex = 0;
                    ListingReservation aListing = MapSingleListingReservation(reader, ref startingIndex);
                    totalCount = reader.GetSafeInt32(startingIndex++);

                    if (list == null)
                    {
                        list = new List<ListingReservation>();
                    }
                    list.Add(aListing);
                }
                );
            if (list != null)
            {
                pagedList = new Paged<ListingReservation>(list, pageIndex, pageSize, totalCount);
            }
            return pagedList;

        }
        public ListingReservation GetById(int id)
        {
            string procName = "[dbo].[ListingReservations_SelectById]";
            ListingReservation listing = null;

            _data.ExecuteCmd(procName, delegate (SqlParameterCollection paramCollection)
            {
                paramCollection.AddWithValue("@Id", id);

            }, delegate (IDataReader reader, short set)
            {
                int startingIndex = 0;
                listing = MapSingleListingReservation(reader, ref startingIndex);
            }
            );
            return listing;
        }
        public void Update(ListingReservationAddRequest request, int id)
        {
            string procName = "[dbo].[ListingReservations_Update_ById]";

            _data.ExecuteNonQuery(procName,
                inputParamMapper: delegate (SqlParameterCollection colparameter)
                {
                    AddCommonParams(request, colparameter);
                    colparameter.AddWithValue("@Id", id);
                },
                returnParameters: null
            );
        }
        public void UpdateStatusByListingId(ListingReservationUpdateStatus request, int listingid)
        {
            string procName = "[dbo].[ListingReservations_Update_StatusByListingId]";

            _data.ExecuteNonQuery(procName,
                inputParamMapper: delegate (SqlParameterCollection colparameter)
                {
                    colparameter.AddWithValue("@StatusId", request.StatusId);
                    colparameter.AddWithValue("@ListingId", listingid);
                },
                returnParameters: null
            );
        }
        public void UpdateStatusById(ListingReservationUpdateStatus request, int id)
        {
            string procName = "[dbo].[ListingReservations_Update_StatusById]";

            _data.ExecuteNonQuery(procName,
                inputParamMapper: delegate (SqlParameterCollection colparameter)
                {
                    colparameter.AddWithValue("@StatusId", request.StatusId);
                    colparameter.AddWithValue("@Id", id);
                },
                returnParameters: null
            );
        }
        public Paged<ListingReservation> GetByOwnerPaginated(int pageIndex, int pageSize, int userId)
        {
            Paged<ListingReservation> pagedList = null;
            List<ListingReservation> listingsList = null;
            int totalCount = 0;
            string procName = "[dbo].[ListingReservations_SelectBy_ListingOwner_Pagination]";
            _data.ExecuteCmd(procName, inputParamMapper: delegate (SqlParameterCollection col)
            {
                col.AddWithValue("@PageIndex", pageIndex);
                col.AddWithValue("@PageSize", pageSize);
                col.AddWithValue("@CreatedBy", userId);
            },
            singleRecordMapper: delegate (IDataReader reader, short set)
            {
                int startingIndex = 0;
                ListingReservation listing = MapSingleListingReservation(reader, ref startingIndex);
                if (totalCount == 0)
                {
                    totalCount = reader.GetSafeInt32(startingIndex++);
                }
                if (listingsList == null)
                {
                    listingsList = new List<ListingReservation>();
                }
                listingsList.Add(listing);
            });
            if (listingsList != null)
            {
                pagedList = new Paged<ListingReservation>(listingsList, pageIndex, pageSize, totalCount);
            }
            return pagedList;
        }
        public List<PopularListingReservation> GetPopularByOwner(int userId)
        {
            List<PopularListingReservation> list = null;
            string procName = "[dbo].[ListingReservations_SelectPopularByCreatedBy]";

            _data.ExecuteCmd(procName, delegate (SqlParameterCollection paramCollection)
            {
                paramCollection.AddWithValue("@CreatedBy", userId);

            }, delegate (IDataReader reader, short set)
            {
                int startingIndex = 0;
                PopularListingReservation listing = MapSIngleReservationPopular(reader, ref startingIndex);
                if (list == null)
                {
                    list = new List<PopularListingReservation>();
                }
                list.Add(listing);

            }
            );
            return list;
        }       

        public Paged<ListingReservation> GetByDateByOwner(int pageIndex, int pageSize, string selectDate, int userId)
        {
            Paged<ListingReservation> pagedList = null;
            List<ListingReservation> listingsList = null;
            int totalCount = 0;
            string procName = "[dbo].[ListingReservations_SelectBy_ListingOwner_Date]";
            _data.ExecuteCmd(procName, inputParamMapper: delegate (SqlParameterCollection col)
            {
                col.AddWithValue("@PageIndex", pageIndex);
                col.AddWithValue("@PageSize", pageSize);
                col.AddWithValue("@SelectDate", selectDate);
                col.AddWithValue("@CreatedBy", userId);
            },
            singleRecordMapper: delegate (IDataReader reader, short set)
            {
                int startingIndex = 0;
                ListingReservation listing = MapSingleListingReservation(reader, ref startingIndex);
                if (totalCount == 0)
                {
                    totalCount = reader.GetSafeInt32(startingIndex++);
                }
                if (listingsList == null)
                {
                    listingsList = new List<ListingReservation>();
                }
                listingsList.Add(listing);
            });
            if (listingsList != null)
            {
                pagedList = new Paged<ListingReservation>(listingsList, pageIndex, pageSize, totalCount);
            }
            return pagedList;
        }
        private static void AddCommonParams(ListingReservationAddRequest model, SqlParameterCollection coll)
        {
            coll.AddWithValue("@ListingId", model.ListingId);
            coll.AddWithValue("@DateCheckIn", model.DateCheckIn);
            coll.AddWithValue("@DateCheckOut", model.DateCheckOut);
            coll.AddWithValue("@ChargeId", model.ChargeId);
            coll.AddWithValue("@StatusId", model.StatusId);
            coll.AddWithValue("@UserId", model.UserId);
        }
        private static ListingReservation MapSingleListingReservation(IDataReader reader, ref int startingIndex)
        {
            ListingReservation aReservation = new ListingReservation();


            aReservation.Id = reader.GetSafeInt32(startingIndex++);
            aReservation.Listing = new Listing();
            aReservation.Listing.Id = reader.GetSafeInt32(startingIndex++);
            aReservation.Listing.InternalName = reader.GetSafeString(startingIndex++);
            aReservation.Listing.Title = reader.GetSafeString(startingIndex++);
            aReservation.Listing.ShortDescription = reader.GetSafeString(startingIndex++);
            aReservation.Listing.Description = reader.GetSafeString(startingIndex++);
            aReservation.Listing.BedRooms = reader.GetSafeInt16(startingIndex++);
            aReservation.Listing.Baths = reader.GetSafeFloat(startingIndex++);
            aReservation.Listing.HousingType = new LookUp();
            aReservation.Listing.HousingType.Id = reader.GetSafeInt32(startingIndex++);
            aReservation.Listing.HousingType.Name = reader.GetSafeString(startingIndex++);
            aReservation.Listing.AccessType = new LookUp();
            aReservation.Listing.AccessType.Id = reader.GetSafeInt32(startingIndex++);
            aReservation.Listing.AccessType.Name = reader.GetSafeString(startingIndex++);
            aReservation.Listing.ListingServices = new List<LookUp>();
            aReservation.Listing.ListingServices = reader.DeserializeObject<List<LookUp>>(startingIndex++);
            aReservation.Listing.ListingAmenities = new List<LookUp>();
            aReservation.Listing.ListingAmenities = reader.DeserializeObject<List<LookUp>>(startingIndex++);
            aReservation.Listing.GuestCapacity = reader.GetSafeInt16(startingIndex++);
            aReservation.Listing.CostPerNight = reader.GetSafeInt32(startingIndex++);
            aReservation.Listing.CostPerWeek = reader.GetSafeInt32(startingIndex++);
            aReservation.Listing.CheckInTime = reader.GetSafeTimeSpan(startingIndex++);
            aReservation.Listing.CheckOutTime = reader.GetSafeTimeSpan(startingIndex++);
            aReservation.Listing.DaysAvailable = reader.GetSafeInt32(startingIndex++);
            aReservation.Listing.Location = _locationMapper.MapLocation(reader, ref startingIndex);
            aReservation.Listing.HasVerifiedOwnerShip = reader.GetSafeBool(startingIndex++);
            aReservation.Listing.IsActive = reader.GetSafeBool(startingIndex++);
            aReservation.Listing.Images = new List<Image>();
            aReservation.Listing.Images = reader.DeserializeObject<List<Image>>(startingIndex++);
            aReservation.Listing.CreatedBy = reader.GetSafeInt32(startingIndex++);
            aReservation.Listing.DateCreated = reader.GetSafeDateTime(startingIndex++);
            aReservation.Listing.DateModified = reader.GetSafeDateTime(startingIndex++);
            aReservation.DateCheckIn = reader.GetSafeDateTime(startingIndex++);
            aReservation.DateCheckOut = reader.GetSafeDateTime(startingIndex++);
            aReservation.ChargeId = reader.GetSafeString(startingIndex++);
            aReservation.ReservationStatus = new LookUp();
            aReservation.ReservationStatus.Id = reader.GetSafeInt32(startingIndex++);
            aReservation.ReservationStatus.Name = reader.GetSafeString(startingIndex++);
            aReservation.User = _baseUserMapper.MapBaseUser(reader, ref startingIndex);
            aReservation.DateCreated = reader.GetSafeDateTime(startingIndex++);
            aReservation.DateModified = reader.GetSafeDateTime(startingIndex++);
            return aReservation;
        }
        private static PopularListingReservation MapSIngleReservationPopular(IDataReader reader, ref int startingIndex)
        {
            PopularListingReservation listing = new PopularListingReservation();

            listing.ListingId = reader.GetSafeInt32(startingIndex++);
            listing.TotalRepetitions = reader.GetSafeInt32(startingIndex++);
            listing.InternalName = reader.GetSafeString(startingIndex++);
            return listing;
        }

    }
}
