export const getPageData = () => {
    if (_PAGE_DATA) {
        return { ..._PAGE_DATA };
    }

    throw new Error('Page data not available');
};
