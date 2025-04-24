using System;
using System.Threading.Tasks;
using Todo.Helpers.Session;
using Todo.ViewModels;
using Todo.Views;
using Xamarin.Forms;

public class LoginViewModel : BaseViewModel
{
    #region Fields
    private string _username;
    private string _password;
    #endregion

    #region Properties
    public Command LoginCommand { get; }
    public Command SignUpCommand { get; }
    public Command ForgotPasswordCommand { get; }

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
    #endregion

    public LoginViewModel()
    {
        PageTitle = "Login Page";

        LoginCommand = new Command(async () => await OnLoginClicked(), ValidateLogin);
        SignUpCommand = new Command(async () => await OnSignUpClicked());
        ForgotPasswordCommand = new Command(async () => await OnForgotPasswordClicked());

        this.PropertyChanged += (_, __) => LoginCommand.ChangeCanExecute();
    }

    private bool ValidateLogin()
    {
        return !string.IsNullOrWhiteSpace(Username) && !string.IsNullOrWhiteSpace(Password);
    }

    private async Task OnLoginClicked()
    {
        if (IsBusy)
            return;

        IsBusy = true;

        try
        {
            bool isValidUser = await UserDataStore.Login(Username, Password);

            if (isValidUser)
            {
                CurrentUser = await UserDataStore.GetUserBy(Username);
                CurrentUser.IsLoggedIn = true;

                SessionService.Instance.CurrentUser = CurrentUser;

                SessionService.Instance.UserTodos = await GetUserTodos();

                // Ensure Toolbar updates after login
                // ✅ Send the "UserLoggedIn" message instead of subscribing
                MessagingCenter.Send(this, "UserLoggedIn");

                await Application.Current.MainPage.Navigation.PushAsync(new TodosPage());

            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Login Failed", "Invalid username or password.", "OK");
            }
        }
        catch (Exception ex)
        {
            await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }



    private async Task OnSignUpClicked()
    {
        await Application.Current.MainPage.Navigation.PushAsync(new RegisterView());
    }

    private async Task OnForgotPasswordClicked()
    {
        // Navigate to ForgotPasswordPage
    }
}
