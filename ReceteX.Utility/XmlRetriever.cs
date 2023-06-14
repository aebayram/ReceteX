using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReceteX.Utility
{
	public class XmlRetriever
	{
		public static Task<string> GetXmlContent(string v)
		{
			throw new NotImplementedException();
		}

		public async Task<string> GetXmlContect(string url)
		{
			using (var client = new HttpClient())
			{
				HttpResponseMessage response = await client.GetAsync(url);
				response.EnsureSuccessStatusCode();
				return await response.Content.ReadAsStringAsync();
			}

		}
	}
}