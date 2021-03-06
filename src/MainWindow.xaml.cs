﻿using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using src.summoner;
using src.util;
using src.views;
using RiotSharp;

namespace src {

    public partial class MainWindow : Window, SummonerListener {

        private Client client;
        private Window summonerView;
        private Window matchesView;
        private Window itemView;
        private Window championView;
        private Window statisticsView;
        private Window settingsView;

        private Window notfoundView;

        public MainWindow() {
            InitializeComponent();
            Hide();

            //init client code
            client = new Client();
            client.summonerHandler.register(this);

            StatusHandler.window = this;

            initViews();
    
            cbxRegion.ItemsSource = Enum.GetValues(typeof(Region));
            cbxTrackedSummoners_Update();

            setMenu(summonerView);
            btnClose.Click += btnClose_Click;
            btnHide.Click += btnHide_Click;
            Show();
        }

        public void initViews() {
            summonerView = new SummonerView();
            itemView = new ItemListView(this);
            championView = new ChampionsView(this);
            matchesView = new MatchesView();
            statisticsView = new StatisticsView(this);
			notfoundView = new NotfoundView(this);
            setMenu(summonerView);
            settingsView = new SettingsView(this);

            notfoundView = new NotfoundView(this);


            cbxRegion.ItemsSource = Enum.GetValues(typeof(Region));
            cbxTrackedSummoners_Update();

            setMenu(summonerView);
        }

        bool mouseDown = false;
        int lastX, lastY;

        private void Window_MouseDown(object sender, MouseButtonEventArgs e) {
            mouseDown = true;
            lastX = (int)e.GetPosition(this).X;
            lastY = (int)e.GetPosition(this).Y;
        }

        private void Window_MouseUp(object sender, MouseButtonEventArgs e) {
            mouseDown = false;
        }

        private void Window_MouseMove(object sender, MouseEventArgs e) {
            if (mouseDown) {
                int xDiff = (int)e.GetPosition(this).X - lastX;
                int yDiff = (int)e.GetPosition(this).Y - lastY;
                this.Left += xDiff;
                this.Top += yDiff;
            }
        }

        private void Window_MouseLeave(object sender, MouseEventArgs e) {
            mouseDown = false;
        }

        private void Menu_Click(object sender, EventArgs args) {
            var button = (sender as Button);
            if (button != null)
                switch (button.Content.ToString()) {
                    case "Summoner":
                    setMenu(summonerView);
                    break;
                    case "Matches":
                    setMenu(matchesView);
                    break;
                    case "Statistics":
                    setMenu(statisticsView);
                    break;
                    case "Champions":
                    setMenu(championView);
                    break;
                    case "Items":
                    setMenu(itemView);
                    break;
                    case "Settings":
                    setMenu(settingsView);
                    break;
                }
        }

        public void setMenu(Window window) {
            content.Content = window.Content;
        }

        private void cbxRegion_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (cbxRegion.IsDropDownOpen) {
                setSummoner();
            }
        }

        private async void setSummoner() {
            if (cbxRegion.SelectedIndex != -1 && tbxSummonername.Text != "") {
                String name = tbxSummonername.Text;
                Region region = Util.resolveRegion(cbxRegion.SelectedItem.ToString());

                if (await client.updateSummoner(name, region)) {
                    setMenu(summonerView);
                } else {
                    ImageBrush image = new ImageBrush();
                    image.ImageSource = Util.CreateImage("http://ddragon.leagueoflegends.com/cdn/img/champion/splash/Heimerdinger_2.jpg");
                    image.Opacity = 0.5;
                    
                    setBackground(image);

                    setMenu(notfoundView);
                }
            }
        }

        private void cbxTrackedSummoners_Update() {
            cbxTrackedSummoners.ItemsSource = client.summonerHandler.trackedSummoners;
            cbxTrackedSummoners.Items.Refresh();
        }

        private void cbxTrackedSummoners_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (cbxTrackedSummoners.IsDropDownOpen) {
                cbxTrackedSummoner_SelectSummoner();
            }
        }
        private void cbxTrackedSummoners_MouseWheel(object sender, MouseWheelEventArgs e) {
            cbxTrackedSummoner_SelectSummoner();
        }

        private void cbxTrackedSummoner_SelectSummoner() {
            TrackedSummoner summoner = cbxTrackedSummoners.SelectedItem as TrackedSummoner;
            tbxSummonername.Text = summoner.Name;
            cbxRegion.SelectedItem = summoner.Region;

            setSummoner();
        }

        private void cbTrack_Click(object sender, RoutedEventArgs e) {
            Summoner selectedSummoner = client.summonerHandler.getSummoner();

            if (selectedSummoner.Id != 0) {
                switch ((sender as CheckBox).IsChecked) {
                    case true:
                    client.summonerHandler.trackSummoner(selectedSummoner);
                    cbxTrackedSummoners_SelectionToCurrent(selectedSummoner);
                    break;
                    case false:
                    client.summonerHandler.untrackSummoner(selectedSummoner);
                    cbxTrackedSummoners.SelectedIndex = -1;
                    break;
                }

                cbTrackSearch.IsChecked = (sender as CheckBox).IsChecked;
                cbTrackTracked.IsChecked = (sender as CheckBox).IsChecked;

                cbxTrackedSummoners_Update();
            }
        }

        public void summonerUpdated(Summoner summoner) {
            if (summoner == null) {
                StatusHandler.error("Error loading summoner..");
                return;
            }
            setSummonerpanel(summoner);
        }

        private void cbxTrackedSummoners_SelectionToCurrent(Summoner summoner) {
            cbxTrackedSummoners.SelectedIndex =
                        cbxTrackedSummoners.Items.IndexOf(cbxTrackedSummoners.Items.Cast<TrackedSummoner>().Single(x => x.Id == summoner.Id && x.Region == summoner.Region));
        }

        private void tbxSummonername_TextChanged(object sender, TextChangedEventArgs e) {
            if (tbxSummonername.Text.Length == 0) {
                resetSummonerpanel();
            }
        }

        private void setSummonerpanel(Summoner summoner) {
            if (summoner == null)
                return;
            if (summoner.Id != 0) {
                cbTrackSearch.IsEnabled = true;
                cbTrackTracked.IsEnabled = true;

                if (client.summonerHandler.isTracked(summoner)) {
                    cbxTrackedSummoners_SelectionToCurrent(summoner);

                    cbTrackSearch.IsChecked = true;
                    cbTrackTracked.IsChecked = true;
                } else {
                    cbxTrackedSummoners.SelectedIndex = -1;
                }

                //Show summoner info
                imgProfileIcon.Source = Util.CreateImage(Core.getInstance().getAssetsPath() + @"profileicon\" + summoner.ProfileIconId + ".png");
                lblSummonerName.Content = summoner.Name;
                lblSummonerLevel.Content = summoner.Level;
                lblSummonerRegion.Content = summoner.Region.ToString().ToUpper();
            } else {
                StatusHandler.error("Summoner not found");
                resetSummonerpanel();
            }
        }

        private void resetSummonerpanel() {
            client.summonerHandler.resetSummoner();

            cbTrackSearch.IsChecked = false;
            cbTrackTracked.IsChecked = false;

            cbTrackSearch.IsEnabled = false;
            cbTrackTracked.IsEnabled = false;

            cbxTrackedSummoners.SelectedIndex = -1;
            cbxRegion.SelectedIndex = -1;
        }

        public void setBackground(ImageBrush brush) {
            gridBackground.Background = brush;
        }

        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            tbxSummonername.Text = "";
            resetSummonerpanel();
        }

        private void Reload_Click(object sender, RoutedEventArgs e) {
            setSummoner();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e) {
            Close();
            Application.Current.Shutdown(0);
        }

        private void btnHide_Click(object sender, RoutedEventArgs e) {
            WindowState = System.Windows.WindowState.Minimized;
        }
    }
}