import React from 'react';
import { Container, Row, Col } from 'react-bootstrap';
import Header from '../../../components/elements/Header';
import Users from './Users';

function AdminDashboard() {
  const crumbs = [
    { name: 'Dashboard', path: '/dashboard' },
    { name: 'Admin', path: '/dashboard/Admin' },
  ];

  return (
    <>
      <Container>
        <Row>
          <Col>
            <Header title="Dashboard" crumbs={crumbs} />
          </Col>
        </Row>
        <Row>
          <Col className="col-12">
            <Users />
          </Col>
        </Row>
      </Container>
    </>
  );
}

export default AdminDashboard;
