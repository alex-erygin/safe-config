using System.Collections.Generic;
using System.IO;
using System.Linq;
using Nelibur.Sword.DataStructures;
using Nelibur.Sword.Extensions;

namespace SafeConfig
{
    public class Scanner
    {
	    private readonly Dictionary<string, object> storedValues = new Dictionary<string, object>(); 

	    public Option<Scanner> AtFolder(string folder)
	    {
		    if (!Directory.Exists(folder))
		    {
			    return Option<Scanner>.Empty;
		    }

		    var files = Directory.GetFiles(folder, "*.safeconfig");
		    if (files.Any())
		    {
			    return new Option<Scanner>(this);
		    }

		    return Option<Scanner>.Empty;
	    }

	    public Option<Scanner> Load()
	    {
		    return this.ToOption();
	    }

	    public Option<Scanner> Set<T>(string key, T value)
	    {
		    storedValues[key] = value;
			return this.ToOption();
		}

	    public Option<T> Get<T>(string key)
	    {
		    return !storedValues.ContainsKey(key) ? Option<T>.Empty : ((T) storedValues[key]).ToOption();
	    }
    }
}