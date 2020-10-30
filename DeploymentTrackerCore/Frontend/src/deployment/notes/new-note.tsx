import React from 'react';
import TextArea from 'antd/lib/input/TextArea';
import { observer } from 'mobx-react-lite';
import SubtleSpinner from '../../shared/interactivity/subtle-spinner';
import NoteStore from './note-store';

interface NewNoteProps {
  noteStore: NoteStore;
}

const isSaveButtonPress = ({
    key,
    ctrlKey,
}: React.KeyboardEvent<HTMLTextAreaElement>) => ctrlKey && key === 'Enter';

const handleKeyPress = (
    event: React.KeyboardEvent<HTMLTextAreaElement>,
    noteStore: NoteStore,
): void => {
    if (isSaveButtonPress(event)) {
        noteStore.save(event.currentTarget.value);
    }
};

const NewNote = ({ noteStore }: NewNoteProps): JSX.Element => {
    const helpText = 'Ctrl+Enter to Save.';

    if (noteStore.isSaving) {
        return (
            <div>
                <SubtleSpinner />
                The note is being saved.
            </div>
        );
    }

    return (
        <TextArea
          title={helpText}
          placeholder={helpText}
          autoSize={{ minRows: 3 }}
          allowClear
          onKeyPress={(event: React.KeyboardEvent<HTMLTextAreaElement>) => handleKeyPress(event, noteStore)}
        />
    );
};

export default observer(NewNote);
