import {
    Deployment,
    DeploymentStatus,
    UserActionDetail,
} from '../../src/deployment/deployment-definition';
import deploymentIsForCurrentUser from '../../src/deployment/deployment-matchers';
import getCurrentUser from '../../src/utils/current-user';

let mockCurrentUser: User;

jest.mock(
    '../../src/utils/current-user',
    () => function () {
        return mockCurrentUser;
    },
);

const mockedCurrentUser = <jest.Mock<typeof getCurrentUser>>(
  (getCurrentUser as unknown)
);

describe('deployment-matchers', () => {
    const defaultUserName = 'foo';
    const createUserActionDetail = (
        userName: string = defaultUserName,
    ): UserActionDetail => ({
        name: 'Foo Bar',
        userName,
    });

    const createDefaultDeployment = (): Deployment => ({
        branchName: 'test/test-2000',
        status: DeploymentStatus.Running,
        createdBy: createUserActionDetail(),
        modifiedBy: createUserActionDetail(),
        siteName: 'boo',
        teardownUrl: 'https://test',
        managementUrls: {
            deploymentTriggerUrl: 'https://deploy',
        },
    });

    describe('deploymentIsForCurrentUser', () => {
        const setCurrentUser = (user: User): void => {
            mockCurrentUser = user;
        };

        const createDeploymentWithUser = (user: string): Deployment => {
            const deployment = createDefaultDeployment();

            deployment.modifiedBy = createUserActionDetail(user);

            return deployment;
        };

        describe('when a user with a user name and an email specified is present', () => {
            const userName = 'sanath';
            const email = 'sanath@testdeployments.com';

            beforeEach(() => setCurrentUser({
                email,
                userName,
                name: 'Sanath Piyadasa',
            }));

            it('is a match when a deployment that uses the user name is provided', () => {
                const deployment = createDeploymentWithUser(userName);

                expect(deploymentIsForCurrentUser(deployment)).toBeTruthy();
            });

            it('is a match when a deployment that uses the email is provided', () => {
                const deployment = createDeploymentWithUser(email);

                expect(deploymentIsForCurrentUser(deployment)).toBeTruthy();
            });

            it('is not a match when a deployment that uses a different email is provided', () => {
                const deployment = createDeploymentWithUser('test@test.com');

                expect(deploymentIsForCurrentUser(deployment)).toBeFalsy();
            });
        });

        describe('when a user with only a user name is present', () => {
            const userName = 'janaki';

            beforeEach(() => setCurrentUser({
                email: '',
                userName,
                name: 'Janaki Wijayadasa',
            }));

            it('is a match when a deployment that uses the user name is provided', () => {
                const deployment = createDeploymentWithUser(userName);

                expect(deploymentIsForCurrentUser(deployment)).toBeTruthy();
            });

            it('is not a match when a deployment that uses a different username is provided', () => {
                const deployment = createDeploymentWithUser('jan');

                expect(deploymentIsForCurrentUser(deployment)).toBeFalsy();
            });
        });
    });
});
