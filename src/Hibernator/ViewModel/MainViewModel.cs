// ----------------------------------------------------------------------------
// <copyright file="MainViewModel.cs" company="FPSFA">
//   Copyright © 2013
// </copyright>
// ----------------------------------------------------------------------------
// <author>Poulin Julien - ICT4.2</author>
// <date>06/01/2014</date>
// <project>Hibernator</project>
// ----------------------------------------------------------------------------


namespace Hibernator.ViewModel
{

  using System;
  using System.Collections.ObjectModel;
  using System.Windows.Forms;
  using System.Windows.Input;
  using GalaSoft.MvvmLight;
  using GalaSoft.MvvmLight.Command;
  using Models;

  public class MainViewModel : ViewModelBase
  {

    /// <summary>
    /// Initializes a new instance of the MainViewModel class.
    /// </summary>
    public MainViewModel()
    {
      TimerDurations = new ObservableCollection<TimeSpan>();
      TimerDurations.Add(new TimeSpan(0, 0, 5));
      TimerDurations.Add(new TimeSpan(0, 5, 0));
      TimerDurations.Add(new TimeSpan(0, 10, 0));
      TimerDurations.Add(new TimeSpan(0, 20, 0));
      TimerDurations.Add(new TimeSpan(0, 30, 0));
      TimerDurations.Add(new TimeSpan(0, 45, 0));
      TimerDurations.Add(new TimeSpan(1, 0, 0));
      TimerDurations.Add(new TimeSpan(2, 0, 0));
      TimerDurations.Add(new TimeSpan(3, 0, 0));
      TimerDurations.Add(new TimeSpan(6, 0, 0));
      TimerDurations.Add(new TimeSpan(12, 0, 0));
      SelectedTimerDuration = TimerDurations[3];
      PowerStates = new ObservableCollection<PowerState>();
      PowerStates.Add(PowerState.Hibernate);
      PowerStates.Add(PowerState.Suspend);
      SelectedPowerState = PowerState.Hibernate;
    }

    private CountdownTimer _CountdownTimer;

    public CountdownTimer CountdownTimer
    {
      get { return _CountdownTimer; }
      set
      {
        if (_CountdownTimer != value)
        {
          if (_CountdownTimer != null)
          {
            _CountdownTimer.CountdownComplete -= CountdownTimer_CountdownComplete;
          }

          _CountdownTimer = value;

          if (_CountdownTimer != null)
          {
            _CountdownTimer.CountdownComplete += CountdownTimer_CountdownComplete;
          }

          RaisePropertyChanged(() => CountdownTimer);
        }
      }
    }

    private bool _IsHibernating;

    public bool IsHibernating
    {
      get { return _IsHibernating; }
      set
      {
        _IsHibernating = value;
        RaisePropertyChanged(() => IsHibernating);
      }
    }

    public ObservableCollection<PowerState> PowerStates { get; set; }

    private PowerState _SelectedPowerState;
    public PowerState SelectedPowerState
    {
      get { return _SelectedPowerState; }
      set
      {
        _SelectedPowerState = value;
        RaisePropertyChanged(() => SelectedPowerState);
      }
    }

    public ObservableCollection<TimeSpan> TimerDurations { get; private set; }

    private TimeSpan _SelectedTimerDuration;

    public TimeSpan SelectedTimerDuration
    {
      get { return _SelectedTimerDuration; }
      set
      {
        _SelectedTimerDuration = value;
        RaisePropertyChanged(() => SelectedTimerDuration);
      }
    }

    #region Commands

    private RelayCommand _StartCommand;

    public ICommand StartCommand
    {
      get { return _StartCommand ?? (_StartCommand = new RelayCommand(Start, CanStart)); }
    }

    private bool CanStart()
    {
      return CountdownTimer == null || (!CountdownTimer.IsRunning && !IsHibernating);
    }

    private void Start()
    {
      CountdownTimer = new CountdownTimer(SelectedTimerDuration);
      CountdownTimer.Start();
    }

    private void CountdownTimer_CountdownComplete(object sender, EventArgs e)
    {
      IsHibernating = true;
      _StartCommand.RaiseCanExecuteChanged();
      _StopCommand.RaiseCanExecuteChanged();
      Application.SetSuspendState(SelectedPowerState, true, true);
    }

    private RelayCommand _StopCommand;

    public ICommand StopCommand
    {
      get { return _StopCommand ?? (_StopCommand = new RelayCommand(Stop, CanStop)); }
    }

    private bool CanStop()
    {
      return CountdownTimer != null && (CountdownTimer.IsRunning && !IsHibernating);
    }

    private void Stop()
    {
      CountdownTimer.Stop();
    }

    #endregion

  }

}