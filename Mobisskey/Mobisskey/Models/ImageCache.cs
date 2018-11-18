using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Disboard.Extensions;

namespace Mobisskey.Models
{
	public class ImageCache : Singleton<ImageCache>
	{
		private readonly Dictionary<string, string> urlPathMap = new Dictionary<string, string>();
		private readonly WebClient web = new WebClient();

		public async Task<string> DownloadFileAsync(string url, string fileId)
		{
			if (urlPathMap.ContainsKey(url))
				return urlPathMap[url];

			var path = Path.Combine(
				Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
				fileId + Path.GetExtension(fileId)
			);

			Console.WriteLine($"url: {url}");

			try
			{
				await web.DownloadFileTaskAsync(url, path);
			}
			catch (WebException ex)
			{ 
				Console.WriteLine($"アイコンダウンロードできなかった {ex.GetType()} {ex.Status}");
			}
			Console.WriteLine($"path: {path}");
			return urlPathMap[url] = path;
		}
	}
}
