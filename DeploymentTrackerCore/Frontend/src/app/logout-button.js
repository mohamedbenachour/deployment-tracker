import React from 'react';
import Media from 'react-media';

import { Button } from 'antd';

import CsrfHiddenInput from '../utils/csrf-hidden-input';

const LogoutButton = () => (
    <Media
        query={{ maxWidth: 450 }}
    >
        {(matches) => (
            <form action="/Account/Logout" method="post">
                <Button
                  icon="logout"
                  htmlType="submit"
                >
                    {matches ? '' : 'Log Out'}
                </Button>
                <CsrfHiddenInput />
            </form>
        )}
    </Media>
);

export default LogoutButton;
