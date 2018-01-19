import * as types from '../../actions/actionTypes'
import initialState from '../../reducers/initialState'

export default function searchBarReducer(state = initialState.searchResults, action) {
    switch (action.type) {
        case types.SEARCHBAR_SUCCESS:
            let searchHistory = [action.searchResults, ...state.history];
            return Object.assign({}, state, {algolia: [], history: searchHistory});
        case types.SEARCHBAR_QUERYUSERNAME_SUCCESS:
            return Object.assign({}, state, {users: action.searchResults});
        case types.SEARCHBAR_RESULTS_FOUND:
            return Object.assign({}, state, {algolia: action.searchResults});
        case types.SEARCHBAR_RESULTS_EMPTY:
            return Object.assign({}, state, {algolia: []});
        case types.SEARCHBARHISTORY_SUCCESS:
            return Object.assign({}, state, {history: action.searchHistory});
        case types.SEARCHBAR_FAILURE:            
            return Object.assign({}, state, { searchStatus: "failure"});
        case types.SEARCHBAR_CLEARALGOLIA:
            return Object.assign({}, state, {algolia:[]});
        default:
            return state;
    }
}
