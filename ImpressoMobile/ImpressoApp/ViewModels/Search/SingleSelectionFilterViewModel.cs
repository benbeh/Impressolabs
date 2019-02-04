using System;
using BaseMvvmToolkit.ViewModels;
using BaseMvvmToolkit.Services;
using System.Windows.Input;
using Xamarin.Forms;
using System.Collections.Generic;
using System.Linq;
using ImpressoApp.Utils;
using System.Threading.Tasks;
using ImpressoApp.Models.Search;
using System.Collections.ObjectModel;
using Newtonsoft.Json.Serialization;

namespace ImpressoApp.ViewModels.Search
{
    public class SingleSelectionFilterViewModel : BaseViewModel
    {
        public ICommand SearchTextChangedCommand { get; private set; }
        public ICommand SearchCompletedCommand { get; private set; }
        public ICommand RowSelected { get; set; }
        public ICommand ClearCommand { get; private set; }

        public bool IsSearchAvailable { get; set; }

        public string SelectedItem { get; set; }

        private SearchFilterItemModel searchModel;

        public ObservableCollection<string> Items { get; private set; } = new ObservableCollection<string>();
        public string CurrentItem { get; set; }

        public SingleSelectionFilterViewModel(INavigationService navigationService) : base(navigationService)
        {
            SearchTextChangedCommand = new Command(OnSearchTextChanged);
            SearchCompletedCommand = new Command(OnSearchCompleted);
            RowSelected = new Command(RowSelectedExecute);
            ClearCommand = new Command(Clear);
        }

        private async void Clear()
        {
            searchModel?.CommitAction?.Invoke(new List<string>() { null });
            await NavigationService.PopAsync();
        }

        private async void RowSelectedExecute(object obj)
        {
            var selectedItem = (obj as SelectedItemChangedEventArgs).SelectedItem as string;
            searchModel?.CommitAction?.Invoke(new List<string>() { selectedItem });
            await NavigationService.PopAsync();
        }

        public override Task Init(object args)
        {
            searchModel = (SearchFilterItemModel)args;
            IsSearchAvailable = searchModel.IsSearchAvailable;
            Items = new ObservableCollection<string>(searchModel.SearchItems);
            CurrentItem = searchModel.GetAction.Invoke().FirstOrDefault();
            Title = searchModel.Name;

            return base.Init(args);
        }

        void OnSearchTextChanged(object obj)
        {
            Search((obj as TextChangedEventArgs).NewTextValue);
        }

        void OnSearchCompleted(object obj)
        {
            Search((obj as TextChangedEventArgs).NewTextValue);
        }

        private void Search(string obj)
        {
            Items = !string.IsNullOrEmpty(obj) ? new ObservableCollection<string>(searchModel.SearchItems.Where(x => x.ContainsIgnoreCase(obj)).ToList()) : new ObservableCollection<string>(searchModel.SearchItems);
        }
    }
}
