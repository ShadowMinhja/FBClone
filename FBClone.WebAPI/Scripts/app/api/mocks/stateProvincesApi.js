import delay from './delay';

// This file mocks a web API by working with the hard-coded data below.
// It uses setTimeout to simulate the delay of an AJAX call.
// All calls return promises.
const stateProvinces = [
    {
        "id":1,
        "countryId":1,
        "name":"AA (Armed Forces Americas)",
        "abbreviation":"AA",
        "published":true,
        "displayOrder":1
    },
    {
        "id":2,
        "countryId":1,
        "name":"AE (Armed Forces Europe)",
        "abbreviation":"AE",
        "published":true,
        "displayOrder":1
    },
    {
        "id":3,
        "countryId":1,
        "name":"Alabama",
        "abbreviation":"AL",
        "published":true,
        "displayOrder":1
    },
    {
        "id":4,
        "countryId":1,
        "name":"Alaska",
        "abbreviation":"AK",
        "published":true,
        "displayOrder":1
    },
    {
        "id":5,
        "countryId":1,
        "name":"American Samoa",
        "abbreviation":"AS",
        "published":true,
        "displayOrder":1
    }
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
            setTimeout(() => {
                resolve(Object.assign([], stateProvinces));
            }, delay);
        });
    }

    static saveStateProvince(stateProvince) {
        return new Promise((resolve, reject) => {
            setTimeout(() => {
                // Simulate server-side validation
                const minStateProvinceTitleLength = 1;
                if (stateProvince.title.length < minStateProvinceTitleLength) {
                    reject(`Title must be at least ${minStateProvinceTitleLength} characters.`);
                }

                if (stateProvince.id) {
                    const existingStateProvinceIndex = stateProvinces.findIndex(a => a.id == stateProvince.id);
                    stateProvinces.splice(existingStateProvinceIndex, 1, stateProvince);
                } else {
                    //Just simulating creation here.
                    //The server would generate ids and watchHref's for new stateProvinces in a real app.
                    //Cloning so copy returned is passed by value rather than by reference.
                    stateProvince.id = generateId(stateProvince);
                    stateProvince.watchHref = `http://www.pluralsight.com/stateProvinces/${stateProvince.id}`;
                    stateProvinces.push(stateProvince);
                }

                resolve(Object.assign({}, stateProvince));
            }, delay);
        });
    }

    static deleteStateProvince(stateProvinceId) {
        return new Promise((resolve, reject) => {
            setTimeout(() => {
                const indexOfStateProvinceToDelete = stateProvinces.findIndex(stateProvince => {
                    stateProvince.stateProvinceId == stateProvinceId;
                });
                stateProvinces.splice(indexOfStateProvinceToDelete, 1);
                resolve();
            }, delay);
        });
    }
}

export default StateProvinceApi;
