import React from 'react';

import withStyles from 'react-jss';

import { Button } from 'antd';

const styles = {
    logoutScreen: {
        position: 'absolute',
        left: '35%',
        top: '10%'
    }
};

const LogoutScreen = ({ classes }) => (
    <div className={classes.logoutScreen}>
        <div>You have successfully logged out.</div>
        <Button href="/Account/Login">Login</Button>
    </div>
);

export default withStyles(styles)(LogoutScreen);