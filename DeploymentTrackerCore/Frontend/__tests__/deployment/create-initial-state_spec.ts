import createInitialState from '../../src/deployment/create-initial-state';
import createDefaultState from '../../src/deployment/default-state';

describe('createInitialState', () => {
    beforeEach((): void => {
        delete (window as any).location;

        window = Object.create(window);
        window.location = <Location>(<unknown> new URL('http://localhost/'));
    });

    const setSearchInUrl = (search: string): void => {
        window.location.hash = `?${search}`;
    };

    describe('when no parameters have been defined in the URL', () => {
        it('creates the default state', () => {
            expect(createInitialState()).toEqual(createDefaultState());
        });

        it('uses the default filters', () => {
            expect(createInitialState().filters).toEqual(
                createDefaultState().filters,
            );
        });
    });

    describe('when a status has been defined', () => {
        const status = 'completed';

        it('should set the expected status in the initial state', () => {
            setSearchInUrl(`status=${status}`);

            expect(createInitialState().filters.status).toBe(status);
        });
    });

    describe('when a status has been defined with different casing', () => {
        const status = 'CoMPLeted';

        it('should set the expected status in the initial state', () => {
            setSearchInUrl(`status=${status}`);

            expect(createInitialState().filters.status).toBe(status.toLowerCase());
        });
    });

    describe('when an invalid status has been defined', () => {
        const status = 'not-a-status';

        it('should set the default status in the initial state', () => {
            setSearchInUrl(`status=${status}`);

            expect(createInitialState().filters.status).toBe(
                createDefaultState().filters.status,
            );
        });
    });

    describe('when a value for the onlyMine filter has been defined', () => {
        it('should set the expected onlyMine value in the initial state', () => {
            setSearchInUrl('onlyMine=true');

            expect(createInitialState().filters.onlyMine).toBe(true);
        });
    });

    describe('when an invalue for the onlyMine filter has been defined', () => {
        it('should set the default onlyMine value in the initial state', () => {
            setSearchInUrl('onlyMine=blal');

            expect(createInitialState().filters.onlyMine).toBe(false);
        });
    });
});
