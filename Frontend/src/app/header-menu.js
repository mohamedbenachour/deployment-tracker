import React from 'react';

import { Menu } from 'antd';

const HeaderMenu = ({ currentSection, sectionChanged }) => (
    <Menu
        onSelect={({ key }) => sectionChanged(key)}
        mode="horizontal"
        selectedKeys={[currentSection]}
        theme="dark"
        style={{ lineHeight: '64px' }}
    >
        <Menu.Item key="deployments">
            {"Deployments"}
        </Menu.Item>
        <Menu.Item key="environments">
            {"Environments"}
        </Menu.Item>
    </Menu>
);

export default HeaderMenu;