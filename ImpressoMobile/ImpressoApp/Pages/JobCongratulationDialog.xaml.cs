using System;
using System.Collections.Generic;
using Xamarin.Forms;
using ImpressoApp.Controls;
using BaseMvvmToolkit;

namespace ImpressoApp.Pages
{
    public partial class JobCongratulationDialog : SlideCustomDialog
    {
        public JobCongratulationDialog()
        {
            InitializeComponent();

            this.HeightRequest = 320;
        }
    }
}
