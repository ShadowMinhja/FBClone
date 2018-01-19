import * as types from '../../actions/actionTypes'
import initialState from '../../reducers/initialState'

export default function wallFeedPaginationReducer(state = initialState.wallFeedPagination, action) {
    switch (action.type) {
        case types.WALLFEED_GET_SUCCESS:
            const obj = Object.assign({}, state, {
                fetching: false,
            })
            if (action.wallFeedBites && action.wallFeedBites.length > 0) {
                const lastItem = action.wallFeedBites.slice(-1)
                if (lastItem[0] && lastItem[0].streamId) {
                    Object.assign(obj, { lastId: lastItem[0].streamId, })
                }
            }
            return obj;
        case types.WALLFEED_FETCH_INPROGRESS:
            return Object.assign({}, state, { fetching: true, });
        default: 
            return state;
    }
}