import getDeploymentForMention from '../../../src/app/user-section/get-deployment-for-mention';
import { Mention } from '../../../src/app/user-section/mention-definitions';
import {
    Deployment,
    DeploymentStatus,
} from '../../../src/deployment/deployment-definition';
import UserActionDetail from '../../../src/shared/definitions/user-action-detail';

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

    describe('when a deployment is present', () => {
        const dummyUserAction: UserActionDetail = {
            name: 'Foobar',
            timestamp: '2020',
            userName: 'foo-bar',
        };

        const matchingDeployment: Deployment = {
            branchName: 'foo-bar',
            createdBy: dummyUserAction,
            hasNotes: false,
            id: defaultDeploymentId,
            managementUrls: {
                deploymentTriggerUrl: null,
            },
            modifiedBy: dummyUserAction,
            siteName: 'test',
            status: DeploymentStatus.Destroyed,
            teardownUrl: 'test',
            type: {
                id: 1,
                name: 'test',
            },
        };

        it('returns the expected deployment for a mention', () => {
            expect(
                getDeploymentForMention(
                    [matchingDeployment],
                    getMention(defaultEntityReference),
                ),
            ).toBe(matchingDeployment);
        });

        it('it does not return a match if the mention references a non-existent deployment', () => {
            const reference = `Deployment::266::DeploymentNote::${defaultDeploymentNoteId}`;

            expect(
                getDeploymentForMention([matchingDeployment], getMention(reference)),
            ).toBeNull();
        });
    });
});
