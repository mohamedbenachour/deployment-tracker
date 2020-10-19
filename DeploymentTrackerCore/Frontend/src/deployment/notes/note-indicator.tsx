import React, { FunctionComponent } from 'react';

import { CommentOutlined } from '@ant-design/icons';
import withStyles from 'react-jss';
import { Popover } from 'antd';
import NotePopover from './note-popover';

const styles = {
    container: {
        display: 'inline',
        paddingLeft: 5,
        cursor: 'pointer',
    },
};

interface NoteIndicatorClasses {
  container: string;
}

interface NoteIndicatorProps {
  deploymentId: number;
  classes: NoteIndicatorClasses;
}

const NoteIndicator = ({
    deploymentId,
    classes: { container },
}: NoteIndicatorProps): JSX.Element => (
    <Popover
      content={<NotePopover deploymentId={deploymentId} />}
      trigger="click"
      destroyTooltipOnHide
    >
        <div className={container}>
            <CommentOutlined />
        </div>
    </Popover>
);

export default withStyles(styles)(NoteIndicator);
