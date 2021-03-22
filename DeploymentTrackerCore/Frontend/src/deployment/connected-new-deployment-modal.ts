import { connect } from 'react-redux';

import { bindActionCreators, Dispatch } from 'redux';

import NewDeploymentModal from './new-deployment-modal';

import {
    deploymentBeingAddedBranchNameChanged,
    deploymentBeingAddedSiteNameChanged,
    deploymentBeingAddedPublicURLChanged,
    deploymentBeingAddedEnvironmentChanged,
    deploymentAddCancelled,
} from './actions';
import { addDeployment } from './async-actions';
import ApplicationState from '../state-definition';
import { DeploymentActionTypes } from './action-types';
import { Deployment } from './deployment-definition';

const mapStateToProps = (state: ApplicationState) => {
    const {
        deployment: { deploymentBeingAdded, addingADeployment, saveInProgress },
        environment: {
            environments: { data },
        },
    } = state;

    return {
        deploymentBeingAdded:
      deploymentBeingAdded
      || (({ deployedEnvironment: {} } as unknown) as Deployment),
        visible: addingADeployment,
        environments: data || [],
        saveInProgress,
    };
};

const mapDispatchToInputProps = (dispatch: Dispatch<DeploymentActionTypes>) => bindActionCreators(
    {
        onBranchNameChange: deploymentBeingAddedBranchNameChanged,
        onSiteNameChange: deploymentBeingAddedSiteNameChanged,
        onPublicUrlChange: deploymentBeingAddedPublicURLChanged,
        onEnvironmentChange: deploymentBeingAddedEnvironmentChanged,
        onOk: addDeployment,
        onCancel: deploymentAddCancelled,
    },
    dispatch,
);

export default connect(
    mapStateToProps,
    mapDispatchToInputProps,
)(NewDeploymentModal);
