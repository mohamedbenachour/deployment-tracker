import { DateTime, Settings } from 'luxon';

export const FormatAsLocalDateTimeString = (serverTimestamp) => {
    const serverDateTime = DateTime.fromISO(serverTimestamp, {
        zone: 'UTC',
    });

    const localDateTime = serverDateTime.setZone(Settings.defaultZone);

    return `${localDateTime.toLocaleString(DateTime.DATE_FULL)} at ${localDateTime.toLocaleString(DateTime.TIME_SIMPLE)}`;
};
