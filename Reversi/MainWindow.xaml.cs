using Reversi.CustomControls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
using Reversi.Enums;
using Reversi.Logic;

namespace Reversi
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public int BoardSize { get; set; }

        private Tile[][] _board;

        private bool _blackTurn;
        private bool BlackTurn
        {
            get
            {
                return _blackTurn;
            }
            set
            {
                _blackTurn = value;
                if (_blackTurn)
                {
                    PlayerTurnIndicatorLabel.Background = Brushes.Black;
                    PlayerTurnIndicatorLabel.Foreground = Brushes.White;
                    PlayerTurnIndicatorLabel.Content = "Black";
                }
                else
                {
                    PlayerTurnIndicatorLabel.Background = Brushes.White;
                    PlayerTurnIndicatorLabel.Foreground = Brushes.Black;
                    PlayerTurnIndicatorLabel.Content = "White";
                }
            }
        }

        private bool _aiIsThinking;
        private bool _useAi;
        private bool _gameHasEnded;

        private HashSet<Tile> _possibleMoves;

        private AIStrategies _aiStrategy;
        private AlgorithmsEnum _usedAlgorithm;

        private MinMaxPlayer _minMaxWhitePlayer;
        private MinMaxPlayer _minMaxBlackPlayer;

        private int _whitePoints;

        private int _blackPoints;

        private const string LogDirectory = @"C:\School\AI4\";
        private readonly StreamWriter _logsWriter = new StreamWriter($"{LogDirectory}{DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss")}.txt");

        private readonly Stopwatch gameStopwatch = new Stopwatch();
        private int stepsCount;

        public MainWindow()
        {
            InitializeComponent();

            BoardSize = Configuration.BOARD_SIZE;
            _possibleMoves = new HashSet<Tile>();
            _useAi = true;

            DrawBoard();

            RestartGame();


            _minMaxWhitePlayer = new MinMaxPlayer(BoardSize, TileStateEnum.White, _aiStrategy, 4);
            _minMaxBlackPlayer = new MinMaxPlayer(BoardSize, TileStateEnum.Black, _aiStrategy, 4);

            //for (int i = 0; i < 20; i++)
            //{
            //    var e = _possibleMoves.GetEnumerator();
            //    e.MoveNext();
            //    PlacePawnAt(e.Current);
            //}

            _logsWriter.AutoFlush = true;
        }

        private void DrawBoard()
        {
            if (_board == null || _board.Length != BoardSize)
            {
                _board = new Tile[BoardSize][];

                for (var i = 0; i < _board.Length; i++)
                {
                    _board[i] = new Tile[BoardSize];
                }
            }

            for (var i = 0; i < BoardSize; i++)
            {
                BoardGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                BoardGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

                for (var j = 0; j < BoardSize; j++)
                {
                    var tile = new Tile();
                    tile.SetValue(Grid.RowProperty, i);
                    tile.SetValue(Grid.ColumnProperty, j);
                    tile.X = j;
                    tile.Y = i;

                    tile.MouseUp += (sender, args) => HandleMouseClick((Tile)sender);

                    _board[j][i] = tile;
                    BoardGrid.Children.Add(tile);
                }
            }
        }

        private void HandleMouseClick(Tile tile)
        {
            if (!_aiIsThinking && !_gameHasEnded)
            {
                PlacePawnAt(tile);
            }
        }

        private void PlaceInitialPawns()
        {
            var rightX = BoardSize / 2;
            var bottomY = BoardSize / 2;

            _board[rightX][bottomY].PutWhite();
            _board[rightX - 1][bottomY].PutBlack();
            _board[rightX - 1][bottomY - 1].PutWhite();
            _board[rightX][bottomY - 1].PutBlack();

            UpdatePossibleMoves();
        }

        private void RestartGame()
        {
            _blackPoints = 0;
            _whitePoints = 0;
            WinnerLabel.Visibility = Visibility.Hidden;
            BlackTurn = true;

            for (var i = 0; i < _board.Length; i++)
            {
                for (var j = 0; j < _board[i].Length; j++)
                {
                    _board[i][j].RemovePawn();
                }
            }

            PlaceInitialPawns();
            ShowMoveHelpers();
        }

        private void PlacePawnAt(PawnLightModel pawn)
        {
            var tile = _board[pawn.X][pawn.Y];
            PlacePawnAt(tile);
        }

        private void EndGame()
        {
            _logsWriter.WriteLine($"Game ended");
            _logsWriter.WriteLine($"{gameStopwatch.ElapsedMilliseconds};{stepsCount}");

            gameStopwatch.Stop();

            _gameHasEnded = true;
            FindWinner();
        }

        private void FindWinner()
        {
            _blackPoints = 0;
            _whitePoints = 0;
            for (var i = 0; i < _board.Length; i++)
            {
                for (var j = 0; j < _board[i].Length; j++)
                {
                    if (_board[i][j].State == TileStateEnum.Black)
                    {
                        _blackPoints++;
                    }
                    else if (_board[i][j].State == TileStateEnum.White)
                    {
                        _whitePoints++;
                    }
                }
            }

            if (_whitePoints == _blackPoints)
            {
                WinnerLabel.Foreground = Brushes.Blue;
                WinnerLabel.Visibility = Visibility.Visible;
                WinnerLabel.Content = "Tie";
            }
            else if (_whitePoints > _blackPoints)
            {
                WinnerLabel.Foreground = Brushes.White;
                WinnerLabel.Visibility = Visibility.Visible;
                WinnerLabel.Content = "White";
            }
            else
            {
                WinnerLabel.Foreground = Brushes.Black;
                WinnerLabel.Visibility = Visibility.Visible;
                WinnerLabel.Content = "Black";
            }
            BlackPointsLabel.Visibility = Visibility.Visible;
            BlackPointsLabel.Content = _blackPoints;
            WhitePointsLabel.Visibility = Visibility.Visible;
            WhitePointsLabel.Content = _whitePoints;
        }

        private async void PlacePawnAt(Tile tile)
        {

            stepsCount++;

            if (tile.State != TileStateEnum.Empty)
            {
                ShowInvalidMoveLabel("Must place pawn on empty tile");
                return;
            }
            if (!(_possibleMoves.Contains(tile)))
            {
                ShowInvalidMoveLabel("This move is illegal. You can use 'show hints' to see possible moves");
                return;
            }


            if (BlackTurn)
            {
                tile.PutBlack();
            }
            else
            {
                tile.PutWhite();
            }

            FlipPawns(tile);

            BlackTurn = !BlackTurn;

            UpdatePossibleMoves();

            if (_possibleMoves.Count == 0)
            {
                BlackTurn = !BlackTurn;
                UpdatePossibleMoves();
                if (_possibleMoves.Count == 0)
                {
                    EndGame();
                    return;
                }
            }

            ShowMoveHelpers();

            AskAI();
        }

        private async void AskAI()
        {
            if (!_useAi || _aiIsThinking) return;
            Debug.WriteLine(BlackTurn ? "Czorny" : "White");
            var sw = new Stopwatch();
            sw.Start();

            switch (_usedAlgorithm)
            {
                case AlgorithmsEnum.MinMax:

                    if (!BlackTurn && _minMaxWhitePlayer != null)
                    {
                        _aiIsThinking = true;
                        var move = await Task.Factory.StartNew(() => _minMaxWhitePlayer.FindNextMove(_board));

                        _logsWriter.WriteLine($"{sw.ElapsedMilliseconds};{_possibleMoves.Count}");

                        await Dispatcher.BeginInvoke(new Action(() =>
                        {
                            _aiIsThinking = false;
                            PlacePawnAt(move.Value);
                        }));
                    }
                    else if (BlackTurn && _minMaxBlackPlayer != null)
                    {
                        _aiIsThinking = true;
                        var move = await Task.Factory.StartNew(() => _minMaxBlackPlayer.FindNextMove(_board));

                        _logsWriter.WriteLine($"{sw.ElapsedMilliseconds};{_possibleMoves.Count}");

                        await Dispatcher.BeginInvoke(new Action(() =>
                        {
                            _aiIsThinking = false;
                            PlacePawnAt(move.Value);
                        }));
                    }

                    break;
                case AlgorithmsEnum.AlfaBeta:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        #region helpers

        private void UpdatePossibleMoves()
        {
            _possibleMoves.Clear();

            var currentPlayerTiles = new List<Tile>(BoardSize * BoardSize);

            for (int i = 0; i < _board.Length; i++)
            {
                for (int j = 0; j < _board[i].Length; j++)
                {
                    if (BlackTurn)
                    {
                        if (_board[i][j].State == Enums.TileStateEnum.Black)
                        {
                            currentPlayerTiles.Add(_board[i][j]);
                        }
                    }
                    else
                    {
                        if (_board[i][j].State == Enums.TileStateEnum.White)
                        {
                            currentPlayerTiles.Add(_board[i][j]);
                        }
                    }
                }
            }

            var currentPlayerColor = BlackTurn ? Enums.TileStateEnum.Black : Enums.TileStateEnum.White;
            var enemyPlayerColor = BlackTurn ? Enums.TileStateEnum.White : Enums.TileStateEnum.Black;

            foreach (var tile in currentPlayerTiles)
            {
                //W
                if (tile.X - 2 >= 0)
                {
                    var currentTile = _board[tile.X - 1][tile.Y];
                    var isTileInBetween = false;
                    while (currentTile != null)
                    {
                        if (!isTileInBetween && currentTile.State == Enums.TileStateEnum.Empty)
                            break;
                        else if (!isTileInBetween && currentTile.State == enemyPlayerColor)
                            isTileInBetween = true;

                        if (isTileInBetween && currentTile.State == Enums.TileStateEnum.Empty)
                        {
                            _possibleMoves.Add(currentTile);
                            break;
                        }
                        else if (isTileInBetween && currentTile.State == currentPlayerColor)
                        {
                            break;
                        }

                        if (currentTile.X - 1 < 0) break;
                        currentTile = _board[currentTile.X - 1][currentTile.Y];
                    }
                }
                //E
                if (tile.X + 2 < _board.Length)
                {
                    var currentTile = _board[tile.X + 1][tile.Y];
                    var isTileInBetween = false;
                    while (currentTile != null)
                    {
                        if (!isTileInBetween && currentTile.State == Enums.TileStateEnum.Empty)
                            break;
                        else if (!isTileInBetween && currentTile.State == enemyPlayerColor)
                            isTileInBetween = true;

                        if (isTileInBetween && currentTile.State == Enums.TileStateEnum.Empty)
                        {
                            _possibleMoves.Add(currentTile);
                            break;
                        }
                        else if (isTileInBetween && currentTile.State == currentPlayerColor)
                        {
                            break;
                        }

                        if (currentTile.X + 1 >= _board.Length) break;
                        currentTile = _board[currentTile.X + 1][currentTile.Y];
                    }
                }
                //N
                if (tile.Y - 2 >= 0)
                {
                    var currentTile = _board[tile.X][tile.Y - 1];
                    var isTileInBetween = false;
                    while (currentTile != null)
                    {
                        if (!isTileInBetween && currentTile.State == Enums.TileStateEnum.Empty)
                            break;
                        else if (!isTileInBetween && currentTile.State == enemyPlayerColor)
                            isTileInBetween = true;

                        if (isTileInBetween && currentTile.State == Enums.TileStateEnum.Empty)
                        {
                            _possibleMoves.Add(currentTile);
                            break;
                        }
                        else if (isTileInBetween && currentTile.State == currentPlayerColor)
                        {
                            break;
                        }

                        if (currentTile.Y - 1 < 0) break;
                        currentTile = _board[currentTile.X][currentTile.Y - 1];
                    }
                }
                //S
                if (tile.Y + 2 < _board.Length)
                {
                    var currentTile = _board[tile.X][tile.Y + 1];
                    var isTileInBetween = false;
                    while (currentTile != null)
                    {
                        if (!isTileInBetween && currentTile.State == Enums.TileStateEnum.Empty)
                            break;
                        else if (!isTileInBetween && currentTile.State == enemyPlayerColor)
                            isTileInBetween = true;

                        if (isTileInBetween && currentTile.State == Enums.TileStateEnum.Empty)
                        {
                            _possibleMoves.Add(currentTile);
                            break;
                        }
                        else if (isTileInBetween && currentTile.State == currentPlayerColor)
                        {
                            break;
                        }

                        if (currentTile.Y + 1 >= _board.Length) break;
                        currentTile = _board[currentTile.X][currentTile.Y + 1];
                    }
                }
                //NW
                if (tile.X - 2 >= 0 && tile.Y - 2 >= 0)
                {
                    var currentTile = _board[tile.X - 1][tile.Y - 1];
                    var isTileInBetween = false;
                    while (currentTile != null)
                    {
                        if (!isTileInBetween && currentTile.State == Enums.TileStateEnum.Empty)
                            break;
                        else if (!isTileInBetween && currentTile.State == enemyPlayerColor)
                            isTileInBetween = true;

                        if (isTileInBetween && currentTile.State == Enums.TileStateEnum.Empty)
                        {
                            _possibleMoves.Add(currentTile);
                            break;
                        }
                        else if (isTileInBetween && currentTile.State == currentPlayerColor)
                        {
                            break;
                        }

                        if (currentTile.X - 1 < 0 || currentTile.Y - 1 < 0) break;
                        currentTile = _board[currentTile.X - 1][currentTile.Y - 1];
                    }
                }
                //NE
                if (tile.X + 2 < _board.Length && tile.Y - 2 >= 0)
                {
                    var currentTile = _board[tile.X + 1][tile.Y - 1];
                    var isTileInBetween = false;
                    while (currentTile != null)
                    {
                        if (!isTileInBetween && currentTile.State == Enums.TileStateEnum.Empty)
                            break;
                        else if (!isTileInBetween && currentTile.State == enemyPlayerColor)
                            isTileInBetween = true;

                        if (isTileInBetween && currentTile.State == Enums.TileStateEnum.Empty)
                        {
                            _possibleMoves.Add(currentTile);
                            break;
                        }
                        else if (isTileInBetween && currentTile.State == currentPlayerColor)
                        {
                            break;
                        }

                        if (currentTile.X + 1 >= _board.Length || currentTile.Y - 1 < 0) break;
                        currentTile = _board[currentTile.X + 1][currentTile.Y - 1];
                    }
                }
                //SE
                if (tile.X + 2 < _board.Length && tile.Y + 2 < _board.Length)
                {
                    var currentTile = _board[tile.X + 1][tile.Y + 1];
                    var isTileInBetween = false;
                    while (currentTile != null)
                    {
                        if (!isTileInBetween && currentTile.State == Enums.TileStateEnum.Empty)
                            break;
                        else if (!isTileInBetween && currentTile.State == enemyPlayerColor)
                            isTileInBetween = true;

                        if (isTileInBetween && currentTile.State == Enums.TileStateEnum.Empty)
                        {
                            _possibleMoves.Add(currentTile);
                            break;
                        }
                        else if (isTileInBetween && currentTile.State == currentPlayerColor)
                        {
                            break;
                        }

                        if (currentTile.X + 1 >= _board.Length || currentTile.Y + 1 >= _board.Length) break;
                        currentTile = _board[currentTile.X + 1][currentTile.Y + 1];
                    }
                }
                //SW
                if (tile.X - 2 >= 0 && tile.Y + 2 < _board.Length)
                {
                    var currentTile = _board[tile.X - 1][tile.Y + 1];
                    var isTileInBetween = false;
                    while (currentTile != null)
                    {
                        if (!isTileInBetween && currentTile.State == Enums.TileStateEnum.Empty)
                            break;
                        else if (!isTileInBetween && currentTile.State == enemyPlayerColor)
                            isTileInBetween = true;

                        if (isTileInBetween && currentTile.State == Enums.TileStateEnum.Empty)
                        {
                            _possibleMoves.Add(currentTile);
                            break;
                        }
                        else if (isTileInBetween && currentTile.State == currentPlayerColor)
                        {
                            break;
                        }

                        if (currentTile.X - 1 < 0 || currentTile.Y + 1 >= _board.Length) break;
                        currentTile = _board[currentTile.X - 1][currentTile.Y + 1];
                    }
                }
            }
        }

        private void FlipPawns(Tile tile)
        {
            var currentPlayerColor = BlackTurn ? Enums.TileStateEnum.Black : Enums.TileStateEnum.White;
            var enemyPlayerColor = BlackTurn ? Enums.TileStateEnum.White : Enums.TileStateEnum.Black;

            //W
            if (tile.X - 2 >= 0)
            {
                var currentTile = _board[tile.X - 1][tile.Y];

                var lastTileInSeqence = currentTile;
                while (lastTileInSeqence.State == enemyPlayerColor)
                {
                    if (lastTileInSeqence.X - 1 < 0) break;
                    lastTileInSeqence = _board[lastTileInSeqence.X - 1][lastTileInSeqence.Y];
                }

                if (lastTileInSeqence.State == currentPlayerColor)
                    while (currentTile != null && currentTile.State == enemyPlayerColor)
                    {
                        currentTile.Flip();

                        if (currentTile.X - 1 < 0) break;
                        currentTile = _board[currentTile.X - 1][currentTile.Y];
                    }
            }
            //E
            if (tile.X + 2 < _board.Length)
            {
                var currentTile = _board[tile.X + 1][tile.Y];

                var lastTileInSeqence = currentTile;
                while (lastTileInSeqence.State == enemyPlayerColor)
                {
                    if (lastTileInSeqence.X + 1 >= _board.Length) break;
                    lastTileInSeqence = _board[lastTileInSeqence.X + 1][lastTileInSeqence.Y];
                }

                if (lastTileInSeqence.State == currentPlayerColor)
                    while (currentTile != null && currentTile.State == enemyPlayerColor)
                    {
                        currentTile.Flip();

                        if (currentTile.X + 1 >= _board.Length) break;
                        currentTile = _board[currentTile.X + 1][currentTile.Y];
                    }
            }
            //N
            if (tile.Y - 2 >= 0)
            {
                var currentTile = _board[tile.X][tile.Y - 1];

                var lastTileInSeqence = currentTile;
                while (lastTileInSeqence.State == enemyPlayerColor)
                {
                    if (lastTileInSeqence.Y - 1 < 0) break;
                    lastTileInSeqence = _board[lastTileInSeqence.X][lastTileInSeqence.Y - 1];
                }

                if (lastTileInSeqence.State == currentPlayerColor)
                    while (currentTile != null && currentTile.State == enemyPlayerColor)
                    {
                        currentTile.Flip();

                        if (currentTile.Y - 1 < 0) break;
                        currentTile = _board[currentTile.X][currentTile.Y - 1];
                    }
            }
            //S
            if (tile.Y + 2 < _board.Length)
            {
                var currentTile = _board[tile.X][tile.Y + 1];

                var lastTileInSeqence = currentTile;
                while (lastTileInSeqence.State == enemyPlayerColor)
                {
                    if (lastTileInSeqence.Y + 1 >= _board.Length) break;
                    lastTileInSeqence = _board[lastTileInSeqence.X][lastTileInSeqence.Y + 1];
                }

                if (lastTileInSeqence.State == currentPlayerColor)
                    while (currentTile != null && currentTile.State == enemyPlayerColor)
                    {
                        currentTile.Flip();

                        if (currentTile.Y + 1 >= _board.Length) break;
                        currentTile = _board[currentTile.X][currentTile.Y + 1];
                    }
            }
            //NW
            if (tile.X - 2 >= 0 && tile.Y - 2 >= 0)
            {
                var currentTile = _board[tile.X - 1][tile.Y - 1];

                var lastTileInSeqence = currentTile;
                while (lastTileInSeqence.State == enemyPlayerColor)
                {
                    if (lastTileInSeqence.X - 1 < 0 || lastTileInSeqence.Y - 1 < 0) break;
                    lastTileInSeqence = _board[lastTileInSeqence.X - 1][lastTileInSeqence.Y - 1];
                }

                if (lastTileInSeqence.State == currentPlayerColor)
                    while (currentTile != null && currentTile.State == enemyPlayerColor)
                    {
                        currentTile.Flip();

                        if (currentTile.X - 1 < 0 || currentTile.Y - 1 < 0) break;
                        currentTile = _board[currentTile.X - 1][currentTile.Y - 1];
                    }
            }
            //NE
            if (tile.X + 2 < _board.Length && tile.Y - 2 >= 0)
            {
                var currentTile = _board[tile.X + 1][tile.Y - 1];

                var lastTileInSeqence = currentTile;
                while (lastTileInSeqence.State == enemyPlayerColor)
                {
                    if (lastTileInSeqence.X + 1 >= _board.Length || lastTileInSeqence.Y - 1 < 0) break;
                    lastTileInSeqence = _board[lastTileInSeqence.X + 1][lastTileInSeqence.Y - 1];
                }

                if (lastTileInSeqence.State == currentPlayerColor)
                    while (currentTile != null && currentTile.State == enemyPlayerColor)
                    {
                        currentTile.Flip();

                        if (currentTile.X + 1 >= _board.Length || currentTile.Y - 1 < 0) break;
                        currentTile = _board[currentTile.X + 1][currentTile.Y - 1];
                    }
            }
            //SE
            if (tile.X + 2 < _board.Length && tile.Y + 2 < _board.Length)
            {
                var currentTile = _board[tile.X + 1][tile.Y + 1];

                var lastTileInSeqence = currentTile;
                while (lastTileInSeqence.State == enemyPlayerColor)
                {
                    if (lastTileInSeqence.X + 1 >= _board.Length || lastTileInSeqence.Y + 1 >= _board.Length) break;
                    lastTileInSeqence = _board[lastTileInSeqence.X + 1][lastTileInSeqence.Y + 1];
                }

                if (lastTileInSeqence.State == currentPlayerColor)
                    while (currentTile != null && currentTile.State == enemyPlayerColor)
                    {
                        currentTile.Flip();

                        if (currentTile.X + 1 >= _board.Length || currentTile.Y + 1 >= _board.Length) break;
                        currentTile = _board[currentTile.X + 1][currentTile.Y + 1];
                    }
            }
            //SW
            if (tile.X - 2 >= 0 && tile.Y + 2 < _board.Length)
            {
                var currentTile = _board[tile.X - 1][tile.Y + 1];

                var lastTileInSeqence = currentTile;
                while (lastTileInSeqence.State == enemyPlayerColor)
                {
                    if (lastTileInSeqence.X - 1 < 0 || lastTileInSeqence.Y + 1 >= _board.Length) break;
                    lastTileInSeqence = _board[lastTileInSeqence.X - 1][lastTileInSeqence.Y + 1];
                }

                if (lastTileInSeqence.State == currentPlayerColor)
                    while (currentTile != null && currentTile.State == enemyPlayerColor)
                    {
                        currentTile.Flip();

                        if (currentTile.X - 1 < 0 || currentTile.Y + 1 >= _board.Length) break;
                        currentTile = _board[currentTile.X - 1][currentTile.Y + 1];
                    }
            }
        }

        #endregion

        #region  UI

        private int invalidMoveSemaphore = 0;

        private async void ShowInvalidMoveLabel(string message = "")
        {
            InvalidMoveLabel.Visibility = Visibility.Visible;
            InvalidMoveLabel.Content = message;

            invalidMoveSemaphore++;

            await Task.Delay(3000);

            invalidMoveSemaphore--;

            if (invalidMoveSemaphore == 0)
            {
                InvalidMoveLabel.Visibility = Visibility.Hidden;
            }
        }

        private void ShowMoveHelpers()
        {
            HideMoveHelpers();
            foreach (var tile in _possibleMoves)
            {
                tile.Background = Brushes.LightPink;
            }
        }

        private void HideMoveHelpers()
        {
            foreach (var row in _board)
            {
                foreach (var tile in row)
                {
                    tile.Background = Brushes.DarkGreen;
                }
            }
        }

        #endregion

        private void ChangedAIStrategy(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var aiStrategyText = ((ComboBox)sender).Text;
                if (string.IsNullOrEmpty(aiStrategyText))
                    _aiStrategy = AIStrategies.MostCapturedTiles;
                else
                    _aiStrategy = (AIStrategies)Enum.Parse(typeof(AIStrategies), aiStrategyText);
            }
            catch (Exception)
            {
                _aiStrategy = AIStrategies.MostCapturedTiles;
            }
        }

        private void ChangedUsedAlgorithm(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var usedAlgorithmText = ((ComboBox)sender).Text;
                if (string.IsNullOrEmpty(usedAlgorithmText))
                    _usedAlgorithm = AlgorithmsEnum.MinMax;
                else
                    _usedAlgorithm = (AlgorithmsEnum)Enum.Parse(typeof(AlgorithmsEnum), usedAlgorithmText);
            }
            catch (Exception)
            {
                _usedAlgorithm = AlgorithmsEnum.MinMax;
            }
        }

        private void StartGameButtonClick(object sender, RoutedEventArgs e)
        {
            if (_aiIsThinking) return;

            _logsWriter.WriteLine($"Starting algorithm");
            _logsWriter.WriteLine($"{_usedAlgorithm};{_aiStrategy}");

            gameStopwatch.Restart();
            stepsCount = 0;

            _useAi = true;
            AskAI();
        }

        private void StopGameButtonClick(object sender, RoutedEventArgs e)
        {
            _useAi = false;
        }

        private void ResetGameButtonClick(object sender, RoutedEventArgs e)
        {
            if (_aiIsThinking) return;

            RestartGame();
        }
    }
}
