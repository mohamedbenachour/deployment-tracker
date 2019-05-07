import { createSelector } from 'reselect'

import { getDeployments } from '../environment/selectors'
import { deploymentIsDestroyed } from './deployment-status';

export const getBranchNameFilter = ({ deployment: { filters: { branchName } }}) => branchName;

export const getShowDestroyed = ({ deployment: {filters: { showDestroyed }}}) => showDestroyed;

export const sortDeployments = (deployments) => deployments.sort((dOne, dTwo) => {
    const dOneDate = Date.parse(dOne.modifiedBy.timestamp);
    const dTwoDate = Date.parse(dTwo.modifiedBy.timestamp);

    return dOneDate < dTwoDate;
});

export const getVisibleDeployments = createSelector(
    [getDeployments, getBranchNameFilter, getShowDestroyed],
    (deployments, branchNameFilter, showDestroyed) =>
        sortDeployments(deployments
        .filter(({ branchName }) => branchName.includes(branchNameFilter))
        .filter(({ status }) => status === 'RUNNING' || (status === 'DESTROYED' && showDestroyed))));