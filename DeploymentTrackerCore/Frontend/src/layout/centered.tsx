import { FunctionComponent } from 'react';

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

interface CenteredClasses {
    container: string;
}

interface CenteredProps {
    classes: CenteredClasses;
    children: JSX.Element;
}

const Centered: FunctionComponent<CenteredProps> = ({
    classes,
    children,
}: CenteredProps) => <div className={classes.container}>{children}</div>;

export default withStyles(styles)(Centered);
