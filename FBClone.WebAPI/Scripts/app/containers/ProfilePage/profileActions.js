import * as types from '../../actions/actionTypes'
import accountApi from '../../api/accountApi'

import profileApi from './profileApi'
import {beginAjaxCall, ajaxCallError} from '../../actions/ajaxStatusActions'

//Follow
export function followProfile(actor) {
    return function(dispatch, getState) {
        const {
            fsLogin
        } = getState()

        dispatch(beginAjaxCall());
        return profileApi.followProfile(fsLogin.access_token, fsLogin.guid, actor).then(result => {
            dispatch(profileFollowSuccess(result));
        }).catch(error => {
            dispatch(profileFollowFailure(error));
        });
    };
}

function profileFollowSuccess(profileInfo) {
    return { type: types.PROFILE_FOLLOW_SUCCESS, profileInfo };
}

function profileFollowFailure(error) {
    return { type: types.PROFILE_FOLLOW_FAILURE, error };
}

//Unfollow
export function unfollowProfile(actor) {
    return function(dispatch, getState) {
        const {
            fsLogin
        } = getState()

        dispatch(beginAjaxCall());
        return profileApi.unfollowProfile(fsLogin.access_token, fsLogin.guid, actor).then(result => {
            dispatch(profileUnfollowSuccess(result));
        }).catch(error => {
            dispatch(profileUnfollowFailure(error));
        });
    };

}
function profileUnfollowSuccess(profileInfo) {
    return { type: types.PROFILE_UNFOLLOW_SUCCESS, profileInfo };
}

function profileUnfollowFailure(error) {
    return { type: types.PROFILE_UNFOLLOW_FAILURE, error };
}

//Profile Retrieval
export function retrieveProfileInfo(userName) {
    return function(dispatch, getState) {
        //Doesn't work, runs too late
        //const {
        //    fsLogin
        //} = getState()

        var fsLogin;
        accountApi.getAccountCache().then(result => {
            fsLogin = result;
            dispatch(beginAjaxCall());
            return profileApi.getProfile(fsLogin.access_token, userName).then(result => {
                dispatch(profileRetrieveSuccess(result));
            }).catch(error => {
                dispatch(profileRetrieveFailure(error));
            });
        });
    };
}

function profileRetrieveSuccess(profileInfo) {
    return { type: types.PROFILE_RETRIEVE_SUCCESS, profileInfo };
}

function profileRetrieveFailure(error) {
    return { type: types.PROFILE_RETRIEVE_FAILURE, error };
}