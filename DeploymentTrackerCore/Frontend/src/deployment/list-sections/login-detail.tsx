import { KeyOutlined } from '@ant-design/icons';
import { Button, Popover } from 'antd';

import { SiteLogin } from '../deployment-definition';
import FieldValueSection from './field-value-section';

interface LoginDetailProps {
    login: SiteLogin;
}

const LoginDetail = ({
    login: { userName, password },
}: LoginDetailProps): JSX.Element => (
    <Popover
      content={(
          <>
              <FieldValueSection fieldName="Username" value={userName} />
              <FieldValueSection fieldName="Password" value={password} allowCopy />
          </>
    )}
      trigger="click"
    >
        <Button size="small" type="link" title="Login details">
            <KeyOutlined />
        </Button>
    </Popover>
);

export default LoginDetail;
