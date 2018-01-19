import React, { PropTypes } from 'react';

import GridTile from 'material-ui/GridList'
import IconButton from 'material-ui/IconButton'

class MenuItem extends React.Component {
    constructor(props, context) {
        super(props, context);
       
        this.state = {
           
        };
        
        this.selectMenuItem = this.selectMenuItem.bind(this);
    }
	
    componentDidMount() {        
    }

    selectMenuItem(item){
        this.props.selectMenuItem(this.props.menuItem);
    }

    render () { 
        const menuItem = this.props.menuItem;        
        const friendlyItemText = this.props.menuItem.itemText.replace("<p>", "").replace("</p>", "");
        return (
            <GridTile className="menu-item">
                <div dangerouslySetInnerHTML={{__html: menuItem.itemText}} /> 
                <div><p>{ menuItem.price == -1 ? 'Market Price' : '$' + menuItem.price }</p></div>
                <IconButton className="menu-item-select-btn"
                    iconClassName="fa fa-chevron-right"
                    tooltip={"I ate this yummy " + friendlyItemText } 
                    tooltipPosition="top-right"
                    onTouchTap={this.selectMenuItem}
                />
            </GridTile>
        )
    }
}

export default MenuItem