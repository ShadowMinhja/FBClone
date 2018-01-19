import Request from 'superagent'
import initialState from '../../reducers/initialState'
import ImageGallery from '../../utils/ImageGallery'
import * as _ from 'lodash'

class wallFeedApi {
    static getWallFeedBites(access_token, last_Id) {
        return new Promise((resolve, reject) => {
            Request.get('/api/WallFeedBites')
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

export default wallFeedApi;
