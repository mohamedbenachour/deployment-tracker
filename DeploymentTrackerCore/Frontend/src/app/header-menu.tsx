import React, { Key } from 'react';

import { Link, useLocation, useMatch } from 'react-router-dom';
import {
    Menu, Button, Row, Col,
} from 'antd';

import LogoutButton from './logout-button';
import ApplicationSection from './application-section';
import ApplicationRoute from './application-route';

const getCurrentSection = (): ApplicationSection => {
    if (useMatch(ApplicationRoute.Environments)) {
        return ApplicationSection.Environments;
    }

    return ApplicationSection.Deployments;
};

const HeaderMenu = () => (
    <>
        <Row>
            <Col span={16}>
                <Menu
                  mode="horizontal"
                  selectedKeys={[`${getCurrentSection()}`]}
                  theme="dark"
                  style={{ lineHeight: '64px' }}
                >
                    <Menu.Item key={ApplicationSection.Deployments}>
                        <Link to={ApplicationRoute.Deployments}>Deployments</Link>
                    </Menu.Item>
                    <Menu.Item key={ApplicationSection.Environments}>
                        <Link to={ApplicationRoute.Environments}>Environments</Link>
                    </Menu.Item>
                </Menu>
            </Col>
            <Col span={1} offset={6}>
                <LogoutButton />
            </Col>
        </Row>
    </>
);

export default HeaderMenu;
