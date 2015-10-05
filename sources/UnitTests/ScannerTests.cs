using System;
using Nelibur.Sword.DataStructures;
using Nelibur.Sword.Extensions;
using SafeConfig;
using Xunit;

namespace UnitTests
{
	public class ScannerTests
	{
		[Fact]
		void AtFolder_ReturnsOption()
		{
			var scanner = new Scanner();
			Assert.IsType<Option<Scanner>>(
				scanner.AtFolder("2ED1FA2A-62B3-46E4-BB02-24008FA4373A"));
		}

		[Fact]
		void AtFolder_NotExistingFolder_EmptyOption()
		{
			var scanner = new Scanner();
			var result = scanner.AtFolder("31032EB7-F965-4FEC-9E51-D58F896AE482");
			Assert.Equal(Option<Scanner>.Empty, result);
		}

		[Fact]
		void AtFolder_ExistingDirectoryWithoutRequiredContent_EmptyOption()
		{
			var scanner = new Scanner();
			var result = scanner.AtFolder(@"data\empty\");
			Assert.Equal(Option<Scanner>.Empty, result);
		}

		[Fact]
		void AtFolder_ExistingDirectoryWithRequiredContent_NotEmpty()
		{
			var scanner = new Scanner();
			var result = scanner.AtFolder(@"data\with-settings\")
				.Do(x => x.Load());
			Assert.NotEqual(Option<Scanner>.Empty, result);
		}

		[Theory]
		[InlineData("sql-connection", "Server=myServerAddress;Database=myDataBase;Uid=myUsername;Pwd=myPassword;")]
		[InlineData("password","qwerty")]
		[InlineData("id", "B9D46109-B6F7-45FD-8DE4-98E8975D6E6F")]
		void Set_SomeKeysAndValues_GetAndEqual(string key, object value)
		{
			string result = null;
			var scanner = new Scanner()
				.Set(key, value)
				.Do(x=>result = x.Get<string>(key).Value);

			Assert.Equal(result, value);
		}
	}
}