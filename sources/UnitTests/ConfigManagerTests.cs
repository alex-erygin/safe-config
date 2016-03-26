using System.Security.Cryptography;
using SafeConfig;
using Xunit;

namespace UnitTests
{
    public class ConfigManagerTests
    {
        [Fact]
        private void AtFolder_ReturnsSelf()
        {
            var configManager = new ConfigManager()
                .WithLocalMachineScope();
            Assert.IsType<ConfigManager>(
                configManager.AtFolder("2ED1FA2A-62B3-46E4-BB02-24008FA4373A"));
        }

        [Theory]
        [InlineData("sql-connection", "Server=myServerAddress;Database=myDataBase;Uid=myUsername;Pwd=myPassword;",
            DataProtectionScope.CurrentUser)]
        [InlineData("password", "qwerty", DataProtectionScope.LocalMachine)]
        [InlineData("id", "B9D46109-B6F7-45FD-8DE4-98E8975D6E6F", DataProtectionScope.CurrentUser)]
        private void Set_SomeKeysAndValues_GetAndEqual(string key, object value, DataProtectionScope scope)
        {
            var result = new ConfigManager()
                .WithScope(scope)
                .Set(key, value)
                .Get<string>(key);

            Assert.Equal(result, value);
        }

        [Theory]
        [InlineData("sql-connection", "Server=myServerAddress;Database=myDataBase;Uid=myUsername;Pwd=myPassword;")]
        private void Save_Load_ValuesAreSame(string key, object value)
        {
            var configManager = new ConfigManager()
                .WithLocalMachineScope()
                .Set(key, value)
                .AtApplicationFolder()
                .Save();

            var loadedValue = configManager
                .Load()
                .Get<string>(key);

            Assert.Equal(value, loadedValue);
        }

        [Fact]
        private void WithEnthropy_SaveValueWithEntropy_LoadWithoutEntropy_GotException()
        {
            Assert.Throws<SafeConfigException>(() =>
            {
                var configManager = new ConfigManager()
                    .WithEntropy(new byte[] {1, 2, 3, 4, 5, 6})
                    .Set("MyAge", 29)
                    .Save();

                configManager = new ConfigManager()
                    .Load();
            });
        }


        [Fact]
        private void WithEntropy_SaveWithEntropy_LoadWithEntropy_LoadedValueIsSame()
        {
            var entropy = new byte[] {1, 2, 3, 4, 5, 6};
            var configManager = new ConfigManager()
                .AtApplicationFolder()
                .WithEntropy(entropy)
                .Set("MyAge", 29)
                .Save();

            configManager = new ConfigManager()
                .AtApplicationFolder()
                .WithEntropy(entropy)
                .Load();

            var myAge = configManager.Get<int>("MyAge");

            Assert.Equal(29, myAge);
        }
    }
}