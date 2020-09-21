/* eslint-disable no-new */
/* eslint-disable class-methods-use-this */

class Notifier {
    constructor() {
        if (Notification.permission !== 'granted') {
            // eslint-disable-next-line @typescript-eslint/no-floating-promises
            Notification.requestPermission();
        }
    }

    notify(title: string, message: string): void {
        if (Notification.permission === 'granted') {
            new Notification(title, {
                body: message,
            });
        }
    }
}

export default Notifier;
