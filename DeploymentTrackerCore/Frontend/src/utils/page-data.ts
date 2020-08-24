/* eslint-disable no-underscore-dangle */

Object.freeze(_PAGE_DATA);

// eslint-disable-next-line import/prefer-default-export
export const getPageData = (): PageData => {
    if (_PAGE_DATA) {
        return _PAGE_DATA;
    }

    throw new Error('Page data not available');
};
