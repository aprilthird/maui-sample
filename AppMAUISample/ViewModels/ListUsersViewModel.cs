using AppMAUISample.Services;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using Core.Models;
using MvvmHelpers;
using System;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AppMAUISample.ViewModels
{
    public class ListUsersViewModel : BindableObject
    {
        public ObservableRangeCollection<AppUser> Users { get; set; }

        public string Search { get; set; }

        public bool IsBusy { get; set; }

        public ICommand RefreshData { get; }

        public ICommand OnAddUserClick { get; }

        public ICommand OnEditUserClick { get; }

        public ICommand OnDeleteUserClick { get; }

        public ListUsersViewModel()
        {
            Users = new ObservableRangeCollection<AppUser>();
            RefreshData = new Command(async () => await LoadAsync());
            OnAddUserClick = new Command(async () => await NewUser());
            OnEditUserClick = new Command<AppUser>(async (appUser) => await EditUser(appUser));
            OnDeleteUserClick = new Command<AppUser>(async (appUser) =>  await DeleteUser(appUser));
        }

        async Task LoadAsync()
        {
            try
            {
                IsBusy = true;
                Users.Clear();
                var results = await ApiService.GetAllAsync(Search);
                Users.AddRange(results);
                IsBusy = false;
                OnPropertyChanged(nameof(IsBusy));
            }
            catch (Exception ex)
            {
                var toast = Toast.Make("An error ocurred while retrieving data.", ToastDuration.Long, 30d);
                await toast.Show();
                Console.Write(ex.StackTrace);
            }
        }

        async Task NewUser()
        {
            await Shell.Current.GoToAsync($"//user?Param={JsonSerializer.Serialize(new AppUser())}");
        }

        async Task EditUser(AppUser appUser)
        {
            await Shell.Current.GoToAsync($"//user?Param={JsonSerializer.Serialize(appUser)}");
        }

        async Task DeleteUser(AppUser user)
        {
            var deleteConfirm = await App.Current.MainPage.DisplayAlert("Are you sure?", "This action irreversible.", "Delete", "Cancel");
            if (deleteConfirm)
            {
                await ApiService.DeleteAsync(user);
                if(!string.IsNullOrEmpty(ApiService.ErrorMessage))
                {
                    var toast = Toast.Make("An error ocurred while saving changes.", ToastDuration.Long, 30d);
                    await toast.Show();
                    await App.Current.MainPage.DisplayAlert("Error", ApiService.ErrorMessage, "Ok");
                }
                else
                {
                    var toast = Toast.Make("Changes saved successfully.", ToastDuration.Long, 30d);
                    await toast.Show();
                    await LoadAsync();
                }
            }
        }
    }
}
