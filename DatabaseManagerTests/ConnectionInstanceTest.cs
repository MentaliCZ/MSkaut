using DatabaseManager;
using UserManager;

namespace DatabaseManagerTests
{
    public class ConnectionInstanceTest
    {
        [Fact]
        public async void InsertNewUser()
        {
            ConnectionInstance instance = await ConnectionInstance.CreateInstance();
            Assert.True(await DBUserRole.CreateUserRole("testRole", "", 0, instance.GetClient()));
        }
    }
}