using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

using SimpleDBExample23.Database;

namespace SimpleDBExample23.ViewModels
{
  internal class DetailsWindowViewModel : ViewModelBase
  {
    private readonly Window mWindow;
    private readonly Person mRelPerson;
    private readonly PersonContext mTableContext;

    public DetailsWindowViewModel(Window aWindow, PersonContext aTableContext, Person aRelPerson)
    {
      mWindow = aWindow;
      mTableContext = aTableContext;
      mRelPerson = aRelPerson;
      mWindowTitle = "Родственники " + mRelPerson.ToString();
    }

    #region RelativeList

    public IEnumerable<Person> RelativeList
    {
      get
      {
        return mRelPerson.Relatives?.Members.Where(p => !p.Equals(mRelPerson));
      }
    }

    #endregion

    #region PersonList

    public IEnumerable<Person> PersonList
    {
      get
      {
        return mTableContext.Persons.Local.Where(p => !(mRelPerson.Equals(p) || (mRelPerson.Relatives != null) && mRelPerson.Relatives.Members.Contains(p)));
      }
    }

    #endregion

    #region WindowTitle

    private readonly string mWindowTitle;
    public string WindowTitle
    {
      get
      {
        return mWindowTitle;
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
        }
      }
    }

    #endregion

    #region SelectedPerson

    private Person mSelectedPerson;
    public Person SelectedPerson
    {
      get { return mSelectedPerson; }
      set
      {
        if (!ReferenceEquals(mSelectedPerson, value))
        {
          mSelectedPerson = value;
          RaisePropertyChanged();
        }
      }
    }

    #endregion

    public void AddMember()
    {
      if (mSelectedPerson != null)
      {
        if (mRelPerson.Relatives == null)
        {
          var rg = new RelativeGroup();
          mTableContext.RelativeGroups.Add(rg);
          mRelPerson.Relatives = rg;
        }
        mSelectedPerson.Relatives = mRelPerson.Relatives;
        mTableContext.SaveChanges();
        RaisePropertyChanged("RelativeList");
        RaisePropertyChanged("PersonList");
      }
    }

    public void RemoveMember()
    {
      if (mCurrentPerson != null)
      {
        mCurrentPerson.Relatives = null;
        mTableContext.SaveChanges();
        var rg = mRelPerson.Relatives;
        if ((rg != null) && (rg.Members.Count <= 1))
        {
          mRelPerson.Relatives = null;
          mTableContext.RelativeGroups.Remove(rg);
          mTableContext.SaveChanges();
        }
        RaisePropertyChanged("RelativeList");
        RaisePropertyChanged("PersonList");
      }
    }
  }
}
