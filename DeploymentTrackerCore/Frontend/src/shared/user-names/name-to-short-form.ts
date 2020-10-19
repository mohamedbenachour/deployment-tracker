const getFirstLetterForPart = (part: string) => part.charAt(0);

const getShortFormForSplitName = (splitName: string[]): string => {
    const firstName = splitName[0];
    const firstLetter = getFirstLetterForPart(firstName);

    if (splitName.length > 1) {
        const lastName = splitName[splitName.length - 1];

        return `${firstLetter}${getFirstLetterForPart(lastName)}`;
    }

    return firstLetter;
};

const nameToShortForm = (name: string): string => {
    const nameSplitBySpace = name.toUpperCase().split(' ');

    if (nameSplitBySpace.length > 0) {
        return getShortFormForSplitName(nameSplitBySpace);
    }

    return '';
};

export default nameToShortForm;
