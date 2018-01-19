import Request from 'superagent'
import initialState from '../../reducers/initialState'

class profileApi {
    static getProfile(access_token, targetUserName) {
        return new Promise((resolve, reject) => {
            Request.get('/api/Profiles')
            .query(`targetUserName=${targetUserName}`)
            .set('Authorization', 'Bearer ' + access_token)
            .end((error, response) => {
                if(error) {
                    reject(error.message);
                }
                else 
                {
                    resolve(Object.assign([], response.body));
                }
            });
        });
    }

    static followProfile(access_token, sourceUserId, actor) {
        return new Promise((resolve, reject) => {
            Request.post('/api/Profiles/FollowProfile')
            .set('Authorization', 'Bearer ' + access_token)
            .send({ Source: sourceUserId, Target: actor.id, Actor: actor })
            .end((error, response) => {
                if (error) {               
                    reject("BAD_CREDENTIAL");
                }
                else {
                    resolve(Object.assign({}, response.body));
                }
            });
        });
    }

    static unfollowProfile(access_token, sourceUserId, actor) {
        return new Promise((resolve, reject) => {
            Request.post('/api/Profiles/UnfollowProfile')
            .set('Authorization', 'Bearer ' + access_token)
            .send({ Source: sourceUserId, Target: actor.id, Actor: actor })
            .end((error, response) => {
                if (error) {               
                    reject("BAD_CREDENTIAL");
                }
                else {
                    resolve(Object.assign({}, response.body));
                }
            });
        });
    }
}

export default profileApi;
