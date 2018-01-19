import * as types from '../../actions/actionTypes'
import feedFormApi from './feedFormApi'
import {beginAjaxCall, ajaxCallError} from '../../actions/ajaxStatusActions'
import ImageGallery from '../../utils/ImageGallery'

export function postBite(bite){
    return function(dispatch, getState) {
        const {
            fsLogin
        } = getState()

        return feedFormApi.postBite(fsLogin.access_token, bite)
            .then(response => {
                var imageGallery = ImageGallery.prototype.processImage(response.images);
                response.images = imageGallery;
                response.categoryScores = bite.surveyQuestionResponseSet.categoryScores;
                dispatch(postBiteSuccess(response));
                return new Promise((resolve, reject) => {
                    resolve("SUCCESS");
                });
            })
            .catch(error => {
                return new Promise((resolve, reject) => {
                    resolve("BAD_CREDENTIAL");
                });
            })
    }
}

export function clearFeedForm() {
    return function(dispatch){
        dispatch(feedFormClear());
    }
}

function postBiteSuccess(bite) {
    return { type: types.BITE_POST_SUCCESS, bite };
}

function feedFormClear() {
    return { type: types.BITE_ADD_CLEAR };
}
