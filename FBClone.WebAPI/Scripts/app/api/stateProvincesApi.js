import Request from 'superagent'

const stateProvinces = [
];

function replaceAll(str, find, replace) {
    return str.replace(new RegExp(find, 'g'), replace);
}

//This would be performed on the server in a real app. Just stubbing in.
const generateId = (stateProvince) => {
    return replaceAll(stateProvince.title, ' ', '-');
};

class StateProvinceApi {
    static getAllStateProvinces() {
        return new Promise((resolve, reject) => {
            Request.get('/api/StateProvinces')
            .end((error, response) => {
                if (error) {               
                
                }
                else {
                    resolve(Object.assign([], response.body));
                }
            });
        });
    }

}

export default StateProvinceApi;
