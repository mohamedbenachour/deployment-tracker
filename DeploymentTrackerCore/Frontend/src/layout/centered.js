import React from 'react';

import withStyles from 'react-jss';

const styles = {
    container: {
        '@media (min-width: 401px)': {
            display: 'flex',
            alignItems: 'center',
            justifyContent: 'center',
            height: '100vh',
        },
        '@media (max-width: 400px)': {
            width: '100%',
        },
    },
};

const Centered = ({ classes, children }) => (
    <div className={classes.container}>
        {children}
    </div>
);

export default withStyles(styles)(Centered);
