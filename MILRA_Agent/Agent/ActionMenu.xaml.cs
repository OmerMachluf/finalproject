using MaterialDesignColors.WpfExample.Domain;
using MaterialDesignDemo.Domain;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mail;
using System.Text;
using System.Text.RegularExpressions;
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

namespace Agent
{
    /// <summary>
    /// Interaction logic for ActtionMenu.xaml
    /// </summary>
    public partial class ActionMenu : UserControl
    {

        enum ValidationStates { OK, ERROR, WARNING };

        // Tables for regex and messages
        Hashtable previewRegex = new Hashtable();
        Hashtable completionRegex = new Hashtable();
        Hashtable errorMessage = new Hashtable();
        Hashtable validationState = new Hashtable();
        Dictionary<string,Button> textBoxtoAceeptB = new Dictionary<string, Button>();

        const string fieldRequired = "This field is required";

        private const string ACCEPT_STRING = "ACCEPT";
        private const string CANCEL_STRING = "CANCEL";

        public ActionMenu()
        {
            InitializeComponent();
            fillHash();
        }


        private void OnSubscribeDialogClosing(object sender, DialogClosingEventArgs eventArgs)
        {
            if (((string)eventArgs.Parameter) == CANCEL_STRING)
                return;

            if ((!string.IsNullOrWhiteSpace(subMail.Text)) && (!string.IsNullOrWhiteSpace(subName.Text)))
                CreateMessage("smtp.gmail.com", subName.Text, subMail.Text);
             //   FruitListBox.Items.Add(FruitTextBox.Text.Trim());
        }

        private void OnOnepDialogClosing(object sender, DialogClosingEventArgs eventArgs)
        {
            if (((string)eventArgs.Parameter) == CANCEL_STRING)
                return;

            if ((!string.IsNullOrWhiteSpace(onepMail.Text)) && (!string.IsNullOrWhiteSpace(onepName.Text)))
                CreateMessage("smtp.gmail.com", subName.Text, subMail.Text);
            //   FruitListBox.Items.Add(FruitTextBox.Text.Trim());
        }



        private void Onepager_OnClick(object sender, RoutedEventArgs e)
        {
            Process.Start("https://twitter.com/James_Willock");
        }

        private void Subscribe_OnClick(object sender, RoutedEventArgs e)
        {
         //   Process.Start("mailto://james@dragablz.net");
        }

        private void Export_OnClick(object sender, RoutedEventArgs e)
        {
            Process.Start("https://pledgie.com/campaigns/31029");
        }

        public static void CreateMessage(string server, string name, string to)
        {
            try
            {
                System.Net.Mail.MailMessage oMsg = new System.Net.Mail.MailMessage("marom@MILRA.com",to, "Thank you for subscribing MILRA", "<HTML><BODY><B>Hello " + name + ",\n" + "Welcome to MILRA.</B></BODY></HTML>");

                // ADD AN ATTACHMENT.
                // TODO: Replace with path to attachment.
                //String sFile = @"C:\temp\Hello.txt";
                //MailAttachment oAttch = new MailAttachment(sFile, MailEncoding.Base64);

                //oMsg.Attachments.Add(oAttch);

                // TODO: Replace with the name of your remote SMTP server.
                System.Net.Mail.SmtpClient  smtp = new System.Net.Mail.SmtpClient("smtp.1and1.com");
                smtp.Send(oMsg);

                oMsg = null;
                //oAttch = null;
            }
            catch (Exception e)
            {
                Console.WriteLine("{0} Exception caught.", e);
            }



            //create the mail message
           // MailMessage mail = new MailMessage();
            //set the FROM address
           // mail.From = new MailAddress("marom@MILRA.com");
            //set the RECIPIENTS
           // mail.To.Add(to);
            //enter a SUBJECT
         //   mail.Subject = "Thank you for subscribing MILRA";
            //Enter the message BODY
         //   mail.Body = "Hello " + name + ",\n" + "Welcome to MILRA.";
            //set the mail server (default should be smtp.1and1.com)
         //   SmtpClient smtp = new SmtpClient("smtp.1and1.com");
            //Enter your full e-mail address and password
        //    smtp.Credentials = new NetworkCredential("marom@MILRA.com", "Aa123456");
            //send the message 
            

          //  string from = "marom@MILRA.com";
          //  MailMessage message = new MailMessage(from, to);
          //  message.Subject = "Thank you for subscribing MILRA";
          //  message.Body = @"Hello " + name + ",\n" + "Welcome to MILRA";
          //  SmtpClient client = new SmtpClient(server);
            // Credentials are necessary if the server requires the client 
            // to authenticate before it will send e-mail on the client's behalf.
           // client.UseDefaultCredentials = true;

        //    try
        //    {
                //client.Send(message);
        //        smtp.Send(mail);
       //     }
       //     catch (Exception ex)
       //     {
       //         Console.WriteLine("Exception caught in CreateMessage(): {0}",
       ///                     ex.ToString());
       //     }
        }

        #region Validation

        private void fillHash()
        {
            previewRegex["Date"] = @"^(\d| |-)*$";
            completionRegex["Date"] = @"^(19|20)\d\d[- /.](0[1-9]|1[012])[- /.](0[1-9]|[12][0-9]|3[01])$";
            errorMessage["Date"] = "Write date on the form YYYY-MM-DD";

            previewRegex["Email"] = @"";
            completionRegex["Email"] = @"^([0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})$";
            errorMessage["Email"] = "Enter a valid email, eg. sven.svensson@sven.se";

            previewRegex["Telephone"] = @"^(\d| |-)*$";
            completionRegex["Telephone"] = @"^(\d| |-)*$"; // This regex needs some improvement!
            errorMessage["Telephone"] = "Valid phonenumbers contains digits, numbers or spaces";

            textBoxtoAceeptB[subName.Name] = subAcceptB;
            textBoxtoAceeptB[subMail.Name] = subAcceptB;
            textBoxtoAceeptB[onepName.Name] = onepAcceptB;
            textBoxtoAceeptB[onepMail.Name] = onepAcceptB;
        }

        private void KeypressValidation(object sender, TextCompositionEventArgs e)
        {
            // Handle to the textbox tjhat should be validated..
            TextBox tbox = (TextBox)sender;
            // Fetch regex..
            Regex regex = new Regex((string)previewRegex[(string)tbox.Tag]);
            // Check match and put error styles and messages..
            if (regex.IsMatch(e.Text))
            {
                if ((ValidationStates)validationState[tbox.Name] != ValidationStates.OK) tbox.Style = (Style)FindResource("textBoxNormalStyle");
                validationState[tbox.Name] = ValidationStates.OK;
                textBoxtoAceeptB[tbox.Name].IsEnabled = true;
            }
            else
            {
                if ((ValidationStates)validationState[tbox.Name] != ValidationStates.WARNING)
                {
                    tbox.Style = (Style)FindResource("textBoxInfoStyle");
                    validationState[tbox.Name] = ValidationStates.WARNING;
                    tbox.UpdateLayout(); // Very important if want to use Template.FindName when changing style dynamically!
                }
                // Fetch the errorimage in the tbox:s control template.. 
                Image errImg = (Image)tbox.Template.FindName("ErrorImage", tbox);
                // And set its tooltip to the errormessage of the textboxs validation code..
                errImg.ToolTip = (string)errorMessage[(string)tbox.Tag];
                // Use this if you dont want the user to enter something in textbox that invalidates it.
                e.Handled = true;

                textBoxtoAceeptB[tbox.Name].IsEnabled = false;
            }
        }

        private void CompletionValidation(object sender, RoutedEventArgs e)
        {
            TextBox tbox = (TextBox)sender;
            Regex regex = new Regex((string)completionRegex[(string)tbox.Tag]);
            if (regex.IsMatch(tbox.Text))
            {
                if ((ValidationStates)validationState[tbox.Name] != ValidationStates.OK) tbox.Style = (Style)FindResource("textBoxNormalStyle");
                validationState[tbox.Name] = ValidationStates.OK;
                textBoxtoAceeptB[tbox.Name].IsEnabled = true;
            }
            else
            {
                if ((ValidationStates)validationState[tbox.Name] != ValidationStates.ERROR)
                {
                    tbox.Style = (Style)FindResource("textBoxErrorStyle");
                    validationState[tbox.Name] = ValidationStates.ERROR;
                    tbox.UpdateLayout();
                }
                Image errImg = (Image)tbox.Template.FindName("ErrorImage", tbox);
                // If regex dont allow empty field.. put alternative message that "this field are required".
                errImg.ToolTip = tbox.Text.Trim().Equals(String.Empty) ? fieldRequired : (string)errorMessage[(string)tbox.Tag];
                textBoxtoAceeptB[tbox.Name].IsEnabled = false;
            }
        }

        private void InitValidation(object sender, RoutedEventArgs e)
        {
            TextBox tbox = (TextBox)sender;
            if (validationState[tbox.Name] == null)
            {
                validationState[tbox.Name] = ValidationStates.OK;
                textBoxtoAceeptB[tbox.Name].IsEnabled = true;
            }
        }
        #endregion
    }
}
