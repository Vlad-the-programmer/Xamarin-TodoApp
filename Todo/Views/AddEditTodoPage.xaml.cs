using Todo.ViewModels;
using Xamarin.Forms;

using TodoModel = Todo.ApiServices.Todo;

namespace Todo.Views
{
    public partial class AddEditTodoPage : ContentPage
    {
        private AddEditTodoViewModel _viewModel;

        public AddEditTodoPage(TodoModel todo = null)
        {
            InitializeComponent();

            _viewModel = new AddEditTodoViewModel(Navigation, todo);
            BindingContext = _viewModel;
        }
    }
}
