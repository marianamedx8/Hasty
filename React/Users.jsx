import React, { useEffect, useState } from 'react';
import { Row, Card, Col } from 'react-bootstrap';
import * as userService from '../../../services/userService';
import ByMonth from './ByMonth';
import SiteReferencesChart from '../../../components/sitereferences/SiteReferenceChart';
import IncomeChart from './IncomeChart';
import toastr from 'toastr';

function Users() {
  const [pageData, setPageData] = useState({
    data: [],
    usersMapped: [],
    totalUsers: 0,
    activeSubscriptions: 0,
    plan: [],
    names: [],
    amount: [],
    month: [],
    monthData: [],
    year: [],
    week: [],
    currentWeek: [],
  });
  useEffect(() => {
    userService.getTotalUsers().then(onGetTotalSuccess).catch(onGetTotalError);
  }, []);
  const onGetTotalSuccess = (response) => {
    const users = response.item;
    setPageData((prevState) => {
      const newData = { ...prevState };
      newData.data = users;
      newData.totalUsers = users.userCount;
      newData.usersMapped = users.status.map(mapStatus);
      newData.activeSubscriptions = users.activeSubscriptions;
      newData.plan = users.income.map(mapPlan);
      newData.names = users.income.map((data) => data.name);
      newData.amount = users.income.map((data) => data.incomeBySub);
      newData.month = users.byMonth.map((data) => data.name);
      newData.monthData = users.byMonth.map((data) => data.id);
      newData.year = users.byYear.map(mapYear);
      newData.currentWeek = users.byWeek.map((data) => data.amount);
      return newData;
    });
  };
  const onGetTotalError = () => {
    toastr['error']('Page Data Fail');
  };

  const mapStatus = (aStatus, index) => {
    return (
      <div key={index}>
        <h6 className=" mt-0">{aStatus.name}</h6>
        <h2 className="my-2" id="active-users-count">
          {aStatus.totalCount}
        </h2>
      </div>
    );
  };
  const mapPlan = (aPlan, index) => {
    return (
      <div key={index}>
        <h6 className=" mt-1">{aPlan.name}</h6>
        <h2 className="my-2" id="active-users-count">
          {aPlan.subsByPlan}
        </h2>
      </div>
    );
  };
  const mapYear = (aYear, index) => {
    return (
      <div key={index}>
        <h6 className=" mt-1"> Year: {aYear.count}</h6>
        <h2 className="my-2" id="active-users-count">
          {aYear.amount}
        </h2>
      </div>
    );
  };

  return (
    <>
      <Row>
        <Col className="col-2">
          <Card className="tilebox-one">
            <Card.Body>
              <i className="uil uil-users-alt float-end mt-1"></i>
              <h6 className="text-uppercase mt-0">Users</h6>
              <div>
                <h2 className="my-2" id="active-users-count">
                  {pageData.totalUsers}
                </h2>
              </div>
              {pageData.usersMapped}
            </Card.Body>
          </Card>
        </Col>
        <Col className="col-2">
          <Card className="tilebox-one">
            <Card.Body>
              <i className="uil uil-atm-card float-end mt-1"></i>
              <h6 className="text-uppercase mt-0">Subscriptions</h6>
              <div>
                <h2 className="my-2" id="active-users-count">
                  {pageData.activeSubscriptions}
                </h2>
              </div>
              {pageData.plan}
              {pageData.year}
              <div>
                <h6 className=" mt-1"> Current Week</h6>
                <h2 className="my-2" id="active-users-count">
                  {pageData.currentWeek[pageData.currentWeek.length - 1]}
                </h2>
              </div>
            </Card.Body>
          </Card>
        </Col>
        <Col className="col-4">
          <SiteReferencesChart />
        </Col>
        <Col className="col-4">
          <IncomeChart names={pageData.names} income={pageData.amount} />
        </Col>
      </Row>
      <Col className="col-12">
        <ByMonth monthData={pageData.monthData} month={pageData.month} />
      </Col>
    </>
  );
}

export default Users;
