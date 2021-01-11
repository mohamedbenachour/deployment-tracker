import UserActionDetail from '../../shared/definitions/user-action-detail';

interface Mention {
    referencedEntity: string;
    createdBy: UserActionDetail;
    id: number;
}

export { Mention };
