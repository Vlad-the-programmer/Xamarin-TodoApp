using System;
using Todo.Helpers.Session;
using Todo.Services;
using Xamarin.Forms;

namespace Todo.ViewModels
{
    public class AddEditTodoViewModel : BaseViewModel
    {
        #region Fields
        private INavigation _navigation;

        private Models.Todo _todo;
        private string _tags;
        private string _text;
        private bool _isDone;
        #endregion

        #region Properties
        public Models.Todo Todo
        {
            get => _todo;
            set => SetProperty(ref _todo, value);
        }

        public string Text
        {
            get => _text;
            set
            {
                if (SetProperty(ref _text, value))
                    Todo.Text = value; // Update Todo when changed
            }
        }

        public bool IsDone
        {
            get => _isDone;
            set
            {
                if (SetProperty(ref _isDone, value))
                    Todo.IsDone = value;
            }
        }

        public string Tags
        {
            get => _tags;
            set => SetProperty(ref _tags, value);
        }
        #endregion

        public AddEditTodoViewModel(INavigation navigation, Models.Todo todo = null)
        {
            PageTitle = "Add Edit Todo Page";
            _navigation = navigation;
            Todo = todo ?? new Models.Todo();

            // Assign existing values if editing
            if (todo != null)
            {
                Text = todo.Text;
                IsDone = todo.IsDone;
                Tags = todo.TagsText;
            }
        }

        #region Commands
        public Command AddCommand => new Command(async () =>
        {
            var existingTodo = await DataStore.GetItemAsync(Todo.Id);

            // Update or add a new item
            if (existingTodo == null)
            {
                Todo.TodoTags.Clear(); // Prevent duplicate tags
                if (!string.IsNullOrWhiteSpace(Tags))
                {
                    Todo.TodoTags.Add(new Models.TodoTag
                    {
                        Tag = new Models.Tag { Name = Tags },
                        TodoId = Todo.Id
                    });
                }

                Todo.User = CurrentUser;
                Todo.UserId = CurrentUser.Id;

                await DataStore.AddItemAsync(Todo);
            }
            else
            {
                existingTodo.Text = Text;
                existingTodo.IsDone = IsDone;
                existingTodo.TodoTags.Clear();
                if (!string.IsNullOrWhiteSpace(Tags))
                {
                    existingTodo.TodoTags.Add(new Models.TodoTag
                    {
                        Tag = new Models.Tag { Name = Tags },
                        TodoId = existingTodo.Id
                    });
                }
                existingTodo.UpdatedAt = DateTime.Now;
                await DataStore.UpdateItemAsync(existingTodo);
            }

            TodoService.Todos.Clear();
            SessionService.Instance.UserTodos = await GetUserTodos();

            //foreach(var todo in SessionService.Instance.UserTodos)
            //    TodoService.Todos.Add(todo);

            await _navigation.PopAsync();
        });
        #endregion
    }
}
