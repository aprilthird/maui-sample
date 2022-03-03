using AppMAUISample.Services;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AppMAUISample.ViewModels
{
    [QueryProperty(nameof(Param), nameof(Param))]
    public class UserFormViewModel : BindableObject
    {
        public string Param 
        { 
            get => Param;
            set
            {
                if (!string.IsNullOrEmpty(value))
                    AppUser = JsonSerializer.Deserialize<AppUser>(value);

                Id = AppUser.Id == 0 ? "(Automatic)" : AppUser.Id.ToString();
                Name = AppUser.Name;
                Age = AppUser.Id == 0 ? null : AppUser.Age.ToString();
                Email = AppUser.Email;
                BirthDate = AppUser.Id == 0 ? DateTime.Now : AppUser.BirthDate;
                OnPropertyChanged(nameof(Id));
                OnPropertyChanged(nameof(Name));
                OnPropertyChanged(nameof(Age));
                OnPropertyChanged(nameof(BirthDate));
                OnPropertyChanged(nameof(Email));
            }
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Age { get; set; }
        public DateTime BirthDate { get; set; }
        public bool IsBusy { get; set; }
        public bool IsNotBusy { get => !IsBusy; }

        private string nameErrorText;
        private string emailErrorText;
        private string ageErrorText;
        private string birthDateErrorText;

        public string NameErrorText 
        {
            get => nameErrorText;
            set
            {
                if (nameErrorText == value)
                    return;
                nameErrorText = value;
                HasErrorOnName = !string.IsNullOrEmpty(value);
                OnPropertyChanged(nameof(NameErrorText));
                OnPropertyChanged(nameof(HasErrorOnName));
            }
        }
        public string EmailErrorText
        {
            get => emailErrorText;
            set
            {
                if (emailErrorText == value)
                    return;
                emailErrorText = value;
                HasErrorOnEmail = !string.IsNullOrEmpty(value);
                OnPropertyChanged(nameof(EmailErrorText));
                OnPropertyChanged(nameof(HasErrorOnEmail));
            }
        }

        public string AgeErrorText 
        { 
            get => ageErrorText;
            set 
            {
                if (ageErrorText == value)
                    return;
                ageErrorText = value;
                HasErrorOnAge = !string.IsNullOrEmpty(value);
                OnPropertyChanged(nameof(AgeErrorText));
                OnPropertyChanged(nameof(HasErrorOnAge));
            } 
        }

        public string BirthDateErrorText 
        { 
            get => birthDateErrorText; 
            set
            {
                if (birthDateErrorText == value)
                    return;
                birthDateErrorText = value;
                HasErrorOnBirthDate = !string.IsNullOrEmpty(value);
                OnPropertyChanged(nameof(BirthDateErrorText));
                OnPropertyChanged(nameof(HasErrorOnBirthDate));
            }
        }    

        public bool HasErrorOnName { get; set; }
        public bool HasErrorOnEmail { get; set; }
        public bool HasErrorOnAge { get; set; }
        public bool HasErrorOnBirthDate { get; set; }

        private AppUser AppUser;

        public ICommand OnSaveClick { get; }

        public ICommand OnCancelClick { get; }

        public UserFormViewModel()
        {
            AppUser = new AppUser();
            OnCancelClick = new Command(async () => await Cancel());
            OnSaveClick = new Command(async () => await Save());
        }

        async Task Cancel()
        {
            await Shell.Current.GoToAsync("//users");
        }

        async Task Save()
        {
            IsBusy = true;
            OnPropertyChanged(nameof(IsNotBusy));

            NameErrorText = string.Empty;
            AgeErrorText = string.Empty;
            BirthDateErrorText = string.Empty;
            EmailErrorText = string.Empty;

            if (string.IsNullOrEmpty(Name))
                NameErrorText = "Name is required";
            else
                AppUser.Name = Name;

            if (string.IsNullOrEmpty(Age))
                AgeErrorText = "Age is required";
            else if (!Int32.TryParse(Age, out int AgeParsed))
                AgeErrorText = "Age is not a valid number";
            else if (AgeParsed < 0 || AgeParsed > 120)
                AgeErrorText = "Age is out of range [0-120]";
            else
                AppUser.Age = AgeParsed;

            if (BirthDate >= DateTime.Now)
                BirthDateErrorText = "Birth Date is not valid";
            else
                AppUser.BirthDate = BirthDate;

            if (string.IsNullOrEmpty(Email))
                EmailErrorText = "Email is required";
            else if (!Email.Contains('@'))
                EmailErrorText = "Email is not valid";
            else
                AppUser.Email = Email;

            if (HasErrorOnName || HasErrorOnAge || HasErrorOnBirthDate || HasErrorOnEmail)
                return;

            await ApiService.SaveAsync(AppUser);
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
                await Shell.Current.GoToAsync("//users");
            }
            IsBusy = false;
            OnPropertyChanged(nameof(IsNotBusy));
        }
    }
}
