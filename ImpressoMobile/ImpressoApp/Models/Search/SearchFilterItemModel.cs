using System;
using System.Collections.Generic;
namespace ImpressoApp.Models.Search
{
    public enum SearchFilterItem
    {
        ListWithSearch,
        CheckmarkSingleChoise,
        CheckmarkMultipleChoise
    }

    public class SearchFilterItemModel
    {
        public SearchFilterItemModel()
        {
        }

        public string Name { get; set; }

        public bool IsSearchAvailable { get; set; } = true;

        public SearchFilterItem SearchType { get; set; }

        public List<string> SearchItems { get; set; }

        public Action<List<string>> CommitAction { get; set; }

        public Func<List<string>> GetAction { get; set; }
    }
}
