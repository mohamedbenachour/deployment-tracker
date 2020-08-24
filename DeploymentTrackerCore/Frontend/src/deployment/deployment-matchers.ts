import { Deployment } from './deployment-definition';
import getCurrentUser from '../utils/current-user';

const deploymentIsForCurrentUser = (deployment: Deployment): boolean => {
    const currentUser = getCurrentUser();
    const userNameToCheckAgainst = deployment.modifiedBy.userName;

    return (
        currentUser.email === userNameToCheckAgainst
    || currentUser.userName === userNameToCheckAgainst
    );
};

export default deploymentIsForCurrentUser;
