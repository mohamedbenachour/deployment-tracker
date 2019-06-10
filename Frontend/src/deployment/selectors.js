import { createSelector } from 'reselect'

import { getDeployments } from '../environment/selectors'
import { statusIsRunning, statusIsDestroyed } from './deployment-status';


const branchNameMatches = (branchName, filter) =>
    branchName.toLowerCase().includes(filter.toLowerCase());

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
        .filter(({ branchName }) => branchNameMatches(branchName, branchNameFilter))
        .filter(({ status }) => statusIsRunning(status) || (statusIsDestroyed(status) && showDestroyed))));