using GameLib;
using SharedApp;
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

namespace MainApp
{
    /// <summary>
    /// Interaction logic for PionView.xaml
    /// </summary>
    /// 
    public delegate void delOnSelected(PionView pion);
    public partial class PionView : UserControl
    {
        private Player player;
        private PionViewModel vm;
        public event delOnSelected OnSelected;

        public PionView(Pion pion, GameLib.Player player)
        {
            InitializeComponent();
            this.player = player;
            vm = new PionViewModel();
            Width = 100;
            Height = 100;
            PionModel = pion;
            PionModel.OnChangePosition += PionModel_OnChangePosition1;
            this.Background = new SolidColorBrush(Colors.Yellow);
            if (pion.PionType == PlayerPionType.Circle)
            {
               border.CornerRadius = new CornerRadius(90);
                this.Background = new SolidColorBrush(Colors.Red);
            }

            this.DataContext = vm;
            
        }


     

        private void PionModel_OnChangePosition1(Position position)
        {
            Grid.SetRow(this, position.Row);
            Grid.SetColumn(this, position.Column);
           vm.IsSelected = false;
        }

        public Pion PionModel { get; }

      

        private void Button_Click(object sender, RoutedEventArgs e)
        {
          if(player.IsPlay)
            {
                vm.IsSelected = !vm.IsSelected;
                if (vm.IsSelected)
                {
                    OnSelected?.Invoke(this);
                }
            }
        }
    }


    public class PionViewModel:BaseViewModel
    {
        private bool isSelected;

        public bool IsSelected
        {
            get { return isSelected; }
            set { SetProperty(ref isSelected ,value); }
        }


    }
}
