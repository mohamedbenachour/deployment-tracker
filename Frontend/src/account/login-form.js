import React from 'react';

const LoginForm = () => (
    <form id="account" method="post">
        <h4>Log In to Deployment Tracker</h4>
        <hr />
        <div class="form-group">
            <label for="Input_UserName">UserName</label>
            <input id="Input_UserName" name="Input.UserName" class="form-control" />
        </div>
        <div class="form-group">
            <label for="Input_Password">Password</label>
            <input type="password" id="Input_Password" name="Input.Password" class="form-control" />
        </div>
        <div class="form-group">
            <div class="checkbox">
                <label for="Input_RememberMe">
                    <input type="checkbox" value="true" id="Input_RememberMe" name="Input.RememberMe" />
                    Remember Me?
                </label>
            </div>
        </div>
        <div class="form-group">
            <button type="submit" class="btn btn-primary">Log in</button>
        </div>
    </form>
);

export default LoginForm;