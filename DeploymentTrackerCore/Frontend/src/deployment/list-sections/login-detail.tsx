import { CopyOutlined } from '@ant-design/icons';
import {
    Button, notification, Popover, Typography,
} from 'antd';
import { SiteLogin } from '../deployment-definition';

interface LoginDetailProps {
    login: SiteLogin;
}

const copyValue = (value: string): void => {
    void navigator.clipboard.writeText(value).then(() => notification.info({
        message: 'Copied to clipboard',
    }));
};

const renderLoginContent = (
    fieldName: string,
    value: string,
    allowCopy = false,
): JSX.Element => {
    const labelStyle: {
        paddingRight: number;
        userSelect: 'none';
        '-moz-user-select': 'none';
        '-webkit-user-select': 'none';
    } = {
        paddingRight: 5,
        userSelect: 'none',
        '-moz-user-select': 'none',
        '-webkit-user-select': 'none',
    };
    const valueStyle = {
        backgroundColor: '#e8e8e8',
        padding: 5,
        border: '1px solid',
        borderRadius: 2,
        borderColor: '',
    };

    valueStyle.borderColor = valueStyle.backgroundColor;

    return (
        <div style={{ margin: 10 }}>
            <label>
                <Typography.Text strong style={labelStyle}>
                    {fieldName}
                </Typography.Text>
            </label>
            <Typography.Text style={valueStyle}>{value}</Typography.Text>
            {allowCopy && (
            <Button
              icon={<CopyOutlined />}
              onClick={() => copyValue(value)}
              style={{ marginLeft: 10 }}
              title="Copy to clipboard"
            />
      )}
        </div>
    );
};

const LoginDetail = ({
    login: { userName, password },
}: LoginDetailProps): JSX.Element => (
    <Popover
      content={(
          <>
              {renderLoginContent('Username', userName)}
              {renderLoginContent('Password', password, true)}
          </>
    )}
      trigger="click"
    >
        <Button size="small" type="link">
            Site Login
        </Button>
    </Popover>
);

export default LoginDetail;
