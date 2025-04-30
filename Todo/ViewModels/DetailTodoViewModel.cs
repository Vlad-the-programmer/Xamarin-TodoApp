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
using TodoModel = Todo.ApiServices.Todo;

namespace Todo.ViewModels
{
    //[QueryProperty(nameof(TodoId), "todoId")]
    public class DetailTodoViewModel : BaseViewModel
    {
        #region Fields
        private INavigation _navigation;

        private TodoModel _todo;
        #endregion

        #region Properties
        public TodoModel Todo
        {
            get => _todo;
            set => SetProperty(ref _todo, value);
        }

        public string TagsText => Todo.TodoTags != null && Todo.TodoTags.Any()
        ? string.Join(", ", Todo.TodoTags.Select(t => t.Tag.Name))
        : "No Tags";

        private int _todoId;
        public int TodoId
        {
            get => _todoId;
            set
            {
                _todoId = value;
                LoadTodo().GetAwaiter().GetResult();
            }
        }

        public ObservableCollection<string> Tags { get; set; }

        public ICommand CloseCommand { get; }
        #endregion

        #region Commands
        public ICommand UpdateCommand => new Command(async () =>
        {
            if (Todo != null)
            {
                if (Todo.UserId != CurrentUser.Id) return;

                // Navigate to the Update Todo page with the selected Todo
                await _navigation.PushAsync(new AddEditTodoPage(Todo));
            }
        });

        public ICommand RemoveCommand => new Command(async () =>
        {
            if (Todo != null)
            {
                if (Todo.UserId != CurrentUser.Id) return;

                var deleteItem = await Application.Current.MainPage.DisplayAlert(
                    "Remove", $"Remove the todo {Todo.Content}", "OK", "Never mind");
                if (deleteItem)
                {
                    await DataStore.DeleteItemAsync(Todo.Id);
                    SessionService.Instance.UserTodos = await GetUserTodos();

                    var _searchTerm = DataStore.SearchTerm;
                    SearchFuncs.Search(string.Empty, ref _searchTerm, TodoService.AllTodos, TodoService.Todos);
                    await _navigation.PopAsync();
                }
            }
        });
        #endregion
        public DetailTodoViewModel(TodoModel todo, INavigation navigation)
        {
            PageTitle = "Todo Detail Page";

            _navigation = navigation;
            Todo = todo;
            Tags = new ObservableCollection<string>(todo.TodoTags?.Select(tt => tt.Tag.Name) ?? new List<string>());
            ;

            CloseCommand = new Command(async () => await navigation.PopAsync());
        }

        private async Task LoadTodo()
        {
            Todo = (await GetUserTodos())
                .FirstOrDefault(t => t.Id == _todoId);

        }
    }
}
