import * as types from '../../actions/actionTypes'
import wallFeedApi from './wallFeedApi'
import {beginAjaxCall, ajaxCallError} from '../../actions/ajaxStatusActions'
import ImageGallery from '../../utils/ImageGallery'
import * as _ from 'lodash'

export function loadWallFeedBites() {
    return function(dispatch, getState) {
        const {
            wallFeedPagination,
            fsLogin
        } = getState()

        //Cancel if already loading
        if (wallFeedPagination.fetching) {
            return new Promise((resolve, reject) => { 
                resolve(null);
            }); //Empty Promise
        }
        
        dispatch(fetchWallFeedInProgress());
        dispatch(beginAjaxCall());
        return wallFeedApi.getWallFeedBites(fsLogin.access_token, wallFeedPagination.lastId).then(wallFeedBites => {
            wallFeedBites = ImageGallery.prototype.processBiteImages(wallFeedBites);
            dispatch(getWallFeedSuccess(wallFeedBites));
        }).catch(error => {
            dispatch(getWallFeedFailure(error));
        });
    };
}

function getWallFeedSuccess(wallFeedBites) {
    return { type: types.WALLFEED_GET_SUCCESS, wallFeedBites };
}

function fetchWallFeedInProgress() {
    return { type: types.WALLFEED_FETCH_INPROGRESS };
}

function getWallFeedFailure(error) {
    return { type: types.WALLFEED_GET_FAILURE, error };
}