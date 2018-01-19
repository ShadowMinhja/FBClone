import React from 'react';
import { Route, IndexRoute } from 'react-router';

import App from './components/App/App.js';
//Home Page
import HomePage from './containers/HomePage/HomePage.js';
//ProfilePage
import ProfilePage from './containers/ProfilePage/ProfilePage.js';
//Login Page
import AuthComplete from './containers/AuthComplete/AuthComplete.js';
import LoginPage from './containers/LoginPage/LoginPage.js';
//Register
import RegisterPage from './containers/RegisterPage/RegisterPage.js';
//Account Section
import AccountPage from './containers/AccountPage/AccountPage.js';
import ConfirmationSent from './containers/AccountPage/ConfirmationSent.js';
import ConfirmEmail from './containers/AccountPage/ConfirmEmail.js';
import ForgotPassword from './containers/AccountPage/ForgotPassword.js';
import ResetPassword from './containers/AccountPage/ResetPassword.js';
//Bite Page
import BitePage from './containers/BitePage/BitePage.js';
//Error Page
import ErrorPage from './containers/ErrorPage/ErrorPage.js';

export default (
  <Route path="/" component={App}>
    <IndexRoute component={HomePage}/>
    <Route path="login" component={LoginPage}/>
    <Route path="register" component={RegisterPage}/>    
    <Route path="account">    
        <IndexRoute component={AccountPage} />
        <Route path="confirmationSent" component={ConfirmationSent}/>    
        <Route path="confirmEmail" component={ConfirmEmail}/>    
        <Route path="forgotPassword" component={ForgotPassword}/>    
        <Route path="resetPassword" component={ResetPassword}/>    
    </Route>
    <Route path="error" component={ErrorPage}/>
    <Route path="bite/:biteId" component={BitePage}/>
    <Route path="authComplete" component={AuthComplete}/>
    <Route path="/:userName" component={ProfilePage}/>
    <Route path="*" component={ErrorPage}/>
  </Route>
);