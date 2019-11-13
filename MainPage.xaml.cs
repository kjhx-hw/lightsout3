using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;
using Windows.UI.Xaml.Navigation;
using Rectangle = Windows.UI.Xaml.Shapes.Rectangle;
using Point = System.Drawing.Point;
using Size = Windows.Foundation.Size;
using Windows.UI.Popups;
using Newtonsoft.Json;
using Windows.Storage;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace LightsOutUWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page {
        private LightsOutModel.LightsOutGame model;

        public MainPage() {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Enabled;

            ApplicationView.PreferredLaunchViewSize = new Size(500, 600);
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;

            model = new LightsOutModel.LightsOutGame();

            CreateGrid();
            DrawGrid();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e) {
            // Restore GRID STATE and SIZE
            if (ApplicationData.Current.LocalSettings.Values.ContainsKey("json")) {
                string json = ApplicationData.Current.LocalSettings.Values["json"] as string;
                model = JsonConvert.DeserializeObject<LightsOutModel.LightsOutGame>(json);
            }

        }

        protected override void OnNavigatedFrom(NavigationEventArgs e) {
            // Save GRID STATE and SIZE
            string json = JsonConvert.SerializeObject(model);
            ApplicationData.Current.LocalSettings.Values["json"] = json;
        }

        private void CreateGrid() {
            int rectSize = (int)theCanvas.Width / model.GridSize;

            for (int r = 0; r < model.GridSize; r++) {
                for (int c = 0; c < model.GridSize; c++) {
                    Rectangle rect = new Rectangle();

                    SolidColorBrush black = new SolidColorBrush(Windows.UI.Colors.Gray);

                    rect.Fill = black;
                    rect.Width = rectSize + 1;
                    rect.Height = rect.Width + 1;
                    rect.Stroke = black;

                    rect.Tag = new Point(r, c);

                    rect.Tapped += Rect_Tapped;

                    Canvas.SetTop(rect, r * rectSize);
                    Canvas.SetLeft(rect, c * rectSize);

                    theCanvas.Children.Add(rect);
                }
            }
        }

        private async void Rect_Tapped(object sender, TappedRoutedEventArgs e) {
            Rectangle rect = sender as Rectangle;
            var rowCol = (Point)rect.Tag;
            int row = (int)rowCol.X;
            int col = (int)rowCol.Y;

            model.Move(row, col);

            DrawGrid();

            if (model.IsGameOver()) {
                MessageDialog msgDialog = new MessageDialog("Congratulations! You won.");
                msgDialog.Commands.Add(new UICommand("OK"));
                await msgDialog.ShowAsync();
                model.NewGame();
                DrawGrid();
            }

            e.Handled = true;
        }

        private void DrawGrid() {
            int index = 0;
            SolidColorBrush black = new SolidColorBrush(Windows.UI.Colors.Gray);
            SolidColorBrush white = new SolidColorBrush(Windows.UI.Colors.White);

            for (int r = 0; r < model.GridSize; r++) {
                for (int c = 0; c < model.GridSize; c++) {
                    Rectangle rect = theCanvas.Children[index] as Rectangle;
                    index++;

                    if (model.GetGridValue(r, c)) {
                        rect.Fill = white;
                        rect.Stroke = black;
                    } else {
                        rect.Fill = black;
                        rect.Stroke = white;
                    }
                }
            }
        }

        private void btnNewGame(object sender, RoutedEventArgs e) {
            model.NewGame();
            DrawGrid();
        }

        private void btnAboutClick(object sender, RoutedEventArgs e) {
            this.Frame.Navigate(typeof(BlankPage1));
        }

        private void btnSettingsClick(object sender, RoutedEventArgs e) {
            this.Frame.Navigate(typeof(SettingsPage));
        }
    }
}
