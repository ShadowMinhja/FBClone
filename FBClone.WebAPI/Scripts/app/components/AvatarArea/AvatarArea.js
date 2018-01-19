import React, { Component } from 'react'
import Avatar from 'material-ui/Avatar';

const styles = {
    root: {
        backgroundColor: '#fafafa',
        display: 'inline-block',
    }
}

/**
 * AvatarArea index component
 */
class AvatarArea extends Component {

    /**
     * defaultProps
     * @type {{email: null, height: number, imgHeight: number}}
     */
    /*
    static defaultProps = {
        email_md5: null,
        height: 155,
        imgHeight: 400,
    }
    */
    /**
     * render
     * @returns markup
     */
    render() {

        const placeHolder = Object.assign({}, styles.root, {
            height: this.props.height,
            width: this.props.height,
        })

        if (!this.props.emailHash) return <div style={placeHolder}/>

        return <Avatar src={`https://s.gravatar.com/avatar/${this.props.emailHash}?s=${this.props.height}`}
                    size={this.props.height} />

    }
}

export default AvatarArea
