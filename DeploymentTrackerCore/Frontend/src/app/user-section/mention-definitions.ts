import UserActionDetail from '../../shared/definitions/user-action-detail';

interface Mention {
  referencedEntity: string;
  createdBy: UserActionDetail;
}

export { Mention };
