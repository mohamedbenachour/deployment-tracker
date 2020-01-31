import { connect } from 'react-redux';

import {
    bindActionCreators,
} from 'redux';

import NewEnvironmentModal from './new-environment-modal';

import { environmentBeingAddedNameChanged, environmentAddCancelled, environmentBeingAddedHostNameChanged } from './actions';
import { addEnvironment } from './async-actions';

const mapStateToProps = ({ environment: { environmentBeingAdded, addingAnEnvironment, saveInProgress } }) => ({
    environmentBeingAdded: environmentBeingAdded || {},
    visible: addingAnEnvironment,
    saveInProgress,
});

const mapDispatchToInputProps = (dispatch) => bindActionCreators({
    onNameChange: environmentBeingAddedNameChanged,
    onHostNameChange: environmentBeingAddedHostNameChanged,
    onOk: addEnvironment,
    onCancel: environmentAddCancelled,
}, dispatch);

export default connect(mapStateToProps, mapDispatchToInputProps)(NewEnvironmentModal);
