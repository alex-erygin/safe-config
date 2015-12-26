using System;
using System.Runtime.Serialization;

namespace SafeConfig
{
	/// <summary>
	/// SafeConfig exception. For details see InnerException property.
	/// </summary>
	public class SafeConfigException : Exception
	{
		/// <summary>
		///     Just create the exception.
		/// </summary>
		public SafeConfigException()
		{
		}

		/// <summary>
		///     Create the exception with description.
		/// </summary>
		/// <param name="message">Exception description</param>
		public SafeConfigException(string message)
			: base(message)
		{
		}

		/// <summary>
		///     Create the exception with description and inner cause.
		/// </summary>
		/// <param name="message">Exception description</param>
		/// <param name="innerException">Exception inner cause</param>
		public SafeConfigException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		/// <summary>
		///     Create the exception from serialized data.
		/// </summary>
		/// <param name="info">Serialization info</param>
		/// <param name="context">Serialization context</param>
		protected SafeConfigException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}