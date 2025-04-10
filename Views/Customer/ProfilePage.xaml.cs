using System.Threading.Tasks;
using Barangay_Office.Services;
using Barangay_Office.ViewModels;
using Barangay_Office.ViewModels.CustomerViewModel;
using Bumptech.Glide.Load.Model;

namespace Barangay_Office.Views;

public partial class ProfilePage : ContentPage
{

    public ProfilePage()
    {
		InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await Task.Run(() => ModelLoaderLoadData());

    }

}