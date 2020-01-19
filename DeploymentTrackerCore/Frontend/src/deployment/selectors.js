import { createSelector } from 'reselect'

import { getDeployments, getTypes } from '../environment/selectors'
import { statusIsRunning, statusIsDestroyed } from './deployment-status';

const branchNameMatches = (branchName, filter) =>
    branchName.toLowerCase().includes(filter.toLowerCase());

export const getBranchNameFilter = ({ deployment: { filters: { branchName } }}) => branchName;

export const getStatusFilter = ({ deployment: { filters: { status }}}) => status;

export const getTypeFilter = ({ deployment: { filters: { type }}}) => type;

export const sortDeployments = (deployments) => deployments.sort((dOne, dTwo) => {
    const dOneDate = Date.parse(dOne.modifiedBy.timestamp);
    const dTwoDate = Date.parse(dTwo.modifiedBy.timestamp);

    return dOneDate < dTwoDate;
});

export const getSortedDeployments = createSelector(
    getDeployments,
    (deployments) => {
        const sortedDeployments = deployments;

        sortDeployments(sortedDeployments);

        return sortedDeployments;
    }
);

const filterRunningDeployments = ({ status }) => statusIsRunning(status);

const filterTorndownDeployments = ({ status }) => statusIsDestroyed(status);

const filterCompletedDeployments = ({ jira }) => {
    if (jira) {
        return jira.status === 'COMPLETED';
    }

    return false;
};

const getDeploymentFilterByStatus = createSelector(
    [getStatusFilter],
    (status) => {
        if (status === 'running') {
            return filterRunningDeployments;
        } else if (status === 'torndown') {
            return filterTorndownDeployments;
        }

        return filterCompletedDeployments;
    }
);

const typeMatches = ({ id }, typeFilter) => typeFilter === null ? true : id === typeFilter;

export const getVisibleDeployments = createSelector(
    [getSortedDeployments, getBranchNameFilter, getDeploymentFilterByStatus, getTypeFilter],
    (deployments, branchNameFilter, statusFilter, typeFilter) =>
        deployments
        .filter(({ type }) => typeMatches(type, typeFilter))
        .filter(({ branchName }) => branchNameMatches(branchName, branchNameFilter))
        .filter(statusFilter));

export const getTypesToFilterOn = createSelector(
    [getTypes],
    (types) => [{ id: null, name: 'All' }].concat(types)
);