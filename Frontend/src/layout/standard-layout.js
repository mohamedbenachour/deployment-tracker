import React from 'react';

import { Layout } from 'antd';

const StandardLayout = ({ header, children }) => (
    <Layout>
        <Layout.Header>{header || <React.Fragment />}</Layout.Header>
        <Layout.Content>{children}</Layout.Content>
        <Layout.Footer><div>Deployment Tracker</div></Layout.Footer>
    </Layout>
);

export default StandardLayout;