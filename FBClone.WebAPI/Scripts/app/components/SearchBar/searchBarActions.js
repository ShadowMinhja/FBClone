import * as types from '../../actions/actionTypes'
import searchBarApi from './searchBarApi'
import {beginAjaxCall, ajaxCallError} from '../../actions/ajaxStatusActions'

export function queryUserName(searchText) {
    return function(dispatch, getState) {
        const {
            fsLogin
        } = getState()
        dispatch(beginAjaxCall());
        return searchBarApi.queryUserName(fsLogin.access_token, searchText).then(searchResults => {
            dispatch(queryUserNameSuccess(searchResults));
        }).catch(error => {
            dispatch(searchFailure(error));
        });
    };
}

export function postSearchResults(algoliaContent) {
    return function(dispatch) {
        if(algoliaContent.hits.length > 0) {
            dispatch(resultsFound(algoliaContent.hits));
        }
        else { //No Results
            dispatch(resultsEmpty());
        }
    }
}

export function captureSearch(searchText) {
    return function(dispatch, getState) {
        const {
            fsLogin
        } = getState()
        dispatch(beginAjaxCall());
        return searchBarApi.saveSearchHistory(fsLogin.access_token, searchText).then(savedSearches => {
            dispatch(searchSuccess(savedSearches));
        }).catch(error => {
            dispatch(searchFailure(error));
        });
    };
}

export function retrieveSearches() {
    return function(dispatch, getState) {
        const {
            fsLogin
        } = getState()
        dispatch(beginAjaxCall());
        return searchBarApi.retrieveSearches(fsLogin.access_token).then(searchHistory => {
            dispatch(searchGetHistorySuccess(searchHistory));
        }).catch(error => {
            dispatch(searchFailure(error));
        });
    };
}

export function clearAlgolia() {
    return { type: types.SEARCHBAR_CLEARALGOLIA };
}

function resultsFound(searchResults) {
    return { type: types.SEARCHBAR_RESULTS_FOUND, searchResults };
}

function resultsEmpty() {
    return { type: types.SEARCHBAR_RESULTS_EMPTY };
}

function queryUserNameSuccess(searchResults) {
    return { type: types.SEARCHBAR_QUERYUSERNAME_SUCCESS, searchResults };
}

function searchSuccess(searchResults) {
    return { type: types.SEARCHBAR_SUCCESS, searchResults };
}

function searchGetHistorySuccess(searchHistory) {
    return { type: types.SEARCHBARHISTORY_SUCCESS, searchHistory };
}

function searchFailure(error) {
    return { type: types.SEARCHBAR_FAILURE, error };
}