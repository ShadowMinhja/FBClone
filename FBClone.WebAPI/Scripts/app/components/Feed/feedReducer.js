import * as types from '../../actions/actionTypes'
import initialState from '../../reducers/initialState'

export default function feedReducer(state = initialState.bites, action) {
    switch (action.type) {
        case types.BITE_ADD_SUCCESS:
            let bites = action.bites;
            return Object.assign([], action.bites);
        case types.BITE_GET_SUCCESS:
            if (action.bites && action.bites.length > 0) {
                const s = [...state]
                action.bites.forEach(r => s.push(r))
                return s;
            }
            return state;            
        case types.BITE_GET_INITIALLOAD_SUCCESS:
            return Object.assign([], action.bites);
        case types.BITE_GET_FAILURE:            
            return Object.assign({}, state, { feedPost: "failure"});
        default:
            return state;
    }
}