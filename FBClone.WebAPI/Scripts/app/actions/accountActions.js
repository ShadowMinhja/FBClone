import * as types from './actionTypes'
import accountApi from '../api/accountApi'
import {beginAjaxCall, ajaxCallError} from './ajaxStatusActions'

export function showSignup(toggle) {
    if(toggle == true) {
        return { type: types.ACCOUNT_SHOW_SIGNUP };
    } else {
        return { type: types.ACCOUNT_SHOW_SIGNUP_RESET };
    }
}

export function retrieveLoginInfo(){
    return function(dispatch) {
        return accountApi.getAccountCache()
            .then(fsLogin => {
                var currentDateTime = moment().format('YYYY-MM-DD HH:mm:ss.SSS');
                var loginDiff = moment.duration(moment(currentDateTime).diff(moment(fsLogin.loginDate))) / 1000; //In Seconds
                if(loginDiff > 86400) { //Force Logoff
                    accountApi.logout(fsLogin.access_token)
                        .then(()=> {
                            dispatch(accountLogoutSuccess());
                        })
                        .catch(error => {
                            throw(error);
                        })
                }
                else
                    dispatch(accountLoginSuccess(fsLogin));
            })
            .catch(error => {
                console.log(error);
                dispatch(accountRetrieveEmpty(error));
            })
    }
}

function cacheLoginInfo(fsLogin) {
    let fsLoginCache = Object.assign({}, fsLogin);
    localStorage.setItem("fsLogin", JSON.stringify(fsLoginCache));
}

export function accountSignup(registrationItem) {
    return function(dispatch) {
        dispatch(beginAjaxCall());
        return accountApi.registerUser(registrationItem)
            .then(signupResult => {
                if(signupResult.Errors)
                    dispatch(accountSignupFailure(signupResult.Errors));
                else
                    dispatch(accountEmailSent(signupResult));
            })
            .catch(error => {                
                dispatch(accountSignupFailure(error));
            });
    };
}

export function accountSignupSuccess(fsLogin) {
    return { type: types.ACCOUNT_SIGNUP_SUCCESS, fsLogin };
}

export function accountSignupFailure(error) {
    return { type: types.ACCOUNT_SIGNUP_FAILURE, error };
}

export function accountLogin(userName, password) {
  return function(dispatch) {
    dispatch(beginAjaxCall());
    return accountApi.login(userName, password)
        .then(fsLogin => {
            cacheLoginInfo(fsLogin);
            dispatch(accountLoginSuccess(fsLogin));
        })
        .catch(error => {
            //throw(error);
            dispatch(accountLoginFail(error));
        });
  };
}

function accountLoginSuccess(fsLogin) {
    return { type: types.ACCOUNT_LOGIN_SUCCESS, fsLogin };
}
function accountLoginFail(error) {
    return { type: types.ACCOUNT_LOGIN_FAIL, error };
}
function accountRetrieveEmpty(error) {
    return { type: types.ACCOUNT_RETRIEVE_EMPTY, error };
}

export function resetLoginStatus() {
    return { type: types.ACCOUNT_LOGIN_REDIRECTED };
}

export function leaveLoginPage() {
    return { type: types.ACCOUNT_LOGIN_RETURN };
}

export function leaveRegisterPage() {
    return { type: types.ACCOUNT_REGISTER_RETURN };
}

export function accountLogout() {
    return function(dispatch, getState) {
        const {
            fsLogin
        } = getState()

        return accountApi.logout(fsLogin.access_token)
            .then(()=> {
                dispatch(accountLogoutSuccess());
            })
            .catch(error => {
                throw(error);
            })
    }
}

export function removeLoginCache() {
    return function(dispatch, getState) {
        return new Promise((resolve, reject) => {
            localStorage.removeItem("fsLogin");
            resolve("Logged Out");
        }).then(() => {
            dispatch(accountLogoutSuccess());
        })
    }
}

function accountLogoutSuccess() {
    return { type: types.ACCOUNT_LOGOUT_SUCCESS };
}

export function hideHeaderLogin(fsLogin) {
    return { type: types.ACCOUNT_HIDE_HEADERLOGIN, fsLogin };
}

//OAuth External
export function accountSignupExternal(registrationItem) {
    return function(dispatch) {
        dispatch(beginAjaxCall());
        return accountApi.registerUserExternal(registrationItem)
            .then(fsLogin => {
                if(fsLogin.Errors) {
                    dispatch(accountSignupFailure(fsLogin.Errors));
                }
                else {
                    fsLogin.email = registrationItem.email;
                    dispatch(accountEmailSent(fsLogin));
                }
            })
            .catch(error => {                
                dispatch(accountSignupFailure(error));
            });
    };
}

export function resendConfirmationEmail(userId) {
    return function(dispatch) {
        dispatch(beginAjaxCall());
        return accountApi.resendConfirmationEmail(userId)
            .then(fsLogin => {
                if(fsLogin.Errors) {
                    dispatch(accountSignupFailure(fsLogin.Errors));
                }
                else {
                    dispatch(accountEmailSent(fsLogin));
                }
            })
            .catch(error => {                
                dispatch(accountSignupFailure(error));
            });
    };
}

function accountEmailSent(fsLogin) {
    return { type: types.ACCOUNT_CONFIRMATION_EMAIL_SENT, fsLogin };
}

export function clearConfirmationSent() {
    return { type: types.ACCOUNT_CONFIRMATION_EMAIL_SENT_CLEAR };
}

export function confirmEmail(confirmationInfo) {
    return function(dispatch) {
        dispatch(beginAjaxCall());
        return accountApi.confirmEmail(confirmationInfo)
            .then(result => {
                if(result.Errors) {
                    dispatch(accountConfirmationFailure(result.Errors));
                }
                else {                    
                    dispatch(accountConfirmationSuccess(result));
                }
            })
            .catch(error => {                
                dispatch(accountConfirmationFailure(error.errorText));
            });
    };
}

function accountConfirmationFailure(error){
    return { type: types.ACCOUNT_CONFIRMATION_EMAIL_FAILURE, error };
}
function accountConfirmationSuccess(result){
    return { type: types.ACCOUNT_CONFIRMATION_EMAIL_SUCCESS, result };
}

export function obtainAccessToken(externalData) {
    return function(dispatch) {
        dispatch(beginAjaxCall());
        return accountApi.obtainAccessToken(externalData)
            .then(fsLogin => {
                if(fsLogin.Errors) {
                    dispatch(accountLoginFail(fsLogin.Errors));
                }
                else {
                    cacheLoginInfo(fsLogin);
                    dispatch(accountLoginSuccess(fsLogin));
                }
            })
            .catch(error => {                
                dispatch(accountLoginFail(error));
            });
    };
}

export function forgotPassword(userName) {
    return function(dispatch) {
        dispatch(beginAjaxCall());
        return accountApi.forgotPassword(userName)
            .then(result => {
                if(result.Errors) {
                    dispatch(accountForgotPasswordFailure());
                }
                else {                    
                    dispatch(accountForgotPasswordSuccess(result));
                }
            })
            .catch(error => {                
                dispatch(accountForgotPasswordFailure());
            });
    };
}

function accountForgotPasswordFailure() {
    return { type: types.ACCOUNT_FORGOT_PASSWORD_FAILURE };
}

function accountForgotPasswordSuccess() {
    return { type: types.ACCOUNT_FORGOT_PASSWORD_SUCCESS };
}

export function leaveForgotPassword() {
    return { type: types.ACCOUNT_FORGOT_PASSWORD_RETURN };
}

export function resetPassword(resetObject) {
    return function(dispatch) {
        dispatch(beginAjaxCall());
        return accountApi.resetPassword(resetObject)
            .then(result => {
                if(result.Errors) {
                    dispatch(accountResetPasswordFailure());
                }
                else {                    
                    dispatch(accountResetPasswordSuccess(result));
                }
            })
            .catch(error => {                
                dispatch(accountResetPasswordFailure());
            });
    };
}

function accountResetPasswordFailure() {
    return { type: types.ACCOUNT_RESET_PASSWORD_FAILURE };
}

function accountResetPasswordSuccess() {
    return { type: types.ACCOUNT_RESET_PASSWORD_SUCCESS };
}

export function leaveResetPassword() {
    return { type: types.ACCOUNT_RESET_PASSWORD_RETURN };
}

export function updateFSLogin(userId, email) {
    var fsLogin = {
        userId: userId,
        email: email
    }
    return { type: types.ACCOUNT_UPDATE_FSLOGIN, fsLogin };
}