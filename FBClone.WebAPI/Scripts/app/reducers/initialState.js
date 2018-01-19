export default {
    ajaxCallsInProgress: 0,
    bites: [],
    bite: {},
    fsLogin: {'.expires': null, '.issued': null, access_token: null, expires_in: null, id: null, organizationName: null, token_type: null, userName: null, isLoggedIn: false},
    feedFormPost: {},
    feedPagination: {
        lastId: null,
        fetching: false,
    },
    foodmenus: null,
    profileInfo: {
        isFollowing: false,
        errorText: null
    },
    searchResults: {
        users: null,
        algolia: [],
        history: []
    },
    stateProvinces: [],
    venuePickerResults: null,
    wallFeedBites: [],
    wallFeedPagination: {
        lastId: null,
        fetching: false,
    },    
};
