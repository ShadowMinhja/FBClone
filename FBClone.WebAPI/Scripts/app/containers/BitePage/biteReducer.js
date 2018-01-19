import * as types from '../../actions/actionTypes'
import initialState from '../../reducers/initialState'

export default function biteReducer(state = initialState.bite, action) {
    switch (action.type) {
        case types.BITE_GET_SUCCESS:
            return Object.assign({}, action.bite);         
        case types.BITE_GET_FAILURE:            
            return Object.assign({}, state, { retrieve: "failure"});
        default:
            return state;
    }
}