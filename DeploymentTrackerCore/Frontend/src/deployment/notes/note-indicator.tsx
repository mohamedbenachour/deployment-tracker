import { CommentOutlined } from '@ant-design/icons';
import withStyles, { WithStylesProps } from 'react-jss';
import { Popover } from 'antd';
import NotePopover from './note-popover';
import { DarkBlue } from '../../shared/styles/colours';

const styles = {
    container: {
        display: 'inline',
        paddingLeft: 5,
        cursor: 'pointer',
    },
    iconColourHighlighted: {
        color: DarkBlue,
    },
};

interface NoteIndicatorProps extends WithStylesProps<typeof styles> {
    deploymentId: number;
    hasNotes: boolean;
}

const NoteIndicator = ({
    deploymentId,
    hasNotes,
    classes: { container, iconColourHighlighted },
}: NoteIndicatorProps): JSX.Element => (
    <Popover
      content={<NotePopover deploymentId={deploymentId} />}
      trigger="click"
      destroyTooltipOnHide
    >
        <div className={container}>
            <CommentOutlined className={hasNotes ? iconColourHighlighted : ''} />
        </div>
    </Popover>
);

export default withStyles(styles)(NoteIndicator);
