using System;
using System.Linq;
using Xamarin.Forms;
using TagModel = Todo.Todo.ApiServices.Tag;
using TodoModel = Todo.Todo.ApiServices.Todo;
using TodoTagModel = Todo.Todo.ApiServices.TodoTag;

namespace Todo.ViewModels
{
    public class AddEditTodoViewModel : BaseViewModel
    {
        #region Fields
        private INavigation _navigation;

        private TodoModel _todo;
        private string _tags;
        private string _text;
        private bool _isDone;
        #endregion

        #region Properties
        public TodoModel Todo
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
                    Todo.Content = value; // Update Todo when changed
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

        public string TagsText => Todo.TodoTags != null && Todo.TodoTags.Any()
        ? string.Join(", ", Todo.TodoTags.Select(t => t.Tag.Name))
        : "No Tags";

        #endregion

        public AddEditTodoViewModel(INavigation navigation, TodoModel todo = null)
        {
            PageTitle = "Add Edit Todo Page";
            _navigation = navigation;
            Todo = todo ?? new TodoModel();

            // Assign existing values if editing
            if (todo != null)
            {
                Text = todo.Content;
                IsDone = todo.IsDone;
                Tags = TagsText;
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
                    Todo.TodoTags.Add(new TodoTagModel
                    {
                        Tag = new TagModel { Name = Tags },
                        TodoId = Todo.Id
                    });
                }

                Todo.User = CurrentUser;
                Todo.UserId = CurrentUser.Id;

                await DataStore.AddItemAsync(Todo);
            }
            else
            {
                existingTodo.Content = Text;
                existingTodo.IsDone = IsDone;
                existingTodo.TodoTags.Clear();
                if (!string.IsNullOrWhiteSpace(Tags))
                {
                    existingTodo.TodoTags.Add(new TodoTagModel
                    {
                        Tag = new TagModel { Name = Tags },
                        TodoId = existingTodo.Id
                    });
                }
                existingTodo.UpdatedAt = DateTime.Now;
                await DataStore.UpdateItemAsync(existingTodo.Id, existingTodo);
            }

            //TodoService.Todos.Clear();
            //SessionService.Instance.UserTodos = await GetUserTodos();

            //foreach(var todo in SessionService.Instance.UserTodos)
            //    TodoService.Todos.Add(todo);

            await _navigation.PopAsync();
        });
        #endregion
    }
}
