import React from 'react';
import PropTypes from 'prop-types';
import listingService from '../../../services/listingService';
import toastr from 'toastr';
import { Trash2 } from 'react-feather';
import './landlord.css';
import swal from 'sweetalert2';
import debug from 'sabio-debug';
const _logger = debug.extend('LANDLORDTABLE-child');

function TableBod(props) {
    const aListing = props.listing;
    const onClick = (e) => {
        _logger(e);
        e.preventDefault();
        const listing = {
            id: aListing.id,
            isActive: !aListing.isActive,
        };
        listingService
            .updateStatus(listing, listing.id, listing.isActive)
            .then(onUpdateStatusSucces)
            .catch(onUpdateStatusError);
    };

    const onUpdateStatusSucces = ({ id, isActive }) => {
        props.updatePage({ id, isActive });
        toastr['success']('Listing Status Updated');
    };

    const onUpdateStatusError = (error) => {
        _logger(error, '******************');
    };

    const onDeleteClicked = () => {
        const id = aListing.id;
        _logger(id);
        swal.fire({
            title: 'Are you sure?',
            text: "You won't be able to revert this!",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, delete it!',
        }).then((result) => {
            if (result.isConfirmed) {
                listingService.deleteListing(id).then(onDeleteSucces).catch(onDeleteError);
            }
        });
    };

    const onDeleteSucces = (id) => {
        _logger('idToDelete', id);
        props.deleteHandler(id);
        swal.fire('Deleted!', 'Your Listing has been deleted.', 'success');
    };

    const onDeleteError = (error) => {
        _logger(error);
    };

    return (
        <>
            <tr>
                <td>{aListing.location.lineOne}</td>
                <td>{aListing.location.state.name}</td>
                <td>{aListing.shortDescription}</td>
                <td>{aListing.housingType.name}</td>
                <td>
                    <input
                        className="ms-4"
                        type="checkbox"
                        value={aListing?.isActive}
                        checked={aListing?.isActive}
                        onChange={onClick}
                    />
                </td>
                <td>
                    <Trash2 size="20px" className="edit-item-icon trash ms-2" type="button" onClick={onDeleteClicked} />
                </td>
            </tr>
        </>
    );
}
TableBod.propTypes = {
    listing: PropTypes.shape({
        housingType: PropTypes.shape({
            name: PropTypes.string.isRequired,
        }).isRequired,
        id: PropTypes.number.isRequired,
        createdBy: PropTypes.number.isRequired,
        isActive: PropTypes.bool.isRequired,
        location: PropTypes.shape({
            city: PropTypes.string.isRequired,
            lineOne: PropTypes.string.isRequired,
            state: PropTypes.shape({
                name: PropTypes.string.isRequired,
            }).isRequired,
        }).isRequired,
        shortDescription: PropTypes.string.isRequired,
    }).isRequired,
    updatePage: PropTypes.func,
    deleteHandler: PropTypes.func,
};
export default TableBod;
