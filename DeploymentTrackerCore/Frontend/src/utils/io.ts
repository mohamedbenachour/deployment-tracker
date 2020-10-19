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

function postJSON<T, O>(
    URL: string,
    object: T,
    onSuccess: (response: O) => void,
    onFailure: (error: any) => void,
) {
    fetch(URL, {
        method: 'POST',
        body: JSON.stringify(object),
        headers: new Headers({
            'Content-Type': 'application/json',
            RequestVerificationToken: getCsrfToken() ?? '',
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

const getJSONPromise = <T>(url: string): Promise<T | null> => new Promise<T>((resolve, reject) => {
    get(url)
        .then((response) => {
            if (response.ok) {
                if (response.status === 204) {
                    resolve();
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

const getJSON = <T>(
    url: string,
    onSuccess: (response: T | null) => void,
    onFailure: (error: any) => void,
) => {
    getJSONPromise<T>(url).then(onSuccess).catch(onFailure);
};

export { postJSON, getJSON, getJSONPromise };
