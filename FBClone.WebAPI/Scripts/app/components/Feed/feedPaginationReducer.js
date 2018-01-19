import * as types from '../../actions/actionTypes'
import initialState from '../../reducers/initialState'

export default function feedPaginationReducer(state = initialState.feedPagination, action) {
    switch (action.type) {
        case types.BITE_GET_SUCCESS:
            const obj = Object.assign({}, state, {
                fetching: false,
            })
            if (action.bites && action.bites.length > 0) {
                const lastItem = action.bites.slice(-1)
                if (lastItem[0] && lastItem[0].streamId) {
                    Object.assign(obj, { lastId: lastItem[0].streamId, })
                }
            }
            return obj
        case types.BITE_GET_INITIALLOAD_SUCCESS:
            const obj2 = Object.assign({}, state, {
                fetching: false,
            })
            if (action.bites && action.bites.length > 0) {
                const lastItem = action.bites.slice(-1)
                if (lastItem[0] && lastItem[0].streamId) {
                    Object.assign(obj2, { lastId: lastItem[0].streamId, })
                }
            }
            return obj2
        case types.BITE_FETCH_INPROGRESS:
            return Object.assign({}, state, { fetching: true, });
        default: 
            return state;
    }
}
