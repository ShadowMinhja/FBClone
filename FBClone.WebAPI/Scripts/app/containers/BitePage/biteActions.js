import * as types from '../../actions/actionTypes'
import biteApi from './biteApi'
import {beginAjaxCall, ajaxCallError} from '../../actions/ajaxStatusActions'

export function getBite(biteId) {
    return function(dispatch, getState) {
        const {
            fsLogin
        } = getState()

        dispatch(beginAjaxCall());
        return biteApi.getBite(fsLogin.access_token, biteId).then(bite => {
            dispatch(getBiteSuccess(bite));
        }).catch(error => {
            dispatch(getBiteFailure(error));
        });
    };
}

function getBiteSuccess(bite) {
    return { type: types.BITE_GET_SUCCESS, bite };
}

function getBiteFailure(error) {
    return { type: types.BITE_GET_FAILURE, error };
}