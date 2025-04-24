using System.Net.Http.Headers;
using Todo.ApiServices;
using Todo.Helpers.Session;
using Todo.Services;
using Todo.Views;
using Xamarin.Forms;

namespace Todo
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            DependencyService.Register<IAuthTokenStore, SecureStorageTokenStore>();
            if (DependencyService.Get<IHttpClientService>() == null)
            {
                DependencyService.Register<IHttpClientService, HttpClientService>();
            }

            DependencyService.Register<TodoApiService>();
            DependencyService.Register<UsersApiService>();

            DependencyService.Register<TodoService>();
            DependencyService.Register<UserDataStore>();

            Routing.RegisterRoute(nameof(AddEditTodoPage), typeof(AddEditTodoPage));
            Routing.RegisterRoute(nameof(TodoDetailView), typeof(TodoDetailView));
            Routing.RegisterRoute(nameof(RegisterView), typeof(RegisterView));
            Routing.RegisterRoute(nameof(LoginView), typeof(LoginView));
            Routing.RegisterRoute(nameof(TodosPage), typeof(TodosPage));

            if (SessionService.Instance.CurrentUser != null && SessionService.Instance.CurrentUser.IsLoggedIn)
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
            var tokenStore = DependencyService.Get<Services.IAuthTokenStore>();
            var token = await tokenStore.GetToken();

            if (!string.IsNullOrEmpty(token))
            {

                var httpClient = DependencyService.Get<IHttpClientService>().GetClient();
                httpClient.DefaultRequestHeaders.Authorization =
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
            var tokenStore = DependencyService.Get<IAuthTokenStore>();
            tokenStore.ClearToken();

            // Also clear from current HttpClient
            var httpClient = DependencyService.Get<IHttpClientService>().GetClient();
            httpClient.DefaultRequestHeaders.Authorization = null;

        }
    }
}
