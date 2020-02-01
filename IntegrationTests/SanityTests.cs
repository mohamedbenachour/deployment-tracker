using System.Threading.Tasks;
using IntegrationTests.EnvironmentSetup;
using NUnit.Framework;

namespace IntegrationTests
{
    [TestFixture]
    public class SanityTests
    {

        [TestCase("/")]
        [TestCase("/Account/Login")]
        [TestCase("/Account/Logout")]
        public async Task Pages(string url)
        {
            var client = TestEnvironment.ClientFactory.CreateClient();

            var response = await client.GetAsync(url);

            response.EnsureSuccessStatusCode();
        }
    }
}