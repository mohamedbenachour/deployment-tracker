import { createSelector } from 'reselect'

import { getDeployments } from '../environment/selectors'

export const getBranchNameFilter = ({ deployment: { filters: { branchName } }}) => branchName;

export const getShowDestroyed = ({ deployment: {filters: { showDestroyed }}}) => showDestroyed;

export const getVisibleDeployments = createSelector(
    [getDeployments, getBranchNameFilter, getShowDestroyed],
    (deployments, branchNameFilter, showDestroyed) =>
        deployments
        .filter(({ branchName }) => branchName.includes(branchNameFilter))
        .filter(({ status }) => status === 'RUNNING' || (status === 'DESTROYED' && showDestroyed)));