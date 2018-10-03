using GameLib;
using MainApp.Models;
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
using System.Windows.Shapes;

namespace MainApp.Views
{
    /// <summary>
    /// Interaction logic for SinglePlayerScore.xaml
    /// </summary>
    public partial class SinglePlayerScore : Window
    {
        public SinglePlayerScore()
        {
            InitializeComponent();
            DataContext = new ScoreViewModel();
        }
    }

    public class ScoreViewModel:BaseNotify
    {
        public ScoreViewModel()
        {
            Source = new List<PlayerModel>();
            SourceView = (CollectionView)CollectionViewSource.GetDefaultView(Source);
            Loaded();
        }

        private void Loaded()
        {
            using (var db = new OcphDbContext())
            {
                var result = from a in db.Players.Select()
                             join b in db.Scores.Select() on a.Id equals b.PlayerId
                             select new PlayerModel { Id = a.Id, Name = a.Name, Score = b };
                foreach(var item in result)
                {
                    Source.Add(item);
                }
            }

            SourceView.Refresh();
        }

        public List<PlayerModel> Source { get; }
        public CollectionView SourceView { get; }
    }
}
