import Request from 'superagent'
import initialState from '../../reducers/initialState'

class feedFormApi {
    
    static multiFormPost(access_token, bite) {
        var requestHeader = Request.post('/api/Bites')
            .set('Authorization', 'Bearer ' + access_token)
            .field('comment', bite.comment)        
            .field('foodType', bite.foodType)            
            .field('foodTags', bite.foodTags)
            .field('allergenTags', bite.allergenTags)
            .field('venue', JSON.stringify(bite.venue))
            .field('menuItem', JSON.stringify(bite.menuItem))
            .field('surveyQuestionResponseSet', JSON.stringify(bite.surveyQuestionResponseSet))
        ;

        _.forEach(bite.images, function(obj) {
            requestHeader.attach('image', obj);
        });
        return requestHeader;
    } 

    static postBite(access_token, bite) {
        const feedFormApi = this;
        return new Promise((resolve, reject) => {
            feedFormApi.multiFormPost(access_token, bite)
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

export default feedFormApi;
