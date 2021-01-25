import React from 'react';
import TextArea from 'antd/lib/input/TextArea';
import { observer } from 'mobx-react-lite';
import SubtleSpinner from '../../shared/interactivity/subtle-spinner';
import NoteStore from './note-store';
import NewNoteInput from './new-note-input';

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

    return <NewNoteInput onSave={(newNote) => noteStore.save(newNote)} />;
};

export default observer(NewNote);
