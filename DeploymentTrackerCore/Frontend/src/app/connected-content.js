import React from 'react';

import { connect } from 'react-redux';

import EnvironmentList from '../environment/connected-environment-list';
import DeploymentList from '../deployment/connected-deployment-list';

const Content = ({ section }) => {
    if (section === 'environments') {
        return <EnvironmentList />;
    }

    if (section === 'deployments') {
        return <DeploymentList />;
    }
};

const mapStateToProps = ({ app: { section } }) => ({
    section,
});

export default connect(mapStateToProps)(Content);
