import { connect } from 'react-redux';

import {
    bindActionCreators
} from 'redux';

import DeploymentList from './deployment-list';

import { getLoading } from '../environment/selectors';
import { getVisibleDeployments, getBranchNameFilter, getStatusFilter } from './selectors';

import { deploymentAddClicked, deploymentSearch, deploymentStatusFilterChanged } from './actions';

const mapStateToProps = (state) => ({
    deployments: getVisibleDeployments(state),
    isLoading: getLoading(state),
    branchNameFilter: getBranchNameFilter(state),
    statusFilter: getStatusFilter(state),
});

const mapDispatchToInputProps = dispatch => bindActionCreators({
    addDeployment: deploymentAddClicked,
    onSearch: deploymentSearch,
    onStatusFilterChange: deploymentStatusFilterChanged,
}, dispatch);

export default connect(mapStateToProps, mapDispatchToInputProps)(DeploymentList);