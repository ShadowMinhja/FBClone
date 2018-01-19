import appReducer from './appReducer'
//http://stackoverflow.com/questions/35622588/how-to-reset-the-state-of-a-redux-store/35641992
const rootReducer = (state, action) => {
    if (action.type === 'ACCOUNT_LOGOUT_SUCCESS') {
        state = undefined
    }
    return appReducer(state, action)
};

export default rootReducer;