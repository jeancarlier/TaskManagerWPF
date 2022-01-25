using System;
using System.ComponentModel;

namespace TaskManager.Models
{
    /// <summary>
    /// Class that represents a task
    /// </summary>
    public class TaskModel: INotifyPropertyChanged
    {
        private int _id;
        private string _description;
        private DateTime _date;
        private bool _concluded;
        private bool _overdue;
        private string _state;

        /// <summary>
        /// Task Id
        /// </summary>
        public int Id
        {
            get { return _id; }
            set { 
                _id = value;
                this.OnPropertyChanged("Id");
            }
        }

        /// <summary>
        /// Task Description
        /// </summary>
        public string Description
        {
            get { return _description; }
            set
            {
                _description = value;
                this.OnPropertyChanged("Description");
            }
        }

        /// <summary>
        /// Task estimated date
        /// </summary>
        public DateTime Date
        {
            get { return _date; }
            set
            {
                _date = value;
                this.OnPropertyChanged("Date");
            }
        }

        /// <summary>
        /// Task Concluded
        /// </summary>
        public bool Concluded 
        {
            get { return _concluded; }
            set
            {
                _concluded = value;
                this.OnPropertyChanged("Concluded");
            }
        }

        /// <summary>
        /// Task Overdue
        /// </summary>
        public bool Overdue {
            get { return _overdue; }
            set
            {
                _overdue = value;
                this.OnPropertyChanged("Overdue");
            }
        }

        /// <summary>
        /// Task State Description
        /// </summary>
        public string State {
            get { return _state; }
            set
            {
                _state = value;
                this.OnPropertyChanged("State");
            }
        }

        /// <summary>
        /// Force binding to all properties
        /// </summary>
        public void UpdateAllProperties() 
        {
            this.OnPropertyChanged("Description");
            this.OnPropertyChanged("Date");
            this.OnPropertyChanged("Concluded");
            this.OnPropertyChanged("Overdue");
            this.OnPropertyChanged("State");
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName) { 
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) 
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }
}
