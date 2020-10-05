import ReactDom from 'react-dom';

const boostrapToPage = (component: React.ReactElement): void => {
    const applicationStart = () => {
        ReactDom.render(component, document.querySelector('.app-mount-point'));
    };

    document.addEventListener('DOMContentLoaded', applicationStart, {
        once: true,
    });
};

export default boostrapToPage;
