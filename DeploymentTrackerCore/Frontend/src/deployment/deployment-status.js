export const Running = 'RUNNING';
export const Destroyed = 'DESTROYED';

export const statusIsRunning = (status) => status === Running;
export const statusIsDestroyed = (status) => status === Destroyed;

export const deploymentIsRunning = ({ status }) => statusIsRunning(status);
export const deploymentIsDestroyed = ({ status }) => statusIsDestroyed(status);