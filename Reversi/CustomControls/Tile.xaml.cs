using Reversi.Enums;
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

namespace Reversi.CustomControls
{
    /// <summary>
    /// Interaction logic for Tile.xaml
    /// </summary>
    public partial class Tile : UserControl
    {

        public TileStateEnum State { get; private set; }

        public int X;

        public int Y;

        public Tile()
        {
            InitializeComponent();

            State = TileStateEnum.Empty;
        }

        public void PutBlack()
        {
            State = TileStateEnum.Black;
            Pawn.Fill = Brushes.Black;
        }

        public void PutWhite()
        {
            State = TileStateEnum.White;
            Pawn.Fill = Brushes.White;
        }

        public void Flip()
        {
            if (State == TileStateEnum.Empty) return;

            if (State == TileStateEnum.Black)
            {
                PutWhite();
            }
            else
            {
                PutBlack();
            }
        }
    }
}
