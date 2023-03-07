import React, { useState } from 'react';
import Logo from '../../assets/images/hastylogo.png';
import { Container, Row, Col, Card, FormLabel } from 'react-bootstrap';
import { Formik, Form, Field, ErrorMessage } from 'formik';
import { Link, useSearchParams, useNavigate } from 'react-router-dom';
import './userstyles.css';
import { changePassSchema } from '../../schemas/changePassSchema';
import * as userService from '../../services/userService';
import swal from 'sweetalert2';

function ChangePassword() {
    const navigate = useNavigate();
    const userFormData = {
        password: '',
        passwordConfirm: '',
    };
    const [searchParams] = useSearchParams();
    const [showPassword, setShowPassword] = useState(true);
    const onEyeClicked = () => {
        setShowPassword((prevState) => {
            return !prevState;
        });
    };

    const onSubmit = (values) => {
        const email = searchParams.get('email');
        const token = searchParams.get('token');
        values.email = email;
        values.token = token;
        userService.changePassword(values).then(onChangeSuccess).catch(onChangeError);
    };

    const onChangeSuccess = () => {
        swal.fire('Hooray!', 'Password Change Successful!', 'success', {
            button: 'Ok',
        }).then(navigate('/login'));
    };

    const onChangeError = () => {
        swal.fire('Error', 'Please try again.', 'error');
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
                                            {'Change Password'}
                                        </h4>
                                        <p className="text-muted mb-4">
                                            {'Create your new password for Hasty so you can log in to your account.'}
                                        </p>
                                    </div>
                                    <Formik
                                        enableReinitialize={true}
                                        initialValues={userFormData}
                                        onSubmit={onSubmit}
                                        validationSchema={changePassSchema}>
                                        <Form>
                                            <div className="mb-3 font-box">
                                                <FormLabel className="form-label">Password</FormLabel>
                                                <Field
                                                    type={showPassword ? 'password' : 'text'}
                                                    className="form-control"
                                                    id="password"
                                                    name="password"
                                                    placeholder="Enter your new Password"
                                                />

                                                <span
                                                    className="password-eye user-eye-toggle-password"
                                                    onClick={onEyeClicked}
                                                />

                                                <ErrorMessage
                                                    name="password"
                                                    component="div"
                                                    className="user-register-error-form "
                                                />
                                            </div>
                                            <div className="mb-3 font-box">
                                                <FormLabel className="form-label">Confirm Password</FormLabel>
                                                <Field
                                                    type={showPassword ? 'password' : 'text'}
                                                    className="form-control"
                                                    id="passwordConfirm"
                                                    name="passwordConfirm"
                                                    placeholder="Confirm Password"
                                                />

                                                <span
                                                    className="password-eye user-eye-toggle-password"
                                                    onClick={onEyeClicked}
                                                />

                                                <ErrorMessage
                                                    name="passwordConfirm"
                                                    component="div"
                                                    className="user-register-error-form "
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
export default ChangePassword;
