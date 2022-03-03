using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppMAUISample.ViewModels
{
    [QueryProperty(nameof(Name), nameof(Name))]
    public class WelcomeViewModel : BindableObject
    {
        public string Name 
        { 
            set 
            {
                WelcomeMessage = $"Welcome {value ?? "User"}!";
                OnPropertyChanged(nameof(WelcomeMessage)); 
            } 
        }

        public string WelcomeMessage { get; private set; }

        public WelcomeViewModel()
        {
        }
    }
}
