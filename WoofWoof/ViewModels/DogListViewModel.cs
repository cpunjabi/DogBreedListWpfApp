using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Windows;
using System.Windows.Media.Imaging;
using Newtonsoft.Json;
using Prism.Mvvm;
using WoofWoof.Helpers;
using WoofWoof.Models;

namespace WoofWoof.ViewModels
{
	public class DogListViewModel : BindableBase
	{
		#region Constants

		private const string BASE_URL = "https://dog.ceo/api";

		private const string LIST_PATH = "/breeds/list";

		private const string IMAGE_NOT_AVAILABLE_PATH = "/WoofWoof;component/Resources/Images/ImageNotAvailable.jpg";

		#endregion

		#region Fields

		private List<string> _Breeds;

		private string _SelectedBreed;

		private BitmapImage _ImageSource;

		private bool _IsListBusy;

		private bool _IsImageBusy;

		#endregion

		#region Properties

		public List<string> Breeds
		{
			get => _Breeds;
			internal set => SetProperty(ref _Breeds, value);
		}

		public string SelectedBreed
		{
			get => _SelectedBreed;
			set
			{
				SetProperty(ref _SelectedBreed, value);
				FetchImage();
			}
		}

		public BitmapImage Image
		{
			get => _ImageSource;
			set => SetProperty(ref _ImageSource, value);
		}

		public bool IsListBusy
		{
			get => _IsListBusy;
			set => SetProperty(ref _IsListBusy, value);
		}

		public bool IsImageBusy
		{
			get => _IsImageBusy;
			set => SetProperty(ref _IsImageBusy, value);
		}

		#endregion

		#region Constructor

		public DogListViewModel()
		{
			Image = ImageHelper.Load(IMAGE_NOT_AVAILABLE_PATH);
			FetchBreeds();
		}

		#endregion

		#region Methods

		private async void FetchBreeds()
		{
			IsListBusy = true;
			HttpClient client = null;
			HttpResponseMessage response = null;

			try
			{
				Breeds = new List<string>();
				client = new HttpClient
				{
					BaseAddress = new Uri($"{BASE_URL}{LIST_PATH}")
				};

				response = await client.GetAsync(string.Empty);

				if (response.IsSuccessStatusCode)
				{
					var json = response.Content.ReadAsStringAsync().Result;
					var jsonObject = JsonConvert.DeserializeObject<ListJsonObject>(json);
					Breeds = jsonObject.Breeds;
				}
				else
				{
					MessageBox.Show($"Unable to fetch Dog Breeds, Failed with Http StatusCode: {response.StatusCode}", "Error");
				}
			}
			catch (Exception e)
			{
				MessageBox.Show($"Unable to fetch Dog Breeds, Failed with Exception: {e.Message}", "Error");
			}
			finally
			{
				IsListBusy = false;
				client?.Dispose();
				response?.Dispose();
			}
		}

		private async void FetchImage()
		{
			IsImageBusy = true;
			HttpClient client = null;
			HttpResponseMessage response = null;

			try
			{
				client = new HttpClient
				{
					BaseAddress = new Uri($"{BASE_URL}/breed/{SelectedBreed}/images/random")
				};

				response = await client.GetAsync(string.Empty);

				if (response.IsSuccessStatusCode)
				{
					var json = response.Content.ReadAsStringAsync().Result;
					var jsonObject = JsonConvert.DeserializeObject<ImageJsonObject>(json);

					client = new HttpClient
					{
						BaseAddress = new Uri(jsonObject.ImageUrl)
					};
					response = await client.GetAsync(string.Empty);

					Image = ImageHelper.Load(response.IsSuccessStatusCode ? jsonObject.ImageUrl : IMAGE_NOT_AVAILABLE_PATH);
				}
			}
			catch
			{
				Image = ImageHelper.Load("/WoofWoof;component/Resources/Images/ImageNotAvailable.jpg");
			}
			finally
			{
				IsImageBusy = false;
				client?.Dispose();
				response?.Dispose();
			}
		}

		#endregion
	}
}