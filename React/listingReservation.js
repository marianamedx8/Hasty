import axios from 'axios';
import { onGlobalError, onGlobalSuccess, API_HOST_PREFIX } from './serviceHelpers';

const endpoint = `${API_HOST_PREFIX}/api/listings/reservations`;

const getByOwnerPaginate = (pageIndex, pageSize) => {
    const config = {
        method: 'GET',
        url: `${endpoint}/paginate/owner?pageIndex=${pageIndex}&pageSize=${pageSize}`,
        crossdomain: true,
        headers: { 'Content-Type': 'application/json' },
    };
    return axios(config).then(onGlobalSuccess).catch(onGlobalError);
};

const getByDateByOwner =(pageIndex, pageSize, selectDate)=>{
    const config ={
        method: 'GET',
        url:`${endpoint}/paginate/date?pageIndex=${pageIndex}&pageSize=${pageSize}&selectDate=${selectDate}`,
        crossdomain: true,
        headers: {'Content-Type' : 'application/json'},
    }
    return axios(config).then(onGlobalSuccess).catch(onGlobalError);
}

const getPopular = ()=>{
    const config ={
        method: 'GET',
        url:`${endpoint}/popular`,
        crossdomain: true,
        headers: {'Content-Type' : 'application/json'},
    }
    return axios(config).then(onGlobalSuccess).catch(onGlobalError);
}

const listingReservation ={
    getByOwnerPaginate, getByDateByOwner,getPopular,
}
export default listingReservation;