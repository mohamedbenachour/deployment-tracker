import React from 'react';

import { Layout } from 'antd';

const StandardLayout = ({ header, children }) => (
    <Layout>
        <Layout.Header style={{ position: 'fixed', zIndex: 100, width: '100%' }}>
            {header || <React.Fragment />}
        </Layout.Header>
        <Layout.Content style={{ marginTop: 64 }}>
            {children}
        </Layout.Content>
        <Layout.Footer><div>Deployment Tracker</div></Layout.Footer>
    </Layout>
);

export default StandardLayout;