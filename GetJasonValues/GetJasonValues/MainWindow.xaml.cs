using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GetJasonValues {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e) {
            TextRange textRange = new TextRange(
                        // TextPointer to the start of content in the RichTextBox.
                        rtbJSONOriginal.Document.ContentStart,
                        // TextPointer to the end of content in the RichTextBox.
                        rtbJSONOriginal.Document.ContentEnd
                    );

            try {
                JObject DynamicData = (JObject)JsonConvert.DeserializeObject(textRange.Text);
                

                dynamic details = JObject.Parse(textRange.Text);
                JsonElement root = details.RootElement;
            } catch (Exception ex) {

            }



        }
    }
}
