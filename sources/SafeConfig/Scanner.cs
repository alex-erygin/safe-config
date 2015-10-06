using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using Nelibur.Sword.DataStructures;
using Nelibur.Sword.Extensions;

namespace SafeConfig
{
	/// <summary>
	/// Secure configuration manager.
	/// </summary>
    public class Scanner
    {
		/// <summary>
		/// Configuration key-value storage.
		/// </summary>
	    private readonly Dictionary<string, object> storedValues = new Dictionary<string, object>();

		/// <summary>
		/// Folder, contains config.
		/// </summary>
	    private string configFolder = ".";

		/// <summary>
		/// Config file name.
		/// </summary>
	    private string defaultSettingsFileName = "settings.saveconfig";

		/// <summary>
		/// Full path to settings file.
		/// </summary>
	    private string SettingsFilePath => Path.Combine(configFolder, defaultSettingsFileName);

		/// <summary>
		/// Set working folder.
		/// </summary>
		/// <param name="folder">Working folder.</param>
		/// <returns>This if folder contains any config files or empty.</returns>
	    public Option<Scanner> AtFolder(string folder)
	    {
		    if (!Directory.Exists(folder))
		    {
			    return Option<Scanner>.Empty;
		    }

		    configFolder = folder;

			var files = Directory.GetFiles(configFolder, "*.safeconfig");
		    if (files.Any())
		    {
			    return new Option<Scanner>(this);
		    }

		    return Option<Scanner>.Empty;
	    }
		
		/// <summary>
		/// Load configuration from file.
		/// </summary>
		/// <returns>This.</returns>
	    public Option<Scanner> Load()
	    {
		    return this.ToOption();
	    }

		/// <summary>
		/// Set setting value.
		/// </summary>
		/// <typeparam name="T">Type of value.</typeparam>
		/// <param name="key">Key.</param>
		/// <param name="value">Value.</param>
		/// <returns>This.</returns>
	    public Option<Scanner> Set<T>(string key, T value)
	    {
		    storedValues[key] = value;
			return this.ToOption();
		}


	    public Option<T> Get<T>(string key)
	    {
		    return !storedValues.ContainsKey(key) ? Option<T>.Empty : ((T) storedValues[key]).ToOption();
	    }

	    public Option<Scanner> Save()
	    {
			var binFormatter = new BinaryFormatter();
			var mStream = new MemoryStream();
			binFormatter.Serialize(mStream, storedValues);

		    var protectedData = ProtectedData.Protect(mStream.GetBuffer(), null, DataProtectionScope.LocalMachine);
			File.WriteAllBytes(SettingsFilePath, protectedData);
		    return this.ToOption();
	    }
    }
}