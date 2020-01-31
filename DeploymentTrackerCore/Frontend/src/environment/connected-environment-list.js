import { connect } from 'react-redux';

import {
    bindActionCreators,
} from 'redux';

import EnvironmentList from './environment-list';

import { environmentAddClicked } from './actions';

const mapStateToProps = ({ environment: { environments: { data, loading } } }) => ({
    environments: data || [],
    isLoading: loading,
});

const mapDispatchToInputProps = (dispatch) => bindActionCreators({
    addEnvironment: environmentAddClicked,
}, dispatch);

export default connect(mapStateToProps, mapDispatchToInputProps)(EnvironmentList);
