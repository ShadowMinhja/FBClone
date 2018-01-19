//http://threejs.org/examples/#webgl_multiple_elements

import React, { PropTypes } from 'react'
import LoadScript from '../../utils/LoadScript'

import styles from './styles.css'

export class GemPlayArea extends React.Component {    
    constructor() {
        super();
        this.LoadScript = LoadScript;
    }

    componentDidMount() {
        var LoadScript = this.LoadScript;
        if(window.gemLoaded == undefined) {
            LoadScript.prototype.load('/assets/threejs/three.min.js', function() {
                LoadScript.prototype.load('/assets/threejs/OrbitControls.js', function() {
                    LoadScript.prototype.load('/assets/threejs/Detector.js', function() {
                        LoadScript.prototype.load('/assets/threejs/launchGemPlayArea.js', function(){});
                        window.gemLoaded = true;
                    });
                });
            });
        }
        else {
            LoadScript.prototype.load('/assets/threejs/OrbitControls.js', function() {
                LoadScript.prototype.load('/assets/threejs/Detector.js', function() {
                    LoadScript.prototype.load('/assets/threejs/launchGemPlayArea.js', function(){});
                    window.gemLoaded = true;
                });
            });
        }
    }

    render() {
        return (
            <div className="gem-play-area mt-20">
                <canvas id="c"></canvas>
                <div id="content-gem">

                </div>

            </div>

        );
    }
}

export default GemPlayArea;