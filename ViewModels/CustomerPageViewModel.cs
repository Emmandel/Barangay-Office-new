/* [grial-metadata] id: Grial#NewsMyProfileViewModel.cs version: 1.0.1 */
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Barangay_Office
{
	public class CustomerPageViewModel : ObservableObject
	{
        private NewsProfileData _profileData;

        public CustomerPageViewModel()
		{
            LoadData();

            ToggleFavoriteCommand = new Command<NewsArticleData>(ToggleFavorite);
        }

        public Command ToggleFavoriteCommand { get; }

        public ObservableCollection<NewsArticleData> List { get; } = new ObservableCollection<NewsArticleData>();
        public ObservableCollection<NewsArticleData> Favorites => new ObservableCollection<NewsArticleData>(List?.Where(a => a.IsFavorite));

        public NewsProfileData ProfileData
        {
            get => _profileData;
            set => SetProperty(ref _profileData, value);
        }

        private void ToggleFavorite(NewsArticleData article)
        {
            article.IsFavorite = !article.IsFavorite;
        }

        private void LoadData()
        {
            List.Clear();

            JsonHelper.Instance.LoadViewModel(this, source: "News.json", pageName: "NewsMyProfilePage.xaml");
        }
    }
}

