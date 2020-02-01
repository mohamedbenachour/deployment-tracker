import React from 'react';
import { Layout, Icon } from 'antd';
import withStyles from 'react-jss';

const styles = {
    container: {
        display: 'flex',
        flexDirection: 'row'
    },
    version: {
        flex: '1 0 auto'
    },
    source: {
        flex: '0 0 auto'
    }
};

const StandardFooter = ({ classes: { container, version, source}}) => (
    <Layout.Footer>
        <div className={container}>
            <div className={version}>{`Deployment Tracker v${process.env.VERSION}`}</div>
            <div className={source}>
                <a
                    href="https://gitlab.com/pmdematagoda/deployment-tracker"
                    target="_blank"
                    >
                    See the Source
                    <Icon
                        type="gitlab"
                        />
                </a>
            </div>
        </div>
    </Layout.Footer>
);

export default withStyles(styles)(StandardFooter);