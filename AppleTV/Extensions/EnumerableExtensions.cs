using System;
using System.Collections.Generic;
using System.Linq;
using Windows.Security.Cryptography;

namespace AppleTV.Extensions
{
	public static class EnumerableExtensions
	{
		public static T TakeRandom<T>(this IEnumerable<T> enumerable)
		{
			var random = new Random((int) CryptographicBuffer.GenerateRandomNumber());
			var index = random.Next(0, enumerable.Count());
			return enumerable.ElementAt(index);
		}
	}
}
