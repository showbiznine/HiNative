using HiNative.API.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml.Media.Imaging;

namespace HiNative.Services
{

    public class DataService
    {
        static string _host = "https://hinative.com/api/v1/";
        static string _hostV2 = "https://hinative.com/api/v2/";
        static Uri _missingImage = new Uri("ms-appx:/Assets/icon_h120.png");

        #region Users
        public static async Task<HNUserProfile> LoadProfile(int profileID)
        {
            string query = _host + "profiles/" + profileID;
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;

            HttpClient http = new HttpClient();
            http.DefaultRequestHeaders.Add("Authorization", "Token token=\"" + localSettings.Values["API_Token"] + "\"");
            var res = await http.GetAsync(query);
            var json = await res.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<HNUserResult>(json);

            #region Format
            foreach (var language in result.profile.user_attributes.study_languages_attributes)
            {
                language.language_name = DecodeService.DecodeLanguage((int)language.language_id);
            }
            foreach (var language in result.profile.user_attributes.native_languages_attributes)
            {
                language.language_name = DecodeService.DecodeLanguage((int)language.language_id);
            }
            foreach (var country in result.profile.user_attributes.user_interested_countries_attributes)
            {
                country.country_name = DecodeService.DecodeCountry((int)country.country_id);
            }
            if (result.profile.image_url == "missing_thumb.png")
            {
                result.profile.profile_image = new BitmapImage { UriSource = _missingImage };
            }
            else
            {
                result.profile.profile_image = new BitmapImage { UriSource = new Uri(result.profile.image_url) };
            } 
            #endregion

            return result.profile;
        }

        public static async Task<HNUserResult> LogIn(string username, string password)
        {
            string query = _host + "users/sign_in";
            HNLogin logInRoot = new HNLogin()
            {
                user = new HNCredintals()
                {
                    device_token = "",
                    login = username,
                    password = password,
                    platform = "android"
                }
            };

            HttpClient http = new HttpClient();
            string postBody = JsonConvert.SerializeObject(logInRoot);
            StringContent LogInPayload = new StringContent(postBody, Encoding.UTF8, "application/json");

            var res = await http.PostAsync(new Uri(query), LogInPayload);
            var json = await res.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<HNUserResult>(json);
            Debug.WriteLine("Login Suceeded: Welcome back " + result.profile.user_attributes.name);
            return result;
        }

        public static async Task<HNUserResult> SignUp(HNNewUserContainer newUser)
        {
            HttpClient http = new HttpClient();
            string request = _host + "users";

            var c = JsonConvert.SerializeObject(newUser,
                Formatting.None,
                new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                });
            HttpContent content = new StringContent(c, Encoding.UTF8, "application/json");
            var res = await http.PostAsync(request, content);
            var json = await res.Content.ReadAsStringAsync();

            var response = JsonConvert.DeserializeObject<HNUserResult>(json);
            return response;
        }

        public static async Task<HNQuestionsResult> ProfileSearch(int page, int id, string type)
        {
            string query = _host + "/profiles/" + id + "/" + type + "?page=" + page;
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
            HttpClient http = new HttpClient();
            http.DefaultRequestHeaders.Add("Authorization", "Token token=\"" + localSettings.Values["API_Token"] + "\"");

            var response = await http.GetAsync(query);
            var jsonMessage = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<HNQuestionsResult>(jsonMessage);

            foreach (var question in result.questions)
            {
                question.text = SetQuestionText(question);

                question.user.native_language = DecodeService.DecodeLanguage((int)question.user.native_languages[0].language_id);

                if (question.user.image_url != "missing_thumb.png")
                {
                    question.user.profile_image = new BitmapImage { UriSource = new Uri(question.user.image_url) };
                }
                else
                {
                    question.user.profile_image = new BitmapImage { UriSource = _missingImage};
                }
            }
            return result;
        }
        #endregion

        #region Questions

        public static async Task<int> PopulateQuestions(int queryID, int pageNumber, ObservableCollection<HNQuestion> questions, bool append, bool language)
        {

            var questionsDataWrapper = new HNQuestionsResult();
            var results = new ObservableCollection<HNQuestion>();
            #region PriorityQuestions
            if (!append)
            {
                questionsDataWrapper = await LoadQuestions(queryID, 1, 1, language);
                results = questionsDataWrapper.questions;
                foreach (var question in results)
                {
                    question.text = SetQuestionText(question);

                    question.user.native_language = question.user.native_languages[0].language_id != null ?
                        DecodeService.DecodeLanguage((int)question.user.native_languages[0].language_id) : "";

                    if (question.user.image_url != "missing_thumb.png")
                    {
                        question.user.profile_image = new BitmapImage { UriSource = new Uri(question.user.image_url) };
                    }
                    else
                    {
                        question.user.profile_image = new BitmapImage { UriSource = new Uri("ms-appx:/Assets/icon_h120.png") };
                    }
                    questions.Add(question);
                } 
            }
            #endregion

            #region PlebQuestions
            questionsDataWrapper = await LoadQuestions(queryID, pageNumber, 0, language);
            var maxPages = questionsDataWrapper.pagination.total_pages;
            results = questionsDataWrapper.questions;

            foreach (var question in results)
            {
                if (question.user.name != "Deleted account")
                {
                    question.text = SetQuestionText(question);

                    question.user.native_language = question.user.native_languages != null ?
                        DecodeService.DecodeLanguage((int)question.user.native_languages[0].language_id) : "";

                    if (question.user.image_url != "missing_thumb.png")
                    {
                        question.user.profile_image = new BitmapImage { UriSource = new Uri(question.user.image_url) };
                    }
                    else
                    {
                        question.user.profile_image = new BitmapImage { UriSource = new Uri("ms-appx:/Assets/icon_h120.png") }; ;
                    }

                    questions.Add(question);
                }
                else break;
            }
            return maxPages;
            #endregion
        }

        public static async Task<HNQuestionsResult> LoadQuestions(int queryID, int pageNumber, int priority, bool language)
        {
            string criteria = language ? "language_id=" : "country_id=";
            string query = _host + "questions?" + criteria + queryID + "&page=" + pageNumber + "&prior=" + priority;
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
            HttpClient http = new HttpClient();
            http.DefaultRequestHeaders.Add("Authorization", "Token token=\"" + localSettings.Values["API_Token"] + "\"");

            var res = await http.GetAsync(query);
            var json = await res.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<HNQuestionsResult>(json);
            //_premiumQs = priority == 1 ? result.questions.Count : 0;
            return result;
        }

        public static async Task<HttpResponseMessage> PostFilter(HNFilter filter)
        {
            HttpClient http = new HttpClient();
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
            http.DefaultRequestHeaders.Add("Authorization", "Token token=\"" + localSettings.Values["API_Token"] + "\"");
            string request = _host + "question_filters";

            var c = JsonConvert.SerializeObject(filter,
                Formatting.None,
                new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                });
            HttpContent content = new StringContent(c, Encoding.UTF8, "application/json");
            var res = await http.PatchAsync(new Uri(request), content);
            return res;
        }

        public static async Task<HNQuestionRoot> PostQuestion(HNQuestionRoot postQuestion)
        {
            HttpClient http = new HttpClient();
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
            http.DefaultRequestHeaders.Add("Authorization", "Token token=\"" + localSettings.Values["API_Token"] + "\"");
            string request = _host + "questions";

            var c = JsonConvert.SerializeObject(postQuestion,
                Formatting.None,
                new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                });
            HttpContent content = new StringContent(c, Encoding.UTF8, "application/json");
            var res = await http.PostAsync(request, content);
            var json = await res.Content.ReadAsStringAsync();

            var response = JsonConvert.DeserializeObject<HNQuestionRoot>(json);
            #region Fix Formatting
            if (response.question.user.image_url != "missing_thumb.png")
            {
                response.question.user.profile_image = new BitmapImage { UriSource = new Uri(response.question.user.image_url) };
            }
            else
            {
                response.question.user.profile_image = new BitmapImage { UriSource = _missingImage };
            }
            response.question.text = SetQuestionText(response.question);
            #endregion
            return response;
        }
        #endregion

        #region Answers
        public static async Task<HNQuestionRoot> LoadQuestionAndAnswers(int questionID)
        {
            string query = _host + "questions/" + questionID;
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
            HttpClient http = new HttpClient();
            http.DefaultRequestHeaders.Add("Authorization", "Token token=\"" + localSettings.Values["API_Token"] + "\"");

            var res = await http.GetAsync(query);
            var json = await res.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<HNQuestionRoot>(json);

            result.question.text = SetQuestionText(result.question);

            result.question.user.native_language = result.question.user.native_languages[0].language_id != null ?
                DecodeService.DecodeLanguage((int)result.question.user.native_languages[0].language_id) : "";

            if (result.question.user.image_url != "missing_thumb.png")
            {
                result.question.user.profile_image = new BitmapImage { UriSource = new Uri(result.question.user.image_url) };
            }
            else
            {
                result.question.user.profile_image = new BitmapImage { UriSource = new Uri("ms-appx:/Assets/icon_h120.png") };
            }

            foreach (var answer in result.question.answers)
            {
                if (answer.user.name != "Deleted account")
                {
                    answer.user.native_language = DecodeService.DecodeLanguage((int)answer.user.native_languages[0].language_id);
                    if (answer.user.image_url != "missing_thumb.png")
                    {
                        answer.user.profile_image = new BitmapImage { UriSource = new Uri(answer.user.image_url) };
                    }
                    else
                    {
                        answer.user.profile_image = new BitmapImage { UriSource = new Uri("ms-appx:/Assets/icon_h120.png") };
                    } 
                }
            }
            return result;
        }

        public static async Task<HNAnswerResult> PostAnswer(HNAnswer answer, int questionID)
        {
            string query = _host + "questions/" + questionID + "/answers";
            HttpClient http = new HttpClient();
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
            http.DefaultRequestHeaders.Add("Authorization", "Token token=\"" + localSettings.Values["API_Token"] + "\"");

            string postBody = JsonConvert.SerializeObject(answer,
                Formatting.None,
                new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            StringContent payload = new StringContent(postBody, System.Text.Encoding.UTF8, "application/json");

            var response = await http.PostAsync(new Uri(query), payload);
            var jsonMessage = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<HNAnswerResult>(jsonMessage);

            if (result.answer.user.image_url != "missing_thumb.png")
            {
                result.answer.user.profile_image = new BitmapImage { UriSource = new Uri(result.answer.user.image_url) };
            }
            else
            {
                result.answer.user.profile_image = new BitmapImage { UriSource = new Uri("ms-appx:/Assets/icon_h120.png") };
            }

            return result;
        }
        #endregion

        public static string SetQuestionText(HNQuestion question)
        {
            switch (question.type)
            {
                case "MeaningQuestion":
                    question.text = "What does \"" +
                        question.keywords[0].name +
                        "\" mean?";
                    break;

                case "ChoiceQuestion":
                    question.text = "Does \"" +
                        question.content +
                        "\" sound natural?";
                    break;
                case "DifferenceQuestion":
                    question.text = "What is the difference between \"" +
                        question.keywords[0].name +
                        "\" and \"" +
                        question.keywords[1].name +
                        "\"?";
                    // This needs to support more than two options!!!
                    break;
                case "WhatsayQuestion":
                    question.text = "How do you say \"" +
                        question.keywords[0].name +
                        "\" in " +
                        DecodeService.DecodeLanguage((int)question.language_id) +
                        "?";
                    // Decode language
                    break;
                case "ExampleQuestion":
                    question.text = "Please show me example sentences with \"" +
                        question.keywords[0].name + "\"";
                    // Decode language
                    break;
                case "FreeQuestion":
                    question.text = question.content;
                    // Decode language
                    break;
                case "CountryQuestion":
                    question.text = question.content;
                    // Decode language
                    break;
            }
            return question.text;
        }

        public static async Task<HNAttachmentResponse> UploadAttachment(StorageFile file, bool image, bool propic)
        {
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
            var id = localSettings.Values["User_ID"].ToString();
            string upload = image ? "/upload_image" : "/upload_audio";
            string destination = propic ? "profiles/" : "users/";
            string url = destination + id + upload;
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", "Token token=\"" + localSettings.Values["API_Token"] + "\"");
            client.BaseAddress = new Uri(_host);
            MultipartFormDataContent form = new MultipartFormDataContent();

            var stream = await file.OpenStreamForReadAsync();
            var content = new StreamContent(stream);
            content.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
            {
                Name = image ? "photo" : "audio",
                FileName = file.Name
            };
            string type = image ? "image/jpeg" : "audio/mpeg";
            content.Headers.ContentType = new MediaTypeHeaderValue(type);
            form.Add(content);

            var response = await client.PostAsync(url, form);
            var jsonMessage = response.Content.ReadAsStringAsync().Result;

            var result = JsonConvert.DeserializeObject<HNAttachmentResponse>(jsonMessage);
            return result;
        }

        #region Answer
        public static async Task<HNBookmark> PostBookmark(int answerID, string type)
        {
            string query = _host + "bookmarks/";
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;

            HNBookmarkRoot bookmarkDetails = new HNBookmarkRoot
            {
                bookmark = new HNBookmark
                {
                    bookmarkable_id = answerID,
                    bookmarkable_type = type
                }
            };

            HttpClient http = new HttpClient();

            string postBody = JsonConvert.SerializeObject(bookmarkDetails);

            StringContent LikeAnswer = new StringContent(postBody, System.Text.Encoding.UTF8, "application/json");
            http.DefaultRequestHeaders.Add("Authorization", "Token token=\"" + localSettings.Values["API_Token"] + "\"");
            var response = await http.PostAsync(new Uri(query), LikeAnswer);
            var jsonMessage = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<HNBookmark>(jsonMessage);
            Debug.WriteLine("Bookmarked comment " + answerID);
            return result;
        }

        public static async Task<string> LikeAnswer(int questionID, int answerID)
        {
            string query = _host + "questions/" + questionID + "/answers/" + answerID + "/like";
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
            HttpClient http = new HttpClient();

            string postBody = "";

            StringContent LikeAnswer = new StringContent(postBody, System.Text.Encoding.UTF8, "application/json");
            http.DefaultRequestHeaders.Add("Authorization", "Token token=\"" + localSettings.Values["API_Token"] + "\"");
            var response = await http.PostAsync(new Uri(query), LikeAnswer);
            var jsonMessage = await response.Content.ReadAsStringAsync();
            Debug.WriteLine("Liked comment " + answerID);
            return jsonMessage;
        }

        public static async Task<HNAnswerResult> EditComment(int answerID, int questionID, HNAnswerResult answer)
        {
            string query = _host + "questions/" + questionID + "/answers/" + answerID;
            HttpClient http = new HttpClient();
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
            http.DefaultRequestHeaders.Add("Authorization", "Token token=\"" + localSettings.Values["API_Token"] + "\"");

            string postBody = JsonConvert.SerializeObject(answer,
                Formatting.None,
                new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            StringContent payload = new StringContent(postBody, System.Text.Encoding.UTF8, "application/json");

            var response = await http.PatchAsync(new Uri(query), payload);
            if (!response.IsSuccessStatusCode)
            {
                Debug.WriteLine("Error updating answer. Error code: " + response.StatusCode);
            }
            var jsonMessage = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<HNAnswerResult>(jsonMessage);
            Debug.WriteLine("Updated answer: " + answerID);
            return result;
        }

        public static async Task<HttpResponseMessage> DeleteComment(int answerID, int questionID)
        {
            string query = _host + "questions/" + questionID + "/answers/" + answerID;
            HttpClient http = new HttpClient();
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
            http.DefaultRequestHeaders.Add("Authorization", "Token token=\"" + localSettings.Values["API_Token"] + "\"");

            var response = await http.DeleteAsync(new Uri(query));
            if (!response.IsSuccessStatusCode)
            {
                Debug.WriteLine("Error deleting answer. Error code: " + response.StatusCode);
            }
            else
            {
                Debug.WriteLine("Deleted answer: " + answerID);
            }
            return response;
        }

        public static async Task<HttpResponseMessage> ChooseOption(int questionID, int keywordID)
        {
            string query = _host + "questions/" + questionID + "/choice";
            HttpClient http = new HttpClient();
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
            http.DefaultRequestHeaders.Add("Authorization", "Token token=\"" + localSettings.Values["API_Token"] + "\"");
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("keyword_id", keywordID.ToString())
            });
            var response = await http.PostAsync(new Uri(query), content);
            if (!response.IsSuccessStatusCode)
            {
                Debug.WriteLine("Error selecting option. Error code: " + response.StatusCode);
            }
            else
            {
                Debug.WriteLine("Selected option");
            }
            return response;
        }
        #endregion

        public static async Task<HNUnreadCount> GetUnreadCount()
        {
            string query = _hostV2 + "activities/unread_count";
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
            HttpClient http = new HttpClient();
            http.DefaultRequestHeaders.Add("Authorization", "Token token=\"" + localSettings.Values["API_Token"] + "\"");

            var response = await http.GetAsync(query);
            var jsonMessage = await response.Content.ReadAsStringAsync();

             var result = JsonConvert.DeserializeObject<HNUnreadCount>(jsonMessage);
            return result;
        }

        public static async Task<HNActivities> GetNotifications(int page)
        {
            string query = _host + "activities?page=" + page;
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
            HttpClient http = new HttpClient();
            http.DefaultRequestHeaders.Add("Authorization", "Token token=\"" + localSettings.Values["API_Token"] + "\"");

            var response = await http.GetAsync(query);
            var jsonMessage = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<HNActivities>(jsonMessage);
            foreach (var ac in result.activities)
            {
                if (ac.action_user_image_url == null)
                {
                    ac.action_user_image_url = "https://hinative.com/assets/missing_thumb-0982db75e299a76516da417e30405397a274d059ca909b0e5390a35b41535387.png";
                }
                ac.payload.text = GetBodyText(ac);
            }
            return result;
        }

        public static async Task<HNActivity> CheckNotification(int id)
        {
            string query = _host + "activities/" + id;
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
            HttpClient http = new HttpClient();
            http.DefaultRequestHeaders.Add("Authorization", "Token token=\"" + localSettings.Values["API_Token"] + "\"");

            var response = await http.GetAsync(query);
            var jsonMessage = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<HNActivity>(jsonMessage);

            return result;
        }

        public static string GetBodyText(HNActivity ac)
        {
            switch (ac.payload.type)
            {
                case "MeaningQuestion":
                    ac.payload.text = "What does \"" +
                        ac.payload.keywords[0].name +
                        "\" mean?";
                    break;

                case "ChoiceQuestion":
                    ac.payload.text = "Does \"" +
                        ac.payload.content +
                        "\" sound natural?";
                    break;
                case "DifferenceQuestion":
                    ac.payload.text = "What is the difference between \"" +
                        ac.payload.keywords[0].name +
                        "\" and \"" +
                        ac.payload.keywords[1].name +
                        "\"?";
                    // This needs to support more than two options!!!
                    break;
                case "WhatsayQuestion":
                    ac.payload.text = "How do you say \"" +
                        ac.payload.keywords[0].name +
                        "\" in " +
                        DecodeService.DecodeLanguage((int)ac.payload.language_id) +
                        "?";
                    // Decode language
                    break;
                case "ExampleQuestion":
                    ac.payload.text = "Please show me example sentences with \"" +
                        ac.payload.keywords[0].name + "\"";
                    // Decode language
                    break;
                case "FreeQuestion":
                    ac.payload.text = ac.payload.content.ToString();
                    // Decode language
                    break;
                case "CountryQuestion":
                    ac.payload.text = ac.payload.content.ToString();
                    // Decode language
                    break;
            }
            return ac.payload.text;
        }

        public static string GetActivityType(string activity_type)
        {
            switch (activity_type)
            {
                case "Mention":
                    return " mentioned you in ";
                case "Answer":
                    return " answered ";
                // Determine whether question is yours, theirs or 3rd party's
                case "Like":
                    return " liked your answer in ";
                default:
                    return " did something we don't understand yet in ";
            }
        }

    }
}
