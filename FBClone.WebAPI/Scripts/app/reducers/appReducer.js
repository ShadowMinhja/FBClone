import { combineReducers } from 'redux'
import bite from '../containers/BitePage/biteReducer.js'
import fsLogin from './accountReducer'
import feedForm from '../components/FeedForm/feedFormReducer.js'
import feed from '../components/Feed/feedReducer.js'
import feedPagination from '../components/Feed/feedPaginationReducer.js'
import foodmenus from '../components/Menu/menuReducer.js'
import profileInfo from '../containers/ProfilePage/profileReducer.js'
import searchResults from '../components/SearchBar/searchBarReducer.js'
import venuePickerResults from '../components/VenuePicker/venuePickerReducer.js'
import stateProvinces from './stateProvincesReducer'
import wallFeed from '../components/WallFeed/wallFeedReducer.js'
import wallFeedPagination from '../components/WallFeed/wallFeedPaginationReducer.js'

const appReducer = combineReducers({
    bite,
    fsLogin, 
    feedForm,
    feed,
    feedPagination,
    foodmenus,
    profileInfo,
    searchResults,
    stateProvinces,
    venuePickerResults,
    wallFeed,
    wallFeedPagination,
});

export default appReducer;