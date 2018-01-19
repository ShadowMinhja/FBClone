'use strict';

angular
    .module("common.services")
    .factory("currentUser",
        currentUser)

function currentUser() {
    var profile = {
        isLoggedIn: false,
        username: "",
        token: "",
        loginDate: "",
        id: ""
    };

    var setProfile = function (username, token, id, loginDate) {
        profile.username = username;
        profile.token = token;
        profile.isLoggedIn = true;
        profile.id = id;
        if (loginDate == null)
            profile.loginDate = moment().format('YYYY-MM-DD hh:mm:ss.SSS');
        else
            profile.loginDate = loginDate;
    };

    var getProfile = function () {
        return profile;
    }

    return {
        setProfile: setProfile,
        getProfile: getProfile
    }
}