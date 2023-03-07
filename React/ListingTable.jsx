import React, { useState, useEffect } from 'react';
import { Card, Table } from 'react-bootstrap';
import listingService from '../../../services/listingService';
import locale from 'rc-pagination';
import Pagination from 'rc-pagination';
import TableBod from './TableBod';
import './landlord.css';
import { Link } from 'react-router-dom';
import debug from 'sabio-debug';
const _logger = debug.extend('LANDLORDTABLE');

function ListingTable() {
    const [pageData, setPageData] = useState({
        listings: [],
        listingsMapped: [],
        filter: 0,
        filterType: true,
        pageIndex: 0,
        pageSize: 4,
        totalCount: 0,
    });

    useEffect(() => {
        if (pageData.filter > 0) {
            listingService
                .getCurrentByStatus(pageData.pageIndex, pageData.pageSize, pageData.filterType)
                .then(onGetPageSuccess)
                .catch(onGetPageError);
        } else {
            listingService
                .getCurrent(pageData.pageIndex, pageData.pageSize)
                .then(onGetPageSuccess)
                .catch(onGetPageError);
        }
    }, [pageData.pageIndex, pageData.filter, pageData.filterType]);

    const onGetPageSuccess = (response) => {
        const listingsArr = response.item.pagedItems;
        setPageData((prevState) => {
            const newData = { ...prevState };
            newData.listings = listingsArr;
            newData.listingsMapped = listingsArr.map(mapListing);
            return newData;
        });
        setPageData((prevState) => {
            const pageData = { ...prevState };
            pageData.totalCount = response.item.totalCount;
            return pageData;
        });
    };

    const onGetPageError = (error) => {
        _logger(error);
    };

    const updatePage = ({ id, isActive }) => {
        _logger(id, isActive);
        setPageData((prevState) => {
            const pd = { ...prevState };
            pd.listings = [...pd.listings];
            const idxOf = pd.listings.findIndex((listing) => listing.id === id);
            if (idxOf >= 0) {
                pd.listings[idxOf].isActive = isActive;
                pd.listingsMapped = pd.listings.map(mapListing);
            }
            return pd;
        });
    };

    const deleteHandler = ({ id }) => {
        _logger('idToBeDeleted', id);
        setPageData((prevState) => {
            const pd = { ...prevState };
            pd.listings = [...pd.listings];
            const idxOf = pd.listings.findIndex((listing) => listing.id === id);
            if (idxOf >= 0) {
                pd.listings.splice(idxOf, 1);
                pd.listingsMapped = pd.listings.map(mapListing);
            }
            return pd;
        });
    };

    const mapListing = (aListing, index) => {
        return <TableBod key={index} listing={aListing} updatePage={updatePage} deleteHandler={deleteHandler} />;
    };

    const onPageChange = (page) => {
        setPageData((prevState) => {
            const currentPageInfo = { ...prevState };
            currentPageInfo.pageIndex = page - 1;
            return currentPageInfo;
        });
    };

    const onShowActive = () => {
        setPageData((prevState) => {
            const newState = { ...prevState };
            newState.filter = 1;
            newState.filterType = true;
            pageData.pageIndex = 0;
            return newState;
        });
    };

    const onShowInActive = () => {
        setPageData((prevState) => {
            const newState = { ...prevState };
            newState.filter = 2;
            newState.filterType = false;
            pageData.pageIndex = 0;
            return newState;
        });
    };

    const onShowAll = () => {
        setPageData((prevState) => {
            const newState = { ...prevState };
            newState.filter = 0;
            pageData.pageIndex = 0;
            _logger(newState);
            return newState;
        });
    };

    return (
        <div className=" height-lord ">
            <Card className="height-lord ">
                <Card.Body>
                    <button className="btn btn-primary btn-size me-1 mb-1" onClick={onShowAll} id="show-all">
                        Show All
                    </button>
                    <button
                        type="button"
                        className="btn btn-primary btn-size  me-1 mb-1"
                        onClick={pageData?.filterType ? onShowInActive : onShowActive}
                        id="show-inactive">
                        {pageData?.filterType ? 'Inactive' : 'Active'}
                    </button>
                    <Link className=" btn btn-primary btn-size position-absolute top-0 end-0 m-3" to="/listing/create">
                        Add Listing
                    </Link>
                    <Table className="border">
                        <thead className="row-color">
                            <tr>
                                <th>Address</th>
                                <th>State</th>
                                <th>Description</th>
                                <th>Type</th>
                                <th>Status</th>
                                <th></th>
                            </tr>
                        </thead>
                        <tbody>{pageData.listingsMapped}</tbody>
                    </Table>
                    <Pagination
                        className="position-absolute bottom-0 start-0 m-3"
                        onChange={onPageChange}
                        current={pageData.pageIndex + 1}
                        total={pageData.totalCount}
                        pageSize={pageData.pageSize}
                        locale={locale}
                    />
                </Card.Body>
            </Card>
        </div>
    );
}

export default ListingTable;
