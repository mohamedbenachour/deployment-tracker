/*
 * This file is part of Deployment Tracker.
 * 
 * Deployment Tracker is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * Deployment Tracker is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with Deployment Tracker. If not, see <https://www.gnu.org/licenses/>.
 */

using DeploymentTrackerCore.Shared;

namespace DeploymentTrackerCore.Services.UserEntityLinks {
    public class LinkTypes : Enumeration {
        public static readonly LinkTypes Deployment = new LinkTypes(1, "Deployment");
        public static readonly LinkTypes DeploymentNote = new LinkTypes(2, "DeploymentNote");

        private LinkTypes(int id, string name) : base(id, name) {

        }
    }
}