class URLBuilder {
    private currentUrl = '';

    constructor(urlBase: string) {
        this.currentUrl = urlBase;
    }

    appendPath(path: string | number | boolean): URLBuilder {
        this.currentUrl += `/${path.toString()}`;

        return this;
    }

    getURL(): string {
        return this.currentUrl;
    }
}

export default URLBuilder;
