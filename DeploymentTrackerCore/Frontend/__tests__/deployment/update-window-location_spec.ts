import { Filters } from '../../src/deployment/default-state';
import updateWindowLocation from '../../src/deployment/update-window-location';

describe('updateWindowLocation', () => {
    describe('when only the status is specified', () => {
        const filters: Filters = {
            status: 'completed',
            branchName: '',
            type: null,
            onlyMine: false,
        };

        it('sets the expected search for the URL', () => {
            updateWindowLocation(filters);

            expect(window.location.hash).toBe('#/?status=completed&onlyMine=false');
        });
    });

    describe('when the branch name is white space', () => {
        const filters: Filters = {
            status: 'completed',
            branchName: '    ',
            type: null,
            onlyMine: false,
        };

        it('does not set the branch name in the URL', () => {
            updateWindowLocation(filters);

            expect(window.location.hash).toBe('#/?status=completed&onlyMine=false');
        });
    });

    describe('when the branch name is specified', () => {
        const filters: Filters = {
            status: 'torndown',
            branchName: 'test-1234',
            type: null,
            onlyMine: false,
        };

        it('the branch name is included in the URL', () => {
            updateWindowLocation(filters);

            expect(window.location.hash).toBe(
                '#/?status=torndown&onlyMine=false&branchName=test-1234',
            );
        });
    });

    describe('when the branch name and type is specified', () => {
        const filters: Filters = {
            status: 'torndown',
            branchName: 'test/123',
            type: 'test',
            onlyMine: false,
        };

        it('only the status is included in the URL', () => {
            updateWindowLocation(filters);

            expect(window.location.hash).toBe(
                '#/?status=torndown&onlyMine=false&branchName=test%2F123',
            );
        });
    });

    describe('when the onleMine filter is true', () => {
        const filters: Filters = {
            status: 'torndown',
            branchName: 'test/123',
            type: 'test',
            onlyMine: true,
        };

        it('sets the expected search for the URL', () => {
            updateWindowLocation(filters);

            expect(window.location.hash).toBe(
                '#/?status=torndown&onlyMine=true&branchName=test%2F123',
            );
        });
    });
});
