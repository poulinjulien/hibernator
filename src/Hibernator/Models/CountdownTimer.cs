// -----------------------------------------------------------------------
// <copyright file="CountdownTimer.cs" company="WebForAll">
//   Copyright © WebForAll. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
// <author>Julien Poulin</author>
// <date>06/01/2014</date>
// <project>Hibernator</project>
// <web>http://www.webforall.be</web>
// -----------------------------------------------------------------------

namespace Hibernator.Models
{

  using System;
  using System.Threading;
  using GalaSoft.MvvmLight;

  public class CountdownTimer : ObservableObject
  {

    private const int TimerInterval = 100;

    private readonly Timer _Timer;

    private TimeSpan _Remaining;

    public TimeSpan Remaining
    {
      get { return _Remaining; }
      private set
      {
        if (_Remaining != value)
        {
          _Remaining = value;
          RaisePropertyChanged(() => Remaining);
        }
      }
    }

    public TimeSpan Duration { get; private set; }

    private bool _IsRunning;

    private DateTime _EndTime;

    public bool IsRunning
    {
      get { return _IsRunning; }
      private set
      {
        if (_IsRunning != value)
        {
          _IsRunning = value;
          RaisePropertyChanged(() => IsRunning);
        }
      }
    }

    public event EventHandler CountdownComplete;

    protected virtual void OnCountdownComplete()
    {
      var handler = CountdownComplete;
      if (handler != null)
      {
        handler(this, EventArgs.Empty);
      }
    }

    public CountdownTimer(TimeSpan duration)
    {
      Duration = duration;
      _Timer = new Timer(_ => TimerCallback(), null, Timeout.Infinite, Timeout.Infinite);
    }

    private void TimerCallback()
    {
      if (IsRunning)
      {
        Remaining = _EndTime - DateTime.UtcNow;
        if (Remaining <= TimeSpan.Zero)
        {
          StopTimer();
          OnCountdownComplete();
        }
      }
      else
      {
        StopTimer();
      }
    }

    private void StartTimer()
    {
      _EndTime = DateTime.UtcNow.Add(Duration);
      _Timer.Change(TimerInterval, TimerInterval);
    }

    private void StopTimer()
    {
      _Timer.Change(Timeout.Infinite, Timeout.Infinite);
    }

    public void Start()
    {
      if (!IsRunning)
      {
        IsRunning = true;
        StartTimer();
      }
    }

    public void Stop()
    {
      if (IsRunning)
      {
        IsRunning = false;
      }
    }

    public void Reset()
    {
      Stop();
      Remaining = Duration;
    }

  }

}