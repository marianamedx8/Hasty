import React from 'react';
import { Formik, Form, Field, ErrorMessage } from 'formik';
import { userSettingsSchema } from '../../schemas/userSettingsSchema';
import UserSocialLinks from './UserSocialLinks';
import PropTypes from 'prop-types';



export default function UserSettings(props) {
    return (<div className="container-fluid">
    <Formik
        enableReinitialize={true}
        initialValues={props.profileData}
        onSubmit={props.handleFormSubmit}
        validationSchema={userSettingsSchema}>
        <Form>
            <h5 className="mb-4 text-uppercase">
                <i className="mdi mdi-account-circle me-1"></i>
                Edit Personal Info
            </h5>
            <div className="row flex">
                <div className="col-md-4">
                    <div className="mb-3">
                        <label className="form-label">First Name</label>
                        <Field
                            name="firstName"
                            type="text"
                            placeholder="Enter first name"
                            className="form-control"
                        />
                        <ErrorMessage
                            name="firstName"
                            component="div"
                            className="user-register-error-form"
                        />
                    </div>
                </div>
                <div className="col-md-4">
                    <div className="mb-3">
                        <label className="form-label">Middle Initial</label>
                        <Field
                            name="mi"
                            type="text"
                            placeholder="Enter middle initial"
                            className="form-control"
                        />
                    </div>
                </div>
                <div className="col-md-4">
                    <div className="mb-3">
                        <label className="form-label">Last Name</label>
                        <Field
                            name="lastName"
                            type="text"
                            placeholder="Enter last name"
                            className="form-control"
                        />
                        <ErrorMessage
                            name="lastName"
                            component="div"
                            className="user-register-error-form"
                        />
                    </div>
                </div>
            </div>
            <div className="row flex">
                <div className="col">
                    <div className="mb-3">
                        <label className="form-label">Bio</label>
                        <textarea
                            name="userbio"
                            type="text"
                            placeholder="Place holder for other user information added in the future. Maybe housing priorites?"
                            rows="4"
                            className="form-control"></textarea>
                    </div>
                </div>
            </div>
            <div className="row flex">
                <div className="mb-3 col-md-6">
                    <div>
                        <label className="form-label">Password</label>
                        <div className="row flex">
                            <span className="text-muted">
                                <a role="button" onClick={props.handleForgotPassword}>
                                    Want to reset your password?
                                </a>
                            </span>
                        </div>
                    </div>
                </div>
            </div>
            <div className="text-end">
                <button type="submit" name="submit" className="btn btn-success mt-2 mb-3">
                    <i className="mdi mdi-content-save"></i>
                    Save
                </button>
            </div>
        </Form>
    </Formik>
    <UserSocialLinks />
</div>)
}

UserSettings.propTypes = {
    profileData: PropTypes.shape({
        firstName: PropTypes.string.isRequired,
        lastName: PropTypes.string.isRequired,
        mi: PropTypes.string.isRequired,
        avatarUrl: PropTypes.string.isRequired,
    }),
    handleFormSubmit: PropTypes.func.isRequired,
    handleForgotPassword: PropTypes.func.isRequired,
};