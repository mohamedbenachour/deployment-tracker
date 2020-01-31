import reducer from '../../src/app/reducer';
import { sectionChanged } from '../../src/app/actions';

describe('app reducer', () => {
    describe('default state', () => {
        const defaultState = reducer(undefined, { action: '___' });

        it('has the expected section set', () => {
            expect(defaultState.section).toBe('deployments');
        });

        describe('when the app section has changed', () => {
            const newSection = 'say-what?';
            const newSectionState = reducer(defaultState, sectionChanged(newSection));

            it('has the new section set', () => {
                expect(newSectionState.section).toBe(newSection);
            });
        });
    });
});
