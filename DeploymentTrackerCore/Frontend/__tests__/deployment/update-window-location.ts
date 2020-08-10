import { Filters } from '../../src/deployment/default-state';
import updateWindowLocation from '../../src/deployment/update-window-location';

describe('updateWindowLocation', () => {
    describe('when only the status is specified', () => {
        const filters: Filters = {
            status: 'completed',
            branchName: '',
            type: null,
        };

        it('sets the expected search for the URL', () => {
            updateWindowLocation(filters);

            expect(window.location.search).toBe('?status=completed');
        });
    });

    describe('when the branch name and type is specified', () => {
        const filters: Filters = {
            status: 'torndown',
            branchName: 'test/123',
            type: 'test',
        };

        it('only the status is included in the URL', () => {
            updateWindowLocation(filters);

            expect(window.location.search).toBe('?status=torndown');
        });
    });
});
