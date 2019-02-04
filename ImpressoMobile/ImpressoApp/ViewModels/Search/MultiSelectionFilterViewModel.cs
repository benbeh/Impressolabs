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
using System.Runtime.CompilerServices;

namespace ImpressoApp.ViewModels.Search
{
    public class MultiSelectionFilterViewModel : BaseViewModel
    {
        public ICommand RowSelected { get; set; }
        public ICommand ClearCommand { get; private set; }
        public ICommand SearchTextChangedCommand { get; private set; }
        public ICommand SearchCompletedCommand { get; private set; }

        public string SelectedItem { get; set; }

        private SearchFilterItemModel searchModel;

        public ObservableCollection<string> Items { get; private set; } = new ObservableCollection<string>();
        public ObservableCollection<string> CurrentItems { get; set; } = new ObservableCollection<string>();

        public MultiSelectionFilterViewModel(INavigationService navigationService) : base(navigationService)
        {
            RowSelected = new Command(RowSelectedExecute);
            ClearCommand = new Command(Clear);
            SearchTextChangedCommand = new Command(OnSearchTextChanged);
            SearchCompletedCommand = new Command(OnSearchCompleted);
        }

        private async void Clear()
        {
            searchModel?.CommitAction?.Invoke(new List<string>() { null });
            await NavigationService.PopAsync();
        }

        private async void RowSelectedExecute(object obj)
        {
            var value = (obj as SelectedItemChangedEventArgs).SelectedItem as string;
            if (CurrentItems.Contains(value))
            {
                CurrentItems.Remove(value);
            }
            else
            {
                CurrentItems.Add(value);
            }

            var selectedItem = value;
            Items = new ObservableCollection<string>(Items.ToList());
            var items = CurrentItems.Where(item => item != null).ToList();
            searchModel?.CommitAction?.Invoke(items.Count > 0 ? items : new List<string>() { null });
        }

        public override Task Init(object args)
        {
            searchModel = (SearchFilterItemModel)args;
            Items = new ObservableCollection<string>(searchModel.SearchItems);
            var items = searchModel.GetAction.Invoke();
            if (items != null)
            {
                CurrentItems = new ObservableCollection<string>(items.Where(item => item != null));
            }
            Title = searchModel.Name;

            return base.Init(args);
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (nameof(SelectedItem) == propertyName)
            {
                SelectedItem = null;
            }
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
