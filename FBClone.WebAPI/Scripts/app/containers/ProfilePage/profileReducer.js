import * as types from '../../actions/actionTypes'
import initialState from '../../reducers/initialState'

export default function profileReducer(state = initialState.profileInfo, action) {
    switch (action.type) {
        case types.PROFILE_FOLLOW_SUCCESS:
            return Object.assign({}, action.profileInfo);
        case types.PROFILE_UNFOLLOW_SUCCESS:
            return Object.assign({}, action.profileInfo);
        case types.PROFILE_RETRIEVE_SUCCESS:            
            return Object.assign({}, action.profileInfo);
        case types.PROFILE_RETRIEVE_FAILURE:
            return Object.assign({}, state, {errorText: action.error});
        default:
            return state;
    }
}