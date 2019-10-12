import { connect } from 'react-redux';

import {
    bindActionCreators
} from 'redux';

import DeploymentList from './deployment-list';

import { getLoading } from '../environment/selectors';
import { getVisibleDeployments, getBranchNameFilter, getStatusFilter, getTypeFilter, getTypesToFilterOn } from './selectors';

import { deploymentAddClicked, deploymentSearch, deploymentStatusFilterChanged, deploymentTypeFilterChange } from './actions';

const mapStateToProps = (state) => ({
    deployments: getVisibleDeployments(state),
    isLoading: getLoading(state),
    branchNameFilter: getBranchNameFilter(state),
    statusFilter: getStatusFilter(state),
    typeFilter: getTypeFilter(state),
    types: getTypesToFilterOn(state)
});

const mapDispatchToInputProps = dispatch => bindActionCreators({
    addDeployment: deploymentAddClicked,
    onSearch: deploymentSearch,
    onStatusFilterChange: deploymentStatusFilterChanged,
    onTypeFilterChange: deploymentTypeFilterChange
}, dispatch);

export default connect(mapStateToProps, mapDispatchToInputProps)(DeploymentList);