using System;
using Todo.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using TodoModel = Todo.ApiServices.Todo;

namespace Todo.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TodosPage : ContentPage
    {
        private TodosViewModel _viewModel;

        public TodosPage()
        {
            InitializeComponent();

            _viewModel = new TodosViewModel();
            BindingContext = _viewModel;

            MessagingCenter.Subscribe<LoginViewModel>(this, "UserLoggedIn", (sender) => UpdateToolbarItems());
            MessagingCenter.Subscribe<TodosViewModel>(this, "UserLoggedOut", (sender) => UpdateToolbarItems());
            UpdateToolbarItems();

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(_viewModel.IsUserLoggedIn))
                    UpdateToolbarItems();
            };
            UpdateToolbarItems(); // Refresh Toolbar when page reappears
        }


        private void UpdateToolbarItems()
        {
            ToolbarItems.Clear();

            if (_viewModel.IsUserLoggedIn)
            {
                ToolbarItems.Add(AddTodoToolbarItem);
                ToolbarItems.Add(LogoutToolbarItem);
            }
            else
            {
                ToolbarItems.Add(LoginToolbarItem);
                ToolbarItems.Add(RegisterToolbarItem);
            }
        }

        private async void ItemTapped(object sender, ItemTappedEventArgs e)
        {
            //TodosListView.SelectedItem = null;
            var tappedItem = (sender as Element)?.BindingContext as TodoModel;
            if (tappedItem == null) return;

            _viewModel.ItemTapped(tappedItem);
            //TodosListView.ItemsSource = null;
            //TodosListView.ItemsSource = await _viewModel.GetUserTodos();
            ItemsListView.ItemsSource = null;
            ItemsListView.ItemsSource = await _viewModel.GetUserTodos();
            ((ListView)sender).SelectedItem = null;
        }

        private async void OnTodoTapped(object sender, EventArgs e)
        {
            if (sender is StackLayout layout && layout.BindingContext is TodoModel todo)
            {
                _viewModel.ItemTapped(todo);

                // Refresh the viewmodel's list
                _viewModel.LoadItemsCommand.Execute(null);
            }
        }

    }
}
