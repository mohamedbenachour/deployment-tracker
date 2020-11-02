import { MoreOutlined } from '@ant-design/icons';
import { Button, Dropdown, Menu } from 'antd';
import React from 'react';
import { Deployment } from '../deployment-definition';

interface MarkAsTorndownParameters {
  siteName: string;
}

interface MoreActionsDropdownProps {
  deployment: Deployment;
  markAsTorndown: (siteParameter: MarkAsTorndownParameters) => void;
}

const actionsMenu = (
    siteName: string,
    markAsTorndown: (siteParameter: MarkAsTorndownParameters) => void,
): JSX.Element => (
    <Menu>
        <Menu.Item onClick={() => markAsTorndown({ siteName })}>
            Mark as torndown
        </Menu.Item>
    </Menu>
);

const MoreActionsDropdown = ({
    markAsTorndown,
    deployment: { siteName },
}: MoreActionsDropdownProps): JSX.Element => (
    <Dropdown overlay={actionsMenu(siteName, markAsTorndown)}>
        <Button size="small">
            <MoreOutlined />
        </Button>
    </Dropdown>
);

export default MoreActionsDropdown;
