using GameLib;
using MainApp.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MainApp.Views
{
    /// <summary>
    /// Interaction logic for SinglePlayerScore.xaml
    /// </summary>
    public partial class SinglePlayerScore : Window
    {
        private ScoreViewModel viewmodel;

        public SinglePlayerScore()
        {
            InitializeComponent();
            DataContext =viewmodel= new ScoreViewModel();
        }

        private void RadioButton_Click(object sender, RoutedEventArgs e)
        {
            viewmodel.LoadBoar(3);
        }

        private void RadioButton_Click_1(object sender, RoutedEventArgs e)
        {
            viewmodel.LoadBoar(4);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            viewmodel.ShowCapture();
        }
    }

    public class ScoreViewModel:BaseNotify
    {
        private PlayerModel _selected;

        public ScoreViewModel()
        {
            Source = new List<PlayerModel>();
            SourceView = (CollectionView)CollectionViewSource.GetDefaultView(Source);
            Loaded();
        }

        private void Loaded()
        {
            LoadBoar(3);
        }



        public void LoadBoar(int length)
        {
            Source.Clear();
            using (var db = new OcphDbContext())
            {
                var result = from a in db.Players.Select()
                             join b in db.Scores.Where(O=>O.Board== length) on a.Id equals b.PlayerId
                             select new PlayerModel { Id = a.Id, Name = a.Name, Score = b, GuidData = b.GuidData };
                foreach (var item in result)
                {
                    Source.Add(item);
                }
            }

            SourceView.Refresh();
        }

        internal void ShowCapture()
        {
            if(SelectedItem!=null)
            {
                var form = new Views.CaptureView(SelectedItem);
                form.ShowDialog();
            }
        }

        public PlayerModel SelectedItem
        {
            get { return _selected; }
            set
            {
                SetProperty(ref _selected, value);
            }
        }


        public List<PlayerModel> Source { get; }
        public CollectionView SourceView { get; }
    }
}
