import { makeAutoObservable } from 'mobx';
import { deleteJSON, getJSON } from '../../utils/io';
import { getMentionsApiUrl } from '../../utils/urls';
import { Mention } from './mention-definitions';

class MentionStore {
  mentions: Mention[] = [];

  isLoading = false;

  hasFailed = false;

  isSaving = false;

  constructor() {
      makeAutoObservable(this);
      this.load();
  }

  load(): void {
      this.isLoading = true;

      getJSON<Mention[]>(
          getMentionsApiUrl().getURL(),
          (fetchedMentions: Mention[] | null) => {
              this.mentions = fetchedMentions ?? [];
              this.isLoading = false;
          },
          () => {
              this.isLoading = false;
              this.hasFailed = true;
          },
      );
  }

  acknowledge(mentionId: number): void {
      const url = getMentionsApiUrl().appendPath(mentionId).getURL();

      deleteJSON(url, null)
          .then(() => {
              this.load();
          })
          .catch(() => {
              console.error('Error');
          });
  }
}

export default MentionStore;
