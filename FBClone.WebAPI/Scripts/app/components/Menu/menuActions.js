import * as types from '../../actions/actionTypes'
import menuApi from './menuApi'
import {beginAjaxCall, ajaxCallError} from '../../actions/ajaxStatusActions'

//export function getMenuForVenue(locationId) {
//    return function(dispatch, getState) {
//        const {
//            fsLogin
//        } = getState()
//        dispatch(beginAjaxCall());
//        return menuApi.getMenuForVenue(fsLogin.access_token, locationId).then(result => {
//            dispatch(getMenuForVenueSuccess(result));
//        }).catch(error => {
//            dispatch(getMenuForVenueFailure(error));
//        });
//    };
//}

//function getMenuForVenueSuccess(result) {
//    return { type: types.MENU_SUCCESS, result };
//}

//function getMenuForVenueFailure(error) {
//    return { type: types.MENU_FAILURE, error };
//}