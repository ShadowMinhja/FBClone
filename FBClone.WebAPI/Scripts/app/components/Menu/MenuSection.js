import React, { PropTypes } from 'react';

import Subheader from 'material-ui/Subheader'
import MenuItem from './MenuItem.js'

class MenuSection extends React.Component {
    constructor(props, context) {
        super(props, context);
       
        this.state = {
           
        };
     
    }
	
    componentDidMount() {        
    }

    render () {
        const section = this.props.item;
        return (
            <div className="menu-section">
                <Subheader>{section.sectionTitle}</Subheader>
                {section.menuItems.map(menuItem =>
                    <MenuItem key={menuItem.id } menuItem={menuItem} selectMenuItem={this.props.selectMenuItem}/>
                )}                
            </div>
        )
    }
}

export default MenuSection