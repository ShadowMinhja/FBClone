import * as types from '../../actions/actionTypes'
import initialState from '../../reducers/initialState'

export default function venuePickerReducer(state = initialState.venuePickerResults, action) {
    switch (action.type) {
        case types.FEEDFORM_QUERYVENUE_SUCCESS:
            return Object.assign({}, state,  action.venuePickerResults);              
        case types.FEEDFORM_QUERYVENUE_FAILURE:            
            return Object.assign({}, state, {venueSearchStatus: "failure"});
        default:
            return state;
    }
}