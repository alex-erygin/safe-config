using System.Security.Cryptography;
using SafeConfig;
using Xunit;

namespace UnitTests
{
	public class ConfigManagerTests
	{
		[Fact]
		private void AtFolder_ReturnsOption()
		{
			var configManager = new ConfigManager();
			Assert.IsType<ConfigManager>(
				configManager.AtFolder("2ED1FA2A-62B3-46E4-BB02-24008FA4373A"));
		}

		[Theory]
		[InlineData("sql-connection", "Server=myServerAddress;Database=myDataBase;Uid=myUsername;Pwd=myPassword;", DataProtectionScope.CurrentUser)]
		[InlineData("password","qwerty", DataProtectionScope.LocalMachine)]
		[InlineData("id", "B9D46109-B6F7-45FD-8DE4-98E8975D6E6F", DataProtectionScope.CurrentUser)]
		void Set_SomeKeysAndValues_GetAndEqual(string key, object value, DataProtectionScope scope)
		{
			string result = new ConfigManager()
				.WithScope(scope)
				.Set(key, value)
				.Get<string>(key);

			Assert.Equal(result, value);
		}

		[Theory]
		[InlineData("sql-connection", "Server=myServerAddress;Database=myDataBase;Uid=myUsername;Pwd=myPassword;")]
		void Save_Load_ValuesAreSame(string key, object value)
		{
			var configManager = new ConfigManager()
				.WithScope(DataProtectionScope.CurrentUser)
				.Set(key, value)
				.AtApplicationFolder()
				.Save();

			var loadedValue = configManager
				.Load()
				.Get<string>(key);

			Assert.Equal(value, loadedValue);
		}
	}
}