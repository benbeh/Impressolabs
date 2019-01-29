using System;
using System.Collections.Generic;
namespace ImpressoApp.Utils
{
    public class ListWithHeading<T> : List<T>
    {
        public string Heading { get; set; }
    }
}
