import React, { PropTypes } from 'react';

const ImgPreview = (item) => {
    return (        
        <img src={item.imgData} width="150" height="150" />
    );
}

ImgPreview.propTypes = {
};

export default ImgPreview;