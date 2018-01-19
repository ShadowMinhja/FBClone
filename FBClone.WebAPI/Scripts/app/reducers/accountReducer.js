import * as types from '../actions/actionTypes'
import initialState from './initialState'
import * as _ from 'lodash'

export default function accountReducer(state = initialState.fsLogin, action) {
    switch (action.type) {
        case types.ACCOUNT_SHOW_SIGNUP:
            return Object.assign({}, fsLogin, {showSignup: true});
        case types.ACCOUNT_SHOW_SIGNUP_RESET:
            return Object.assign({}, initialState.fsLogin);
        case types.ACCOUNT_SIGNUP_SUCCESS:
            return Object.assign({}, fsLogin);
        case types.ACCOUNT_SIGNUP_FAILURE:  
            let errorText = _.map(action.error, 'Value.0')[0] !== undefined ? _.map(action.error, 'Value.0') : JSON.parse(action.error.response.text).modelState[""][0];
            return Object.assign({}, state, { signup: "Error", signupError: errorText});
        case types.ACCOUNT_LOGIN_SUCCESS:
            let fsLogin = action.fsLogin;
            if(fsLogin.isLoggedIn === undefined && fsLogin.access_token != null){
                return Object.assign({}, fsLogin);       
            }
            else 
                return Object.assign({}, fsLogin);       
        case types.ACCOUNT_LOGIN_FAIL:
            return Object.assign({}, state, {loginError: action.error.errorText, id: action.error.userId, confirmationError: action.error.errorText});
        case types.ACCOUNT_RETRIEVE_EMPTY:
            return Object.assign({}, state, {loginEmpty: true});
        case types.ACCOUNT_LOGIN_REDIRECTED:
            return Object.assign({}, state, {loginFail: undefined, hideHeaderLogin: undefined});
        case types.ACCOUNT_LOGOUT_SUCCESS:
            return Object.assign({}, initialState.fsLogin);    
        case types.ACCOUNT_HIDE_HEADERLOGIN:
            return Object.assign({}, state, {hideHeaderLogin: true, loginError: undefined});
        case types.ACCOUNT_LOGIN_RETURN:
            return Object.assign({}, state, {hideHeaderLogin: undefined});
        case types.ACCOUNT_REGISTER_RETURN:
            return Object.assign({}, state, { signup: undefined, signupError: undefined});
        case types.ACCOUNT_CONFIRMATION_EMAIL_SENT:
            let emailSent = Object.assign({}, action.fsLogin, { confirmationSent: true });
            return Object.assign({}, state, emailSent);       
        case types.ACCOUNT_CONFIRMATION_EMAIL_SENT_CLEAR:
            let emailSentClear = Object.assign({}, action.fsLogin, { confirmationSent: false });
            return Object.assign({}, state, emailSentClear);       
        case types.ACCOUNT_CONFIRMATION_EMAIL_FAILURE:
            return Object.assign({}, state, { confirmationError: action.error });       
        case types.ACCOUNT_CONFIRMATION_EMAIL_SUCCESS:
            let emailConfirmed = Object.assign({}, action.result, { confirmationError: false });
            return Object.assign({}, state, emailConfirmed);       
        case types.ACCOUNT_FORGOT_PASSWORD_FAILURE:
            return Object.assign({}, state, {forgotPasswordResult: false });
        case types.ACCOUNT_FORGOT_PASSWORD_SUCCESS:
            return Object.assign({}, state, {forgotPasswordResult: true });
        case types.ACCOUNT_FORGOT_PASSWORD_RETURN:
            return Object.assign({}, state, {forgotPasswordResult: undefined, hideHeaderLogin: undefined });
        case types.ACCOUNT_RESET_PASSWORD_FAILURE:
            return Object.assign({}, state, {resetPasswordResult: false });
        case types.ACCOUNT_RESET_PASSWORD_SUCCESS:
            return Object.assign({}, state, {resetPasswordResult: true });
        case types.ACCOUNT_RESET_PASSWORD_RETURN:
            return Object.assign({}, state, {resetPasswordResult: undefined, hideHeaderLogin: undefined });
        case types.ACCOUNT_UPDATE_FSLOGIN:
            return Object.assign({}, state, {id: action.fsLogin.userId, email: action.fsLogin.email});
        default:
            return state;
    }
}