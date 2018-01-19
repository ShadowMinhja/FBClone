'use strict';

angular
    .module("common.services")
    .factory("currentUser",
        currentUser)

function currentUser() {
    var profile = {
        isLoggedIn: false,
        userName: null,
        access_token: null,
        loginDate: null,
        id: null,
        token_type: "bearer",
        expires_in: null,
        ".issued": null,
        ".expires":null
    };

    var setProfile = function (userName, access_token, id, loginDate) {
        profile.userName = userName;
        profile.access_token = access_token;
        profile.isLoggedIn = true;
        profile.id = id;
        if (loginDate == null)
            profile.loginDate = moment().format('YYYY-MM-DD HH:mm:ss.SSS');
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