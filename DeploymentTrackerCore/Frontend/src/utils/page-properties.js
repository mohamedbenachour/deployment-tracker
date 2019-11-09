const getPageProperties = () => {
    if (_PAGE_PROPERTIES) {
        return Object.assign({}, _PAGE_PROPERTIES);
    }

    throw new Error("Page properties not available");
};

export const getCsrfToken = () => getPageProperties().csrfToken;