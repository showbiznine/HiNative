using HiNative.API.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Content Dialog item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace HiNative.Resources.Dialogs
{
    public sealed partial class SelectCountriesDialog : ContentDialog
    {
        public HNCountry SelectedCountry { get; set; }
        public ObservableCollection<HNCountry> CountriesList { get; set; }

        public SelectCountriesDialog()
        {
            this.InitializeComponent();
            SetupLanguageList();
        }

        private void SetupLanguageList()
        {
            CountriesList = new ObservableCollection<HNCountry>();
            #region Countries
            CountriesList.Add(new HNCountry { country_id = 1, country_name = "Afghanistan" });
            CountriesList.Add(new HNCountry { country_id = 2, country_name = "Åland Islands" });
            CountriesList.Add(new HNCountry { country_id = 3, country_name = "Albania" });
            CountriesList.Add(new HNCountry { country_id = 4, country_name = "Algeria" });
            CountriesList.Add(new HNCountry { country_id = 5, country_name = "American Samoa" });
            CountriesList.Add(new HNCountry { country_id = 6, country_name = "Andorra" });
            CountriesList.Add(new HNCountry { country_id = 7, country_name = "Angola" });
            CountriesList.Add(new HNCountry { country_id = 8, country_name = "Anguilla" });
            CountriesList.Add(new HNCountry { country_id = 9, country_name = "Antarctica" });
            CountriesList.Add(new HNCountry { country_id = 10, country_name = "Antigua And Barbuda" });
            CountriesList.Add(new HNCountry { country_id = 11, country_name = "Argentina" });
            CountriesList.Add(new HNCountry { country_id = 12, country_name = "Armenia" });
            CountriesList.Add(new HNCountry { country_id = 13, country_name = "Aruba" });
            CountriesList.Add(new HNCountry { country_id = 14, country_name = "Australia" });
            CountriesList.Add(new HNCountry { country_id = 15, country_name = "Austria" });
            CountriesList.Add(new HNCountry { country_id = 16, country_name = "Azerbaijan" });
            CountriesList.Add(new HNCountry { country_id = 17, country_name = "Bahamas" });
            CountriesList.Add(new HNCountry { country_id = 18, country_name = "Bahrain" });
            CountriesList.Add(new HNCountry { country_id = 19, country_name = "Bangladesh" });
            CountriesList.Add(new HNCountry { country_id = 20, country_name = "Barbados" });
            CountriesList.Add(new HNCountry { country_id = 21, country_name = "Belarus" });
            CountriesList.Add(new HNCountry { country_id = 22, country_name = "Belgium" });
            CountriesList.Add(new HNCountry { country_id = 23, country_name = "Belize" });
            CountriesList.Add(new HNCountry { country_id = 24, country_name = "Benin" });
            CountriesList.Add(new HNCountry { country_id = 25, country_name = "Bermuda" });
            CountriesList.Add(new HNCountry { country_id = 26, country_name = "Bhutan" });
            CountriesList.Add(new HNCountry { country_id = 27, country_name = "Bolivia, Plurinational State Of" });
            CountriesList.Add(new HNCountry { country_id = 28, country_name = "Bonaire, Sint Eustatius And Saba" });
            CountriesList.Add(new HNCountry { country_id = 29, country_name = "Bosnia And Herzegovina" });
            CountriesList.Add(new HNCountry { country_id = 30, country_name = "Botswana" });
            CountriesList.Add(new HNCountry { country_id = 31, country_name = "Bouvet Island" });
            CountriesList.Add(new HNCountry { country_id = 32, country_name = "Brazil" });
            CountriesList.Add(new HNCountry { country_id = 33, country_name = "British Indian Ocean Territory" });
            CountriesList.Add(new HNCountry { country_id = 34, country_name = "Brunei Darussalam" });
            CountriesList.Add(new HNCountry { country_id = 35, country_name = "Bulgaria" });
            CountriesList.Add(new HNCountry { country_id = 36, country_name = "Burkina Faso" });
            CountriesList.Add(new HNCountry { country_id = 37, country_name = "Burundi" });
            CountriesList.Add(new HNCountry { country_id = 38, country_name = "Cambodia" });
            CountriesList.Add(new HNCountry { country_id = 39, country_name = "Cameroon" });
            CountriesList.Add(new HNCountry { country_id = 40, country_name = "Canada" });
            CountriesList.Add(new HNCountry { country_id = 41, country_name = "Cape Verde" });
            CountriesList.Add(new HNCountry { country_id = 42, country_name = "Cayman Islands" });
            CountriesList.Add(new HNCountry { country_id = 43, country_name = "Central African Republic" });
            CountriesList.Add(new HNCountry { country_id = 44, country_name = "Chad" });
            CountriesList.Add(new HNCountry { country_id = 45, country_name = "Chile" });
            CountriesList.Add(new HNCountry { country_id = 46, country_name = "China" });
            CountriesList.Add(new HNCountry { country_id = 47, country_name = "Christmas Island" });
            CountriesList.Add(new HNCountry { country_id = 48, country_name = "Cocos (keeling) islands" });
            CountriesList.Add(new HNCountry { country_id = 49, country_name = "Colombia" });
            CountriesList.Add(new HNCountry { country_id = 50, country_name = "Comoros" });
            CountriesList.Add(new HNCountry { country_id = 51, country_name = "Congo" });
            CountriesList.Add(new HNCountry { country_id = 52, country_name = "Congo, The Democratic Republic Of The" });
            CountriesList.Add(new HNCountry { country_id = 53, country_name = "Cook Islands" });
            CountriesList.Add(new HNCountry { country_id = 54, country_name = "Costa Rica" });
            CountriesList.Add(new HNCountry { country_id = 56, country_name = "Croatia" });
            CountriesList.Add(new HNCountry { country_id = 57, country_name = "Cuba" });
            CountriesList.Add(new HNCountry { country_id = 58, country_name = "Curaçao" });
            CountriesList.Add(new HNCountry { country_id = 59, country_name = "Cyprus" });
            CountriesList.Add(new HNCountry { country_id = 60, country_name = "Czech Republic" });
            CountriesList.Add(new HNCountry { country_id = 55, country_name = "Côte D'ivoire" });
            CountriesList.Add(new HNCountry { country_id = 61, country_name = "Denmark" });
            CountriesList.Add(new HNCountry { country_id = 62, country_name = "Djibouti" });
            CountriesList.Add(new HNCountry { country_id = 63, country_name = "Dominica" });
            CountriesList.Add(new HNCountry { country_id = 64, country_name = "Dominican Republic" });
            CountriesList.Add(new HNCountry { country_id = 65, country_name = "Ecuador" });
            CountriesList.Add(new HNCountry { country_id = 66, country_name = "Egypt" });
            CountriesList.Add(new HNCountry { country_id = 67, country_name = "El Salvador" });
            CountriesList.Add(new HNCountry { country_id = 68, country_name = "Equatorial Guinea" });
            CountriesList.Add(new HNCountry { country_id = 69, country_name = "Eritrea" });
            CountriesList.Add(new HNCountry { country_id = 70, country_name = "Estonia" });
            CountriesList.Add(new HNCountry { country_id = 71, country_name = "Ethiopia" });
            CountriesList.Add(new HNCountry { country_id = 72, country_name = "Falkland Islands (malvinas)" });
            CountriesList.Add(new HNCountry { country_id = 73, country_name = "Faroe Islands" });
            CountriesList.Add(new HNCountry { country_id = 74, country_name = "Fiji" });
            CountriesList.Add(new HNCountry { country_id = 75, country_name = "Finland" });
            CountriesList.Add(new HNCountry { country_id = 76, country_name = "France" });
            CountriesList.Add(new HNCountry { country_id = 77, country_name = "French Guiana" });
            CountriesList.Add(new HNCountry { country_id = 78, country_name = "French Polynesia" });
            CountriesList.Add(new HNCountry { country_id = 79, country_name = "French Southern Territories" });
            CountriesList.Add(new HNCountry { country_id = 80, country_name = "Gabon" });
            CountriesList.Add(new HNCountry { country_id = 81, country_name = "Gambia" });
            CountriesList.Add(new HNCountry { country_id = 82, country_name = "Georgia" });
            CountriesList.Add(new HNCountry { country_id = 83, country_name = "Germany" });
            CountriesList.Add(new HNCountry { country_id = 84, country_name = "Ghana" });
            CountriesList.Add(new HNCountry { country_id = 85, country_name = "Gibraltar" });
            CountriesList.Add(new HNCountry { country_id = 86, country_name = "Greece" });
            CountriesList.Add(new HNCountry { country_id = 87, country_name = "Greenland" });
            CountriesList.Add(new HNCountry { country_id = 88, country_name = "Grenada" });
            CountriesList.Add(new HNCountry { country_id = 89, country_name = "Guadeloupe" });
            CountriesList.Add(new HNCountry { country_id = 90, country_name = "Guam" });
            CountriesList.Add(new HNCountry { country_id = 91, country_name = "Guatemala" });
            CountriesList.Add(new HNCountry { country_id = 92, country_name = "Guinea" });
            CountriesList.Add(new HNCountry { country_id = 93, country_name = "Guinea-bissau" });
            CountriesList.Add(new HNCountry { country_id = 94, country_name = "Guyana" });
            CountriesList.Add(new HNCountry { country_id = 95, country_name = "Haiti" });
            CountriesList.Add(new HNCountry { country_id = 96, country_name = "Heard Island And Mcdonald Islands" });
            CountriesList.Add(new HNCountry { country_id = 97, country_name = "Holy See (vatican City State)" });
            CountriesList.Add(new HNCountry { country_id = 98, country_name = "Honduras" });
            CountriesList.Add(new HNCountry { country_id = 99, country_name = "Hong Kong" });
            CountriesList.Add(new HNCountry { country_id = 100, country_name = "Hungary" });
            CountriesList.Add(new HNCountry { country_id = 101, country_name = "Iceland" });
            CountriesList.Add(new HNCountry { country_id = 102, country_name = "India" });
            CountriesList.Add(new HNCountry { country_id = 103, country_name = "Indonesia" });
            CountriesList.Add(new HNCountry { country_id = 104, country_name = "Iran, Islamic Republic Of" });
            CountriesList.Add(new HNCountry { country_id = 105, country_name = "Iraq" });
            CountriesList.Add(new HNCountry { country_id = 106, country_name = "Ireland" });
            CountriesList.Add(new HNCountry { country_id = 107, country_name = "Isle Of Man" });
            CountriesList.Add(new HNCountry { country_id = 108, country_name = "Israel" });
            CountriesList.Add(new HNCountry { country_id = 109, country_name = "Italy" });
            CountriesList.Add(new HNCountry { country_id = 110, country_name = "Jamaica" });
            CountriesList.Add(new HNCountry { country_id = 111, country_name = "Japan" });
            CountriesList.Add(new HNCountry { country_id = 112, country_name = "Jersey" });
            CountriesList.Add(new HNCountry { country_id = 113, country_name = "Jordan" });
            CountriesList.Add(new HNCountry { country_id = 114, country_name = "Kazakhstan" });
            CountriesList.Add(new HNCountry { country_id = 115, country_name = "Kenya" });
            CountriesList.Add(new HNCountry { country_id = 116, country_name = "Kiribati" });
            CountriesList.Add(new HNCountry { country_id = 117, country_name = "Korea, Democratic People's Republic Of" });
            CountriesList.Add(new HNCountry { country_id = 118, country_name = "Korea, Republic Of" });
            CountriesList.Add(new HNCountry { country_id = 119, country_name = "Kuwait" });
            CountriesList.Add(new HNCountry { country_id = 120, country_name = "Kyrgyzstan" });
            CountriesList.Add(new HNCountry { country_id = 121, country_name = "Lao People's Democratic Republic" });
            CountriesList.Add(new HNCountry { country_id = 122, country_name = "Latvia" });
            CountriesList.Add(new HNCountry { country_id = 123, country_name = "Lebanon" });
            CountriesList.Add(new HNCountry { country_id = 124, country_name = "Lesotho" });
            CountriesList.Add(new HNCountry { country_id = 125, country_name = "Liberia" });
            CountriesList.Add(new HNCountry { country_id = 126, country_name = "Libya" });
            CountriesList.Add(new HNCountry { country_id = 127, country_name = "Liechtenstein" });
            CountriesList.Add(new HNCountry { country_id = 128, country_name = "Lithuania" });
            CountriesList.Add(new HNCountry { country_id = 129, country_name = "Luxembourg" });
            CountriesList.Add(new HNCountry { country_id = 130, country_name = "Macao" });
            CountriesList.Add(new HNCountry { country_id = 131, country_name = "Macedonia, The Former Yugoslav Republic Of" });
            CountriesList.Add(new HNCountry { country_id = 132, country_name = "Madagascar" });
            CountriesList.Add(new HNCountry { country_id = 133, country_name = "Malawi" });
            CountriesList.Add(new HNCountry { country_id = 134, country_name = "Malaysia" });
            CountriesList.Add(new HNCountry { country_id = 135, country_name = "Maldives" });
            CountriesList.Add(new HNCountry { country_id = 136, country_name = "Mali" });
            CountriesList.Add(new HNCountry { country_id = 137, country_name = "Malta" });
            CountriesList.Add(new HNCountry { country_id = 138, country_name = "Marshall Islands" });
            CountriesList.Add(new HNCountry { country_id = 139, country_name = "Martinique" });
            CountriesList.Add(new HNCountry { country_id = 140, country_name = "Mauritania" });
            CountriesList.Add(new HNCountry { country_id = 141, country_name = "Mauritius" });
            CountriesList.Add(new HNCountry { country_id = 142, country_name = "Mayotte" });
            CountriesList.Add(new HNCountry { country_id = 143, country_name = "Mexico" });
            CountriesList.Add(new HNCountry { country_id = 144, country_name = "Micronesia, Federated States Of" });
            CountriesList.Add(new HNCountry { country_id = 145, country_name = "Moldova, Republic Of" });
            CountriesList.Add(new HNCountry { country_id = 146, country_name = "Monaco" });
            CountriesList.Add(new HNCountry { country_id = 147, country_name = "Mongolia" });
            CountriesList.Add(new HNCountry { country_id = 148, country_name = "Montenegro" });
            CountriesList.Add(new HNCountry { country_id = 149, country_name = "Montserrat" });
            CountriesList.Add(new HNCountry { country_id = 150, country_name = "Morocco" });
            CountriesList.Add(new HNCountry { country_id = 151, country_name = "Mozambique" });
            CountriesList.Add(new HNCountry { country_id = 152, country_name = "Myanmar" });
            CountriesList.Add(new HNCountry { country_id = 153, country_name = "Namibia" });
            CountriesList.Add(new HNCountry { country_id = 154, country_name = "Nauru" });
            CountriesList.Add(new HNCountry { country_id = 155, country_name = "Nepal" });
            CountriesList.Add(new HNCountry { country_id = 156, country_name = "Netherlands" });
            CountriesList.Add(new HNCountry { country_id = 157, country_name = "New Caledonia" });
            CountriesList.Add(new HNCountry { country_id = 158, country_name = "New Zealand" });
            CountriesList.Add(new HNCountry { country_id = 159, country_name = "Nicaragua" });
            CountriesList.Add(new HNCountry { country_id = 160, country_name = "Niger" });
            CountriesList.Add(new HNCountry { country_id = 161, country_name = "Nigeria" });
            CountriesList.Add(new HNCountry { country_id = 162, country_name = "Niue" });
            CountriesList.Add(new HNCountry { country_id = 163, country_name = "Norfolk Island" });
            CountriesList.Add(new HNCountry { country_id = 164, country_name = "Northern Mariana Islands" });
            CountriesList.Add(new HNCountry { country_id = 165, country_name = "Norway" });
            CountriesList.Add(new HNCountry { country_id = 166, country_name = "Oman" });
            CountriesList.Add(new HNCountry { country_id = 167, country_name = "Pakistan" });
            CountriesList.Add(new HNCountry { country_id = 168, country_name = "Palau" });
            CountriesList.Add(new HNCountry { country_id = 169, country_name = "Palestine, State Of" });
            CountriesList.Add(new HNCountry { country_id = 170, country_name = "Panama" });
            CountriesList.Add(new HNCountry { country_id = 171, country_name = "Papua New Guinea" });
            CountriesList.Add(new HNCountry { country_id = 172, country_name = "Paraguay" });
            CountriesList.Add(new HNCountry { country_id = 173, country_name = "Peru" });
            CountriesList.Add(new HNCountry { country_id = 174, country_name = "Philippines" });
            CountriesList.Add(new HNCountry { country_id = 175, country_name = "Pitcairn" });
            CountriesList.Add(new HNCountry { country_id = 176, country_name = "Poland" });
            CountriesList.Add(new HNCountry { country_id = 177, country_name = "Portugal" });
            CountriesList.Add(new HNCountry { country_id = 178, country_name = "Puerto Rico" });
            CountriesList.Add(new HNCountry { country_id = 179, country_name = "Qatar" });
            CountriesList.Add(new HNCountry { country_id = 181, country_name = "Romania" });
            CountriesList.Add(new HNCountry { country_id = 182, country_name = "Russian Federation" });
            CountriesList.Add(new HNCountry { country_id = 183, country_name = "Rwanda" });
            CountriesList.Add(new HNCountry { country_id = 180, country_name = "Réunion" });
            CountriesList.Add(new HNCountry { country_id = 184, country_name = "Saint Barthélemy" });
            CountriesList.Add(new HNCountry { country_id = 185, country_name = "Saint Helena, Ascension And Tristan Da Cunha" });
            CountriesList.Add(new HNCountry { country_id = 186, country_name = "Saint Kitts And Nevis" });
            CountriesList.Add(new HNCountry { country_id = 187, country_name = "Saint Lucia" });
            CountriesList.Add(new HNCountry { country_id = 188, country_name = "Saint Martin" });
            CountriesList.Add(new HNCountry { country_id = 189, country_name = "Saint Pierre And Miquelon" });
            CountriesList.Add(new HNCountry { country_id = 190, country_name = "Saint Vincent And The Grenadines" });
            CountriesList.Add(new HNCountry { country_id = 191, country_name = "Samoa" });
            CountriesList.Add(new HNCountry { country_id = 192, country_name = "San Marino" });
            CountriesList.Add(new HNCountry { country_id = 193, country_name = "Sao Tome And Principe" });
            CountriesList.Add(new HNCountry { country_id = 194, country_name = "Saudi Arabia" });
            CountriesList.Add(new HNCountry { country_id = 195, country_name = "Senegal" });
            CountriesList.Add(new HNCountry { country_id = 196, country_name = "Serbia" });
            CountriesList.Add(new HNCountry { country_id = 197, country_name = "Seychelles" });
            CountriesList.Add(new HNCountry { country_id = 198, country_name = "Sierra Leone" });
            CountriesList.Add(new HNCountry { country_id = 199, country_name = "Singapore" });
            CountriesList.Add(new HNCountry { country_id = 200, country_name = "Sint Maarten" });
            CountriesList.Add(new HNCountry { country_id = 201, country_name = "Slovakia" });
            CountriesList.Add(new HNCountry { country_id = 202, country_name = "Slovenia" });
            CountriesList.Add(new HNCountry { country_id = 203, country_name = "Solomon Islands" });
            CountriesList.Add(new HNCountry { country_id = 204, country_name = "Somalia" });
            CountriesList.Add(new HNCountry { country_id = 205, country_name = "South Africa" });
            CountriesList.Add(new HNCountry { country_id = 206, country_name = "South Georgia And The South Sandwich Islands" });
            CountriesList.Add(new HNCountry { country_id = 207, country_name = "South Sudan" });
            CountriesList.Add(new HNCountry { country_id = 208, country_name = "Spain" });
            CountriesList.Add(new HNCountry { country_id = 209, country_name = "Sri Lanka" });
            CountriesList.Add(new HNCountry { country_id = 210, country_name = "Sudan" });
            CountriesList.Add(new HNCountry { country_id = 211, country_name = "Suriname" });
            CountriesList.Add(new HNCountry { country_id = 212, country_name = "Svalbard And Jan Mayen" });
            CountriesList.Add(new HNCountry { country_id = 213, country_name = "Swaziland" });
            CountriesList.Add(new HNCountry { country_id = 214, country_name = "Sweden" });
            CountriesList.Add(new HNCountry { country_id = 215, country_name = "Switzerland" });
            CountriesList.Add(new HNCountry { country_id = 216, country_name = "Syrian Arab Republic" });
            CountriesList.Add(new HNCountry { country_id = 217, country_name = "Taiwan" });
            CountriesList.Add(new HNCountry { country_id = 218, country_name = "Tajikistan" });
            CountriesList.Add(new HNCountry { country_id = 219, country_name = "Tanzania, United Republic Of" });
            CountriesList.Add(new HNCountry { country_id = 220, country_name = "Thailand" });
            CountriesList.Add(new HNCountry { country_id = 221, country_name = "Timor-leste" });
            CountriesList.Add(new HNCountry { country_id = 222, country_name = "Tokelau" });
            CountriesList.Add(new HNCountry { country_id = 223, country_name = "Tonga" });
            CountriesList.Add(new HNCountry { country_id = 224, country_name = "Trincountry_idad And Tobago" });
            CountriesList.Add(new HNCountry { country_id = 225, country_name = "Tunisia" });
            CountriesList.Add(new HNCountry { country_id = 226, country_name = "Turkey" });
            CountriesList.Add(new HNCountry { country_id = 227, country_name = "Turkmenistan" });
            CountriesList.Add(new HNCountry { country_id = 228, country_name = "Turks And Caicos Islands" });
            CountriesList.Add(new HNCountry { country_id = 229, country_name = "Tuvalu" });
            CountriesList.Add(new HNCountry { country_id = 230, country_name = "Uganda" });
            CountriesList.Add(new HNCountry { country_id = 231, country_name = "Ukraine" });
            CountriesList.Add(new HNCountry { country_id = 232, country_name = "United Arab Emirates" });
            CountriesList.Add(new HNCountry { country_id = 233, country_name = "United Kingdom" });
            CountriesList.Add(new HNCountry { country_id = 234, country_name = "United States" });
            CountriesList.Add(new HNCountry { country_id = 235, country_name = "United States Minor Outlying Islands" });
            CountriesList.Add(new HNCountry { country_id = 236, country_name = "Uruguay" });
            CountriesList.Add(new HNCountry { country_id = 237, country_name = "Uzbekistan" });
            CountriesList.Add(new HNCountry { country_id = 238, country_name = "Vanuatu" });
            CountriesList.Add(new HNCountry { country_id = 239, country_name = "Venezuela, Bolivarian Republic Of" });
            CountriesList.Add(new HNCountry { country_id = 240, country_name = "Vietnam" });
            CountriesList.Add(new HNCountry { country_id = 241, country_name = "Virgin Islands, British" });
            CountriesList.Add(new HNCountry { country_id = 242, country_name = "Virgin Islands, U.S." });
            CountriesList.Add(new HNCountry { country_id = 243, country_name = "Wallis And Futuna" });
            CountriesList.Add(new HNCountry { country_id = 244, country_name = "Western Sahara" });
            CountriesList.Add(new HNCountry { country_id = 245, country_name = "Yemen" });
            CountriesList.Add(new HNCountry { country_id = 246, country_name = "Zambia" });
            CountriesList.Add(new HNCountry { country_id = 247, country_name = "Zimbabwe" });

            #endregion
            lstCountry.ItemsSource = CountriesList;
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }
    }
}
