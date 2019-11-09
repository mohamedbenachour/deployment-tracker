export const getPageData = () => {
    if (_PAGE_DATA) {
        return Object.assign({}, _PAGE_DATA);
    }

    throw new Error("Page data not available");
};