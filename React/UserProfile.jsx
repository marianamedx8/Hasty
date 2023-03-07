import React, { useCallback } from 'react';
import { useNavigate } from 'react-router-dom';
import { useDropzone } from 'react-dropzone';
import { Card, Tab, Nav } from 'react-bootstrap';
import swal from 'sweetalert2';
import toastr from 'toastr';
import PropTypes from 'prop-types';
import UserSettings from './UserSettings';
import * as userService from '../../services/userService';
import fileService from '../../services/fileService';
import 'toastr/build/toastr.css';
import './userstyles.css';
import debug from 'sabio-debug';
const _logger = debug.extend('ProfileSettings');

export default function UserProfile(props) {
    const user = props.currentUser;
    const navigate = useNavigate();

    const profileData = {
        firstName: user.firstName,
        lastName: user.lastName,
        mi: user.mi,
        avatarUrl: user.avatarUrl,
    };

    const handleForgotPassword = () => {
        userService
            .resetPassword(JSON.parse(`{"email": "${user.email}"}`))
            .then(onResetSuccess)
            .catch(onResetError);
    };

    const onResetSuccess = () => {
        swal.fire('Email Sent!', 'An email has been sent to you to reset your password.', 'success');
    };

    const onResetError = () => {
        swal.fire('Error', 'Reset Failed. Please make sure email is correct & try again.', 'error');
    };

    const handleFormSubmit = (values) => {
        _logger(values);
        userService.updateCurrentUser(values).then(onUpdateFormSuccess).catch(onUpdateError);
    };

    const onUpdateFormSuccess = (response) => {
        _logger(response);
        toastr.success('Profile information successfully updated.');
    };

    const onUpdateError = (error) => {
        _logger(error);
        toastr.error('Profile information was not updated, please try again.');
    };

    const onDrop = useCallback((acceptedFiles) => {
        let formData = new FormData();

        acceptedFiles.forEach((file) => {
            formData.append('file', file);
        });

        fileService.uploadFiles(formData).then(onUploadSuccess).catch(onUploadError);
    }, []);

    const onUploadSuccess = (response) => {
        _logger(response);
        const updatedUser = {
            firstName: user.firstName,
            lastName: user.lastName,
            mi: user.mi,
            avatarUrl: response.items[0].url,
        };
        userService.updateCurrentUser(updatedUser).then(onUpdateSuccess).catch();

        const stateUserForTransport = { type: 'USER_SETTINGS', payload: updatedUser };
        swal.fire('Hooray!', 'Profile Picture Updated!', 'success', {
            button: 'Ok',
        }).then(navigate('/profile/settings', { state: stateUserForTransport }));
    };

    const onUpdateSuccess = (response) => {
        _logger(response);
    };

    const onUploadError = (error) => {
        _logger(error);
        toastr.error('File failed to upload, please try again.');
    };

    const { getRootProps, getInputProps } = useDropzone({ onDrop, maxFiles: 1 });

    return (
        <div className="container-fluid">
            <div className="row flex">
                <div className="col">
                    <div className="page-title-box">
                        <div className="page-title-right">
                            <nav>
                                <ol className="breadcrumb m-0">
                                    <li className="breadcrumb-item">
                                        <a href="/">Hasty</a>
                                    </li>
                                    <li className="breadcrumb-item">
                                        <a href="/profile">Profile</a>
                                    </li>
                                </ol>
                            </nav>
                        </div>
                        <h4 className="page-title">Profile</h4>
                    </div>
                </div>
            </div>
            <div className="row flex">
                <div className="col-xl-4 col-lg-5">
                    <div className="text-center card flex">
                        <div className="card-body">
                            <div className="user-settings-image-overlay">
                                <input {...getInputProps()}></input>
                                <img
                                    src={user.avatarUrl}
                                    role="button"
                                    alt=""
                                    className="rounded-circle avatar-xl img-thumbnail user-settings-image"
                                    title="Edit Profile Picture"
                                    {...getRootProps()}></img>
                                <i className="uil-camera user-settings-image-icon"></i>
                            </div>
                            <h4 className="mb-2 mt-2">{user.firstName}</h4>
                            <div className="text-start mt-4">
                                <h4 className="font-13 text-uppercase">About Me:</h4>
                                <p className="text-muted mb-2 font-13">
                                    <strong>Full Name: </strong>
                                    <span className="ms-2">{`${user.firstName} ${user.mi ? user.mi : ''} ${
                                        user.lastName
                                    }`}</span>
                                </p>
                                <p className="text-muted mb-2 font-13">
                                    <strong>Email: </strong>
                                    <span className="ms-2">{user.email}</span>
                                </p>
                            </div>
                            <ul className="list-inline user-settings-social-links mt-3 mb-0">
                                <li className="list-inline-item">
                                    <a
                                        className="social-list-item border-primary text-primary"
                                        href="https://www.facebook.com/"
                                        target="_blank"
                                        rel="noreferrer">
                                        <i className="mdi mdi-facebook"></i>
                                    </a>
                                </li>
                                <li className="list-inline-item">
                                    <a
                                        className="social-list-item border-danger text-danger"
                                        href="https://myaccount.google.com/"
                                        target="_blank"
                                        rel="noreferrer">
                                        <i className="mdi mdi-google"></i>
                                    </a>
                                </li>
                                <li className="list-inline-item">
                                    <a
                                        className="social-list-item border-info text-info"
                                        href="https://twitter.com/home"
                                        target="_blank"
                                        rel="noreferrer">
                                        <i className="mdi mdi-twitter"></i>
                                    </a>
                                </li>
                            </ul>
                        </div>
                    </div>
                </div>
                <div className="col-xl-8 col-lg-7">
                    <Tab.Container defaultActiveKey="settings">
                    <Card className="card flex">
                        <Card.Body className="card-body">
                            <Nav className="nav nav-pills bg-nav-pills nav-justified mb-3 flex" role="tablist">
                                <Nav.Item className="nav-item">
                                    <Nav.Link to="#" role="tab" eventKey="profile" className="nav-link rounded-0">
                                        Profile
                                    </Nav.Link>
                                </Nav.Item>
                                <Nav.Item className="nav-item">
                                    <Nav.Link to="#" role="tab" eventKey="settings" className="nav-link rounded-0">
                                        Settings
                                    </Nav.Link>
                                </Nav.Item>
                            </Nav>
                            <Tab.Content>
                                <Tab.Pane eventKey="profile"></Tab.Pane>
                                <Tab.Pane eventKey="settings">
                                    <UserSettings
                                        profileData={profileData}
                                        handleFormSubmit={handleFormSubmit}
                                        handleForgotPassword={handleForgotPassword}></UserSettings>
                                </Tab.Pane>
                            </Tab.Content>
                        </Card.Body>
                    </Card>
                    </Tab.Container>
                </div>
            </div>
        </div>
    );
}

UserProfile.propTypes = {
    currentUser: PropTypes.shape({
        avatarUrl: PropTypes.string.isRequired,
        email: PropTypes.string.isRequired,
        firstName: PropTypes.string.isRequired,
        lastName: PropTypes.string.isRequired,
        mi: PropTypes.string.isRequired,
    }),
};
