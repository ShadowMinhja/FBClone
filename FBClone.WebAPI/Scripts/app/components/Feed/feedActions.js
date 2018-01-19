import * as types from '../../actions/actionTypes'
import feedApi from './feedApi'
import {beginAjaxCall, ajaxCallError} from '../../actions/ajaxStatusActions'
import ImageGallery from '../../utils/ImageGallery'
import * as _ from 'lodash'

export function addBite(bites, bite) {
    return function(dispatch) {
        let bitesInserted = Object.assign([], bites);
        bitesInserted.unshift(bite);
        dispatch(addBiteSuccess(bitesInserted));
    }
}

export function loadBites(userName, flush) {
    return function(dispatch, getState) {
        const {
            feedPagination,
            fsLogin
        } = getState()

        //Cancel if already loading
        if (feedPagination.fetching) 
            return new Promise((resolve, reject) => { 
                resolve(null);
            }); //Empty Promise
        
        dispatch(fetchBitesInProgress());
        dispatch(beginAjaxCall());
        var feedPaginationLastId = null;
        if(flush == false){
            feedPaginationLastId = feedPagination.lastId;
        }
        return feedApi.getBites(fsLogin.access_token, userName, feedPaginationLastId).then(bites => {
            bites = ImageGallery.prototype.processBiteImages(bites);
            if(flush)
                dispatch(getBitesInitialLoadSuccess(bites));
            else 
                dispatch(getBitesSuccess(bites));
        }).catch(error => {
            dispatch(getBitesFailure(error));
        });
    };
}

function addBiteSuccess(bites) {
    return { type: types.BITE_ADD_SUCCESS, bites };
}

function getBitesInitialLoadSuccess(bites) {
    return { type: types.BITE_GET_INITIALLOAD_SUCCESS, bites };
}

function getBitesSuccess(bites) {
    return { type: types.BITE_GET_SUCCESS, bites };
}

function fetchBitesInProgress() {
    return { type: types.BITE_FETCH_INPROGRESS };
}

function getBitesFailure(error) {
    return { type: types.BITE_GET_FAILURE, error };
}