# safe-config
Simple configuration manager that uses [Data Protection API](https://msdn.microsoft.com/en-us/library/ms229741(v=vs.110).aspx)

```cs
//Save some configuration data at folder data\temp\
var configManager = new ConfigManager()
	.WithOptions(DataProtectionScope.CurrentUser)
	.Set(key, value)
	.AtFolder(@"data\temp\")
	.Save();

	...

//Load configuration data
var loadedValue = new ConfigManager()
	.AtFolder(@"data\temp\")
	.Load()
	.Get<string>(key);
```
