import React, { useEffect } from 'react';
import { Link, useSearchParams, useNavigate } from 'react-router-dom';
import { Container, Row, Col, Card } from 'react-bootstrap';
import Logo from '../../assets/images/hastylogo.png';
import './userstyles.css';
import swal from 'sweetalert2';
import * as userService from '../../services/userService';
import debug from 'sabio-debug';
const _logger = debug.extend('CONFIRM');
function Confirm() {
    const [searchParams] = useSearchParams();
    const navigate = useNavigate();

    useEffect(() => {
        const email = searchParams.get('email');
        const token = searchParams.get('token');
        _logger(email, token);
        userService.updateConfirm(email, token).then(onConfirmSuccess).catch(onConfirmFailure);
    }, []);

    const onConfirmSuccess = () => {
        swal.fire('Hooray!', 'Confirmation Successful!', 'success', {
            button: 'Ok',
        }).then(navigate('/login'));
    };

    const onConfirmFailure = (error) => {
        _logger(error);
        swal.fire('Error', 'Confirmation Failed.', 'error');
    };
    return (
        <>
            <div className="account-pages pt-2 pt-sm-5 pb-4 pb-sm-5">
                <Container>
                    <Row className="justify-content-center">
                        <Col md={8} lg={6} xl={5} xxl={4}>
                            <Card>
                                <Card.Header className="pt-1 pb-1 text-center primary-color-head">
                                    <Link to="/">
                                        <span>
                                            {' '}
                                            <img src={Logo} alt="hasty-logo-navigate-home" height="60" />{' '}
                                        </span>
                                    </Link>
                                </Card.Header>
                                <Card.Body className="p-4">
                                    <h4 className="text-dark-50 text-center mt-4 fw-bold">Confirming Email...</h4>
                                </Card.Body>
                            </Card>
                        </Col>
                    </Row>
                </Container>
            </div>
            <footer className="footer footer-alt">{'2023 © Hasty - Hasty.com'}</footer>
        </>
    );
}

export default Confirm;
