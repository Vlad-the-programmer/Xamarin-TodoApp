using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Todo.Helpers;
using Todo.Helpers.Session;
using Todo.Services;
using Todo.Views;
using Xamarin.Forms;

namespace Todo.ViewModels
{
    public class TodosViewModel : BaseViewModel
    {
        #region Fields
        private INavigation _navigation;
        #endregion

        #region Properties
        public ObservableCollection<Models.Todo> Todos
        {
            get => new ObservableCollection<Models.Todo>(SessionService.Instance.UserTodos);
            set => SetProperty(ref TodoService.Todos, value);
        }


        public string SearchTerm { get; set; } = "";
        #endregion

        public TodosViewModel()
        {
            PageTitle = "Todos";

            Todos = new ObservableCollection<Models.Todo>(
                (SessionService.Instance.UserTodos != null && SessionService.Instance.UserTodos.Any())
                ? SessionService.Instance.UserTodos.ToList()
                : new List<Models.Todo>()
            );
        }

        #region Commands
        public ICommand LoadItemsCommand => new Command(async () => await GetUserTodos());

        public ICommand DetailCommand => new Command<Models.Todo>(async (todo) =>
        {
            if (todo != null)
            {
                // Navigate to Todo detail page
                await Application.Current.MainPage.Navigation.PushAsync(new TodoDetailView(todo));
                //await Shell.Current.GoToAsync($"TodoDetailView?todoId={todo.Id}");
            }
        });

        public ICommand AddTodoCommand
        {
            get => new Command(async () =>
            {
                await Application.Current.MainPage.Navigation.PushAsync(new AddEditTodoPage());
                RefreshTodosForCurrentUser();
            });
        }

        public ICommand UpdateCommand => new Command<Models.Todo>(async (todo) =>
        {
            if (todo != null)
            {
                if (todo.UserId != CurrentUser.Id) return;

                // Navigate to the Update Todo page with the selected Todo
                await Application.Current.MainPage.Navigation.PushAsync(new AddEditTodoPage(todo));
                RefreshTodosForCurrentUser();
            }
        });

        public ICommand RemoveCommand => new Command<Models.Todo>(async (todo) =>
        {
            if (todo != null)
            {
                if (todo.UserId != CurrentUser.Id) return;
                var deleteItem = await Application.Current.MainPage.DisplayAlert(
                    "Remove", $"Remove the todo {todo.Text}", "OK", "Never mind");
                if (deleteItem)
                {
                    await DataStore.DeleteItemAsync(todo);
                    // Refresh the Todos list
                    //SessionService.Instance.UserTodos = await GetUserTodos();
                    //Todos = SessionService.Instance.UserTodos;
                    RefreshTodosForCurrentUser();
                }
            }
        });

        public Command LoginCommand
        {
            get => new Command(async () =>
            {
                await Application.Current.MainPage.Navigation.PushAsync(new LoginView());
            });
        }

        public Command RegisterCommand
        {
            get => new Command(async () =>
            {
                await Application.Current.MainPage.Navigation.PushAsync(new RegisterView());
            });
        }

        public ICommand LogoutCommand
        {
            get => new Command(async () =>
            {
                await OnLogout();
                MessagingCenter.Send(this, "UserLoggedOut"); // Notify TodosPage
            });
        }


        public Command SearchCommand
        {
            get
            {
                return new Command<string>(async (searchTerm) =>
                {
                    var _searchTerm = DataStore.SearchTerm;
                    SearchFuncs.Search(searchTerm, ref _searchTerm, TodoService.AllTodos, Todos);
                    RefreshTodosForCurrentUser();
                });
            }
        }

        #endregion

        #region helperFuncs
        private async Task OnLogout()
        {
            if (CurrentUser != null)
            {
                CurrentUser.IsLoggedIn = false;
                CurrentUser = null;
                OnPropertyChanged(nameof(IsUserLoggedIn));
                OnPropertyChanged(nameof(IsUserLoggedOut));
                OnPropertyChanged(nameof(CurrentUser));

                Todos.Clear(); // Clear todos on logout
                UsersTodos.Clear();
                SessionService.Instance.Logout();
                await Application.Current.MainPage.Navigation.PushAsync(new LoginView());
            }
        }

        public async void ItemTapped(Models.Todo todo)
        {
            todo.IsDone = !todo.IsDone;

            await DataStore.UpdateItemAsync(todo); // Save to data store
            await DbFileHandler.SaveTodos((IList<Models.Todo>)await DataStore.GetItemsAsync()); // Persist to file/db
        }


        private async Task RefreshTodosForCurrentUser()
        {
            Todos.Clear();
            var userTodos = await GetUserTodos();

            foreach (var todo in userTodos)
                Todos.Add(todo);
        }
        #endregion
    }
}
