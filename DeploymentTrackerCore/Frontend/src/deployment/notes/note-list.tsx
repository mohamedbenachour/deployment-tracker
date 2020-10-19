import React, { useState, useEffect } from 'react';

import withStyles from 'react-jss';
import { DeploymentNote } from '../default-state';
import NoteEntry from './note-entry';
import { getJSON } from '../../utils/io';
import SubtleSpinner from '../../shared/interactivity/subtle-spinner';

const containerWidth = 200;
const containerMaxHeight = 85;

const styles = {
    container: {
        display: 'flex',
        flexDirection: 'column',
        minWidth: containerWidth,
        maxWidth: containerWidth,
        minHeight: containerMaxHeight,
        maxHeight: containerMaxHeight,
        overflowY: 'auto',
        scrollbarWidth: 'thin',
    },
};

interface NoteListClasses {
  container: string;
}

interface NoteListProps {
  deploymentId: number;
  classes: NoteListClasses;
}

const renderNotes = (notes: DeploymentNote[]): JSX.Element => {
    if (notes.length === 0) {
        return <div>No notes</div>;
    }

    const notesInOrder = notes.sort(
        (noteA: DeploymentNote, noteB: DeploymentNote): number => noteB.id - noteA.id,
    );

    return (
        <>
            {notesInOrder.map((note: DeploymentNote) => (
                <NoteEntry key={note.id} note={note} />
            ))}
        </>
    );
};

const renderListContent = (
    notes: DeploymentNote[],
    isLoading: boolean,
): JSX.Element => {
    if (isLoading) {
        return <SubtleSpinner />;
    }

    return renderNotes(notes);
};

const NoteList = ({
    deploymentId,
    classes: { container },
}: NoteListProps): JSX.Element => {
    const [isLoading, setIsLoading] = useState(true);
    const [notes, setNotes] = useState<DeploymentNote[]>([]);

    useEffect(() => {
        if (isLoading) {
            getJSON<DeploymentNote[]>(
                `/api/deployment/${deploymentId}/note`,
                (fetchedNotes: DeploymentNote[] | null) => {
                    setNotes(fetchedNotes ?? []);
                    setIsLoading(false);
                },
                () => console.error('WOOOT??'),
            );
        }
    });

    return <div className={container}>{renderListContent(notes, isLoading)}</div>;
};

export default withStyles(styles)(NoteList);
