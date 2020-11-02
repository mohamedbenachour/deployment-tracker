import React from 'react';

import { Divider } from 'antd';
import NoteList from './note-list';
import NewNote from './new-note';
import NoteStore from './note-store';

interface NotePopoverProps {
  deploymentId: number;
}

const NotePopover = ({ deploymentId }: NotePopoverProps): JSX.Element => {
    const [noteStore] = React.useState(new NoteStore(deploymentId));

    return (
        <>
            <NewNote noteStore={noteStore} />
            <Divider type="horizontal" />
            <NoteList noteStore={noteStore} />
        </>
    );
};

export default NotePopover;
