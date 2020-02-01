import React from 'react';

import { Layout } from 'antd';
import StandardFooter from './standard-footer';

const StandardLayout = ({ header, children }) => (
    <Layout>
        <Layout.Header style={{ position: 'fixed', zIndex: 100, width: '100%' }}>
            {header || <></>}
        </Layout.Header>
        <Layout.Content style={{ marginTop: 64 }}>
            {children}
        </Layout.Content>
        <StandardFooter />
    </Layout>
);

export default StandardLayout;
