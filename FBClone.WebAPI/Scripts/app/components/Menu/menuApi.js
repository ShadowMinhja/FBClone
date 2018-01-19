import Request from 'superagent'
import initialState from '../../reducers/initialState'

//class menuApi {
//    static getMenuForVenue(access_token, locationId) {
//        return new Promise((resolve, reject) => {
//            Request.get('/api/Locations/GetMenuForLocation')
//            .set('Authorization', 'Bearer ' + access_token)
//            .query({ locationId: locationId })
//            .end((error, response) => {
//                if (error) {               
//                    reject("BAD_CREDENTIAL");
//                }
//                else {
//                    resolve(Object.assign([], response.body));
//                }
//            });
//        });
//    }
//}

//export default menuApi;
