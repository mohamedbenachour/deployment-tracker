import Media from 'react-media';

import { Button } from 'antd';
import { LogoutOutlined } from '@ant-design/icons';

import CsrfHiddenInput from '../../utils/csrf-hidden-input';

const LogoutButton = (): JSX.Element => (
    <Media query={{ maxWidth: 450 }}>
        {(matches) => (
            <form action="/Account/Logout" method="post">
                <Button icon={<LogoutOutlined />} htmlType="submit">
                    {matches ? '' : 'Log Out'}
                </Button>
                <CsrfHiddenInput />
            </form>
        )}
    </Media>
);

export default LogoutButton;
