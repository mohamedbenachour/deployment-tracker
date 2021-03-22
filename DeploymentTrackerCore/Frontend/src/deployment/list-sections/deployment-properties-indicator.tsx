import { UnorderedListOutlined } from '@ant-design/icons';
import { Button, Popover } from 'antd';
import FieldValueSection from './field-value-section';

interface DeploymentPropertiesIndicatorProps {
    properties: Record<string, string> | null;
}

const renderProperties = (
    properties: Record<string, string>,
): JSX.Element[] => {
    const elements: JSX.Element[] = [];

    for (const propertyName in properties) {
        const propertyValue = properties[propertyName];

        elements.push(
            <FieldValueSection
              key={propertyName}
              fieldName={propertyName}
              value={propertyValue}
            />,
        );
    }

    return elements;
};

const DeploymentPropertiesIndicator = ({
    properties,
}: DeploymentPropertiesIndicatorProps): JSX.Element => {
    if (properties === null) {
        return <></>;
    }

    return (
        <Popover content={renderProperties(properties)} trigger="click">
            <Button size="small" type="link" title="Site properties">
                <UnorderedListOutlined />
            </Button>
        </Popover>
    );
};

export default DeploymentPropertiesIndicator;
