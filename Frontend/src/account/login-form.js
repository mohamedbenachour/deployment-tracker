import React from 'react';

import {
    Button,
    Input,
    Checkbox,
    Typography
} from 'antd';

import withStyles from 'react-jss';

const styles = {
    loginForm: {
        padding: 15,
        border: 'black solid 2px',
        borderRadius: 4,
        width: 400,
        position: 'absolute',
        top: '15%',
        left: '35%'
    },
    formGroup: {
        marginBottom: 10,
        '& label': {
            marginRight: 5,
        }
    }
};

const LoginForm = ({ classes }) => (
    <form className={classes.loginForm} method="post">
        <Typography.Title level={3}>Log In to Deployment Tracker</Typography.Title>
        <hr />
        <div class={classes.formGroup}>
            <label for="Input_UserName">User Name</label>
            <Input id="Input_UserName" name="Input.UserName" />
        </div>
        <div class={classes.formGroup}>
            <label for="Input_Password">Password</label>
            <Input type="password" id="Input_Password" name="Input.Password" />
        </div>
        <div class={classes.formGroup}>
            <Checkbox id="Input_RememberMe" name="Input.RememberMe" value="true">Remember Me?</Checkbox>
        </div>
        <div class={classes.formGroup}>
            <Button type="primary" icon="login" htmlType="submit">Log in</Button>
        </div>
    </form>
);

export default withStyles(styles)(LoginForm);