using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace PointMarker.ViewModel
{
    /// <summary>
    /// Простейшая реализация ICommand из интернета
    /// </summary>

    public class RelayCommand : ICommand
    {
        #region Static
        public static readonly string NullParameterValue = "<N/A>";
        /// <summary>
        /// Всегда недоступная команда
        /// </summary>
        public static readonly RelayCommand DisabledCommand = new RelayCommand(p => { }, p => false);

        /// <summary>
        /// Команда, которая не производит никаких действий
        /// </summary>
        public static readonly RelayCommand DoNothingCommand = new RelayCommand(p => { });

        /// <summary>
        /// Команда перезапуска
        /// </summary>
        public static readonly RelayCommand RestartCommand = new RelayCommand(p =>
        {
            // запускаю процесс с теми же аргументами
            Process.Start(Application.ResourceAssembly.Location, string.Join(" ", Environment.GetCommandLineArgs().Skip(1)));
            Application.Current.Shutdown();
        });

        /// <summary>
        /// Команда завершения работы
        /// </summary>
        public static readonly RelayCommand ShutdownCommand = new RelayCommand(p =>
        {
            Application.Current.Shutdown();
        });

        #endregion

        /// <summary>
        /// Метод выполнения команды
        /// </summary>
        private readonly Action<object> _execute;

        /// <summary>
        /// Метод проверки доступности команды
        /// </summary>
        private readonly Predicate<object> _canExecute;

        /// <summary>
        /// Конструктор от обязательных параметров
        /// </summary>
        /// <param name="execute">метод выполнения команды</param>
        /// <param name="canExecute">метод проверки доступности команды</param>
        public RelayCommand(Action<object> execute, Predicate<object> canExecute = null)
        {
            if (execute == null) throw new ArgumentNullException(nameof(execute));

            _execute = execute;
            _canExecute = canExecute;
        }

        #region ICommand Members

        /// <inheritdoc/>
        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute(parameter);
        }

        /// <inheritdoc/>
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        /// <inheritdoc/>
        public void Execute(object parameter)
        {
            _execute(parameter ?? NullParameterValue);
        }

        #endregion
    }

}

/*
public class RelayCommand<T> : ICommand
{
    #region Fields

    readonly Action<T> _execute = null;
    readonly Predicate<T> _canExecute = null;

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of <see cref="DelegateCommand{T}"/>.
    /// </summary>
    /// <param name="execute">Delegate to execute when Execute is called on the command.  This can be null to just hook up a CanExecute delegate.</param>
    /// <remarks><seealso cref="CanExecute"/> will always return true.</remarks>
    public RelayCommand(Action<T> execute)
        : this(execute, null)
    {
    }

    /// <summary>
    /// Creates a new command.
    /// </summary>
    /// <param name="execute">The execution logic.</param>
    /// <param name="canExecute">The execution status logic.</param>
    public RelayCommand(Action<T> execute, Predicate<T> canExecute)
    {
        if (execute == null)
            throw new ArgumentNullException("execute");

        _execute = execute;
        _canExecute = canExecute;
    }

    #endregion

    #region ICommand Members

    ///<summary>
    ///Defines the method that determines whether the command can execute in its current state.
    ///</summary>
    ///<param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
    ///<returns>
    ///true if this command can be executed; otherwise, false.
    ///</returns>
    public bool CanExecute(object parameter)
    {
        return _canExecute == null ? true : _canExecute((T)parameter);
    }

    ///<summary>
    ///Occurs when changes occur that affect whether or not the command should execute.
    ///</summary>
    public event EventHandler CanExecuteChanged
    {
        add { CommandManager.RequerySuggested += value; }
        remove { CommandManager.RequerySuggested -= value; }
    }

    ///<summary>
    ///Defines the method to be called when the command is invoked.
    ///</summary>
    ///<param name="parameter">Data used by the command. If the command does not require data to be passed, this object can be set to <see langword="null" />.</param>
    public void Execute(object parameter)
    {
        _execute((T)parameter);
    }

    #endregion
}

}
*/
