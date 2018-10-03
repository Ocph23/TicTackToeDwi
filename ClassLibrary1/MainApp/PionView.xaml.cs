using GameLib;
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
     
            PionModel = pion;
            PionModel.OnChangePosition += PionModel_OnChangePosition1;
          
            if (pion.PionType == PlayerPionType.Circle)
            {
                BitmapImage img = new BitmapImage(
                   new Uri(@"eclipse.png", UriKind.RelativeOrAbsolute));

                ImageBrush imgBrush = new ImageBrush();
                imgBrush.ImageSource = img;
                this.Background = imgBrush;
                //var eclipse = new Ellipse() { StrokeThickness=30};

                //var stopgradiendCOllection = new GradientStopCollection();
                //stopgradiendCOllection.Add(new GradientStop((Color)ColorConverter.ConvertFromString("#FFA40F0F"), 0));
                //stopgradiendCOllection.Add(new GradientStop((Color)ColorConverter.ConvertFromString("#FF250404"), 1));
                //stopgradiendCOllection.Add(new GradientStop((Color)ColorConverter.ConvertFromString("#FEFF0909"), 0.587));
                //var radial = new RadialGradientBrush(stopgradiendCOllection);
                //eclipse.Stroke = radial;
                //dataView.Children.Add(eclipse);
            }else
            {
                BitmapImage img = new BitmapImage(
                 new Uri(@"cross.png", UriKind.RelativeOrAbsolute));

                ImageBrush imgBrush = new ImageBrush();
                imgBrush.ImageSource = img;
                this.Background = imgBrush;
                //var canvas = new Grid();
                //var pol1 = new Polyline() { StrokeThickness = 30 };
                //pol1.Points = new PointCollection() {
                //    new Point(30,30),
                //    new Point(170,170),

                //};
                //pol1.RenderTransformOrigin = new Point(0.5, 0.5);
                //var stopgradiendCOllection = new GradientStopCollection();
                //stopgradiendCOllection.Add(new GradientStop((Color)ColorConverter.ConvertFromString("#FF083C05"), 0));
                //stopgradiendCOllection.Add(new GradientStop((Color)ColorConverter.ConvertFromString("#FF083C05"), 0.877));
                //stopgradiendCOllection.Add(new GradientStop((Color)ColorConverter.ConvertFromString("#FF1FDE16"), 0.587));
                //var radial = new LinearGradientBrush(stopgradiendCOllection) { EndPoint=new Point(0.5,1), MappingMode= BrushMappingMode.RelativeToBoundingBox , StartPoint=new Point(0.5,0) };
                //pol1.Stroke = radial;



                //var pol2 = new Polyline() { StrokeThickness = 30 };
                //pol2.Points = new PointCollection() {
                //    new Point(170,30),
                //    new Point(30,170),

                //};
                //pol2.RenderTransformOrigin = new Point(0.5, 0.5);
                //var stopgradiendCOllection2 = new GradientStopCollection();
                //stopgradiendCOllection2.Add(new GradientStop((Color)ColorConverter.ConvertFromString("#FF083C05"), 0));
                //stopgradiendCOllection2.Add(new GradientStop((Color)ColorConverter.ConvertFromString("#FF083C05"), 0.877));
                //stopgradiendCOllection2.Add(new GradientStop((Color)ColorConverter.ConvertFromString("#FF1FDE16"), 0.587));
                //var radial2 = new LinearGradientBrush(stopgradiendCOllection2) { EndPoint = new Point(0.5, 1), MappingMode = BrushMappingMode.RelativeToBoundingBox, StartPoint = new Point(0.5, 0) };
                //pol2.Stroke = radial2;

                //canvas.Children.Add(pol1);
                //canvas.Children.Add(pol2);
                //dataView.Children.Add(canvas);
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
