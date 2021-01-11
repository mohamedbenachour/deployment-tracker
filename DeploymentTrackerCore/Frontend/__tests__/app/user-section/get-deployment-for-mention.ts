import getDeploymentForMention from '../../../src/app/user-section/get-deployment-for-mention';
import { Mention } from '../../../src/app/user-section/mention-definitions';

describe('getDeploymentForMention', () => {
    const defaultDeploymentId = 23;
    const defaultDeploymentNoteId = 123;

    const defaultEntityReference = `Deployment::${defaultDeploymentId}::DeploymentNote::${defaultDeploymentNoteId}`;

    const getMention = (reference: string): Mention => ({
        id: 1,
        referencedEntity: reference,
        createdBy: {
            name: 'Blahh',
            timestamp: 'Date',
            userName: 'bla.hh',
        },
    });

    it('returns null for an empty list of deployments', () => {
        expect(
            getDeploymentForMention([], getMention(defaultEntityReference)),
        ).toBeNull();
    });
});
