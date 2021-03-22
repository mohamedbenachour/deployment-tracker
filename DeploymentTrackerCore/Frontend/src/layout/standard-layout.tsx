import { Layout } from 'antd';
import StandardFooter from './standard-footer';

interface StandardLayoutProps {
    header: JSX.Element;
    children: JSX.Element;
}

const StandardLayout = ({
    header,
    children,
}: StandardLayoutProps): JSX.Element => (
    <Layout>
        <Layout.Header style={{ position: 'fixed', zIndex: 100, width: '100%' }}>
            {header || <></>}
        </Layout.Header>
        <Layout.Content style={{ marginTop: 64 }}>{children}</Layout.Content>
        <StandardFooter />
    </Layout>
);

export default StandardLayout;
