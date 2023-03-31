using System;
using System.Windows.Input;

namespace AMD3DConfigurator.Misc;

public class ActionCommand : ICommand
{
    #region Properties & Fields

    private readonly Func<bool>? _canExecute;
    private readonly Action _command;

    #endregion

    #region Events

    public event EventHandler? CanExecuteChanged;

    #endregion

    #region Constructors

    public ActionCommand(Action command, Func<bool>? canExecute = null)
    {
        this._command = command;
        this._canExecute = canExecute;
    }

    #endregion

    #region Methods

    public bool CanExecute(object? parameter) => _canExecute?.Invoke() ?? true;
    public void Execute(object? parameter) => _command.Invoke();
    public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);

    #endregion
}

public class ActionCommand<TParam> : ICommand
    where TParam : class
{
    #region Properties & Fields

    private readonly Func<TParam?, bool>? _canExecute;
    private readonly Action<TParam?> _command;

    #endregion

    #region Events

    public event EventHandler? CanExecuteChanged;

    #endregion

    #region Constructors

    public ActionCommand(Action<TParam?> command, Func<TParam?, bool>? canExecute = null)
    {
        this._command = command;
        this._canExecute = canExecute;
    }

    #endregion

    #region Methods

    public bool CanExecute(object? parameter) => _canExecute?.Invoke(parameter as TParam) ?? true;
    public void Execute(object? parameter) => _command.Invoke(parameter as TParam);
    public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);

    #endregion
}