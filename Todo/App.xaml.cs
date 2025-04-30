using System.Net.Http;
using System.Net.Http.Headers;
using Todo.Helpers.Session;
using Todo.Services;
using Todo.Views;
using Xamarin.Forms;

using TodoClient = Todo.ApiServices.Client;

namespace Todo
{
    public partial class App : Application
    {
        private readonly HttpClient _httpClient;
        private readonly IAuthTokenStore _tokenStore;
        public App()
        {
            InitializeComponent();

            DependencyService.Register<IAuthTokenStore, SecureStorageTokenStore>();
            _tokenStore = DependencyService.Get<IAuthTokenStore>();

            DependencyService.Register<IHttpClientService, HttpClientService>();

            _httpClient = DependencyService.Get<IHttpClientService>().GetClient();

            DependencyService.RegisterSingleton(new TodoClient(Constants.Constants.BASE_URL, _httpClient));

            DependencyService.Register<TodoService>();
            DependencyService.Register<UserDataStore>();

            Routing.RegisterRoute(nameof(AddEditTodoPage), typeof(AddEditTodoPage));
            Routing.RegisterRoute(nameof(TodoDetailView), typeof(TodoDetailView));
            Routing.RegisterRoute(nameof(RegisterView), typeof(RegisterView));
            Routing.RegisterRoute(nameof(LoginView), typeof(LoginView));
            Routing.RegisterRoute(nameof(TodosPage), typeof(TodosPage));

            if (SessionService.Instance.CurrentUser != null && SessionService.Instance.IsCurrentUserLoggedIn)
            {
                // User is logged in, go to TodosPage
                MainPage = new NavigationPage(new TodosPage())
                {
                    BarBackgroundColor = (Color)Current.Resources["navBarBackgroundColor"],
                    BarTextColor = (Color)Current.Resources["navBarTextColor"]
                };
            }
            else
            {
                // User is NOT logged in, go to LoginPage
                MainPage = new NavigationPage(new LoginView())
                {
                    BarBackgroundColor = (Color)Current.Resources["navBarBackgroundColor"],
                    BarTextColor = (Color)Current.Resources["navBarTextColor"]
                };
            }
        }

        protected override async void OnStart()
        {
            LogoutUser();
            var token = await _tokenStore.GetToken();

            if (!string.IsNullOrEmpty(token))
            {

                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            }
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
            LogoutUser();
        }


        protected override void OnResume()
        {
            LogoutUser();
        }

        private void LogoutUser()
        {
            SessionService.Instance.Logout();
            _tokenStore.ClearToken();

            // Also clear from current HttpClient
            _httpClient.DefaultRequestHeaders.Authorization = null;

        }
    }
}
