import { connect } from 'react-redux';
import { getDeployments } from '../../environment/selectors';
import ApplicationState from '../../state-definition';
import MentionsMenu from './mentions-menu';

const mapStateToProps = (state: ApplicationState) => ({
    deployments: getDeployments(state),
});

export default connect(mapStateToProps)(MentionsMenu);
