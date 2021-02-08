import getCurrentParameters from '../../../src/utils/window-location/get-current-parameters';

const serialise = (searchParams: URLSearchParams): string => JSON.stringify(searchParams);

describe('getCurrentParameters', () => {
    describe('when no current parameters are present', () => {
        window.location.search = '';

        it('returns an empty list of parameters', () => {
            expect(serialise(getCurrentParameters())).toEqual(
                serialise(new URLSearchParams()),
            );
        });
    });

    describe('when a parameter is present', () => {
        const parameterName = 'whyt';
        const parameterValue = 'wot';

        window.history.pushState({}, '', `/?${parameterName}=${parameterValue}`);

        it('the parameter values are returned', () => {
            const expectedSearchParams = new URLSearchParams();

            expectedSearchParams.append(parameterName, parameterValue);

            expect(serialise(getCurrentParameters())).toEqual(
                serialise(expectedSearchParams),
            );
        });
    });

    describe('when a URL encoded parameter is present', () => {
        const parameterName = 'whyt';
        const parameterValue = 'test/123';

        window.history.pushState(
            {},
            '',
            `/?${parameterName}=${encodeURI(parameterValue)}`,
        );

        it('the parameter values are returned', () => {
            const expectedSearchParams = new URLSearchParams();

            expectedSearchParams.append(parameterName, parameterValue);

            expect(serialise(getCurrentParameters())).toEqual(
                serialise(expectedSearchParams),
            );
        });
    });
});
