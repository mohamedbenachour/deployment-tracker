import React from 'react';

import { Menu, Button, Row, Col } from 'antd';

import LogoutButton from './logout-button';

const HeaderMenu = ({ currentSection, sectionChanged }) => (
    <React.Fragment>
    <Row>
        <Col span={16}>
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
        </Col>
        <Col span={1} offset={6}>
            <LogoutButton />
        </Col>
    </Row>
    </React.Fragment>

);

export default HeaderMenu;