import deploymentReducer from '../../src/deployment/reducer';
import { deploymentSearch, deploymentStatusFilterChanged } from '../../src/deployment/actions';

describe('deployment reducer', () => {
    describe('default state', () => {
        const defaultState = deploymentReducer(undefined, { type: '__' });

        it('has the expected state', () => {
            expect(defaultState).toEqual({
                addingADeployment: false,
                saveInProgress: false,
                deploymentBeingAdded: null,
                filters: {
                    branchName: '',
                    status: 'running',
                    type: null,
                },
            });
        });

        describe('when the search term has changed', () => {
            const searchValue = 'test-bt';
            const searchTermState = deploymentReducer(defaultState, deploymentSearch(searchValue));

            it('sets the branch name filter', () => {
                expect(searchTermState.filters.branchName).toBe(searchValue);
            });
        });

        describe('when the search status has changed', () => {
            const statusValue = 'status-y';
            const statusState = deploymentReducer(defaultState, deploymentStatusFilterChanged(statusValue));

            it('sets the status filter', () => {
                expect(statusState.filters.status).toBe(statusValue);
            });
        });
    });
});
