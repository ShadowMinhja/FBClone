import * as types from '../../actions/actionTypes'
import initialState from '../../reducers/initialState'
import * as _ from 'lodash'

export default function menuReducer(state = initialState.foodmenus, action) {
    switch (action.type) {
        case types.MENU_SUCCESS:
            let foodmenus = _.filter(action.result, function(obj) {
                return obj.menuSections != undefined && obj.menuSections.length > 0
            });
            return Object.assign({}, state, {menus: foodmenus});
        case types.MENU_FAILURE:
            return Object.assign({}, state, {error: action.error});
        default:
            return state;
    }
}