using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todo.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Todo.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TodoDetailView : ContentPage
    {
        public TodoDetailView(Models.Todo todo = null)
        {
            InitializeComponent();
            BindingContext = new DetailTodoViewModel(todo, Navigation);
        }
    }
}