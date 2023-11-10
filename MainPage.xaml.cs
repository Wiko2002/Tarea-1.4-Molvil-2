using System;
using System.IO;
using Microsoft.Maui.Controls;

namespace PhotoApp
{
    public partial class MainPage : ContentPage
    {
        DatabaseService databaseService;

        public MainPage()
        {
            InitializeComponent();
            databaseService = new DatabaseService("photos.db");
            RefreshPhotosList();
        }

        async void OnTakePhotoClicked(object sender, EventArgs e)
        {
            var photo = await MediaPicker.CapturePhotoAsync();

            if (photo != null)
            {
                var filePath = Path.Combine(FileSystem.AppDataDirectory, $"{Guid.NewGuid()}.jpg");
                File.Move(photo.FullPath, filePath);

                var photoModel = new Photo { FilePath = filePath };
                databaseService.InsertPhoto(photoModel);
                RefreshPhotosList();
            }
        }

        void RefreshPhotosList()
        {
            photosListView.ItemsSource = databaseService.GetPhotos();
        }

        async void OnPhotoSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null)
            {
                var selectedPhoto = (Photo)e.SelectedItem;
                await DisplayAlert("Foto Seleccionada", $"Ruta de archivo: {selectedPhoto.FilePath}", "OK");
                photosListView.SelectedItem = null;
            }
        }
    }
}
