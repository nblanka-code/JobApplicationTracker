using JobApplicationTracker.Models;
using JobApplicationTracker.Services;

namespace JobApplicationTracker
{
    public partial class JobEntryPage : ContentPage
    {
        private readonly DatabaseService _databaseService;
        private JobApplication _currentApplication;

        public JobEntryPage()
        {
            InitializeComponent();
            _databaseService = new DatabaseService();
            _currentApplication = new JobApplication();
        }

        public JobEntryPage(JobApplication app)
        {
            InitializeComponent();
            _databaseService = new DatabaseService();
            _currentApplication = app;

            JobTitleEntry.Text = app.JobTitle;
            CompanyNameEntry.Text = app.CompanyName;
            JobDescriptionEditor.Text = app.JobDescription;
            SalaryEntry.Text = app.Salary;
            WeeklyHoursEntry.Text = app.WeeklyWorkHours.ToString();
            CityEntry.Text = app.City;
            StreetAddressEntry.Text = app.StreetAddress;
            ApplicationDatePicker.Date = app.ApplicationDate;
            ContactStatusEntry.Text = app.ContactStatus;
            InterviewDateEntry.Text = app.InterviewDate;
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            _currentApplication.JobTitle = JobTitleEntry.Text ?? string.Empty;
            _currentApplication.CompanyName = CompanyNameEntry.Text ?? string.Empty;
            _currentApplication.JobDescription = JobDescriptionEditor.Text ?? string.Empty;
            _currentApplication.Salary = SalaryEntry.Text ?? string.Empty;

            if (int.TryParse(WeeklyHoursEntry.Text, out int hours))
            {
                _currentApplication.WeeklyWorkHours = hours;
            }

            _currentApplication.City = CityEntry.Text ?? string.Empty;
            _currentApplication.StreetAddress = StreetAddressEntry.Text ?? string.Empty;
            _currentApplication.ApplicationDate = ApplicationDatePicker.Date.Value;
            _currentApplication.ContactStatus = ContactStatusEntry.Text ?? string.Empty;
            _currentApplication.InterviewDate = InterviewDateEntry.Text ?? string.Empty;

            await _databaseService.SaveApplicationAsync(_currentApplication);
            await Navigation.PopAsync();
        }
    }
}