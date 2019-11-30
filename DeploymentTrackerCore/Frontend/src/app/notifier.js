class Notifier {
    constructor() {
        if (Notification.permission !== 'granted') {
            Notification.requestPermission();
        }
    }

    notify(title, message) {
        if (Notification.permission === 'granted') {
            new Notification(title, {
                body: message,
            });
        }
    }
}

export default Notifier;