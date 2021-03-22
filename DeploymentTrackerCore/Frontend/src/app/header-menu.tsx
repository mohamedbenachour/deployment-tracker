import { Link, useMatch } from 'react-router-dom';
import { Menu, Row, Col } from 'antd';

import ApplicationSection from './application-section';
import ApplicationRoute from './application-route';
import UserMenuSection from './user-section/user-menu-section';

const getCurrentSection = (): ApplicationSection => {
    if (useMatch(ApplicationRoute.Environments)) {
        return ApplicationSection.Environments;
    }

    return ApplicationSection.Deployments;
};

const HeaderMenu = (): JSX.Element => (
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
            <Col span={3} offset={4}>
                <UserMenuSection />
            </Col>
        </Row>
    </>
);

export default HeaderMenu;
