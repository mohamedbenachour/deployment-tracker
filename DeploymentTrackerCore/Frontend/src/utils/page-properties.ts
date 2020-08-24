/* eslint-disable no-underscore-dangle */

Object.freeze(_PAGE_PROPERTIES);

const getPageProperties = (): PageProperties => {
    if (_PAGE_PROPERTIES) {
        return _PAGE_PROPERTIES;
    }

    throw new Error('Page properties not available');
};

// eslint-disable-next-line import/prefer-default-export
export const getCsrfToken = (): string | undefined => getPageProperties().csrfToken;
