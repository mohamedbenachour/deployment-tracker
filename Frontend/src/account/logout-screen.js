import React from 'react';

import { Button } from 'antd';

const LogoutScreen = () => (
    <div>
        <div>You have successfully logged out.</div>
        <Button href="/Account/Login">Login</Button>
    </div>
);

export default LogoutScreen;