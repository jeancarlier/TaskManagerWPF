using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TaskManager.ViewModels;

namespace TaskManager.Commands
{
    internal class TaskDeleteCommand: ICommand
    {
        private TaskViewModel _viewModel;
        public TaskDeleteCommand(TaskViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            return _viewModel.CanDeleteTask();
        }

        public void Execute(object parameter)
        {
            _viewModel.DeleteTask();
        }
    }
}
