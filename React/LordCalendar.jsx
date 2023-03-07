import React, { useState, useEffect } from 'react';
import { Col, Card, Row, Button } from 'react-bootstrap';
import DatePicker from 'react-datepicker';
import listingReservation from '../../../services/listingReservation';
import './landlord.css';
import locale from 'rc-pagination';
import Pagination from 'rc-pagination';
import { Calendar, MapPin, User } from 'react-feather';
import { format } from 'date-fns';
import toastr from 'toastr';
import debug from 'sabio-debug';
const _logger = debug.extend('***LORD***');

function LordCalendar() {
  const [pageData, setPageData] = useState({
    reservations: [],
    reservMap: [],
    resetMap: [],
    dates: [],
    pageIndex: 0,
    pageSize: 3,
    totalCount: 0,
  });
  const [date, setDate] = useState(new Date());

  useEffect(() => {
    listingReservation
      .getByOwnerPaginate(pageData.pageIndex, pageData.pageSize)
      .then(onGetPageSuccess)
      .catch(onGetPageError);
  }, [pageData.pageIndex]);

  const onGetPageSuccess = (response) => {
    const resArr = response.item.pagedItems;
    setPageData((prevState) => {
      const newData = { ...prevState };
      newData.reservations = resArr;
      newData.reservMap = resArr.map(mapRes);
      newData.resetMap = resArr.map(mapRes);
      newData.dates = resArr.map((aDate) => new Date(aDate.dateCheckIn));
      _logger(newData);
      return newData;
    });
    setPageData((prevState) => {
      const pageData = { ...prevState };
      pageData.totalCount = response.item.totalCount;
      return pageData;
    });
  };

  const onGetPageError = () => {
    toastr['error']('Page Data Fail');
  };

  const onClick = (date) => {
    setDate(date);
    const newDate = new Date(date).toISOString();
    listingReservation
      .getByDateByOwner(pageData.pageIndex, pageData.pageSize, newDate)
      .then(onGetDateSuccess)
      .catch(onGetDateError);
  };

  const onGetDateSuccess = (response) => {
    const newArr = response.item.pagedItems;
    setPageData((prevState) => {
      const newData = { ...prevState };
      newData.reservations = newArr;
      newData.reservMap = newArr.map(mapRes);
      return newData;
    });
  };

  const mapRes = (aReservation) => {
    return (
      <li className="mb-4" key={aReservation.id}>
        <p className="text-muted mb-1 font-15">
          <Calendar size="20px" /> {(aReservation.dateCheckIn = format(new Date(aReservation.dateCheckIn), 'MMM d Y'))}
          {' - '}
          {(aReservation.dateCheckOut = format(new Date(aReservation.dateCheckOut), 'MMM d Y'))}
        </p>
        <h6>
          <MapPin size="20px" className="me-1" />
          {aReservation.listing.internalName}
          <p>
            <User size="20px" className="me-1" />
            {aReservation.user.lastName}
          </p>
        </h6>
      </li>
    );
  };

  const onGetDateError = () => {
    toastr['error']('No dates scheduled');
  };

  const onPageChange = (page) => {
    setPageData((prevState) => {
      const currentPageInfo = { ...prevState };
      currentPageInfo.pageIndex = page - 1;
      return currentPageInfo;
    });
  };
  const onShowAll = () => {
    listingReservation
      .getByOwnerPaginate(pageData.pageIndex, pageData.pageSize)
      .then(onGetPageSuccess)
      .catch(onGetPageError);
  };

  return (
    <>
      <div>
        <Card className="height-calendar">
          <Card.Body>
            <Row>
              <Col className="col-4 ms-4 mt-2 mb-1 ">
                <h4 className="header-title mb-3">Calendar</h4>
                <ul className="list-unstyled mt-3 ">{pageData.reservMap}</ul>
                <Pagination
                  className="position-absolute bottom-0 start-0 ms-3 mb-3 "
                  onChange={onPageChange}
                  current={pageData.pageIndex + 1}
                  total={pageData.totalCount}
                  pageSize={pageData.pageSize}
                  locale={locale}
                />
                <Button onClick={onShowAll} type="button" className="btn-size position-absolute bottom-0 reset-button">
                  Show All
                </Button>
              </Col>
              <Col className="calendar-widget col-7 ms-2 mt-2">
                <DatePicker value={date} onChange={onClick} highlightDates={pageData.dates} fixedHeight inline />
              </Col>
            </Row>
          </Card.Body>
        </Card>
      </div>
    </>
  );
}
export default LordCalendar;
