using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RedAssistance
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ExtraDetailsPage : ContentPage
    {
        public ExtraDetailsPage(int v)
        {
            InitializeComponent();
            if (v == 1)
            {
                Title = "Blood Details";
                DetailImg.IsVisible = true;
                TermsCondition.IsVisible = false;
                Eligibility.IsVisible = false;
            }
            if (v == 2)
            {
                Title = "Terms and Conditions";
                DetailImg.IsVisible = false;
                TermsCondition.IsVisible = true;
                Eligibility.IsVisible = false;
            }
            if (v == 3)
            {
                Title = "Are you Eligible?";
                DetailImg.IsVisible = false;
                TermsCondition.IsVisible = false;
                Eligibility.IsVisible = true;
            }
        }
    }
}