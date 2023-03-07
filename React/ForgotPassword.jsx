import React from 'react';
import { Container, Row, Col, Card, FormLabel } from 'react-bootstrap';
import Logo from '../../assets/images/hastylogo.png';
import { Link, useNavigate } from 'react-router-dom';
import { Formik, Form, Field, ErrorMessage } from 'formik';
import './userstyles.css';
import { passwordSchema } from '../../schemas/passwordSchema';
import * as userService from '../../services/userService';
import swal from 'sweetalert2';
import debug from 'sabio-debug';
const _logger = debug.extend('FORGOTPASSWORD');

function ForgotPassword() {
    const navigate = useNavigate();
    const userFormData = {
        email: '',
    };

    const onSubmit = (values) => {
        _logger(values);
        userService.resetPassword(values).then(onResetSuccess).catch(onResetFailure);
    };

    const onResetSuccess = () => {
        swal.fire('Email Sent!', 'An email has been sent to you to reset your password.', 'success').then(
            navigate('/')
        );
    };

    const onResetFailure = (response) => {
        _logger(response);
        swal.fire('Error', 'Reset Failed. Please make sure email is correct & try again.', 'error');
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
                                    <div className="text-center m-auto">
                                        <h4 className="text-dark-50 text-center mt-0 font-weight-bold">
                                            {'Reset Password'}
                                        </h4>
                                        <p className="text-muted mb-4">
                                            {
                                                "Enter your email address and we'll send you an email with instructions to reset your password"
                                            }
                                        </p>
                                    </div>
                                    <Formik
                                        enableReinitialize={true}
                                        initialValues={userFormData}
                                        onSubmit={onSubmit}
                                        validationSchema={passwordSchema}>
                                        <Form>
                                            <div className="mb-3 font-box">
                                                <FormLabel className="form-label">Email address</FormLabel>
                                                <Field
                                                    type="email"
                                                    className="form-control"
                                                    placeholder="Enter Your Email Address"
                                                    id="email"
                                                    name="email"
                                                />
                                                <ErrorMessage
                                                    name="email"
                                                    component="div"
                                                    className="user-register-error-form"
                                                />
                                            </div>
                                            <div className="mb-3 mb-0 text-center font-box">
                                                <button
                                                    type="submit"
                                                    name="submit"
                                                    className="btn btn-lg btn-color-font submit">
                                                    Submit
                                                </button>
                                            </div>
                                        </Form>
                                    </Formik>
                                </Card.Body>
                            </Card>
                            <Row className="mt-3">
                                <Col className="text-center">
                                    <p className="text-muted">
                                        {'Back to'}
                                        <Link to={'/login'} className="text-muted ms-1">
                                            <b>{'Log In'}</b>
                                        </Link>
                                    </p>
                                </Col>
                            </Row>
                        </Col>
                    </Row>
                </Container>
            </div>

            <footer className="footer footer-alt">{'2023 © Hasty - Hasty.com'}</footer>
        </>
    );
}
export default ForgotPassword;
