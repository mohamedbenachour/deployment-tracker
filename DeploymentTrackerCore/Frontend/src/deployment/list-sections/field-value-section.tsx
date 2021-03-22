import { CopyOutlined } from '@ant-design/icons';
import { Button, notification, Typography } from 'antd';

interface FieldValueSectionProps {
    fieldName: string;
    value: string;
    allowCopy?: boolean;
}

const copyValue = (value: string): void => {
    void navigator.clipboard.writeText(value).then(() => notification.info({
        message: 'Copied to clipboard',
    }));
};

const FieldValueSection = ({
    fieldName,
    value,
    allowCopy,
}: FieldValueSectionProps): JSX.Element => {
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

FieldValueSection.defaultProps = {
    allowCopy: false,
};

export default FieldValueSection;
