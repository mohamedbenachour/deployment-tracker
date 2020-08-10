const SetCurrentParameters = (searchParams: URLSearchParams): void => {
    const newUrl = new URL(window.location.href);
    let searchString = searchParams.toString();

    if (searchString.length !== 0) {
        searchString = `?${searchParams.toString()}`;
    }

    newUrl.search = searchString;

    window.history.pushState(null, '', newUrl.href);
};

export default SetCurrentParameters;
