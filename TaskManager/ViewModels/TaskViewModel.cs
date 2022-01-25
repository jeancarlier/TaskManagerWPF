using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using TaskManager.Commands;
using TaskManager.Models;

namespace TaskManager.ViewModels
{
    /// <summary>
    /// View Model for Tasks
    /// </summary>
    public class TaskViewModel: INotifyPropertyChanged
    {
        private string _description;
        private DateTime _date;
        private bool _concluded;
        private string _currentDate;
        
        private ObservableCollection<TaskModel> _tasks = new ObservableCollection<TaskModel>();
        private TaskModel _selectedTask;
        
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        /// <summary>
        /// Task ViewModel Constructor
        /// </summary>
        public TaskViewModel()
        {
            TasksList.Add(new TaskModel { Id = 1, Description = "Create user form", Date = new DateTime(2022, 2, 15), Completed = false, Overdue = isOverdue(new DateTime(2022, 2, 15), false), State = getStateDescription(new DateTime(2022, 2, 15), false) });
            TasksList.Add(new TaskModel { Id = 2, Description = "Create delete user functionality", Date = new DateTime(2022, 2, 10), Completed = false, Overdue = isOverdue(new DateTime(2022, 2, 10), false), State = getStateDescription(new DateTime(2022, 2, 10), false) });
            TasksList.Add(new TaskModel { Id = 3, Description = "Create edit user form", Date = new DateTime(2022, 2, 16), Completed = true, Overdue = isOverdue(new DateTime(2022, 2, 16), true), State = getStateDescription(new DateTime(2022, 2, 16), true) });
            TasksList.Add(new TaskModel { Id = 4, Description = "Test full application", Date = new DateTime(2022, 1, 10), Completed = false, Overdue = isOverdue(new DateTime(2022, 1, 10), false), State = getStateDescription(new DateTime(2022, 1, 10), false) });
            TasksList.Add(new TaskModel { Id = 5, Description = "Deploy 1st version of application", Date = new DateTime(2022, 2, 20), Completed = false, Overdue = isOverdue(new DateTime(2022, 2, 20), false), State = getStateDescription(new DateTime(2022, 2, 20), false) });

            DeleteCommand = new TaskDeleteCommand(this);
            NewTaskCommand = new NewTaskCommand(this);
            SaveTaskCommand = new SaveTaskCommand(this);

            CurrentDate = DateTime.Now.ToString("MMMM dd, yyyy");
        }

        /// <summary>
        /// Task description
        /// </summary>
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        /// <summary>
        /// Estimate task finish date
        /// </summary>
        public DateTime Date
        {
            get { return _date; }
            set { _date = value; }
        }

        /// <summary>
        /// Identifies if the task is done
        /// </summary>
        public bool Completed
        {
            get { return _concluded; }
            set { _concluded = value; }
        }

        /// <summary>
        /// Represents the current date
        /// </summary>
        public string CurrentDate
        {
            get { return _currentDate; }
            set { _currentDate = value; }
        }

        /// <summary>
        /// List of tasks
        /// </summary>
        public ObservableCollection<TaskModel> TasksList
        {
            get { return _tasks; }
            set { _tasks = value; }
        }

        /// <summary>
        /// Current selected task
        /// </summary>
        public TaskModel SelectedTask
        {
            get
            {
                return _selectedTask;
            }
            set
            {
                if (value != null)
                {
                    _selectedTask = new TaskModel
                    {
                        Id = value.Id,
                        Description = value.Description,
                        Date = value.Date,
                        Overdue = value.Overdue,
                        Completed = value.Completed,
                        State = value.State
                    };
                    this.OnPropertyChanged("SelectedTask");
                }
                else {
                    _selectedTask = value;
                }
            }
        }

        /// <summary>
        /// Defines if the tasks can be deleted and allow to be deleted in UI
        /// </summary>
        /// <returns></returns>
        public bool CanDeleteTask()
        {
            if (SelectedTask == null || (SelectedTask != null && String.IsNullOrWhiteSpace(SelectedTask.Description)))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Defines if the edit fields can be saved
        /// </summary>
        /// <returns></returns>
        public bool CanSaveTask()
        {
            if (SelectedTask != null && SelectedTask.Id > 0 && !String.IsNullOrWhiteSpace(SelectedTask.Description) && SelectedTask.Date != null)
            {
                return true;
            }
            else 
            {
                return false;
            }
        }


        /// <summary>
        /// Deletes the current selected task
        /// </summary>
        public void DeleteTask()
        {
            try
            {
                if (SelectedTask != null && MessageBox.Show($"Do you really want to delete the task {SelectedTask.Id}?", "Warning", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    TasksList.Remove(TasksList.Where(T => T.Id == SelectedTask.Id).First());                    
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fail to delete task: " + ex.Message, "Error", MessageBoxButton.OK);
            }
        }

        /// <summary>
        /// Clear the current selected task and prepares to edit and save
        /// </summary>
        public void NewTask()
        {
            try
            {
                var index = TasksList.Count > 0 ? TasksList.Max(o => o.Id) + 1 : 1;
                SelectedTask = new TaskModel
                {
                    Id = index,
                    Description = "",
                    Date = DateTime.Now,
                    Completed = false,
                    Overdue = isOverdue(DateTime.Now, false)
                };

                this.OnPropertyChanged("SelectedTask");

            }
            catch (Exception ex)
            {
                MessageBox.Show("Fail to add task: " + ex.Message, "Error", MessageBoxButton.OK);
            }
        }

        /// <summary>
        /// Save task operation to the list of tasks
        /// </summary>
        public void Save() {
            if (SelectedTask != null)
            {
                SelectedTask.Overdue = isOverdue(SelectedTask.Date, SelectedTask.Completed);
                SelectedTask.State = getStateDescription(SelectedTask.Date, SelectedTask.Completed);

                var existentRecord = TasksList.Where(T => T.Id == SelectedTask.Id).FirstOrDefault();
                if (existentRecord == null)
                {
                    TasksList.Add(SelectedTask);
                    this.OnPropertyChanged("TaskList");
                }
                else {
                    existentRecord.Description = SelectedTask.Description;
                    existentRecord.Date = SelectedTask.Date;
                    existentRecord.Completed = SelectedTask.Completed;
                    existentRecord.State = getStateDescription(SelectedTask.Date, SelectedTask.Completed);
                    existentRecord.Overdue = isOverdue(SelectedTask.Date, SelectedTask.Completed);
                }
                                
                SelectedTask = null;
                this.OnPropertyChanged("SelectedTask");
            }

        }

        /// <summary>
        /// Defines if the task is overdue according to the task estimated date and current date
        /// </summary>
        /// <param name="date">Task date</param>
        /// <param name="concluded">Is task done</param>
        /// <returns>Boolean</returns>
        public bool isOverdue(DateTime date, bool concluded)
        {
            if (!concluded && DateTime.Now > date)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Return the task description status
        /// </summary>
        /// <param name="date">Task Date</param>
        /// <param name="concluded">Is Task done</param>
        /// <returns>A string with status description</returns>
        public string getStateDescription(DateTime date, bool concluded) 
        {
            if (concluded)
            {
                return "Completed";
            }
            else if (isOverdue(date,concluded))
            {
                return "Overdue";
            }
            else {
                return "Pending";
            }
        }

        /// <summary>
        /// Gets the delete command for the ViewModel 
        /// </summary>
        public ICommand DeleteCommand 
        {
            get; private set;
        }

        /// <summary>
        /// Gets the new task command for the ViewModel 
        /// </summary>
        public ICommand NewTaskCommand
        {
            get; private set;
        }

        /// <summary>
        /// Gets the new task command for the ViewModel 
        /// </summary>
        public ICommand SaveTaskCommand
        {
            get; private set;
        }
    }
}
