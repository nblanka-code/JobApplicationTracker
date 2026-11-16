using JobApplicationTracker.Models;
using JobApplicationTracker.Services;

namespace JobApplicationTracker
{
    public partial class MainPage : ContentPage
    {
        private readonly DatabaseService _databaseService;

        public MainPage()
        {
            InitializeComponent();
            _databaseService = new DatabaseService();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await LoadDataAsync();
        }

        private async Task LoadDataAsync(string searchTerm = "")
        {
            List<JobApplication> applications;

            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                applications = await _databaseService.GetApplicationsAsync();
            }
            else
            {
                applications = await _databaseService.SearchApplicationsAsync(searchTerm);
            }

            applicationsView.ItemsSource = applications;
        }

        private async void OnSearchTextChanged(object sender, TextChangedEventArgs e)
        {
            string keyword = e.NewTextValue;
            await LoadDataAsync(keyword);
        }

        private async void filterBtn_Clicked(object sender, EventArgs e)
        {
            string action = await DisplayActionSheet(
                "Filter and Sort...",
                "Cancel",
                null,
                "Show All",
                "Sort by date (newest)",
                "Sort by date (oldest)",
                "City",
                "Contact Status (not None)",
                "Interviews"
            );

            if (string.IsNullOrEmpty(action) || action == "Cancel")
                return;

            var list = await _databaseService.GetApplicationsAsync();

            switch (action)
            {
                case "Show All":
                    await LoadDataAsync();
                    break;

                case "Sort by date (newest)":
                    applicationsView.ItemsSource = list.OrderByDescending(x => x.ApplicationDate).ToList();
                    break;

                case "Sort by date (oldest)":
                    applicationsView.ItemsSource = list.OrderBy(x => x.ApplicationDate).ToList();
                    break;

                case "City":
                    
                    var uniqueCities = list.Where(x => !string.IsNullOrWhiteSpace(x.City))
                                           .Select(x => x.City)
                                           .Distinct()
                                           .OrderBy(c => c)
                                           .ToArray();

                    if (uniqueCities.Length == 0)
                    {
                        await DisplayAlert("Info", "No cities found in the database yet.", "OK");
                        return;
                    }

                    string selectedCity = await DisplayActionSheet("Select City", "Cancel", null, uniqueCities);

                    if (!string.IsNullOrEmpty(selectedCity) && selectedCity != "Cancel")
                    {
                        var filteredByCity = list.Where(x => x.City != null && x.City.Equals(selectedCity, StringComparison.OrdinalIgnoreCase)).ToList();
                        applicationsView.ItemsSource = filteredByCity;
                    }
                    break;

                case "Contact Status (not None)":
                    var callbackList = list.Where(x => !string.IsNullOrWhiteSpace(x.ContactStatus)
                                                    && !x.ContactStatus.Equals("none", StringComparison.OrdinalIgnoreCase)).ToList();
                    applicationsView.ItemsSource = callbackList;
                    break;

                case "Interviews":
                    var interviewList = list.Where(x => !string.IsNullOrWhiteSpace(x.InterviewDate)
                                                     && !x.InterviewDate.Equals("none", StringComparison.OrdinalIgnoreCase))
                                            .OrderBy(x => x.InterviewDate)
                                            .ToList();
                    applicationsView.ItemsSource = interviewList;
                    break;
            }
        }

        private async void addNewBtn_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new JobEntryPage());
        }


        private async void OnEditClicked(object sender, EventArgs e)
        {
            if (sender is Button button && button.BindingContext is JobApplication app)
            {
                await Navigation.PushAsync(new JobEntryPage(app));
            }
        }

        private async void OnDeleteClicked(object sender, EventArgs e)
        {
            if (sender is Button button && button.BindingContext is JobApplication app)
            {
                bool answer = await DisplayAlert("Confirmation", $"Are you sure you want to delete {app.CompanyName}?", "Yes", "No");
                if (answer)
                {
                    await _databaseService.DeleteApplicationAsync(app);
                    await LoadDataAsync();
                }
            }
        }
    }
}
