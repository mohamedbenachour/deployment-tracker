import { makeAutoObservable } from 'mobx';
import { deleteJSON, getJSON, postJSON } from '../../utils/io';
import URLBuilder from '../../utils/url-builder';
import { getDeploymentApiUrl } from '../../utils/urls';
import { DeploymentNote } from '../default-state';

const getDeploymentNoteApiUrl = (deploymentId: number): URLBuilder => getDeploymentApiUrl().appendPath(deploymentId).appendPath('note');

class NoteStore {
  notes: DeploymentNote[] = [];

  isLoading = false;

  hasFailed = false;

  isSaving = false;

  private deploymentId: number;

  constructor(deploymentId: number) {
      this.deploymentId = deploymentId;
      makeAutoObservable(this);
      this.load();
  }

  load(): void {
      this.isLoading = true;

      getJSON<DeploymentNote[]>(
          getDeploymentNoteApiUrl(this.deploymentId).getURL(),
          (fetchedNotes: DeploymentNote[] | null) => {
              this.notes = fetchedNotes ?? [];
              this.isLoading = false;
          },
          () => {
              this.isLoading = false;
              this.hasFailed = true;
          },
      );
  }

  save(content: string): void {
      const note = {
          content,
      };
      const url = getDeploymentNoteApiUrl(this.deploymentId).getURL();

      this.isSaving = true;

      postJSON(
          url,
          note,
          () => {
              this.isSaving = false;
              this.load();
          },
          () => {
              this.isSaving = false;
          },
      );
  }

  delete(noteId: number): void {
      const url = getDeploymentNoteApiUrl(this.deploymentId)
          .appendPath(noteId)
          .getURL();

      deleteJSON(url, null)
          .then(() => {
              this.load();
          })
          .catch(() => {
              console.error('Error');
          });
  }
}

export default NoteStore;
