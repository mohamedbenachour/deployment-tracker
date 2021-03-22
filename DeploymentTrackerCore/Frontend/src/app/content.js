import { Route, Routes } from 'react-router-dom';

import EnvironmentList from '../environment/connected-environment-list';
import DeploymentList from '../deployment/connected-deployment-list';
import ApplicationRoute from './application-route';

const Content = () => (
    <Routes>
        <Route path={ApplicationRoute.Environments}>
            <EnvironmentList />
        </Route>
        <Route path={ApplicationRoute.Deployments}>
            <DeploymentList />
        </Route>
    </Routes>
);

export default Content;
