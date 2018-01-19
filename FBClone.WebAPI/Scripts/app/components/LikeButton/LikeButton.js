import React, { Component } from 'react'
import { connect } from 'react-redux'
import { likePhoto } from '../../utils/analytics.js'

import FontIcon from 'material-ui/FontIcon';
import IconButton from 'material-ui/IconButton';

//import {
//    User as UserActions,
//} from 'actions'

/**
 * LikeButton index component
 */
class LikeButton extends Component {

    constructor(props, context) {
        super(props, context);

        this.handleClick = this.handleClick.bind(this);
    }
    /**
     * defaultProps
     * @type {{id: string, onLike: LikeButton.defaultProps.onLike}}
     */
    /*
    static defaultProps = {
        id: '',
        onLike: () => {},
    }
    */

    /**
     * handleClick
     * @param e event
     */
    handleClick(e) {
        //this.props.onLike({id: this.props.id, liked: !this.props.liked})
        //if (!this.props.liked) {
        //    likePhoto(this.props.user.id, this.props.id)
        //}
    }

    /**
     * render
     * @returns markup
     */
    render() {
        let classes = ['item']
        if (this.props.liked) classes.push('ion-ios-heart')
        if (this.props.liked == undefined || !this.props.liked) classes.push('ion-ios-heart-outline')

        return <IconButton tooltip="Like" iconClassName={classes.join(' ')} onClick={this.handleClick} />
    }

}

export default connect(state => ({
    //user: state.User,
}))(LikeButton)
