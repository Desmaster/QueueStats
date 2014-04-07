using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using src.summoner;
using src.util;
using src.views;
using RiotSharp;

namespace src {

    public partial class MainWindow : Window, SummonerListener {

        Client client;
        private Window currentWindow;

        public MainWindow() {
        InitializeComponent();

        //init client code
        client = new Client();
        client.summonerHandler.register(this);

        cbxRegion.ItemsSource = Enum.GetValues(typeof(Region));
        cbxTrackedSummoners_Update();

        currentWindow = new SummonerView();
        content.Content = currentWindow.Content;

        StatusHandler.window = this;
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
                if (currentWindow.GetType() == typeof(SummonerView)) return;
                setMenu(new SummonerView());
                break;
                case "Champions":
                if (currentWindow.GetType() == typeof(ChampionsView)) return;
                setMenu(new ChampionsView(this));
                break;
            }
        }

        public void setMenu(Window window) {
        currentWindow = window;
        content.Content = currentWindow.Content;
        }

        private void tbxSummonername_LostFocus(object sender, RoutedEventArgs e) {
        if (client.summonerHandler.getSummoner() != null) {
        if (tbxSummonername.Text != client.summonerHandler.getSummoner().Name) {
        setSummoner();
        }
        } else {
        setSummoner();
        }
        }

        private void cbxRegion_SelectionChanged(object sender, SelectionChangedEventArgs e) {
        setSummoner();
        }

        private void setSummoner() {
        if (cbxRegion.SelectedIndex != -1 && tbxSummonername.Text != "") {
        String name = tbxSummonername.Text;
        Region region = Util.resolveRegion(cbxRegion.SelectedItem.ToString());

        client.updateSummoner(name, region);
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
        client.updateSummoner(summoner);
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
        setSummonerpanel(summoner);
        }

        /// <summary>Sets the selected item of the TrackedSummoners combobox to the current TRACKEDSUMMONER</summary>
        /// <param name="summoner">The TrackedSummoner which will be set as the selected item</param>

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
        }

    }
}