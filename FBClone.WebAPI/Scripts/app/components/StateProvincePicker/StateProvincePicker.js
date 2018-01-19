import React, { PropTypes } from 'react'
import SelectField from 'material-ui/SelectField'
import MenuItem from 'material-ui/MenuItem'

const StateProvincePicker = ({stateProvinces}) => {    
    return (
        <SelectField>
        {
            stateProvinces.map(stateProvince =>
                <MenuItem key={stateProvince.id} value={stateProvince.name} primaryText={stateProvince.name} />
        )}
        </SelectField>
    );
}

StateProvincePicker.propTypes = {
    stateProvinces: PropTypes.array.isRequired,
    handleChange: PropTypes.func
};

export default StateProvincePicker;

/* Sample Usage */
// 1) Import the Web Component
// 2) Pass the api results <StateProvincePicker stateProvinces={stateProvinces} />