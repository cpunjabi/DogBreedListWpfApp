using System.Windows;
using WoofWoof.ViewModels;

namespace WoofWoof
{
	/// <summary>
	/// Interaction logic for DogListView.xaml
	/// </summary>
	public partial class DogListView : Window
	{
		#region Fields

		private DogListViewModel _ViewModel;

		#endregion

		#region Constructor

		public DogListView()
		{
			InitializeComponent();

			_ViewModel = new DogListViewModel();
			DataContext = _ViewModel;
		}

		#endregion
	}
}