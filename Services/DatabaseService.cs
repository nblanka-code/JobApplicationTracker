using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using JobApplicationTracker.Models;

namespace JobApplicationTracker.Services
{
    public class DatabaseService
    {
        private SQLiteAsyncConnection? _database;

        private async Task InitAsync()
        {
            if (_database is not null)
                return;

            var dbPath = Path.Combine(FileSystem.AppDataDirectory, "JobApplications.db3");
            _database = new SQLiteAsyncConnection(dbPath);
            await _database.CreateTableAsync<JobApplication>();
        }

        public async Task<List<JobApplication>> GetApplicationsAsync()
        {
            await InitAsync();
            return await _database!.Table<JobApplication>().ToListAsync();
        }

        public async Task<int> SaveApplicationAsync(JobApplication app)
        {
            await InitAsync();
            if (app.Id != 0)
            {
                return await _database!.UpdateAsync(app);
            }
            else
            {
                return await _database!.InsertAsync(app);
            }
        }

        public async Task<int> DeleteApplicationAsync(JobApplication app)
        {
            await InitAsync();
            return await _database!.DeleteAsync(app);
        }

        public async Task<List<JobApplication>> SearchApplicationsAsync(string searchTerm)
        {
            await InitAsync();
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return await GetApplicationsAsync();
            }

            return await _database.Table<JobApplication>()
                                  .Where(x => x.JobTitle.ToLower().Contains(searchTerm.ToLower()) ||
                                              x.CompanyName.ToLower().Contains(searchTerm.ToLower()))
                                  .ToListAsync();
        }
    }
}