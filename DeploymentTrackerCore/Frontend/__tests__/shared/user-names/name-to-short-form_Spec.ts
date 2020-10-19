import nameToShortForm from '../../../src/shared/user-names/name-to-short-form';

describe('nameToShortForm', () => {
    const assertShortFormForName = (name: string, expectedShortForm: string) => describe(`for '${name}'`, () => {
        it(`should return the short form '${expectedShortForm}'`, () => {
            expect(nameToShortForm(name)).toBe(expectedShortForm);
        });
    });

    assertShortFormForName('', '');
    assertShortFormForName('Pramod Dematagoda', 'PD');
    assertShortFormForName('Pramod', 'P');
    assertShortFormForName('Pramod Maddy Dematagoda', 'PD');
    assertShortFormForName('pramod dematagoda', 'PD');
});
