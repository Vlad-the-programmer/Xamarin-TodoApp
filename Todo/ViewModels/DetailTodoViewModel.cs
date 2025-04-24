using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Todo.Helpers;
using Todo.Helpers.Session;
using Todo.Models;
using Todo.Services;
using Todo.Views;
using Xamarin.Forms;

namespace Todo.ViewModels
{
    //[QueryProperty(nameof(TodoId), "todoId")]
    public class DetailTodoViewModel : BaseViewModel
    {
        #region Fields
        private INavigation _navigation;

        private Models.Todo _todo;
        #endregion

        #region Properties
        public Models.Todo Todo
        {
            get => _todo;
            set => SetProperty(ref _todo, value);
        }

        private int _todoId;
        public int TodoId
        {
            get => _todoId;
            set
            {
                _todoId = value;
                LoadTodo();
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
                    "Remove", $"Remove the todo {Todo.Text}", "OK", "Never mind");
                if (deleteItem)
                {
                    await DataStore.DeleteItemAsync(Todo);
                    SessionService.Instance.UserTodos = await GetUserTodos();

                    var _searchTerm = DataStore.SearchTerm;
                    SearchFuncs.Search(string.Empty, ref _searchTerm, TodoService.AllTodos, TodoService.Todos);
                    await _navigation.PopAsync();
                }
            }
        });
        #endregion
        public DetailTodoViewModel(Models.Todo todo, INavigation navigation)
        {
            PageTitle = "Todo Detail Page";

            _navigation = navigation;
            Todo = todo;
            Tags = new ObservableCollection<string>(todo.TodoTags?.Select(tt => tt.Tag.Name) ?? new List<string>());
;

            CloseCommand = new Command(async () => await navigation.PopAsync());
        }

        private void LoadTodo()
        {
            Todo = SessionService.Instance.UserTodos
                .FirstOrDefault(t => t.Id == _todoId);

        }
    }
}
