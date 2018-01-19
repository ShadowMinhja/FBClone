import Request from 'superagent'
import initialState from '../reducers/initialState';

class AccountApi {
    static login(userName, password) {
        return new Promise((resolve, reject) => {
            Request.post('/Token')
            .set('Content-Type', 'application/x-www-form-urlencoded')
            .send(`email=${userName}`)
            .send('grant_type=password')
            .send(`password=${password}`)
            .send(`userName=${userName}`)
            .end((error, response) => {
                if (error || !response.ok) {               
                    var errorText;
                    var userId;
                    if(response.body.error_description.indexOf("&userId") != -1) {
                        errorText = response.body.error_description.substring(0, response.body.error_description.indexOf("&userId"));
                        userId = response.body.error_description.substring(response.body.error_description.indexOf("=") + 1, response.body.error_description.length);
                    } else {
                        errorText = response.body.error_description;
                    }
                    reject(Object.assign({}, { errorText: errorText, userId: userId}));
                }
                else {
                    resolve(Object.assign([], response.body, {
                        loginDate: moment().format('YYYY-MM-DD HH:mm:ss.SSS'),
                        isLoggedIn: true
                    }
                    ));
                }
            });
        });
    }

    static logout(access_token) {
        return new Promise((resolve, reject) => {
            Request.post('/api/Account/Logout')
            .set('Authorization', 'Bearer ' + access_token)
            .end((error, response) => {
                if(error) {
                    reject(Object.assign([], response.body.exceptionMessage));
                }
                else 
                {
                    localStorage.removeItem("fsLogin");
                    resolve("Logged Out");
                }
            });
        });
    }

    static getAccountCache() {
        return new Promise((resolve, reject) => {
           let fsLoginCache = JSON.parse(localStorage.getItem("fsLogin"));
           if(fsLoginCache != null)
               resolve(Object.assign([], fsLoginCache));
           else
               reject("Empty fsLogin");
        });
    }

    static registerUser(registrationItem) {
        return new Promise((resolve, reject) => {
            registrationItem.confirmPassword = registrationItem.password;
            Request.post('/api/Account/Register')
            .set('Content-Type', 'application/x-www-form-urlencoded')
            .send(`userName=${registrationItem.userName}`)
            .send(`email=${registrationItem.email}`)
            .send(`firstName=${registrationItem.firstName}`)
            .send(`lastName=${registrationItem.lastName}`)
            .send('organizationName="N/A"')
            .send(`password=${registrationItem.password}`)
            .send(`confirmPassword=${registrationItem.confirmPassword}`)
            .send('subscriptionPlan="free"')
            .end((error, response) => {            
                if(error){
                    reject(error);
                }
                else {
                    resolve(Object.assign([], {
                        id: response.body.id,
                        email: response.body.email,
                        isLoggedIn: false
                    }));
                }
            });
        });
    }

    static registerUserExternal(registrationItem) {
        return new Promise((resolve, reject) => {
            Request.post('/api/Account/RegisterExternal')
            .set('Content-Type', 'application/x-www-form-urlencoded')
            .send(`userName=${registrationItem.userName}`)
            .send(`email=${registrationItem.email}`)
            .send(`firstName=${registrationItem.firstName}`)
            .send(`lastName=${registrationItem.lastName}`)
            .send('organizationName="N/A"')
            .send('subscriptionPlan="free"')
            .send(`provider=${registrationItem.provider}`)
            .send(`externalAccessToken=${registrationItem.externalAccessToken}`)
            .end((error, response) => {
                if(error){
                    reject(error);
                }
                else {
                    resolve(Object.assign([], response.body, {
                        isLoggedIn: false
                    }
                ));
                }
            });
        });
    }

    static resendConfirmationEmail(userId) {
        return new Promise((resolve, reject) => {
            Request.get('/api/Account/ResendConfirmationEmail')
            .query('userId=' + userId)
            .end((error, response) => {
                if(error) {
                    reject({errorText: response.body.message});
                }
                else 
                {
                    resolve(Object.assign([], {
                        isLoggedIn: false,
                        email: response.body.email
                    }
                ));
                }
            });
        });
    }

    static confirmEmail(confirmationInfo) {
        return new Promise((resolve, reject) => {
            Request.get('/api/Account/ConfirmEmail')
            .query('userId=' + confirmationInfo.userId)
            .query('code=' + confirmationInfo.code)
            .end((error, response) => {
                if(error) {
                    reject({errorText: response.body.message});
                }
                else 
                {
                    if(response.body == null) { //Normal Login
                        resolve(Object.assign([], response.body, {
                            isLoggedIn: false
                        }
                        ));
                    } else { //External Login
                        resolve(Object.assign([], response.body, {
                            isLoggedIn: true
                        }
                        ));
                    }
                }
            });
        });
    }

    static obtainAccessToken(externalData) {
        return new Promise((resolve, reject) => {
            Request.get('/api/Account/ObtainLocalAccessToken')
            .query('provider=' + externalData.loginProvider)
            .query('externalAccessToken=' + externalData.externalAccessToken)
            .end((error, response) => {
                if(error) {                    
                    reject({errorText: response.body.message});
                }
                else 
                {
                    resolve(Object.assign([], response.body, {
                        loginDate: moment().format('YYYY-MM-DD HH:mm:ss.SSS'),
                        isLoggedIn: true
                    }
                    ));
                }
            });
        });
    }

    static forgotPassword(userName) {
        return new Promise((resolve, reject) => {
            Request.post('/api/Account/ForgotPassword')
            .set('Content-Type', 'application/x-www-form-urlencoded')
            .send(`email=${userName}`)
            .end((error, response) => {
                if(error){
                    reject(error);
                }
                else {
                    resolve(Object.assign([], response.body));
                }
            });
        });
    }

    static resetPassword(resetObject) {
        return new Promise((resolve, reject) => {
            Request.post('/api/Account/ResetPassword')
            .set('Content-Type', 'application/x-www-form-urlencoded')
            .send(`userId=${resetObject.userId}`)
            .send(`code=${resetObject.code}`)
            .send(`password=${resetObject.password}`)
            .end((error, response) => {
                if(error){
                    reject(error);
                }
                else {
                    resolve(Object.assign([], response.body));
                }
            });
        });
    }

}

export default AccountApi;
