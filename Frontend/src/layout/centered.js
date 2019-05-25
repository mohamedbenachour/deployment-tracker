import React from 'react';

import withStyles from 'react-jss';

const styles = {
    container: {
        '@media (min-width: 401px)': {
            position: 'absolute',
            top: '15%',
            left: '35%'
        },
        '@media (max-width: 400px)': {
            width: '100%'
        }
    }
};

const Centered = ({ classes, children }) => (
    <div className={classes.container}>
        {children}
    </div>
);

export default withStyles(styles)(Centered);