import React from 'react';

import { Divider } from 'antd';
import NoteList from './note-list';
import NewNote from './new-note';

interface NotePopoverProps {
  deploymentId: number;
}

const NotePopover = ({ deploymentId }: NotePopoverProps): JSX.Element => (
    <>
        <NewNote deploymentId={deploymentId} />
        <Divider type="horizontal" />
        <NoteList deploymentId={deploymentId} />
    </>
);

export default NotePopover;
