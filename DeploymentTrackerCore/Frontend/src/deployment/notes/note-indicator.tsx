import React, { FunctionComponent } from 'react';

import { CommentOutlined } from '@ant-design/icons';
import withStyles from 'react-jss';
import { Popover } from 'antd';
import NotePopover from './note-popover';
import { DarkBlue } from '../../shared/styles/colours';

const styles = {
    container: {
        display: 'inline',
        paddingLeft: 5,
        cursor: 'pointer',
    },
    iconColour: ({ hasNotes }: NoteIndicatorProps) => ({
        color: hasNotes ? DarkBlue : undefined,
    }),
};

interface NoteIndicatorClasses {
  container: string;
  iconColour: string;
}

interface NoteIndicatorProps {
  deploymentId: number;
  hasNotes: boolean;
  classes: NoteIndicatorClasses;
}

const NoteIndicator = ({
    deploymentId,
    classes: { container, iconColour },
}: NoteIndicatorProps): JSX.Element => (
    <Popover
      content={<NotePopover deploymentId={deploymentId} />}
      trigger="click"
      destroyTooltipOnHide
    >
        <div className={container}>
            <CommentOutlined className={iconColour} />
        </div>
    </Popover>
);

export default withStyles(styles)(NoteIndicator);
