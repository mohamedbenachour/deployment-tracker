import { connect } from 'react-redux';

import {
    bindActionCreators
} from 'redux';

import DeploymentList from './deployment-list';

import { getDeployments, getLoading } from '../environment/selectors';

import { deploymentAddClicked } from './actions';

const mapStateToProps = (state) => ({
    deployments: getDeployments(state),
    isLoading: getLoading(state),
});

const mapDispatchToInputProps = dispatch => bindActionCreators({
    addDeployment: deploymentAddClicked,
}, dispatch);

export default connect(mapStateToProps, mapDispatchToInputProps)(DeploymentList);