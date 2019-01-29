using System;
using ImpressoApp.Models.Search;
using ImpressoApp.Utils;

namespace ImpressoApp.Models.Job
{
    public class JobListWithHeading : ListWithHeading<string>
    {
        public JobListWithHeading(SearchFilterItemModel model)
        {
            Heading = model.Name;
            AddRange(model.SearchItems);
        }
    }
}
