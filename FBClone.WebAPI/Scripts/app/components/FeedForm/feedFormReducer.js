import * as types from '../../actions/actionTypes'
import initialState from '../../reducers/initialState'

export default function feedFormReducer(state = initialState.feedFormPost, action) {
    switch (action.type) {
        case types.BITE_POST_SUCCESS:
            return Object.assign({}, state, { insertedPost: action.bite, status: "success"});
        case types.BITE_POST_FAILURE:            
            return Object.assign({}, state, { status: "failure"});
        case types.BITE_ADD_CLEAR:
            return Object.assign({}, {});
        default:
            return state;
    }
}
