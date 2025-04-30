using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Todo.Helpers.Session;
using Todo.Services;
using Xamarin.Forms;
using TodoModel = Todo.Todo.ApiServices.Todo;
using UserModel = Todo.Todo.ApiServices.User;

namespace Todo.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public ITodoService DataStore => DependencyService.Get<ITodoService>();
        public IUserDatStore UserDataStore => DependencyService.Get<IUserDatStore>();

        #region Global Fields

        private UserModel _currentUser;
        public UserModel CurrentUser
        {
            get => _currentUser;
            set
            {
                SetProperty(ref _currentUser, value);
                OnPropertyChanged(nameof(IsUserLoggedIn)); // Notify UI when user logs in/out
                OnPropertyChanged(nameof(IsUserLoggedOut));
            }
        }

        public bool IsUserLoggedIn => CurrentUser != null && SessionService.Instance.IsCurrentUserLoggedIn;
        public bool IsUserLoggedOut => !IsUserLoggedIn;

        //private IList<TodoModel> _usersTodos;
        //public IList<TodoModel> UsersTodos
        //{
        //    get => _usersTodos;
        //    set
        //    {
        //        SetProperty(ref _usersTodos, value);
        //    }
        //}

        bool isBusy = false;
        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }

        string title = string.Empty;
        public string PageTitle
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        private bool _isRefreshing;
        public bool IsRefreshing
        {
            get => _isRefreshing;
            set => SetProperty(ref _isRefreshing, value);
        }
        #endregion

        public BaseViewModel()
        {
            CurrentUser = SessionService.Instance.CurrentUser;
            //UsersTodos = SessionService.Instance.UserTodos;

            Debug.WriteLine("LoggedIn: " + IsUserLoggedIn);
        }

        public async Task<List<TodoModel>> GetUserTodos()
        {
            IsRefreshing = true;
            try
            {
                return await UserDataStore.GetUserTodos(CurrentUser.Id);
            }
            finally
            {
                IsRefreshing = false;
            }
        }


        protected bool SetProperty<T>(ref T backingStore, T value,
            [CallerMemberName] string propertyName = "",
            Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
