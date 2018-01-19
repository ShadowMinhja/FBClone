import Request from 'superagent'
import initialState from '../../reducers/initialState'
import ImageGallery from '../../utils/ImageGallery'

class biteApi {
    static getBite(access_token, biteId) {
        return new Promise((resolve, reject) => {
            Request.get('/api/Bites/GetBiteById')
            .query(biteId == null ? 'biteId=' : 'biteId=' + biteId)
            .set('Authorization', 'Bearer ' + access_token)
            .end((error, response) => {
                if(error) {
                    reject(Object.assign({}, response.body.exceptionMessage));
                }
                else 
                {
                    var result = response.body;
                    var imageGallery = ImageGallery.prototype.processImage(response.body.images);
                    result.images = imageGallery;
                    resolve(Object.assign({}, result));
                }
            });
        });
    }
}

export default biteApi;
