using Nelibur.Sword.DataStructures;
using SafeConfig;
using Xunit;

namespace UnitTests
{
	public class ScannerTests
	{
		[Fact]
		void Scan_ReturnsOption()
		{
			var scanner = new Scanner();
			Assert.IsType<Option<Scanner>>(scanner.AtFolder("2ED1FA2A-62B3-46E4-BB02-24008FA4373A"));
		}

		[Fact]
		void Scan_NotExistingFolder_EmptyOption()
		{
			var scanner = new Scanner();
			var result = scanner.AtFolder("31032EB7-F965-4FEC-9E51-D58F896AE482");
			Assert.Equal(Option<Scanner>.Empty, result);
		}

		[Fact]
		void Scan_ExistingDirectoryWithoutRequiredContent_EmptyOption()
		{
			var scanner = new Scanner();
			var result = scanner.AtFolder(@"data\empty\");
			Assert.Equal(Option<Scanner>.Empty, result);
		}

		[Fact]
		void Scan_ExistingDirectoryWithRequiredContent_NotEmpty()
		{
			var scanner = new Scanner();
			var result = scanner.AtFolder(@"data\with-settings\");
			Assert.NotEqual(Option<Scanner>.Empty, result);
		}
	}
}