using Todo.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using TodoModel = Todo.Todo.ApiServices.Todo;

namespace Todo.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TodoDetailView : ContentPage
    {
        public TodoDetailView(TodoModel todo = null)
        {
            InitializeComponent();
            BindingContext = new DetailTodoViewModel(todo, Navigation);
        }
    }
}