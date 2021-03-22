import getUrlForDeployment from '../../../src/app/user-section/get-url-for-deployment';
import {
    Deployment,
    DeploymentStatus,
} from '../../../src/deployment/deployment-definition';
import JiraStatus from '../../../src/jira/jira-status';
import UserActionDetail from '../../../src/shared/definitions/user-action-detail';

describe('getUrlForDeployment', () => {
    const dummyUserAction: UserActionDetail = {
        name: 'Foobar',
        timestamp: '2020',
        userName: 'foo-bar',
    };

    const getDeploymentForBranchName = (branchName = 'test'): Deployment => ({
        branchName,
        createdBy: dummyUserAction,
        hasNotes: false,
        id: 2,
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
        environmentId: 1,
        jira: {
            status: JiraStatus.Completed,
            url: '',
        },
        properties: {},
        publicURL: '',
        siteLogin: {
            password: '',
            userName: '',
        },
        url: '',
    });

    it('returns a URL with the branchName query parameter set', () => {
        expect(getUrlForDeployment(getDeploymentForBranchName())).toBe(
            '/#?branchName=test',
        );
    });

    it('returns a URL with the branchName query parameter encoded if required', () => {
        const branchName = 'test/TEST-1234 M & G and more?';

        expect(getUrlForDeployment(getDeploymentForBranchName(branchName))).toBe(
            `/#?branchName=${encodeURI(branchName)}`,
        );
    });
});
