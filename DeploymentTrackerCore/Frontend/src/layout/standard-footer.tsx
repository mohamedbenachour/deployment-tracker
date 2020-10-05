import React from 'react';
import { Layout } from 'antd';
import { GitlabOutlined } from '@ant-design/icons';
import withStyles from 'react-jss';

const styles = {
    container: {
        display: 'flex',
        flexDirection: 'row',
    },
    version: {
        flex: '1 0 auto',
    },
    source: {
        flex: '0 0 auto',
    },
};

interface Classes {
  container: string;
  version: string;
  source: string;
}

interface StandardFooterProps {
  classes: Classes;
}

const StandardFooter = ({
    classes: { container, version, source },
}: StandardFooterProps): JSX.Element => (
    <Layout.Footer>
        <div className={container}>
            <div className={version}>
                {`Deployment Tracker v${
                    process.env.VERSION ?? ''
                }`}
            </div>
            <div className={source}>
                <a
                    href="https://gitlab.com/pmdematagoda/deployment-tracker"
                    target="_blank"
                >
                    See the Source
                    <GitlabOutlined />
                </a>
            </div>
        </div>
    </Layout.Footer>
);

export default withStyles(styles)(StandardFooter);
