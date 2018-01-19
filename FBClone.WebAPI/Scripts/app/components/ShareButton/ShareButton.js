import React, { Component } from 'react'
import { connect } from 'react-redux'
//import { likePhoto } from '../../utils/analytics.js'

import FontIcon from 'material-ui/FontIcon';
import IconButton from 'material-ui/IconButton';

//import {
//    User as UserActions,
//} from 'actions'

/**
 * ShareButton index component
 */
class ShareButton extends Component {

    constructor(props, context) {
        super(props, context);

        this.handleClick = this.handleClick.bind(this);
    }
    /**
     * defaultProps
     * @type {{id: string, onShare: ShareButton.defaultProps.onShare}}
     */
    /*
    static defaultProps = {
        id: '',
        onShare: () => {},
    }
    */

    /**
     * handleClick
     * @param e event
     */
    handleClick(e) {
        //this.props.onShare({id: this.props.id, liked: !this.props.liked})
        //if (!this.props.liked) {
        //    likePhoto(this.props.user.id, this.props.id)
        //}
        this.props.onShare({id: this.props.id});
    }

    /**
     * render
     * @returns markup
     */
    render() {
        let classes = ['item']
        if (this.props.liked) classes.push('ion-ios-redo')
        if (!this.props.liked) classes.push('ion-ios-redo-outline')

        return <IconButton tooltip="Share" iconClassName={classes.join(' ')} onClick={this.handleClick}/>
    }

}

export default connect(state => ({
    //user: state.User,
}))(ShareButton)
