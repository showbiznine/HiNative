using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using HiNative.Services;
using HiNative.Views;
using HiNativeShared.API.Models;
using HiNativeShared.Services;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Foundation.Metadata;
using Windows.Media.Capture;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Shapes;

namespace HiNative.ViewModels
{
    public class QuestionViewModel : ViewModelBase
    {
        #region Fields
        private INavigationService _navigationService { get { return ServiceLocator.Current.GetInstance<INavigationService>(); } }
        public bool InCall { get; set; }
        public bool IsCameraAvailable { get; set; }
        public bool CanSendAnswer { get; set; }
        public HNQuestion CurrentQuestion { get; set; }
        public ObservableCollection<HNAnswer> Answers { get; set; }
        public Ellipse LastClickedProfilePic { get; set; }
        public TextBlock LastClickedUsername { get; private set; }
        public ObservableCollection<BitmapImage> UploadImages { get; set; }
        public string AnswerText { get; set; }
        public int TotalVotes { get; set; }
        #endregion

        #region Commands
        public RelayCommand<TappedRoutedEventArgs> GoToOPProfileCommand { get; set; }
        public RelayCommand<TappedRoutedEventArgs> GoToProfileCommand { get; set; }

        public RelayCommand SubmitAnswerCommand { get; set; }
        public RelayCommand<ItemClickEventArgs> SelectOption { get; set; }

        public RelayCommand SelectPhotoCommand { get; set; }
        public RelayCommand TakePhotoCommand { get; set; }
        public RelayCommand RecordAudioCommand { get; set; }
        #endregion

        public QuestionViewModel()
        {
            if (IsInDesignMode)
            {

            }
            else
            {
                InitializeCommands();
                CheckCameras();
                CanSendAnswer = true;
                UploadImages = new ObservableCollection<BitmapImage>();
            }
        }

        private void InitializeCommands()
        {
            GoToProfileCommand = new RelayCommand<TappedRoutedEventArgs>(args =>
            {
                LastClickedProfilePic = args.OriginalSource as Ellipse;
                var parentGrid = LastClickedProfilePic.Parent as Grid;
                LastClickedUsername = parentGrid.FindName("lblAnswerUsername") as TextBlock;
                var dc = LastClickedProfilePic.DataContext as HNAnswer;
                App.ViewModelLocator.Profile.LoadUser((int)dc.user.id);
                App.ViewModelLocator.Profile.ProfilePicture = dc.user.profile_image;
                App.ViewModelLocator.Profile.UserName = dc.user.name;
                if (ApiInformation.IsTypePresent("Windows.UI.Xaml.Media.Animation.ConnectedAnimationService"))
                {
                    ConnectedAnimationService.GetForCurrentView().PrepareToAnimate("ProfilePicture", LastClickedProfilePic);
                    ConnectedAnimationService.GetForCurrentView().PrepareToAnimate("Username", LastClickedUsername);
                }
                _navigationService.NavigateTo(typeof(ProfilePage));

            });
            GoToOPProfileCommand = new RelayCommand<TappedRoutedEventArgs>(args =>
            {
                LastClickedProfilePic = args.OriginalSource as Ellipse;
                var parentGrid = (LastClickedProfilePic.Parent as Border).Parent as Grid;
                LastClickedUsername = parentGrid.FindName("lblUsername") as TextBlock;
                App.ViewModelLocator.Profile.LoadUser((int)CurrentQuestion.user.id);
                App.ViewModelLocator.Profile.ProfilePicture = CurrentQuestion.user.profile_image;
                App.ViewModelLocator.Profile.UserName = CurrentQuestion.user.name;
                if (ApiInformation.IsTypePresent("Windows.UI.Xaml.Media.Animation.ConnectedAnimationService"))
                {
                    ConnectedAnimationService.GetForCurrentView().PrepareToAnimate("ProfilePicture", LastClickedProfilePic);
                    ConnectedAnimationService.GetForCurrentView().PrepareToAnimate("Username", LastClickedUsername);
                }
                _navigationService.NavigateTo(typeof(ProfilePage));
            });

            SubmitAnswerCommand = new RelayCommand(async () =>
            {
                if (!string.IsNullOrWhiteSpace(AnswerText))
                {
                    CanSendAnswer = false;
                    InCall = true;

                    HNAnswerResult answer = new HNAnswerResult
                    {
                        answer = new HNAnswer
                        {
                            content = AnswerText,
                            quick = false
                        }
                    };

                    #region Attachments
                    if (UploadImages.Count > 0)
                    {
                        var file = await StorageFile.GetFileFromPathAsync(UploadImages[0].UriSource.AbsolutePath);
                        var response = await DataService.UploadAttachment(file, true, false);
                        answer.image = new HNImage { id = response.image.id };
                    }
                    #endregion

                    HNAnswerResult result = await DataService.PostAnswer(answer.answer, (int)CurrentQuestion.id);
                    UploadImages.Clear();
                    AnswerText = "";
                    Answers.Add(result.answer);
                    InCall = false;
                    CanSendAnswer = true;
                }
                else
                    await new MessageDialog("The answer box is empty").ShowAsync();
            });
            SelectOption = new RelayCommand<ItemClickEventArgs>(args =>
            {
                var lst = args.OriginalSource as ListView;
                int i = lst.IndexFromContainer(args.ClickedItem as Grid);
            });
            #region Attachments

            SelectPhotoCommand = new RelayCommand(async () =>
            {
                var picker = new FileOpenPicker();
                picker.ViewMode = PickerViewMode.Thumbnail;
                picker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
                picker.FileTypeFilter.Add(".jpg");
                picker.FileTypeFilter.Add(".jpeg");
                picker.FileTypeFilter.Add(".png");

                var imageFile = await picker.PickSingleFileAsync();
                if (imageFile != null)
                {
                    // Application now has read/write access to the picked file
                    Debug.WriteLine("Picked photo: " + imageFile.Name);
                    var uploadImage = new BitmapImage();
                    FileRandomAccessStream stream = (FileRandomAccessStream)await imageFile.OpenAsync(FileAccessMode.Read);
                    uploadImage.SetSource(stream);
                    UploadImages.Add(uploadImage);
                }
                else
                {
                    Debug.WriteLine("Operation cancelled.");
                }
            });
            TakePhotoCommand = new RelayCommand(async () =>
            {
                CameraCaptureUI captureUI = new CameraCaptureUI();
                captureUI.PhotoSettings.Format = CameraCaptureUIPhotoFormat.Jpeg;

                var imageFile = await captureUI.CaptureFileAsync(CameraCaptureUIMode.Photo);

                if (imageFile == null)
                    return;

                IRandomAccessStream stream = await imageFile.OpenAsync(FileAccessMode.Read);

                BitmapImage image = new BitmapImage();
                await image.SetSourceAsync(stream);
                UploadImages.Clear();
                UploadImages.Add(image);

            });
            RecordAudioCommand = new RelayCommand(() =>
            {

            });
            #endregion
        }

        private async void CheckCameras()
        {
            var devices = await DeviceInformation.FindAllAsync(Windows.Devices.Enumeration.DeviceClass.VideoCapture);
            IsCameraAvailable = false;
            if (devices.Count < 1)
                IsCameraAvailable = false;
            else
                IsCameraAvailable = true;
            SelectPhotoCommand.CanExecute(IsCameraAvailable);
        }

        public async void LoadAnswers(int qID)
        {
            InCall = true;
            TotalVotes = 0;
            foreach (var key in CurrentQuestion.keywords)
            {
                TotalVotes = TotalVotes + (int)key.count;
            }
            Answers = new ObservableCollection<HNAnswer>();
            var temp = await DataService.LoadQuestionAndAnswers(qID);
            foreach (var item in temp.question.answers)
            {
                Answers.Add(item);
            }
            InCall = false;
        }

        public async void LoadQuestion(int qID)
        {
            InCall = true;
            TotalVotes = 0;
            Answers = new ObservableCollection<HNAnswer>();
            var temp = await DataService.LoadQuestionAndAnswers(qID);
            CurrentQuestion = temp.question;
            foreach (var key in CurrentQuestion.keywords)
            {
                TotalVotes = TotalVotes + (int)key.count;
            }
            foreach (var item in temp.question.answers)
            {
                Answers.Add(item);
            }
            InCall = false;
        }
    }
}
