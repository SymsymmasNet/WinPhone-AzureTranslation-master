using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using My_Translation_App.Services;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace My_Translation_App
{
    public partial class MainPage : PhoneApplicationPage, INotifyPropertyChanged
    {
        TranslationService _translator;

        public MainPage()
        {
            InitializeComponent();
            DataContext = this;

            Languages = new List<string> { "Anglais", "Espagnol", "Français", "Italien", "Russe", "Créole" };
            LanguagesValues = new List<string> { "en", "es", "fr", "it", "ru", "ht" };
            
            // Set selected items.
            fromLanguage.SelectedIndex = 2;
            toLanguage.SelectedIndex = 0;

            // Change max items allowed in one single view, from 5 to 10 items.
            fromLanguage.SetValue(Microsoft.Phone.Controls.ListPicker.ItemCountThresholdProperty, 10);
            toLanguage.SetValue(Microsoft.Phone.Controls.ListPicker.ItemCountThresholdProperty, 10);

            _translator = new TranslationService();
            _translator.TranslationComplete += _translator_TranslationComplete;
            _translator.TranslationFailed += _translator_TranslationFailed;
        }
        
        void _translator_TranslationComplete(object sender, TranslationCompleteEventArgs e)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                targetTextBox.Text = e.Translation;
            });
        }

        void _translator_TranslationFailed(object sender, TranslationFailedEventArgs e)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                MessageBox.Show("Bummer, the translation failed. \n " + e.ErrorDescription);
            });
        }

        private void On_CheckClicked(object sender, System.EventArgs e)
        {
            if (sourceTextBox.Text.Length != 0) {
                _translator.GetTranslation(sourceTextBox.Text, LanguagesValues[fromLanguage.SelectedIndex], LanguagesValues[toLanguage.SelectedIndex]);
            }   
        }

        /**************************************************
        * INotifyPropertyChanged interface implementation *
        **************************************************/
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler) {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private bool NotifyPropertyChanged<T>(ref T variable, T valeur, [CallerMemberName] string nomPropriete = null)
        {
            if (object.Equals(variable, valeur)) {
                return false;
            }
            variable = valeur;
            NotifyPropertyChanged(nomPropriete);
            return true;
        }

        private List<string> languages;
        public List<string> Languages
        {
            get { return languages; }
            set { NotifyPropertyChanged(ref languages, value); }
        }

        public List<string> LanguagesValues { get; set; }
    }
}