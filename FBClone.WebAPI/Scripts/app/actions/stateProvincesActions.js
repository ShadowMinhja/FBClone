import * as types from './actionTypes'
import stateProvincesApi from '../api/stateProvincesApi'
import {beginAjaxCall, ajaxCallError} from './ajaxStatusActions'

export function loadStateProvincesSuccess(stateProvinces) {
  return { type: types.LOAD_STATEPROVINCES_SUCCESS, stateProvinces };
}

export function loadStateProvinces() {
  return function(dispatch) {
    dispatch(beginAjaxCall());
    return stateProvincesApi.getAllStateProvinces().then(stateProvinces => {
        dispatch(loadStateProvincesSuccess(stateProvinces));
    }).catch(error => {
      throw(error);
    });
  };
}