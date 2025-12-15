using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace CatchSquare
{
    public partial class MainWindow : Window
    {
        Rectangle square;
        Random rnd = new Random();
        int score = 0, time = 30, record = 0;
        bool gameActive = false;
        DispatcherTimer timer;

        public MainWindow()
        {
            InitializeComponent();
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            LoadRecord();
        }

        void LoadRecord() => record = Игра.Properties.Settings.Default.Record;
        void SaveRecord() => Игра.Properties.Settings.Default.Record = record;

        void StartBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!gameActive) StartGame();
            else EndGame();
        }

        void StartGame()
        {
            score = 0; time = 30;
            gameActive = true;
            StartBtn.Content = "Стоп";
            GameCanvas.Children.Clear();
            CreateSquare();
            timer.Start();
            UpdateUI();
        }

        void EndGame()
        {
            gameActive = false;
            timer.Stop();
            StartBtn.Content = "Старт";
            if (score > record) { record = score; SaveRecord(); }
            UpdateUI();
        }

        void CreateSquare()
        {
            square = new Rectangle
            {
                Width = 50,
                Height = 50,
                Fill = Brushes.Red,
                Stroke = Brushes.Black
            };
            Canvas.SetLeft(square, rnd.Next(0, 350));
            Canvas.SetTop(square, rnd.Next(0, 300));
            GameCanvas.Children.Add(square);
        }

        void GameCanvas_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (!gameActive || square == null) return;

            if (square.IsMouseOver)
            {
                score++;
                GameCanvas.Children.Remove(square);
                CreateSquare();
                UpdateUI();
            }
        }

        void Timer_Tick(object sender, EventArgs e)
        {
            time--;
            if (time <= 0) EndGame();
            UpdateUI();
        }

        void UpdateUI()
        {
            ScoreText.Text = score.ToString();
            TimeText.Text = time.ToString();
            RecordText.Text = record.ToString();
        }
    }
}
