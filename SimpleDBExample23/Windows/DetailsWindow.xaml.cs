using System;
using System.Windows;

using SimpleDBExample23.ViewModels;

namespace SimpleDBExample23.Windows
{
  /// <summary>
  /// Interaction logic for DetailsWindow.xaml
  /// </summary>
  public partial class DetailsWindow : Window
  {
    private readonly DetailsWindowViewModel mModel;

    public DetailsWindow(MainWindowViewModel aMainWindowModel)
    {
      InitializeComponent();

      Owner = aMainWindowModel.ParentWindow;
      mModel = new DetailsWindowViewModel(this, aMainWindowModel.TableContext, aMainWindowModel.CurrentPerson);
      DataContext = mModel;
    }

    private void ButtonAdd_Click(object sender, RoutedEventArgs e)
    {
      mModel.AddMember();
    }

    private void ButtonDelete_Click(object sender, RoutedEventArgs e)
    {
      mModel.RemoveMember();
    }
  }
}
