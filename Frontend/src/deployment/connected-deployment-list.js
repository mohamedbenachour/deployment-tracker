import { connect } from 'react-redux';

import {
    bindActionCreators
} from 'redux';

import DeploymentList from './deployment-list';

import { getLoading } from '../environment/selectors';
import { getVisibleDeployments, getBranchNameFilter, getShowDestroyed } from './selectors';

import { deploymentAddClicked, deploymentSearch, deploymentShowDestroyed } from './actions';

const mapStateToProps = (state) => ({
    deployments: getVisibleDeployments(state),
    isLoading: getLoading(state),
    branchNameFilter: getBranchNameFilter(state),
    showDestroyed: getShowDestroyed(state),
});

const mapDispatchToInputProps = dispatch => bindActionCreators({
    addDeployment: deploymentAddClicked,
    onSearch: deploymentSearch,
    onShowDestroyedChange: deploymentShowDestroyed,
}, dispatch);

export default connect(mapStateToProps, mapDispatchToInputProps)(DeploymentList);