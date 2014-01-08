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

  using System.Windows;
  using System.Windows.Input;
  using GalaSoft.MvvmLight.Command;

  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {

    private ICommand _RestoreWindowCommand;

    public MainWindow()
    {
      InitializeComponent();
    }

    public ICommand RestoreWindowCommand
    {
      get { return _RestoreWindowCommand ?? (_RestoreWindowCommand = new RelayCommand(RestoreWindow)); }
    }

    private void RestoreWindow()
    {
      WindowState = WindowState.Normal;
    }

  }

}