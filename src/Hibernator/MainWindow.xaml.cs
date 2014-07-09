// ----------------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="FPSFA">
//   Copyright © 2013
// </copyright>
// ----------------------------------------------------------------------------
// <author>Poulin Julien - ICT4.2</author>
// <date>06/01/2014</date>
// <project>Hibernator</project>
// ----------------------------------------------------------------------------

namespace Hibernator
{

  using System;
  using System.Windows;
  using System.Windows.Input;
  using GalaSoft.MvvmLight.Command;
  using Microsoft.Win32;

  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {

    private ICommand _RestoreWindowCommand;

    public MainWindow()
    {
      InitializeComponent();
      SystemEvents.PowerModeChanged +=SystemEvents_PowerModeChanged;
    }

    private void SystemEvents_PowerModeChanged(object sender, PowerModeChangedEventArgs e)
    {
      if(e.Mode == PowerModes.Resume)
      {
        UpdateLayout();
      }
    }

    public ICommand RestoreWindowCommand
    {
      get { return _RestoreWindowCommand ?? (_RestoreWindowCommand = new RelayCommand(RestoreWindow)); }
    }

    private void RestoreWindow()
    {
      WindowState = WindowState.Normal;
    }

    protected override void OnStateChanged(EventArgs e)
    {
      base.OnStateChanged(e);

      ShowInTaskbar = WindowState == WindowState.Normal;
    }

  }

}