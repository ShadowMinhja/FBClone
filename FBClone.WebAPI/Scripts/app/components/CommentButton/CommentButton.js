import React, { Component } from 'react'
import { connect } from 'react-redux'
//import { likePhoto } from '../../utils/analytics.js'

import FontIcon from 'material-ui/FontIcon';
import IconButton from 'material-ui/IconButton';

//import {
//    User as UserActions,
//} from 'actions'

/**
 * CommentButton index component
 */
class CommentButton extends Component {

    constructor(props, context) {
        super(props, context);

        this.handleClick = this.handleClick.bind(this);
    }
    /**
     * defaultProps
     * @type {{id: string, onComment: CommentButton.defaultProps.onComment}}
     */
    /*
    static defaultProps = {
        id: '',
        onComment: () => {},
    }
    */

    /**
     * handleClick
     * @param e event
     */
    handleClick(e) {
        //this.props.onComment({id: this.props.id, liked: !this.props.liked})
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
        if (this.props.liked) classes.push('ion-ios-chatbubbles')
        if (!this.props.liked) classes.push('ion-ios-chatbubbles-outline')

        return <IconButton tooltip="Comment" iconClassName={classes.join(' ')} onClick={this.handleClick}/>
    }

}

export default connect(state => ({
    //user: state.User,
}))(CommentButton)
