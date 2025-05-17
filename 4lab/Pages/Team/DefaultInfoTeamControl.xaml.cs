using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace _4lab
{
    public partial class DefaultInfoTeamControl : UserControl
    {
        public DefaultInfoTeamControl()
        {
            InitializeComponent();
        }

        public void SetRegion(string region)
        {
            RegionTextBlock.Text = region;
        }

        public void SetTeamDescription(string description)
        {
            TeamDescriptionTextBlock.Text = description;
        }
    }
}
