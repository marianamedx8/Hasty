import React from 'react';
import { Card, Row } from 'react-bootstrap';
import PropTypes from 'prop-types';
import './landlord.css';
import { format } from 'date-fns';

function Files(props) {
    const file = props.file;

    return (
        <>
            <Card className="mt-2 me-2 ms-2 mb-0 border col-5 rounded edit-item-icon">
                <div className="p-1 ">
                    <Row>
                        <div className="col-4 col">
                            {(props.file.type.name === 'jpeg') | (props.file.type.name === 'png') ? (
                                <div>
                                    <img
                                        data-dz-thumbnail=""
                                        className="avatar-sm rounded bg-light mt-1"
                                        alt={file.name}
                                        src={file.url}
                                    />
                                </div>
                            ) : (
                                <div>
                                    <div className="avatar-sm">
                                        <span className="avatar-title bg-primary rounded mt-1">{file.type.name}</span>
                                    </div>
                                </div>
                            )}
                        </div>
                        <div className="col-8">
                            <a href={file.url} target="_blank" rel="noreferrer">
                                {file.type.id}.{file.type.name}
                            </a>

                            <h6 className="mt-3 text-muted">
                                Uploaded: {format(new Date(file.dateCreated), 'MMM d, yyy')}
                            </h6>
                        </div>
                    </Row>
                </div>
            </Card>
        </>
    );
}
Files.propTypes = {
    file: PropTypes.shape({
        id: PropTypes.number.isRequired,
        name: PropTypes.string.isRequired,
        type: PropTypes.shape({
            name: PropTypes.string.isRequired,
            id: PropTypes.number.isRequired,
        }),
        url: PropTypes.string.isRequired,
        dateCreated: PropTypes.string.isRequired,
    }).isRequired,
};

export default Files;
