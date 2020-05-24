using System;
using System.Windows;
using System.Windows.Input;

using SimpleDBExample23.ViewModels;
using SimpleDBExample23.Windows;

namespace SimpleDBExample23
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window, IDisposable
  {
    private readonly MainWindowViewModel mModel;

    public MainWindow()
    {
      InitializeComponent();

      mModel = new MainWindowViewModel(this);
      DataContext = mModel;
    }

    private void ButtonInsert_Click(object sender, RoutedEventArgs e)
    {
      mModel.AddPerson(false);
    }

    private void ButtonDelete_Click(object sender, RoutedEventArgs e)
    {
      mModel.DeletePerson();
    }

    private void ButtonEdit_Click(object sender, RoutedEventArgs e)
    {
      mModel.AddPerson(true);
    }

    private void ButtonDetails_Click(object sender, RoutedEventArgs e)
    {
      if (mModel.CurrentPerson != null)
      {
        DetailsWindow w = new DetailsWindow(mModel);
        w.ShowDialog();
      }
    }

    private void ListView_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Delete)
      {
        mModel.DeletePerson();
      }
    }

    #region IDisposable Support
    private bool disposedValue = false; // To detect redundant calls

    protected virtual void Dispose(bool disposing)
    {
      if (!disposedValue)
      {
        if (disposing)
        {
          // TODO: dispose managed state (managed objects).
          mModel.Dispose();
        }

        // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
        // TODO: set large fields to null.

        disposedValue = true;
      }
    }

    // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
    // ~MainWindow() {
    //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
    //   Dispose(false);
    // }

    // This code added to correctly implement the disposable pattern.
    public void Dispose()
    {
      // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
      Dispose(true);
      // TODO: uncomment the following line if the finalizer is overridden above.
      // GC.SuppressFinalize(this);
    }
    #endregion

  }
}
