/* [grial-metadata] id: Grial#NewsData.cs version: 1.0.1 */
namespace Barangay_Office
{

    public class NewsArticleData : ObservableObject
    {
        private bool _isFavorite;

        public string MainImage { get; set; }
        public string DetailImage { get; set; }
        public string ListImage { get; set; }     
        public string Title { get; set; }
        public string Section { get; set; }
        public string Length { get; set; }
        public string When { get; set; }
        public string SourceLogo { get; set; }
        public string SourceId { get; set; }
        public string Author { get; set; }
        public bool IsFavorite
        {
            get { return _isFavorite; }
            set { SetProperty(ref _isFavorite, value); }
        }
        public bool IsPro { get; set; }
        public string Body { get; set; }
        public string BodyImage { get; set; }
        public string BodyImageCopy { get; set; }
        public string SubSectionTitle { get; set; }
        public string SubSection { get; set; }       
    }

    public class NewsProfileData
    {
        public string Avatar { get; set; }
        public string Name { get; set; }
        public string Job { get; set; }
        public string Quote { get; set; }
        public string Media1 { get; set; }
        public string Media1Icon { get; set; }
        public string Media2 { get; set; }
        public string Media2Icon { get; set; }
        public string Articles { get; set; }
        public string Followers { get; set; }
        public string Following { get; set; }
    }

}
