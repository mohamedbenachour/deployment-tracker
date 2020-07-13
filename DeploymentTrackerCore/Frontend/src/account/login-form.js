import React from 'react';

import {
    Button,
    Input,
    Checkbox,
    Typography,
} from 'antd';

import { LoginOutlined } from '@ant-design/icons';

import withStyles from 'react-jss';

import CsrfHiddenInput from '../utils/csrf-hidden-input';

import Centered from '../layout/centered';

const styles = {
    loginForm: {
        padding: 15,
        '@media (min-width: 401px)': {
            border: 'black solid 2px',
            borderRadius: 4,
        },
    },
    formGroup: {
        marginBottom: 10,
        '& label': {
            marginRight: 5,
        },
    },
};

const LoginForm = ({ classes }) => (
    <Centered>
        <form className={classes.loginForm} method="post">
            <Typography.Title level={3}>Log In to Deployment Tracker</Typography.Title>
            <hr />
            <div className={classes.formGroup}>
                <label htmlFor="Input_UserName">User Name</label>
                <Input id="Input_UserName" name="Input.UserName" />
            </div>
            <div className={classes.formGroup}>
                <label htmlFor="Input_Password">Password</label>
                <Input.Password id="Input_Password" name="Input.Password" />
            </div>
            <div className={classes.formGroup}>
                <Checkbox id="Input_RememberMe" name="Input.RememberMe" value="true">Remember Me?</Checkbox>
            </div>
            <div className={classes.formGroup}>
                <Button type="primary" icon={<LoginOutlined />} htmlType="submit">Log in</Button>
            </div>
            <CsrfHiddenInput />
        </form>
    </Centered>
);

export default withStyles(styles)(LoginForm);
