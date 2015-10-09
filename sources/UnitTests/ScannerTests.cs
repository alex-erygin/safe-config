using SafeConfig;
using Xunit;

namespace UnitTests
{
	public class ScannerTests
	{
		[Fact]
		private void AtFolder_ReturnsOption()
		{
			var scanner = new ConfigManager();
			Assert.IsType<ConfigManager>(
				scanner.AtFolder("2ED1FA2A-62B3-46E4-BB02-24008FA4373A"));
		}

		[Theory]
		[InlineData("sql-connection", "Server=myServerAddress;Database=myDataBase;Uid=myUsername;Pwd=myPassword;")]
		[InlineData("password","qwerty")]
		[InlineData("id", "B9D46109-B6F7-45FD-8DE4-98E8975D6E6F")]
		void Set_SomeKeysAndValues_GetAndEqual(string key, object value)
		{
			string result = new ConfigManager()
				.Set(key, value)
				.Get<string>(key);

			Assert.Equal(result, value);
		}

		[Theory]
		[InlineData("sql-connection", "Server=myServerAddress;Database=myDataBase;Uid=myUsername;Pwd=myPassword;")]
		void Save_Load_ValuesAreSame(string key, object value)
		{
			var scanner = new ConfigManager()
				.Set(key, value)
				.AtFolder(@"data\temp\")
				.Save();

			var loadedValue = new ConfigManager()
				.AtFolder(@"data\temp\")
				.Load()
				.Get<string>(key);

			Assert.Equal(value, loadedValue);
		}
	}
}