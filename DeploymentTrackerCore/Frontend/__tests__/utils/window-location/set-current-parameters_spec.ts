import SetCurrentParameters from '../../../src/utils/window-location/set-current-parameters';

describe('setCurrentParameters', () => {
    describe('when an empty URLSearchParams is passed in', () => {
        const searchParams = new URLSearchParams();

        it('updates the history', () => {
            SetCurrentParameters(searchParams);

            expect(window.history.length).toBe(2);
        });

        it('sets the expected URL', () => {
            SetCurrentParameters(searchParams);

            expect(window.location.search).toBe('');
        });
    });

    describe('when a populated URLSearchParams is passed in', () => {
        const queryParam = 'test';
        const queryValue = 'foobar';
        const searchParams = new URLSearchParams();

        searchParams.append(queryParam, queryValue);

        it('sets the expected URL', () => {
            SetCurrentParameters(searchParams);

            expect(window.location.search).toBe(`?${queryParam}=${queryValue}`);
        });
    });
});
