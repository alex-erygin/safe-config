using System.IO;
using System.Linq;
using Nelibur.Sword.DataStructures;

namespace SafeConfig
{
    public class Scanner
    {
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
    }
}
