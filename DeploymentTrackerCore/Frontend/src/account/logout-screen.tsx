import React from 'react';

import withStyles from 'react-jss';

import { Button } from 'antd';

import Centered from '../layout/centered';

const styles = {
    logoutScreen: {
        padding: 15,
    },
};

interface LogoutScreenClasses {
    logoutScreen: string;
}

interface LogoutScreenProps {
    classes: LogoutScreenClasses;
}

const LogoutScreen = ({ classes }: LogoutScreenProps) => (
    <Centered>
        <div className={classes.logoutScreen}>
            <div>You have successfully logged out.</div>
            <Button href="/Account/Login">Login</Button>
        </div>
    </Centered>
);

export default withStyles(styles)(LogoutScreen);
