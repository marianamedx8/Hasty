import React, { useEffect, useState } from 'react';
import { Container, Row, Col } from 'react-bootstrap';
import LandLordProfile from './LandLordProfile';
import ListingTable from './ListingTable';
import LordFiles from './LordFiles';
import LordCalendar from './LordCalendar';
import Analytics from './Analytics';
import * as userService from '../../../services/userService';
import './landlord.css';
import Header from '../../../components/elements/Header';
import debug from 'sabio-debug';
const _logger = debug.extend('LANDLORD');
function LandlordDashboard() {
    const [currentLord, setCurrentLord] = useState({
        id: 0,
        firstName: '',
        lastName: '',
        mi: '',
        email: '',
        avatarUrl: '',
        roles: [],
    });
    const crumbs = [
        { name: 'Dashboard', path: '/dashboard' },
        { name: 'Landlord', path: '/dashboard/landlord' },
    ];
    useEffect(() => {
        userService.getUserDetails().then(ongGetUserSuccess).catch(onGetUserError);
    }, []);

    const ongGetUserSuccess = (response) => {
        const landlord = response.item;
        setCurrentLord((prevState) => {
            return {
                ...prevState,
                ...landlord,
            };
        });
    };

    const onGetUserError = (error) => {
        _logger(error);
    };

    return (
        <>
            <Container>
                <Row>
                    <Col>
                        <Header title="Dashboard" crumbs={crumbs} />
                    </Col>
                </Row>
                <Row>
                    <Col className="col-8">
                        <ListingTable />
                    </Col>
                    <Col className="col-4">
                        <LandLordProfile lord={currentLord} />
                    </Col>
                    <Col className="col-7">
                        <LordFiles />
                    </Col>
                    <Col className="col-5">
                        <Analytics />
                    </Col>
                </Row>
                <Row>
                    <Col>
                        <LordCalendar />
                    </Col>
                </Row>
            </Container>
        </>
    );
}

export default LandlordDashboard;
