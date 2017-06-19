﻿using AODamageMeter.UI.Properties;
using AODamageMeter.UI.ViewModels;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace AODamageMeter.UI.Views
{
    public partial class DamageMeterView : Window
    {
        private readonly DamageMeterViewModel _damageMeterViewModel;

        public DamageMeterView()
        {
            InitializeComponent();
            DataContext = _damageMeterViewModel = new DamageMeterViewModel();
        }

        private void FileButton_Click_ShowCharacterSelection(object sender, RoutedEventArgs e)
        {
            string previousSelectedCharacterName = Settings.Default.SelectedCharacterName;
            string previousSelectedLogFilePath = Settings.Default.SelectedLogFilePath;

            var characterSelectionView = new CharacterSelectionView(_damageMeterViewModel);
            if (characterSelectionView.ShowDialog() == true)
            {
                _damageMeterViewModel.TryInitializeDamageMeter(
                    Settings.Default.SelectedCharacterName, Settings.Default.SelectedLogFilePath);
            }
        }

        private void OptionsButton_Click_ShowOptions(object sender, RoutedEventArgs e)
            => new OptionsView { Owner = this }.ShowDialog();

        private void HeaderRow_MouseDown_Drag(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                DragMove();
            }
        }

        private void MainRowView_ProgressViewRequested_TryProgressView(object sender, RoutedEventArgs e)
            => _damageMeterViewModel.TryProgressView(((MainRowView)e.OriginalSource).MainRow);

        private void MainGridView_MouseRightButtonDown_TryRegressView(object sender, MouseButtonEventArgs e)
            => _damageMeterViewModel.TryRegressView();

        private void CloseButton_Click_CloseApplication(object sender, RoutedEventArgs e)
            => Close();

        protected override void OnClosing(CancelEventArgs e)
        {
            Settings.Default.Save();
            _damageMeterViewModel.DisposeDamageMeter();

            base.OnClosing(e);
        }
    }
}
