using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AppMAUISample.ViewModels
{
    public class LoginViewModel : BindableObject
    {
        public string Email { get; set; }

        public string Password { get; set; }

        private string emailErrorText;
        private string passwordErrorText;

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

        public string PasswordErrorText 
        { 
            get => passwordErrorText; 
            set
            {
                if (passwordErrorText == value)
                    return;
                passwordErrorText = value;
                HasErrorOnPassword = !string.IsNullOrEmpty(value);
                OnPropertyChanged(nameof(PasswordErrorText));
                OnPropertyChanged(nameof(HasErrorOnPassword));
            }
        }

        public bool HasErrorOnEmail { get; private set; }

        public bool HasErrorOnPassword { get; private set; }

        public ICommand OnLoginClick { get; }

        public LoginViewModel()
        {
            OnLoginClick = new Command(async () => await Login());
        }

        async Task Login()
        {
            EmailErrorText = string.Empty;
            PasswordErrorText = string.Empty;

            if (string.IsNullOrEmpty(Email))
                EmailErrorText = "Email is required";
            else if (!Email.Contains('@'))
                EmailErrorText = "Email is not valid";

            if (string.IsNullOrEmpty(Password))
                PasswordErrorText = "Password is required";

            if (HasErrorOnEmail || HasErrorOnPassword)
                return;
            
            await Shell.Current.GoToAsync($"//home?Name={Email}");
        }
    }
}
