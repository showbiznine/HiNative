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
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.Media.Capture;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Media.Imaging;

namespace HiNative.ViewModels
{
    public class ComposeQuestionViewModel : ViewModelBase
    {
        #region Fields
        private INavigationService _navigationService { get { return ServiceLocator.Current.GetInstance<INavigationService>(); } }
        public bool InCall { get; set; }
        public string QuestionType { get; set; }
        public string PrimaryText { get; set; }
        public string SecondaryText { get; set; }
        public ObservableCollection<object> Topics { get; set; }
        public ObservableCollection<BitmapImage> UploadImages { get; set; }
        public object SelectedTopic { get; set; }
        public double KeyboardHeight { get; set; }

        #region QuestionText
        public string PrimaryQuestionText { get; set; }
        public string DifferenceQuestionText1 { get; set; }
        public string DifferenceQuestionText2 { get; set; }
        public string DifferenceQuestionText3 { get; set; }
        public string DifferenceQuestionText4 { get; set; }
        public string AdditionalText { get; set; }
        #endregion

        #endregion

        #region Commands
        public RelayCommand ToggleMenuCommand { get; set; }
        public RelayCommand PostQuestionCommand { get; set; }

        public RelayCommand SelectPhotoCommand { get; set; }
        public RelayCommand TakePhotoCommand { get; set; }
        public RelayCommand RecordAudioCommand { get; set; }
        #endregion

        public ComposeQuestionViewModel()
        {
            if (IsInDesignMode)
            {

            }
            else
            {
                Topics = new ObservableCollection<object>();
                UploadImages = new ObservableCollection<BitmapImage>();
                InitializeCommands();
            }
        }

        private void InitializeCommands()
        {
            ToggleMenuCommand = new RelayCommand(() => { App.ViewModelLocator.Shell.IsMenuOpen = !App.ViewModelLocator.Shell.IsMenuOpen; });
            PostQuestionCommand = new RelayCommand(async () =>
            {
                #region Build Question
                InCall = true;
                var question = new HNQuestionRoot();
                question.question = new HNQuestion();
                question.type = QuestionType;

                var language = SelectedTopic as HNLanguage;

                #region Type
                switch (QuestionType)
                {
                    case "MeaningQuestion":
                        question.question.question_keywords_attributes = new ObservableCollection<HNQuestionKeywordsAttribute>();
                        question.question.language_id = language.language_id;
                        question.question.prior = 0;
                        question.question.question_keywords_attributes.Add(new HNQuestionKeywordsAttribute
                        {
                            _destroy = 0,
                            name = PrimaryQuestionText
                        });
                        break;

                    case "ChoiceQuestion":
                        question.question.question_keywords_attributes = new ObservableCollection<HNQuestionKeywordsAttribute>();
                        question.question.language_id = language.language_id;
                        question.question.prior = 0;
                        question.question.question_keywords_attributes.Add(new HNQuestionKeywordsAttribute
                        {
                            _destroy = 0,
                            name = PrimaryQuestionText
                        });
                        break;
                    case "DifferenceQuestion":
                        question.question.question_keywords_attributes = new ObservableCollection<HNQuestionKeywordsAttribute>();
                        question.question.language_id = language.language_id;
                        question.question.prior = 0;
                        question.question.question_keywords_attributes.Add(new HNQuestionKeywordsAttribute
                        {
                            _destroy = 0,
                            name = DifferenceQuestionText1
                        });
                        question.question.question_keywords_attributes.Add(new HNQuestionKeywordsAttribute
                        {
                            _destroy = 0,
                            name = DifferenceQuestionText2
                        });
                        if (!string.IsNullOrWhiteSpace(DifferenceQuestionText3))
                        {
                            question.question.question_keywords_attributes.Add(new HNQuestionKeywordsAttribute
                            {
                                _destroy = 0,
                                name = DifferenceQuestionText3
                            });
                        }
                        if (!string.IsNullOrWhiteSpace(DifferenceQuestionText4))
                        {
                            question.question.question_keywords_attributes.Add(new HNQuestionKeywordsAttribute
                            {
                                _destroy = 0,
                                name = DifferenceQuestionText4
                            });
                        }
                        break;
                    case "WhatsayQuestion":
                        question.question.question_keywords_attributes = new ObservableCollection<HNQuestionKeywordsAttribute>();
                        question.question.language_id = language.language_id;
                        question.question.prior = 0;
                        question.question.question_keywords_attributes.Add(new HNQuestionKeywordsAttribute
                        {
                            _destroy = 0,
                            name = PrimaryQuestionText
                        });
                        break;
                    case "ExampleQuestion":
                        question.question.question_keywords_attributes = new ObservableCollection<HNQuestionKeywordsAttribute>();
                        question.question.language_id = language.language_id;
                        question.question.prior = 0;
                        question.question.question_keywords_attributes.Add(new HNQuestionKeywordsAttribute
                        {
                            _destroy = 0,
                            name = PrimaryQuestionText
                        });
                        break;
                    case "FreeQuestion":
                        question.question.question_keywords_attributes = new ObservableCollection<HNQuestionKeywordsAttribute>();
                        question.question.language_id = language.language_id;
                        question.question.prior = 0;
                        question.question.question_keywords_attributes.Add(new HNQuestionKeywordsAttribute
                        {
                            _destroy = 0,
                            name = PrimaryQuestionText
                        });
                        break;
                }
                #endregion

                if (!string.IsNullOrWhiteSpace(AdditionalText))
                {
                    question.question.supplement = AdditionalText;
                }
                #endregion

                #region Attachments
                if (UploadImages.Count > 0)
                {
                    var file = await StorageFile.GetFileFromPathAsync(UploadImages[0].UriSource.AbsolutePath);
                    var response = await DataService.UploadAttachment(file, true, false);
                    question.image = new HNImage { id = response.image.id };
                }
                #endregion

                HNQuestionRoot result = new HNQuestionRoot ();
                try
                {
                    result = await DataService.PostQuestion(question);
                    LoggerService.LogEvent("Question_posted");
                    InCall = false;
                    App.ViewModelLocator.Question.CurrentQuestion = result.question;
                    App.ViewModelLocator.Question.LoadAnswers((int)result.question.id);
                    Random rnd = new Random();
                    if (rnd.Next(1,3) == 2)
                        App.ViewModelLocator.Shell.ShowAd();
                    _navigationService.NavigateTo(typeof(QuestionPage));
                }
                catch (Exception ex)
                {
                    if (ex is HttpRequestException)
                        await new MessageDialog("We're having trouble connecting to the HiNative servers").ShowAsync();
                    LoggerService.LogEvent("Posting_question_failed");
                }
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
                    UploadImages = new ObservableCollection<BitmapImage> { uploadImage };
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
                UploadImages = new ObservableCollection<BitmapImage> { image };

            });
            RecordAudioCommand = new RelayCommand(() =>
            {

            });
            #endregion
        }

        public void SetupQuestion()
        {
            switch (QuestionType)
            {
                case "WhatsayQuestion":
                    PrimaryText = "How do you say";
                    SecondaryText = "in";
                    break;
                case "ChoiceQuestion":
                    PrimaryText = "Does";
                    SecondaryText = "sound natural in";
                    break;
                case "ExampleQuestion":
                    PrimaryText = "Show me example sentences with";
                    SecondaryText = "in";
                    break;
                case "MeaningQuestion":
                    PrimaryText = "What does";
                    SecondaryText = "mean in";
                    break;
                case "DifferenceQuestion":
                    PrimaryText = "What is the difference between";
                    SecondaryText = "in";
                    break;
                case "FreeQuestion":
                    PrimaryText = "";
                    SecondaryText = "This question is about";
                    break;
                case "CountryQuestion":
                    PrimaryText = "";
                    SecondaryText = "This question is about";
                    break;
                default:
                    break;
            }
        }
    }
}
