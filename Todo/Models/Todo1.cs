using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Todo.Models
{
    public class Todo: INotifyPropertyChanged
    {
        private string _text;
        private bool _isDone;

        public int Id { get; set; }

        public string Text
        {
            get => _text;
            set
            {
                if (_text != value)
                {
                    _text = value;
                    OnPropertyChanged(nameof(Text));
                }
            }
        }

        public bool IsDone
        {
            get => _isDone;
            set
            {
                if (_isDone != value)
                {
                    _isDone = value;
                    OnPropertyChanged(nameof(IsDone));
                }
            }
        }

        public int UserId { get; set; }  // This is the foreign key
        public User User { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; }

        public IList<TodoTag> TodoTags { get; set; } = new List<TodoTag>();

        public string TagsText => TodoTags != null && TodoTags.Any()
        ? string.Join(", ", TodoTags.Select(t => t.Tag.Name))
        : "No Tags";

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
