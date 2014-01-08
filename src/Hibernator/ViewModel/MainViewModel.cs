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
  using System.Threading;
  using System.Windows.Forms;
  using System.Windows.Input;
  using GalaSoft.MvvmLight;
  using GalaSoft.MvvmLight.Command;
  using Timer = System.Threading.Timer;

  public class MainViewModel : ViewModelBase
  {

    private const int TimerInterval = 100;

    private readonly Timer _Timer;

    private DateTime? _HibernateTime;

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
      _Timer = new Timer(_ => TimerCallback(), null, Timeout.Infinite, Timeout.Infinite);
    }

    private void TimerCallback()
    {
      if (_HibernateTime.HasValue)
      {
        var now = DateTime.Now;
        RemainingTime = _HibernateTime.Value - now;
        if (now > _HibernateTime)
        {
          Hibernate();
        }
      }
    }

    private void StopTimer()
    {
      _Timer.Change(Timeout.Infinite, Timeout.Infinite);
    }

    private void StartTimer()
    {
      _Timer.Change(TimerInterval, TimerInterval);
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

    private void Hibernate()
    {
      IsHibernating = true;
      StopTimer();
      _StartCommand.RaiseCanExecuteChanged();
      _StopCommand.RaiseCanExecuteChanged();
      Application.SetSuspendState(SelectedPowerState, true, true);
    }

    public ObservableCollection<PowerState> PowerStates { get; set; }

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
        RemainingTime = _SelectedTimerDuration;
      }
    }

    private TimeSpan _RemainingTime;

    public TimeSpan RemainingTime
    {
      get { return _RemainingTime; }
      set
      {
        _RemainingTime = value;
        RaisePropertyChanged(() => RemainingTime);
      }
    }

    private bool _IsTimerStarted;

    public bool IsTimerStarted
    {
      get { return _IsTimerStarted; }
      set
      {
        _IsTimerStarted = value;
        RaisePropertyChanged(() => IsTimerStarted);
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
      return !IsTimerStarted && !IsHibernating;
    }

    private void Start()
    {
      _HibernateTime = DateTime.Now.Add(SelectedTimerDuration);
      StartTimer();
      IsTimerStarted = true;
    }

    private RelayCommand _StopCommand;
    private PowerState _SelectedPowerState;

    public ICommand StopCommand
    {
      get { return _StopCommand ?? (_StopCommand = new RelayCommand(Stop, CanStop)); }
    }

    private bool CanStop()
    {
      return IsTimerStarted && ! IsHibernating;
    }

    private void Stop()
    {
      _HibernateTime = null;
      RemainingTime = SelectedTimerDuration;
      StopTimer();
      IsTimerStarted = false;
    }

    #endregion
  }

}