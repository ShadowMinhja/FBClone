import * as types from '../../actions/actionTypes'
import venuePickerApi from './venuePickerApi'
import {beginAjaxCall, ajaxCallError} from '../../actions/ajaxStatusActions'

export function queryVenue(gPlaceId) {
    return function(dispatch, getState) {
        const {
            fsLogin
        } = getState()
        dispatch(beginAjaxCall());
        return venuePickerApi.queryVenue(fsLogin.access_token, gPlaceId).then(venuePickerResults => {
            dispatch(queryVenueSuccess(venuePickerResults));
            return new Promise((resolve, reject) => {
                resolve(venuePickerResults);
            });
        }).catch(error => {
            dispatch(queryVenueFailure(error));
        });
    };
}

function queryVenueSuccess(venuePickerResults) {
    return { type: types.FEEDFORM_QUERYVENUE_SUCCESS, venuePickerResults };
}

function queryVenueFailure(error) {
    return { type: types.FEEDFORM_QUERYVENUE_FAILURE, error };
}