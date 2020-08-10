const getCurrentParameters = (): URLSearchParams => {
    const parameters = new URLSearchParams(window.location.search);

    return parameters;
};

export default getCurrentParameters;
