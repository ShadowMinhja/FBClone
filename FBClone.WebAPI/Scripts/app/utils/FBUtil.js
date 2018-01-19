import React from 'react';
import * as _ from 'lodash'

export default class FBUtil {

    shareBite(url) {
        FB.ui({
            method: 'share',
            href: url,
        }, function(response){});
    }
}