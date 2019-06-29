using MainApp.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
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
using System.Windows.Shapes;

namespace MainApp.Views
{
    /// <summary>
    /// Interaction logic for CaptureView.xaml
    /// </summary>
    public partial class CaptureView : Window
    {

        public CollectionView SourceView { get; set; }
        public List<ICapture> Source { get; set; }
        public CaptureView(PlayerModel selectedItem)
        {
            InitializeComponent();

            if (selectedItem.Score.Board == 3)
                lbl.Content = "Ukuran Papan 3 x 3";
            else
                lbl.Content = "Ukuran Papan 4 x 4";

            this.Score = selectedItem;
            Source = new List<ICapture>();
            SourceView = (CollectionView)CollectionViewSource.GetDefaultView(Source);

            RbGreedy_Click(null, null);
            this.DataContext = this;
        }

        public PlayerModel Score { get; }

      

        private void RbGreedy_Click(object sender, RoutedEventArgs e)
        {
            using (var db = new OcphDbContext())
            {
                var result = db.GreedyCapture.Where(x => x.Guid == Score.GuidData && x.Board == Score.Score.Board).ToList();
                Source.Clear();
                foreach (var item in result)
                {
                    Source.Add(item);
                }
            }


            SourceView.Refresh();
        }

        private void RbBnB_Click(object sender, RoutedEventArgs e)
        {
            using (var db = new OcphDbContext())
            {
                var result = db.BandBCapture.Where(x => x.Guid == Score.GuidData && x.Board == Score.Score.Board).ToList();
                Source.Clear();
                foreach (var item in result)
                {
                    Source.Add(item);
                }
            }


            SourceView.Refresh();
        }
    }
}
