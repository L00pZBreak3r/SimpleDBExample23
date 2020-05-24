using System;
using System.Windows;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

using SimpleDBExample23.Database;

namespace SimpleDBExample23.ViewModels
{
  public class MainWindowViewModel : ViewModelBase, IDisposable
  {
    private static readonly DateTime MinimumDate = new DateTime(1900, 1, 1);

    public MainWindowViewModel(Window aWindow)
    {
      mWindow = aWindow;
      mTableContext = new PersonContext();
      mTableContext.Persons.Load();
      mTableContext.RelativeGroups.Load();
    }

    #region ParentWindow

    private readonly Window mWindow;
    public Window ParentWindow
    {
      get
      {
        return mWindow;
      }
    }

    #endregion

    #region PeopleList

    public ObservableCollection<Person> PeopleList
    {
      get
      {
        IEnumerable<Person> loc = mTableContext.Persons.Local;
        if (!string.IsNullOrWhiteSpace(mSearchLastName))
          loc = loc.Where(p => p.LastName.StartsWith(mSearchLastName, StringComparison.CurrentCultureIgnoreCase));
        if (!string.IsNullOrWhiteSpace(mSearchFirstName))
          loc = loc.Where(p => p.FirstName.StartsWith(mSearchFirstName, StringComparison.CurrentCultureIgnoreCase));
        if (mSearchBirthDateDate >= MinimumDate)
          loc = loc.Where(p => p.BirthDate == mSearchBirthDateDate);
        return new ObservableCollection<Person>(loc);
      }
    }

    #endregion

    #region TableContext

    private readonly PersonContext mTableContext;
    public PersonContext TableContext
    {
      get
      {
        return mTableContext;
      }
    }

    #endregion

    #region CurrentPerson

    private Person mCurrentPerson;
    public Person CurrentPerson
    {
      get { return mCurrentPerson; }
      set
      {
        if (!ReferenceEquals(mCurrentPerson, value))
        {
          mCurrentPerson = value;
          RaisePropertyChanged();
          if (mCurrentPerson != null)
          {
            AddLastName = mCurrentPerson.LastName;
            AddFirstName = mCurrentPerson.FirstName;
            AddMiddleName = mCurrentPerson.MiddleName;
            AddBirthDate = mCurrentPerson.BirthDate.ToString("dd/MM/yyyy");
            AddAddress = mCurrentPerson.Address;
          }
        }
      }
    }

    #endregion

    #region SearchLastName

    private string mSearchLastName;
    public string SearchLastName
    {
      get
      {
        return mSearchLastName;
      }

      set
      {
        if (mSearchLastName != value)
        {
          mSearchLastName = value;
          RaisePropertyChanged();
          FilterView();
        }
      }
    }

    #endregion

    #region SearchFirstName

    private string mSearchFirstName;
    public string SearchFirstName
    {
      get
      {
        return mSearchFirstName;
      }

      set
      {
        if (mSearchFirstName != value)
        {
          mSearchFirstName = value;
          RaisePropertyChanged();
          FilterView();
        }
      }
    }

    #endregion

    #region SearchBirthDate

    private string mSearchBirthDate;
    private DateTime mSearchBirthDateDate;
    public string SearchBirthDate
    {
      get
      {
        return mSearchBirthDate;
      }

      set
      {
        if (mSearchBirthDate != value)
        {
          mSearchBirthDate = value;
          DateTime.TryParse(mSearchBirthDate, out mSearchBirthDateDate);
          RaisePropertyChanged();
          FilterView();
        }
      }
    }

    #endregion

    #region AddLastName

    private string mAddLastName;
    public string AddLastName
    {
      get
      {
        return mAddLastName;
      }

      set
      {
        if (mAddLastName != value)
        {
          mAddLastName = value;
          RaisePropertyChanged();
        }
      }
    }

    #endregion

    #region AddFirstName

    private string mAddFirstName;
    public string AddFirstName
    {
      get
      {
        return mAddFirstName;
      }

      set
      {
        if (mAddFirstName != value)
        {
          mAddFirstName = value;
          RaisePropertyChanged();
        }
      }
    }

    #endregion

    #region AddMiddleName

    private string mAddMiddleName;
    public string AddMiddleName
    {
      get
      {
        return mAddMiddleName;
      }

      set
      {
        if (mAddMiddleName != value)
        {
          mAddMiddleName = value;
          RaisePropertyChanged();
        }
      }
    }

    #endregion

    #region AddBirthDate

    private string mAddBirthDate;
    private DateTime mAddBirthDateDate;
    public string AddBirthDate
    {
      get
      {
        return mAddBirthDate;
      }

      set
      {
        if (mAddBirthDate != value)
        {
          mAddBirthDate = value;
          DateTime.TryParse(mAddBirthDate, out mAddBirthDateDate);
          RaisePropertyChanged();
        }
      }
    }

    #endregion

    #region AddAddress

    private string mAddAddress;
    public string AddAddress
    {
      get
      {
        return mAddAddress;
      }

      set
      {
        if (mAddAddress != value)
        {
          mAddAddress = value;
          RaisePropertyChanged();
        }
      }
    }

    #endregion

    private void FilterView()
    {
      RaisePropertyChanged("PeopleList");
    }

    public void AddPerson(bool aEditCurrent = false)
    {
      if (!string.IsNullOrWhiteSpace(mAddLastName) && !string.IsNullOrWhiteSpace(mAddFirstName) && !string.IsNullOrWhiteSpace(mAddMiddleName) && (mAddBirthDateDate >= MinimumDate))
      {
        if (aEditCurrent)
        {
          if (mCurrentPerson != null)
          {
            mCurrentPerson.LastName = mAddLastName;
            mCurrentPerson.FirstName = mAddFirstName;
            mCurrentPerson.MiddleName = mAddMiddleName;
            mCurrentPerson.BirthDate = mAddBirthDateDate;
            mCurrentPerson.Address = mAddAddress;
            mTableContext.SaveChanges();
            RaisePropertyChanged("PeopleList");
          }
        }
        else
        {
          bool nf = true;
          var p = new Person(mAddLastName, mAddFirstName, mAddMiddleName, mAddBirthDateDate, mAddAddress);

          var ps = mTableContext.Persons;
          foreach (var d in ps)
            if (p.Equals(d))
            {
              nf = false;
              break;
            }
          if (nf)
          {
            mTableContext.Persons.Add(p);
            mTableContext.SaveChanges();
            AddLastName = string.Empty;
            AddFirstName = string.Empty;
            AddMiddleName = string.Empty;
            AddBirthDate = string.Empty;
            AddAddress = string.Empty;
            RaisePropertyChanged("PeopleList");
          }
        }
      }
    }

    public void DeletePerson()
    {
      if ((mCurrentPerson != null) && (MessageBox.Show(mWindow, "Удалить выбранную запись из базы?", "Удаление записи", MessageBoxButton.YesNo) == MessageBoxResult.Yes))
      {
        mTableContext.Persons.Remove(mCurrentPerson);
        mTableContext.SaveChanges();
        RaisePropertyChanged("PeopleList");
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
          mTableContext.Dispose();
        }

        // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
        // TODO: set large fields to null.

        disposedValue = true;
      }
    }

    // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
    // ~MainWindowViewModel() {
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
