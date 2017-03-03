using AppleTV.Extensions;
using AppleTV.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AppleTV.ViewModels
{
	public class MainPageViewModel : ObservableObject
	{
		private const string AppleTVUrl = "http://a1.phobos.apple.com/us/r1000/000/Features/atv/AutumnResources/videos/entries.json";
		private IReadOnlyCollection<Entry> _entires = new List<Entry>();
		private bool _canProgress = false;

		public Asset SelectedAsset
		{
			get { return _selectedAsset; }
			set { SetValue(ref _selectedAsset, value); }
		}
		private Asset _selectedAsset = null;

		public ICommand NextCommand
		{
			get { return _nextCommand; }
			set { SetValue(ref _nextCommand, value); }
		}
		private ICommand _nextCommand = null;

		public MainPageViewModel(){
			NextCommand = new RelayCommand(() => setMedia());
		}

		public async Task Setup()
		{
			var store = IsolatedStorageFile.GetUserStoreForApplication();
			if (store.FileExists("entries.json"))
			{
				using (var file = store.OpenFile("entries.json", FileMode.Open))
				using (var reader = new StreamReader(file))
				{
					_entires = JsonConvert.DeserializeObject<IReadOnlyCollection<Entry>>(reader.ReadToEnd());
				}

				// Update entries (fire-and-forget)
				updateEntries().Start();
			}
			else
			{
				// Get entries from Apple for the first time
				updateEntries().Wait();

				// Write them to Isolated Storage
				using (var file = store.OpenFile("entries.jso", FileMode.OpenOrCreate))
				using (var writer = new StreamWriter(file))
				{
					var jsonStr = JsonConvert.SerializeObject(_entires);
					await writer.WriteAsync(jsonStr);
					await writer.FlushAsync();
				}
			}

			setMedia(true);
		}

		private async Task updateEntries()
		{
			using (var httpClient = new HttpClient())
			{
				httpClient.Timeout = TimeSpan.FromSeconds(10);

				var resp = await httpClient.GetAsync(AppleTVUrl).ConfigureAwait(continueOnCapturedContext: false);
				var jsonStr = await resp.Content.ReadAsStringAsync();
				_entires = JsonConvert.DeserializeObject<IReadOnlyCollection<Entry>>(jsonStr);
			}
		}

		private void setMedia(bool init = false)
		{
			if (init) _canProgress = true;
			if (!_canProgress) return;
			_canProgress = false;

			SelectedAsset = _entires.TakeRandom().Assets.TakeRandom();

			_canProgress = true;
		}
	}
}
