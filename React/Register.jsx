import React, { useState } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { Formik, Form, Field, ErrorMessage } from 'formik';
import { Container, Row, Col, Card, FormLabel } from 'react-bootstrap';
import './userstyles.css';
import Logo from '../../assets/images/hastylogo.png';
import * as userService from '../../services/userService';
import swal from 'sweetalert2';
import { registrationSchema } from '../../schemas/registrationSchema';

function Register() {
  const navigate = useNavigate();
  const userFormData = {
    email: '',
    firstName: '',
    lastName: '',
    mi: '',
    avatarUrl: '',
    password: '',
    passwordConfirm: '',
    roleId: '',
    isConfirmed: false,
    statusId: 3,
  };
  const [showPassword, setShowPassword] = useState(true);
  const onEyeClicked = () => {
    setShowPassword((prevState) => {
      return !prevState;
    });
  };

  const onSubmit = (values) => {
    userService.register(values).then(onRegisterSuccess).catch(onRegisterError);
  };

  const onRegisterSuccess = () => {
    swal
      .fire('Hooray!', 'Registration Successful! An Email has been sent to you!', 'success', {
        button: 'Ok',
      })
      .then(navigate('/login'));
  };
  const onRegisterError = () => {
    swal.fire('Error', 'Registration Failed.', 'error');
  };

  return (
    <>
      <div className="account-pages pt-2 pt-sm-5 pb-4 pb-sm-5 ">
        <Container className="col-5">
          <Row className="justify-content-center">
            <Col>
              <Card className="rounded">
                <Card.Header className="pt-1 pb-1 text-center primary-color-head">
                  <Link to="/">
                    <span>
                      {' '}
                      <img src={Logo} alt="hasty-logo-navigate-home" height="60" />{' '}
                    </span>
                  </Link>
                </Card.Header>
                <Card.Body className="p-4">
                  <div className="text-center w-45 m-auto">
                    <h4 className="text-dark-50 text-center mt-0 fw-bold">{'Free Sign Up'}</h4>
                    <p className="text-muted mb-4">
                      {"Don't have an account? Create your account, it takes less than a minute."}
                    </p>
                  </div>
                  <Formik
                    enableReinitialize={true}
                    initialValues={userFormData}
                    onSubmit={onSubmit}
                    validationSchema={registrationSchema}>
                    <Form>
                      <div className="row">
                        <div className="col-6">
                          <div className="mb-3 font-box form-group">
                            <FormLabel className="form-label">First Name</FormLabel>
                            <Field
                              type="text"
                              className="form-control"
                              id="firstName"
                              placeholder="Enter Your First Name"
                              name="firstName"
                            />
                            <ErrorMessage name="firstName" component="div" className="user-register-error-form" />
                          </div>
                        </div>
                        <div className="col-6">
                          <div className="mb-3 font-box form-group">
                            <FormLabel className="form-label">Middle Inital</FormLabel>
                            <Field
                              type="text"
                              className="form-control"
                              id="mi"
                              placeholder="Middle Initial"
                              name="mi"
                            />
                            <ErrorMessage name="mi" component="div" className="user-register-error-form" />
                          </div>
                        </div>
                      </div>

                      <div className="row flex-row">
                        <div className="col-6">
                          <div className="mb-3 font-box form-group">
                            <FormLabel className="form-label">Last Name</FormLabel>
                            <Field
                              type="text"
                              className="form-control"
                              id="lastName"
                              placeholder="Enter Your Last Name"
                              name="lastName"
                            />
                            <ErrorMessage name="lastName" component="div" className="user-register-error-form" />
                          </div>
                        </div>
                        <div className="col-6">
                          <div className="mb-3 font-box form-group">
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
                        </div>
                      </div>
                      <div className="row flex-row">
                        <div className="col-6">
                          <div className="mb-3 font-box form-group">
                            <FormLabel className="form-label">Password</FormLabel>
                            <Field
                              type={showPassword ? 'password' : 'text'}
                              className="form-control"
                              id="password"
                              name="password"
                              placeholder="Enter your Password"
                            />
                            <span className="password-eye user-eye-toggle-password" onClick={onEyeClicked} />
                            <ErrorMessage name="password" component="div" className="user-register-error-form " />
                          </div>
                        </div>
                        <div className="col-6">
                          <div className="mb-3 font-box form-group">
                            <FormLabel className="form-label">Confirm Password</FormLabel>
                            <Field
                              type={showPassword ? 'password' : 'text'}
                              className="form-control"
                              id="passwordConfirm"
                              name="passwordConfirm"
                              placeholder="Confirm Password"
                            />
                            <span className="password-eye user-eye-toggle-password" onClick={onEyeClicked} />
                            <ErrorMessage
                              name="passwordConfirm"
                              component="div"
                              className="user-register-error-form "
                            />
                          </div>
                        </div>
                      </div>

                      <div className="mb-3 font-box">
                        <FormLabel className="form-label">Role</FormLabel>
                        <Field component="select" id="roleId" name="roleId" className="form-control">
                          <option value="">Select Role</option>
                          <option value="2">Proprietor</option>
                          <option value="3">Civilian</option>
                          <option value="4">Active Duty</option>
                          <option value="5">Veteran</option>
                        </Field>
                        <ErrorMessage name="roleId" component="div" className="user-register-error-form " />
                      </div>

                      <div className="mb-3 font-box">
                        <FormLabel className="form-label">Profile Picture</FormLabel>
                        <Field
                          type="avatarUrl"
                          className="form-control"
                          id="avatarUrl"
                          placeholder="Provide a Url to an image"
                          name="avatarUrl"
                        />
                        <ErrorMessage name="avatarUrl" component="div" className="user-register-error-form " />
                      </div>
                      <div className="mb-3 d-none font-box">
                        <FormLabel className="form-label">StatusId</FormLabel>
                        <Field type="text" className="form-control" id="statusId" name="statusId" />
                      </div>
                      <div className=" mb-0 text-center font-box d-grid gap-2">
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
                    {'Already have account?'} <Link to={'/login'}>{'Log In'}</Link>
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
export default Register;
