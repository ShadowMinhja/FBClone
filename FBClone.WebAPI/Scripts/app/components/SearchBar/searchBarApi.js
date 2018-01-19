import Request from 'superagent'
import initialState from '../../reducers/initialState'

class searchBarApi {
    static queryUserName(access_token, searchText) {
        return new Promise((resolve, reject) => {
            Request.post('/api/Search/QueryUserName')
            .set('Authorization', 'Bearer ' + access_token)
            .send({ searchText: searchText })
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

    static captureSearch(access_token, searchText) {
        return new Promise((resolve, reject) => {
            Request.post('/api/Search/SearchGeneric')
            .set('Authorization', 'Bearer ' + access_token)
            .send({ searchText: searchText })
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

    static saveSearchHistory(access_token, searchText) {
        return new Promise((resolve, reject) => {
            Request.post('/api/Search')
            .set('Authorization', 'Bearer ' + access_token)
            .send({ searchText: searchText })
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

    static retrieveSearches(access_token) {
        return new Promise((resolve, reject) => {
            Request.get('/api/Search')
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

export default searchBarApi;
