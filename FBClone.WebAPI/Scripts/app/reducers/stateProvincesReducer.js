import * as types from '../actions/actionTypes'
import initialState from './initialState'

export default function stateProvinceReducer(state = initialState.stateProvinces, action) {
    switch (action.type) {
        case types.LOAD_STATEPROVINCES_SUCCESS:
            return action.stateProvinces;
       
        default:
            return state;
    }
}
