import { makeAutoObservable } from 'mobx';
import { getJSON, postJSON } from '../../utils/io';
import { DeploymentNote } from '../default-state';

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
          `/api/deployment/${this.deploymentId}/note`,
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
      const url = `/api/deployment/${this.deploymentId}/note`;

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
}

export default NoteStore;
