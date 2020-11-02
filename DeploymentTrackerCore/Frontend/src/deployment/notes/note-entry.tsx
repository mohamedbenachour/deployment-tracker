import withStyles from 'react-jss';
import React from 'react';

import { DeleteOutlined } from '@ant-design/icons';
import { Button } from 'antd';
import { DeploymentNote } from '../default-state';
import UserNameBadge from '../../shared/user-names/user-name-badge';
import NoteTooltip from './note-tooltip';

const sectionPadding = '0px 5px 0px 5px';

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
        padding: sectionPadding,
        borderRadius: 4,
    },
    actionsContainer: {
        padding: sectionPadding,
    },
};

interface NoteEntryClasses {
  container: string;
  noteOutline: string;
  actionsContainer: string;
  deleteIcon: string;
}

interface NoteEntryProps {
  note: DeploymentNote;
  classes: NoteEntryClasses;
  onDelete: () => void;
}

const NoteEntry = ({
    note: { content, createdBy },
    classes: { container, noteOutline, actionsContainer },
    onDelete,
}: NoteEntryProps): JSX.Element => (
    <div className={container}>
        <div className={noteOutline}>{content}</div>
        <NoteTooltip createdBy={createdBy}>
            <span>
                <UserNameBadge name={createdBy.name} />
            </span>
        </NoteTooltip>
        <div className={actionsContainer}>
            <Button
              onClick={onDelete}
              size="small"
              shape="circle"
              danger
              icon={<DeleteOutlined />}
            />
        </div>
    </div>
);

export default withStyles(styles)(NoteEntry);
