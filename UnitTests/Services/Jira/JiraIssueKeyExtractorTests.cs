using System.Collections.Generic;

using DeploymentTrackerCore.Services.Jira;

using FluentAssertions;

using NUnit.Framework;

namespace UnitTests.Services.Jira {
    [TestFixture]
    public class JiraIssueKeyExtractorTests {
        [TestCase("DEV", "DEV-12345", "DEV-12345")]
        [TestCase("DEV", "DEV-12345", "task/DEV-12345-foo-bar-isssh")]
        [TestCase("DEV", "DEV-12345", "task/dev-12345-foo-bar-isssh")]
        [TestCase("DEV", "DEV-12345", "DEV-24/DEV-12345-foo-bar-isssh")]
        [TestCase("SKU", "SKU-24566", "task/DEV-12345-foo-bar-isssh-SKU-24566")]
        public void ASingleProjectKeySpecifiedWorks(string projectKey, string issueKey, string nameToExtractFrom) {
            new JiraIssueKeyExtractor(new HashSet<string> { projectKey }).Extract(nameToExtractFrom).Should().Be(issueKey);
        }

        [TestCase("DEV", "SKU-12345")]
        [TestCase("DEV", "task/dev123-format-ain'tright")]
        public void AMissingProjectKeyInTheNameReturnsNull(string projectKey, string nameToExtractFrom) {
            new JiraIssueKeyExtractor(new HashSet<string> { projectKey }).Extract(nameToExtractFrom).Should().BeNull();
        }

        public static readonly MultipleProjectKeyTestCase[] MultipleProjectKeyTestCases = new MultipleProjectKeyTestCase[] {
            new MultipleProjectKeyTestCase {
            ProjectKeys = new string[] { "DEV", "SKU", "FIB" },
            NameToExtractFrom = "DEV-1234",
            ExpectedResult = "DEV-1234"
            },

            new MultipleProjectKeyTestCase {
            ProjectKeys = new string[] { "DEV", "SKU", "FIB" },
            NameToExtractFrom = "task/FIB-12-Some-task-or-another",
            ExpectedResult = "FIB-12"
            },

            new MultipleProjectKeyTestCase {
            ProjectKeys = new string[] { "DEV", "SKU", "FIB" },
            NameToExtractFrom = "SKU-1/HAD-12-things-FIB-13-real",
            ExpectedResult = "FIB-13"
            },

            new MultipleProjectKeyTestCase {
            ProjectKeys = new string[] { "DEV", "SKU", "FIB" },
            NameToExtractFrom = "task/sku-1300-real-thingy",
            ExpectedResult = "SKU-1300"
            },
        };

        [TestCaseSource(nameof(MultipleProjectKeyTestCases))]
        public void MultipleProjectKeysCanBeSpecifiedAndUsedToExtractFrom(MultipleProjectKeyTestCase testCase) {
            new JiraIssueKeyExtractor(new HashSet<string>(testCase.ProjectKeys))
                .Extract(testCase.NameToExtractFrom)
                .Should().Be(testCase.ExpectedResult);
        }

        public class MultipleProjectKeyTestCase {
            internal string[] ProjectKeys { get; set; }
            public string NameToExtractFrom { get; set; }
            public string ExpectedResult { get; set; }
        }
    }
}