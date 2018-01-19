import React from 'react';
import * as _ from 'lodash'
import config from '../config'

export default class ImageGallery {
    
    processImage (srcImages) {
        var imageGallery = [];
        _.forEach(srcImages, function(obj) {
            imageGallery.push({
                original: `${obj}?auto=format&q=${config.imgix.imageQuality}`,
                thumbnail: `${obj}?auto=format&q=${config.imgix.thumbQuality}&fit=crop&w=100&h=75`,
                originalClass: 'image-gallery-original',
            });
        });
        return imageGallery;
    }

    processBiteImages(bites) {
        const ImageGallery = this;
        _.forEach(bites, function(obj){
            var imageGallery = ImageGallery.processImage(obj.images);
            obj.images = imageGallery;
        });
        return bites;
    }
}