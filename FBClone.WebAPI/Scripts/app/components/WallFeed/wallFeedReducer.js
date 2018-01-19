import * as types from '../../actions/actionTypes'
import initialState from '../../reducers/initialState'

export default function wallFeedReducer(state = initialState.wallFeedBites, action) {
    switch (action.type) {        
        case types.WALLFEED_GET_SUCCESS:
            if (action.wallFeedBites && action.wallFeedBites.length > 0) {
                const s = [...state]
                action.wallFeedBites.forEach(r => s.push(r))
                return s;
            }
            return state;
            //return Object.assign([], action.wallBites);
        case types.WALLFEED_GET_FAILURE:            
            return Object.assign({}, state, { wallFeedPost: "failure"});
        default:
            return state;
    }
}