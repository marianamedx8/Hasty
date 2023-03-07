import React from 'react';
import { Formik, Form, Field } from 'formik';

export default function UserSocialLinks() {
    const profileData = {
        socialLinks: {
            facebook: '',
            twitter: '',
            instagram: '',
            linkedIn: '',
        },
    };
    
    return (
        <Formik enableReinitialize={true} initialValues={profileData.socialLinks}>
            <Form>
                <h5 className="mb-3 text-uppercase bg-light p-2">
                    <i className="mdi mdi-earth me-1"></i>
                    Social
                </h5>
                <div className="row flex">
                    <div className="mb-3 col-md-6">
                        <label className="form-label">Facebook</label>
                        <div className="mb-0 input-group flex">
                            <span className="input-group-text flex">
                                <i className="mdi mdi-facebook"></i>
                            </span>
                            <Field name="facebook" type="text" placeholder="Url" className="form-control"></Field>
                        </div>
                    </div>
                    <div className="mb-3 col-md-6">
                        <label className="form-label">Twitter</label>
                        <div className="mb-0 input-group flex">
                            <span className="input-group-text flex">
                                <i className="mdi mdi-twitter"></i>
                            </span>
                            <Field
                                name="twitter"
                                type="text"
                                placeholder="Twitter Handle"
                                className="form-control"></Field>
                        </div>
                    </div>
                    <div className="mb-3 col-md-6">
                        <label className="form-label">Instagram</label>
                        <div className="mb-0 input-group flex">
                            <span className="input-group-text flex">
                                <i className="mdi mdi-instagram"></i>
                            </span>
                            <Field name="instagram" type="text" placeholder="Url" className="form-control"></Field>
                        </div>
                    </div>
                    <div className="mb-3 col-md-6">
                        <label className="form-label">LinkedIn</label>
                        <div className="mb-0 input-group flex">
                            <span className="input-group-text flex">
                                <i className="mdi mdi-linkedin"></i>
                            </span>
                            <Field name="linkedIn" type="text" placeholder="Url" className="form-control"></Field>
                        </div>
                    </div>
                </div>
            </Form>
        </Formik>
    );
}
