import { connect } from 'react-redux';

import {
    bindActionCreators
} from 'redux';

import NewDeploymentModal from './new-deployment-modal';

import {
    deploymentBeingAdded,
    deploymentBeingAddedBranchNameChanged,
    deploymentBeingAddedSiteNameChanged,
    deploymentBeingAddedPublicURLChanged,
    deploymentBeingAddedEnvironmentChanged,
    deploymentAddCancelled
} from './actions';
import { addDeployment } from './async-actions';

const mapStateToProps = (state) => {
    const {
        deployment: { deploymentBeingAdded, addingADeployment, saveInProgress },
        environment: { environments: { data }}
    } = state;

    return {
        deploymentBeingAdded: deploymentBeingAdded || { deployedEnvironment : {} },
        visible: addingADeployment,
        environments: data || [],
        saveInProgress,
    };
};

const mapDispatchToInputProps = dispatch => bindActionCreators({
    onBranchNameChange: deploymentBeingAddedBranchNameChanged,
    onSiteNameChange: deploymentBeingAddedSiteNameChanged,
    onPublicUrlChange: deploymentBeingAddedPublicURLChanged,
    onEnvironmentChange: deploymentBeingAddedEnvironmentChanged,
    onOk: addDeployment,
    onCancel: deploymentAddCancelled,
}, dispatch);

export default connect(mapStateToProps, mapDispatchToInputProps)(NewDeploymentModal);