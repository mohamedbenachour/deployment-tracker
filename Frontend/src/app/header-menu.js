import React from 'react';

import { Menu, Button, Row, Col } from 'antd';

const HeaderMenu = ({ currentSection, sectionChanged }) => (
    <React.Fragment>
    <Row>
      <Col span={8}>        <Menu
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
        </Menu></Col>
      <Col span={1} offset={14}>
      <form action="/Account/Logout" method="post">
        <Button
                icon="logout"
                htmlType="submit">
                {'Log Out'}
        </Button>
      </form>
    </Col>
    </Row>
    </React.Fragment>

);

export default HeaderMenu;