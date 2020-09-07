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

using System;
using System.Reflection;
using System.Web;

namespace DeploymentTrackerCore.Services.DeploymentManagement.TypeBased {
    public class ClassPropertyBasedStringTemplater<ClassType> {
        public string Template(ClassType objectInstance, string templateString) {
            var currentString = templateString;

            foreach (var property in ClassProperties) {
                currentString = TemplateProperty(property, objectInstance, currentString);
            }

            return currentString;
        }

        private PropertyInfo[] ClassProperties => typeof(ClassType).GetProperties();

        private string TemplateProperty(PropertyInfo property, ClassType objectInstance, string templateString) {
            if (CanTemplateProperty(property)) {
                var propertyValue = property.GetValue(objectInstance) ?? string.Empty;

                return templateString.Replace($"{{{{{property.Name}}}}}", HttpUtility.UrlEncode(propertyValue.ToString()), StringComparison.OrdinalIgnoreCase);
            }

            return templateString;
        }

        private bool CanTemplateProperty(PropertyInfo property) => property.PropertyType.IsPrimitive || property.PropertyType == typeof(string);
    }
}