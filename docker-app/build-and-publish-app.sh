#! /bin/sh

set -eux

rm -rf 'built-app'

cd ../DeploymentTrackerCore/Frontend

echo 'Building the tracker frontend assets'
npm run build

cd ../..

echo 'Publishing the final app'
dotnet publish DeploymentTrackerCore/ -o 'docker-app/built-app'