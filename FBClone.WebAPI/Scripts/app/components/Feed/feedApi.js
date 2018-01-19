import Request from 'superagent'
import initialState from '../../reducers/initialState'

class feedApi {
    static getBites(access_token, userName, last_Id) {
        return new Promise((resolve, reject) => {
            Request.get('/api/Bites')
            .query(userName == undefined ? 'userName=' : 'userName=' + userName)
            .query(last_Id == null ? 'lastId=' : 'lastId=' + last_Id)
            .set('Authorization', 'Bearer ' + access_token)
            .end((error, response) => {
                if(error) {
                    reject(Object.assign([], response.body.exceptionMessage));
                }
                else 
                {
                    resolve(Object.assign([], response.body));
                }
            });
        });
    }
}

export default feedApi;
