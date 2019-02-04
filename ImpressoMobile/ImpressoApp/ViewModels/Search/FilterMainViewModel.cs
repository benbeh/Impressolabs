using System;
using System.Threading.Tasks;
using BaseMvvmToolkit.ViewModels;
using BaseMvvmToolkit.Services;
using System.Collections.Generic;
using ImpressoApp.Utils;
using System.Windows.Input;
using Xamarin.Forms;
using ImpressoApp.Models.Search;
using System.Collections.ObjectModel;
using System.Linq;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ImpressoApp.ViewModels.Search
{
    public class FilterMainViewModel : BaseViewModel
    {
        public class FilterString
        {
            public string Name { get; set; }
        }

        private List<SearchFilterItemModel> filterModels;
        public FilterString SelectedFilter { get; set; }

        public FilterMainViewModel(INavigationService navigationService) : base(navigationService)
        {
            Title = "Filters";
            RowSelected = new Command(RowSelectedExecute);
            ClearAllCommand = new Command(ClearAllCommandExecute);
            Filters = new ObservableCollection<ListWithHeading<FilterString>>();
        }

        public ICommand RowSelected { get; set; }
        public ICommand ClearAllCommand { get; private set; }

        public ObservableCollection<ListWithHeading<FilterString>> Filters { get; set; }

        private void ClearAllCommandExecute(object obj)
        {
            foreach (var filter in filterModels)
            {
                filter.CommitAction?.Invoke(new List<string>() { null });
            }

            InitFilters(filterModels);
        }

        private async void RowSelectedExecute(object obj)
        {
            var id = -1;
            for (int i = 0; i < Filters.Count; i++)
            {
                if (Filters[i].First() == ((SelectedItemChangedEventArgs)obj).SelectedItem)
                {
                    id = i;
                    break;
                }
            }

            if (id >= 0)
            {
                var model = filterModels[id];

                switch (model.SearchType)
                {
                    case SearchFilterItem.CheckmarkSingleChoise:
                        await this.NavigationService.NavigateToAsync<SingleSelectionFilterViewModel>(model);
                        break;
                    case SearchFilterItem.CheckmarkMultipleChoise:
                        await this.NavigationService.NavigateToAsync<MultiSelectionFilterViewModel>(model);
                        break;
                }
            }
        }

        public override async Task Init(object args)
        {
            await base.Init(args);

            filterModels = (List<SearchFilterItemModel>)args;

            InitFilters(filterModels);
        }

        public override void OnAppearing()
        {
            base.OnAppearing();

            InitFilters(filterModels);
        }

        private void InitFilters(List<SearchFilterItemModel> filters)
        {
            Filters.Clear();
            foreach (var filter in filters)
            {
                var itemsModel = new ListWithHeading<FilterString>() { Heading = filter.Name };
                var selectedItems = filter.GetAction?.Invoke();
                var title = CreateTitleForSelectedItems(selectedItems);
                itemsModel.Add(new FilterString() { Name = title });
                Filters.Add(itemsModel);
            }
        }

        private string CreateTitleForSelectedItems(List<string> selectedItems)
        {
            if (selectedItems == null || selectedItems.Count == 0 || string.IsNullOrEmpty(selectedItems[0]))
            {
                return "None";
            }

            var separator = ", ";
            var text = string.Empty;

            foreach (var item in selectedItems)
            {
                text = text + item + separator;
            }

            return text.Substring(0, text.Length - separator.Length);
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (nameof(SelectedFilter) == propertyName)
            {
                SelectedFilter = null;
            }
        }
    }
}
