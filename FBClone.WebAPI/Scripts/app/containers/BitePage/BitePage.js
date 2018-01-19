import React, { PropTypes } from 'react'
import {connect} from 'react-redux'
import {bindActionCreators} from 'redux'
import BiteCard from '../../components/BiteCard/BiteCard.js'
import * as biteActions from './biteActions'
import config from '../../config'

import RaisedButton from 'material-ui/RaisedButton'
import {Card, CardActions, CardHeader, CardMedia, CardTitle, CardText} from 'material-ui/Card'
import FlatButton from 'material-ui/FlatButton'
import Divider from 'material-ui/Divider'
import Paper from 'material-ui/Paper'
import TextField from 'material-ui/TextField'
import IconButton from 'material-ui/IconButton'
import ImageGallery from 'react-image-gallery'
import {teal300} from 'material-ui/styles/colors'

import Actor from '../../components/BiteCard/Actor.js'
import LikeButton from '../../components/LikeButton/LikeButton.js'
import CommentButton from '../../components/CommentButton/CommentButton.js'
import ShareButton from '../../components/ShareButton/ShareButton.js'

import styles from './styles.css'

class BitePage extends React.Component {
    constructor(props, context) {
        super(props, context);

        const apiServiceBaseUri = config.api.baseUrl;  
        this.state = {         
            apiServiceBaseUri: apiServiceBaseUri
        };

        this.handleImageLoad = this.handleImageLoad.bind(this);
    }
	
    handleImageLoad(event) {
    }

    componentDidMount() {
        this.props.actions.getBite(this.props.biteId);
    }

    componentDidUpdate(prevProps, prevState) {
    }

    componentWillUnmount() {
    }

    componentWillReceiveProps(nextProps) {
    }

    render () {
        const {bite, onLike, onComment, onShare} = this.props;
        var isExpanded; 
        if(bite.id !== undefined) {
            isExpanded = bite.images !== null ? bite.images.length > 0 ? true : false : false;
            return (
                <div className="bite-page">
                    <BiteCard item={bite} onLike={onLike} onComment={onComment} onShare={onShare} width='offset-md-2 col-md-8'/>
                </div>
            )
        }
        else {
            return (
                <div className="feed-area-refresh" style={{paddingTop: '15%'}}>
                    <span className="fa fa-refresh fa-spin fa-5x loading-feed"></span>
                </div>
            );
        }
    }
}

function mapStateToProps(state, ownProps) {
    return {
        fsLogin: state.fsLogin,
        biteId: ownProps.params.biteId,
        bite: state.bite
    };
    }

function mapDispatchToProps(dispatch) {
    return {
        actions: bindActionCreators(biteActions, dispatch),
    };
    }

export default connect(mapStateToProps, mapDispatchToProps)(BitePage);