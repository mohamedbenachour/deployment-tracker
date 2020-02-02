import {
    DEPLOYMENT_ADD_CLICKED,
    DEPLOYMENT_ADD_CANCEL,
    DEPLOYMENT_ADD_BRANCH_NAME_CHANGE,
    DEPLOYMENT_ADD_SITE_NAME_CHANGE,
    DEPLOYMENT_ADD_PUBLIC_URL_CHANGE,
    DEPLOYMENT_ADD_ENVIRONMENT_CHANGE,
    DEPLOYMENT_NEW,
    DEPLOYMENT_SAVE_STARTED,
    DEPLOYMENT_SAVE_FAILED,
    DEPLOYMENT_SEARCH,
    DEPLOYMENT_STATUS_FILTER_CHANGE,
    DEPLOYMENT_TYPE_FILTER_CHANGE,
} from './action-types';
import getReducerForActionsMap from '../utils/reducer-for-actions-map';

const defaultState = {
    addingADeployment: false,
    saveInProgress: false,
    deploymentBeingAdded: null,
    filters: {
        branchName: '',
        status: 'running',
        type: null,
    },
};

const actionsMap = {
    [DEPLOYMENT_SAVE_STARTED]: (draft) => draft.saveInProgress = true,
    [DEPLOYMENT_SAVE_FAILED]: (draft) => draft.saveInProgress = false,
    [DEPLOYMENT_ADD_CANCEL]: (draft) => {
        draft.addingADeployment = false;
        draft.deploymentBeingAdded = null;
    },
    [DEPLOYMENT_ADD_CLICKED]: (draft) => {
        draft.addingADeployment = true;
        draft.deploymentBeingAdded = {
            branchName: '',
            publicURL: 'https://',
            environmentId: null,
            siteName: '',
        };
    },
    [DEPLOYMENT_SEARCH]: (draft, { searchName }) => draft.filters.branchName = searchName,
    [DEPLOYMENT_STATUS_FILTER_CHANGE]: (draft, { value }) => draft.filters.status = value,
    [DEPLOYMENT_TYPE_FILTER_CHANGE]: (draft, { typeId }) => draft.filters.type = typeId,
    [DEPLOYMENT_ADD_BRANCH_NAME_CHANGE]: (draft, { branchName }) => draft.deploymentBeingAdded.branchName = branchName,
    [DEPLOYMENT_ADD_SITE_NAME_CHANGE]: (draft, { siteName }) => draft.deploymentBeingAdded.siteName = siteName,
    [DEPLOYMENT_ADD_PUBLIC_URL_CHANGE]: (draft, { publicURL }) => draft.deploymentBeingAdded.publicURL = publicURL,
    [DEPLOYMENT_ADD_ENVIRONMENT_CHANGE]: (draft, { environmentId }) => draft.deploymentBeingAdded.environmentId = environmentId,
    [DEPLOYMENT_NEW]: (draft) => {
        draft.saveInProgress = false;
        draft.addingADeployment = false;
        draft.deploymentBeingAdded = null;
    }
};

export default getReducerForActionsMap(actionsMap, defaultState);
