import { connect } from 'react-redux';

import {
    bindActionCreators
} from 'redux';

import HeaderMenu from './header-menu';

import { sectionChanged } from './actions';

const mapStateToProps = ({ app: { section } }) => ({
    currentSection: section,
});

const mapDispatchToInputProps = dispatch => bindActionCreators({
    sectionChanged,
}, dispatch);

export default connect(mapStateToProps, mapDispatchToInputProps)(HeaderMenu);