import withStyles from 'react-jss';
import { observer } from 'mobx-react-lite';
import { DeploymentNote } from '../default-state';
import NoteEntry from './note-entry';
import SubtleSpinner from '../../shared/interactivity/subtle-spinner';
import NoteStore from './note-store';

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
    noteStore: NoteStore;
    classes: NoteListClasses;
}

const renderNotes = (
    notes: DeploymentNote[],
    noteStore: NoteStore,
): JSX.Element => {
    if (notes.length === 0) {
        return <div>No notes</div>;
    }

    const notesInOrder = notes
        .slice()
        .sort(
            (noteA: DeploymentNote, noteB: DeploymentNote): number => noteB.id - noteA.id,
        );

    return (
        <>
            {notesInOrder.map((note: DeploymentNote) => (
                <NoteEntry
                  key={note.id}
                  note={note}
                  onDelete={() => noteStore.delete(note.id)}
                />
            ))}
        </>
    );
};

const renderListContent = (
    notes: DeploymentNote[],
    isLoading: boolean,
    hasErrored: boolean,
    noteStore: NoteStore,
): JSX.Element => {
    if (hasErrored) {
        return <div>An error occurred</div>;
    }

    if (isLoading) {
        return <SubtleSpinner />;
    }

    return renderNotes(notes, noteStore);
};

const NoteList = ({
    noteStore,
    classes: { container },
}: NoteListProps): JSX.Element => (
    <div className={container}>
        {renderListContent(
            noteStore.notes,
            noteStore.isLoading,
            noteStore.hasFailed,
            noteStore,
        )}
    </div>
);

export default withStyles(styles)(observer(NoteList));
