/**
 * Copyright (C) 2019  Pramod Dematagoda <pmdematagoda@mykolab.ch>
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */

import { getCsrfToken } from './page-properties';

const getHeadersWithCsrfToken = (
    additionalHeaders: Record<string, string>,
): Headers => new Headers({
    ...additionalHeaders,
    RequestVerificationToken: getCsrfToken() ?? '',
});

const handlePromiseAsJSON = <T>(
    originalPromise: Promise<Response>,
): Promise<T | null> => new Promise<T | null>((resolve, reject) => {
    originalPromise
        .then((response) => {
            if (response.ok) {
                if (response.status === 204) {
                    resolve(null);
                } else {
                    response
                        .json()
                        .then(resolve)
                        .catch(() => console.error('Failed to convert to JSON'));
                }
            } else {
                reject();
            }
        })
        .catch((failure) => {
            reject(failure);
        });
});

function postJSON<T, O>(
    URL: string,
    object: T,
    onSuccess: (response: O) => void,
    onFailure: (error: any) => void,
): void {
    fetch(URL, {
        method: 'POST',
        body: JSON.stringify(object),
        headers: getHeadersWithCsrfToken({
            'Content-Type': 'application/json',
        }),
    })
        .then((response) => {
            if (response.ok) {
                response.json().then(onSuccess);
            } else {
                response.text().then((possibleJSON) => {
                    try {
                        onFailure(JSON.parse(possibleJSON));
                    } catch {
                        onFailure(possibleJSON);
                    }
                });
            }
        })
        .catch((failure) => {
            onFailure(failure);
        });
}

const get = (url: string): Promise<Response> => fetch(url);

const getJSONPromise = <T>(url: string): Promise<T | null> => handlePromiseAsJSON(get(url));

const getJSON = <T>(
    url: string,
    onSuccess: (response: T | null) => void,
    onFailure: (error: any) => void,
): void => {
    getJSONPromise<T>(url).then(onSuccess).catch(onFailure);
};

const deleteJSON = <T>(url: string, payload?: unknown): Promise<T | null> => {
    const body = payload !== undefined ? JSON.stringify(payload) : null;

    const ioPromise = fetch(url, {
        method: 'DELETE',
        body,
        headers: getHeadersWithCsrfToken({
            'Content-Type': 'application/json',
        }),
    });

    return handlePromiseAsJSON(ioPromise);
};

export {
    postJSON, getJSON, getJSONPromise, deleteJSON,
};
