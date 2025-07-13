using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using ChessLogic;

namespace ChessUI
{
    public partial class MainWindow : Window
    {
        private readonly Image[,] pieceImages = new Image[8, 8];
        private readonly Rectangle[,] highlights = new Rectangle[8, 8];
        private readonly Dictionary<Position, Move> moveCache = new();

        private GameState gameState;
        private Position selectedPos = null;

        // Drag & Drop
        private bool isDragging = false;
        private Image draggingImage = null;
        private Point dragStartPoint;
        private Position dragStartPos;

        public MainWindow()
        {
            InitializeComponent();
            InitializeBoard();

            gameState = new GameState(Player.White, Board.Initial());
            DrawBoard(gameState.Board);
            SetCursor(gameState.CurrentPlayer);

            MouseMove += Window_MouseMove;
            MouseUp += Window_MouseUp;
        }

        private void InitializeBoard()
        {
            for (int r = 0; r < 8; r++)
            {
                for (int c = 0; c < 8; c++)
                {
                    Image image = new();
                    pieceImages[r, c] = image;
                    PieceGrid.Children.Add(image);

                    Rectangle highlight = new();
                    highlights[r, c] = highlight;
                    HighlightGrid.Children.Add(highlight);
                }
            }
        }
        


        private void DrawBoard(Board board)
        {
            for (int r = 0; r < 8; r++)
            {
                for (int c = 0; c < 8; c++)
                {
                    Piece piece = board[r, c];
                    pieceImages[r, c].Source = Images.GetImage(piece);
                    pieceImages[r, c].RenderTransform = null; // reset drag transform
                }
            }
        }

        private void BoardGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (IsMenuOnScreen())
                return;

            Point point = e.GetPosition(BoardGrid);
            Position pos = ToSquarePosition(point);

            if (!gameState.Board.IsEmpty(pos) && gameState.Board[pos].Color == gameState.CurrentPlayer)
            {
                IEnumerable<Move> moves = gameState.LegalMovesForPiece(pos);
                if (moves.Any())
                {
                    CacheMoves(moves);
                    ShowHighlights();

                    draggingImage = pieceImages[pos.Row, pos.Column];
                    dragStartPoint = point;
                    dragStartPos = pos;
                    isDragging = true;
                    Panel.SetZIndex(draggingImage, 100);
                }
            }
        }

        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging && draggingImage != null)
            {
                Point currentPoint = e.GetPosition(BoardGrid);
                Vector offset = currentPoint - dragStartPoint;

                draggingImage.RenderTransform = new TranslateTransform(offset.X, offset.Y);
            }
        }

        private void Window_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (!isDragging || draggingImage == null) return;

            isDragging = false;

            Point dropPoint = e.GetPosition(BoardGrid);
            Position dropPos = ToSquarePosition(dropPoint);

            draggingImage.RenderTransform = null;
            Panel.SetZIndex(draggingImage, 0);

            selectedPos = dragStartPos;
            HideHighlights();
            if (moveCache.TryGetValue(dropPos, out Move move))
            {
                if (move.Type == MoveType.PawnPromotion)
                {
                    HandlePromotion(move.FromPos, move.ToPos);
                }
                else
                {
                    HandleMove(move);
                }
            }

            draggingImage = null;
        }

        private Position ToSquarePosition(Point point)
        {
            double squareSize = BoardGrid.ActualWidth / 8;
            int row = (int)(point.Y / squareSize);
            int col = (int)(point.X / squareSize);
            return new Position(row, col);
        }

        private void HandlePromotion(Position from, Position to)
        {
            pieceImages[to.Row, to.Column].Source = Images.GetImage(gameState.CurrentPlayer, PieceType.Pawn);
            pieceImages[from.Row, from.Column].Source = null;

            PromotionMenu menu = new(gameState.CurrentPlayer);
            MenuContainer.Content = menu;

            menu.PieceSelected += type =>
            {
                MenuContainer.Content = null;
                Move promMove = new PawnPromotion(from, to, type);
                HandleMove(promMove);
            };
        }

        private void HandleMove(Move move)
        {
            gameState.MakeMove(move);
            DrawBoard(gameState.Board);
            SetCursor(gameState.CurrentPlayer);

            if (gameState.IsGameOver())
            {
                ShowGameOver();
            }
        }

        private void CacheMoves(IEnumerable<Move> moves)
        {
            moveCache.Clear();
            foreach (Move move in moves)
            {
                moveCache[move.ToPos] = move;
            }
        }

        private void ShowHighlights()
        {
            Color color = Color.FromArgb(150, 125, 255, 125);
            foreach (Position to in moveCache.Keys)
            {
                highlights[to.Row, to.Column].Fill = new SolidColorBrush(color);
            }
        }

        private void HideHighlights()
        {
            foreach (Position to in moveCache.Keys)
            {
                highlights[to.Row, to.Column].Fill = Brushes.Transparent;
            }
        }

        private void SetCursor(Player player)
        {
            Cursor = player == Player.White ? ChessCursors.WhiteCursor : ChessCursors.BlackCursor;
        }

        private bool IsMenuOnScreen()
        {
            return MenuContainer.Content != null;
        }

        private void ShowGameOver()
        {
            GameOverMenu menu = new(gameState);
            MenuContainer.Content = menu;
            menu.OptionSelected += option =>
            {
                if (option == Option.Restart)
                {
                    MenuContainer.Content = null;
                    RestartGame();
                }
                else
                {
                    Application.Current.Shutdown();
                }
            };
        }

        private void RestartGame()
        {
            selectedPos = null;
            HideHighlights();
            moveCache.Clear();
            gameState = new GameState(Player.White, Board.Initial());
            DrawBoard(gameState.Board);
            SetCursor(gameState.CurrentPlayer);
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (!IsMenuOnScreen() && e.Key == Key.Escape)
            {
                ShowPauseMenu();
            }
        }

        private void ShowPauseMenu()
        {
            PauseMenu menu = new();
            MenuContainer.Content = menu;

            menu.OptionSelected += option =>
            {
                MenuContainer.Content = null;
                if (option == Option.Restart)
                {
                    RestartGame();
                }
            };
        }
    }
}
