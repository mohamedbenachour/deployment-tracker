import React, { useState } from 'react';
import TextArea from 'antd/lib/input/TextArea';
import SubtleSpinner from '../../shared/interactivity/subtle-spinner';
import { postJSON } from '../../utils/io';

interface NewNoteProps {
  deploymentId: number;
}

const isSaveButtonPress = ({
    key,
    ctrlKey,
}: React.KeyboardEvent<HTMLTextAreaElement>) => ctrlKey && key === 'Enter';

const handleKeyPress = (
    event: React.KeyboardEvent<HTMLTextAreaElement>,
    setIsSaving: (isSaving: boolean) => void,
    deploymentId: number,
): void => {
    if (isSaveButtonPress(event)) {
        setIsSaving(true);

        const note = {
            content: event.currentTarget.value,
        };
        const url = `/api/deployment/${deploymentId}/note`;

        postJSON(
            url,
            note,
            () => setIsSaving(false),
            () => console.error('Unable to save'),
        );
    }
};

const NewNote = ({ deploymentId }: NewNoteProps): JSX.Element => {
    const helpText = 'Ctrl+Enter to Save.';
    const [currentContent, setCurrentContent] = useState('');
    const [isSaving, setIsSaving] = useState(false);

    if (isSaving) {
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
          onKeyPress={(event: React.KeyboardEvent<HTMLTextAreaElement>) => handleKeyPress(event, setIsSaving, deploymentId)}
          onChange={({
                target: { value },
            }: React.ChangeEvent<HTMLTextAreaElement>) => {
                setCurrentContent(value);
            }}
        />
    );
};

export default NewNote;
