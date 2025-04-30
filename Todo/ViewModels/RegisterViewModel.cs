using System.Threading.Tasks;
using Todo.Helpers;
using Todo.Views;
using Xamarin.Forms;

using UserRegisterDto = Todo.ApiServices.UserRegistrationDto;

namespace Todo.ViewModels
{
    public class RegisterViewModel : BaseViewModel
    {
        #region Fields
        private readonly UserAuth _UserAuthHelpers;

        private string _username;
        private string _password;
        private string _email;
        #endregion

        #region Properties
        public string Username
        {
            get => _username;
            set => SetProperty(ref _username, value);
        }

        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        public Command RegisterCommand { get; }
        public Command LoginCommand { get; }
        #endregion

        public RegisterViewModel()
        {
            PageTitle = "Register Page";

            RegisterCommand = new Command(async () => await OnRegister());
            LoginCommand = new Command(async () => await OnLoginClicked());
        }

        private async Task OnRegister()
        {

            var newUser = new UserRegisterDto
            {
                Username = Username,
                Password = Password,
                Email = Email,
            };

            var IsSuccess = await UserDataStore.Register(newUser);

            if (IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert("Success", "Account created successfully!", "OK");
            }

            await Application.Current.MainPage.Navigation.PushAsync(new LoginView());
        }

        private async Task OnLoginClicked()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new LoginView());
        }
    }
}
