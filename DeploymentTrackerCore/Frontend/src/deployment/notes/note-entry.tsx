import withStyles from 'react-jss';
import React from 'react';
import { DeploymentNote } from '../default-state';
import UserNameBadge from '../../shared/user-names/user-name-badge';
import NoteTooltip from './note-tooltip';

const styles = {
    container: {
        display: 'flex',
        flexDirection: 'row',
        padding: 5,
    },
    noteOutline: {
        border: '1px solid grey',
        display: 'inline',
        marginRight: 5,
        padding: '0px 5px 0px 5px',
        borderRadius: 4,
    },
};

interface NoteEntryClasses {
  container: string;
  noteOutline: string;
}

interface NoteEntryProps {
  note: DeploymentNote;
  classes: NoteEntryClasses;
}

const NoteEntry = ({
    note: { content, createdBy },
    classes: { container, noteOutline },
}: NoteEntryProps): JSX.Element => (
    <div className={container}>
        <div className={noteOutline}>{content}</div>
        <NoteTooltip createdBy={createdBy}>
            <span>
                <UserNameBadge name={createdBy.name} />
            </span>
        </NoteTooltip>
    </div>
);

export default withStyles(styles)(NoteEntry);
