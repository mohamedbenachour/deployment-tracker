import ReactDom from 'react-dom';

const boostrapToPage = (
    component: React.DOMElement<React.DOMAttributes<Element>, Element>,
): void => {
    const applicationStart = () => {
        ReactDom.render(component, document.querySelector('.app-mount-point'));
    };

    document.addEventListener('DOMContentLoaded', applicationStart, {
        once: true,
    });
};

export default boostrapToPage;
