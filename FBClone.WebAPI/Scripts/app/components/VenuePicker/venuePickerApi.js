import Request from 'superagent'
import initialState from '../../reducers/initialState'

class venuePickerApi {
    static queryVenue(access_token, gPlaceId) {
        return new Promise((resolve, reject) => {
            Request.get('/api/Locations/SearchLocation')
            .set('Authorization', 'Bearer ' + access_token)
            .query({ placeId: gPlaceId })
            .end((error, response) => {
                if (error) {               
                    reject("BAD_CREDENTIAL");
                }
                else {
                    resolve(Object.assign([], response.body));
                }
            });
        });
    }

}

export default venuePickerApi;
