using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;

namespace SafeConfig
{
	/// <summary>
	/// Secure configuration manager.
	/// </summary>
    public class ConfigManager
    {
		/// <summary>
		/// Configuration key-value storage.
		/// </summary>
	    private Dictionary<string, object> storedValues = new Dictionary<string, object>();

		/// <summary>
		/// Folder, contains config.
		/// </summary>
	    private string configFolder = ".";

		/// <summary>
		/// ConfigManager file name.
		/// </summary>
	    private string defaultSettingsFileName = "settings.saveconfig";

		/// <summary>
		/// Full path to settings file.
		/// </summary>
	    private string SettingsFilePath => Path.Combine(configFolder, defaultSettingsFileName);

		/// <summary>
		/// Data protection scope. <see cref="DataProtectionScope"/>.
		/// </summary>
		private DataProtectionScope dataProtectionScope = DataProtectionScope.CurrentUser;

		/// <summary>
		/// Set working folder to application folder.
		/// </summary>
		/// <returns>This.</returns>
		public ConfigManager AtApplicationFolder()
		{
			string applicationFolder = AppDomain.CurrentDomain.BaseDirectory;
			configFolder = applicationFolder;
			return this;
		}

		/// <summary>
		/// Set working folder.
		/// </summary>
		/// <param name="folder">Working folder.</param>
		/// <returns>This if folder contains any config files or empty.</returns>
	    public ConfigManager AtFolder(string folder)
	    {
		    configFolder = folder;
			if (!Directory.Exists(configFolder))
			{
				Directory.CreateDirectory(configFolder);
			}

		    return this;
	    }

		/// <summary>
		/// Set DataProtectionScope.
		/// </summary>
		/// <param name="scope">DataProtectionScope.</param>
		/// <returns>This.</returns>
		public ConfigManager WithScope(DataProtectionScope scope)
		{
			dataProtectionScope = scope;
			return this;
		}
		
		/// <summary>
		/// Load configuration from file.
		/// </summary>
		/// <returns>This.</returns>
	    public ConfigManager Load()
		{
			if (!File.Exists(SettingsFilePath))
			{
				return this;
			}

			var protectedBuffer = File.ReadAllBytes(SettingsFilePath);
			var unprotectedBuffer = ProtectedData.Unprotect(protectedBuffer, null, dataProtectionScope);

			var binFormatter = new BinaryFormatter();
			using (var mStream = new MemoryStream(unprotectedBuffer))
			{
				storedValues = (Dictionary<string, object>)binFormatter.Deserialize(mStream);
			}

			return this;
	    }

		/// <summary>
		/// Set setting value.
		/// </summary>
		/// <typeparam name="T">Type of value.</typeparam>
		/// <param name="key">Key.</param>
		/// <param name="value">Value.</param>
		/// <returns>This.</returns>
	    public ConfigManager Set<T>(string key, T value)
	    {
		    storedValues[key] = value;
			return this;
		}

		/// <summary>
		/// Get setting value.
		/// </summary>
		/// <typeparam name="T">Type of value.</typeparam>
		/// <param name="key">Key.</param>
		/// <returns>Value or empty.</returns>
	    public T Get<T>(string key)
	    {
		    return !storedValues.ContainsKey(key) ? default(T) : (T) storedValues[key];
	    }

		/// <summary>
		/// Save settings to file.
		/// </summary>
		/// <returns>This or empty.</returns>
	    public ConfigManager Save()
	    {
			var binFormatter = new BinaryFormatter();
			using (var mStream = new MemoryStream())
			{
				binFormatter.Serialize(mStream, storedValues);
				var protectedData = ProtectedData.Protect(mStream.GetBuffer(), null, DataProtectionScope.LocalMachine);
				File.WriteAllBytes(SettingsFilePath, protectedData);
			}

			return this;
	    }
    }
}