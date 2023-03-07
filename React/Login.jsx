import React, { useState } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { Formik, Form, Field, ErrorMessage } from 'formik';
import { Container, Row, Col, Card, FormLabel } from 'react-bootstrap';
import './userstyles.css';
import Logo from '../../assets/images/hastylogo.png';
import * as userService from '../../services/userService';
import swal from 'sweetalert2';
import { loginSchema } from '../../schemas/loginSchema';
import debug from 'sabio-debug';
const _logger = debug.extend('LOGIN');

function Login() {
  const navigate = useNavigate();
  const userFormData = {
    email: '',
    password: '',
  };
  const [showPassword, setShowPassword] = useState(true);
  const onEyeClicked = () => {
    setShowPassword((prevState) => {
      return !prevState;
    });
  };
  const onSubmit = (values) => {
    userService.login(values).then(onLoginSuccess).catch(onLoginError);
  };

  const onLoginSuccess = () => {
    userService.getCurrentUser().then(onGetCurrentSuccess).catch(onGetCurrentError);
  };

  const onLoginError = (error) => {
    _logger(error);
    swal.fire('Error', 'Login Unsuccessful, please try again. ', 'error');
  };

  const onGetCurrentSuccess = (response) => {
    const user = response.item;
    swal.fire('Hooray!', 'Login Successful!', 'success', {
      button: 'Ok',
    });
    userService
      .checkUserProfile(user.id)
      .then((response) => switchNav(user, response.item))
      .catch(onGetCurrentError);
  };
  const onGetCurrentError = (error) => {
    _logger(error);
  };

  const switchNav = (user, bool) => {
    const stateUserForTransport = { type: 'USER_VIEW', payload: user };
    let hasProfile = bool;

    if (hasProfile) {
      switch (user.roles[user.roles.length - 1]) {
        case 'Admin':
          navigate('/dashboard/admin', { state: stateUserForTransport });
          break;
        case 'Proprietor':
          navigate('/dashboard/landlord', { state: stateUserForTransport });
          break;
        case 'Civilian':
          navigate('/dashboard/civilian', { state: stateUserForTransport });
          break;
        case 'Active Duty':
          navigate('dashboard/activeduty', { state: stateUserForTransport });
          break;
        case 'Veteran':
          navigate('dashboard/veteran', { state: stateUserForTransport });
          break;
        default:
          navigate('/', { state: stateUserForTransport });
      }
    } else {
      switch (user.roles[user.roles.length - 1]) {
        case 'Admin':
          navigate('/', { state: stateUserForTransport });
          break;
        case 'Proprietor':
          navigate('/dashboard/landlord', { state: stateUserForTransport });
          break;
        case 'Civilian':
          navigate('/profiles/civilian/form', { state: stateUserForTransport });
          break;
        case 'Active Duty':
          navigate('/profiles/military/form', { state: stateUserForTransport });
          break;
        case 'Veteran':
          navigate('/profiles/military/form', { state: stateUserForTransport });
          break;
        default:
          navigate('/', { state: stateUserForTransport });
      }
    }
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
                  <div className="text-center w-75 m-auto">
                    <h4 className="text-dark-50 text-center mt-0 fw-bold">{'Sign In'}</h4>
                    <p className="text-muted mb-4">{'Enter your email address and password to access admin panel.'}</p>
                  </div>
                  <Formik
                    enableReinitialize={true}
                    initialValues={userFormData}
                    onSubmit={onSubmit}
                    validationSchema={loginSchema}>
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
                        <ErrorMessage name="email" component="div" className="user-register-error-form" />
                      </div>

                      <div className="mb-3 font-box">
                        <Link to="/forgot-password" className="text-muted float-end">
                          <small>{'Forgot your password?'}</small>
                        </Link>
                        <FormLabel className="form-label">Password</FormLabel>
                        <Field
                          type={showPassword ? 'password' : 'text'}
                          className="form-control"
                          id="password"
                          name="password"
                          placeholder="Enter your Password"
                        />

                        <span className="password-eye user-eye-toggle-password" onClick={onEyeClicked} />

                        <ErrorMessage name="password" component="div" className="user-register-error-form" />
                      </div>

                      <div className=" mb-0 text-center font-box">
                        <button type="submit" name="submit" className="btn btn-lg btn-color-font submit">
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
                    {"Don't have account?"} <Link to={'/register'}>{'Sign Up'}</Link>
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
export default Login;
