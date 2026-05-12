using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System;
using System.Collections.Generic;


namespace CyberSecurityChatbotGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Dictionary<string, List<string>> responses = new Dictionary<string, List<string>>();
        Random random = new Random();
        string userName = "";
        string favouriteTopic = "";
        string lastBotResponse = "";

        //convo stages
        bool nameCaptured = false;
        bool topicCaptured = false;

        public MainWindow()

        {
            InitializeComponent();

            responses.Add("password", new List<string>()
    {
        "Use strong password with symbols and numbers.",
        "Avoid using personal information in passwords.",
        "Change your passwords regularly."
    });

            responses.Add("phishing", new List<string>()
    {
        "DONT click suspicious links.",
        "Always verify email senders.",
        "Phishing scams try to steal your information."
    });

            responses.Add("privacy", new List<string>()
    {
        "Review your privacy settings online.",
        "Avoid sharing sensitive information publicly.",
        "Use two-step verification for better security."
    });

            AddMessage("Bot: Hello. Welcome to the Cybersecurity Awareness Bot.");
            AddMessage("Bot: What is your name?");
            AddMessage("Bot: What is your favourite cybersecurity topic?:)");
        }
        private void AddMessage(string message)
        {
            lastBotResponse = message;

            Paragraph paragraph = new Paragraph(new Run(message));
            paragraph.Margin = new Thickness(0, 5, 0, 5);

            rtbChat.Document.Blocks.Add(new Paragraph(new Run(message)));
            rtbChat.ScrollToEnd();

        }
        private void btnSend_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string input = txtUserInput.Text.ToLower();

                if (string.IsNullOrWhiteSpace(input))
                {
                    MessageBox.Show("Please enter a message.");
                    return;
                }

                AddMessage(" You: " + txtUserInput.Text);

                if (!nameCaptured)
                {
                    userName = txtUserInput.Text;

                    nameCaptured = true;

                    AddMessage("Bot: Nice to meet you, " + userName + "!");
                    AddMessage("Bot: What is your favourite cybersecurity topic?");

                    txtUserInput.Clear();
                    return;
                }

                if (!topicCaptured)
                {
                    favouriteTopic = txtUserInput.Text;

                    topicCaptured = true;

                    AddMessage("Bot: Great choice! I will remember that you like " + favouriteTopic);

                    txtUserInput.Clear();
                    return;

                }
                else
                {
                    if (input == "what is my name")
                    {
                        AddMessage("Bot: Your name is " + userName);
                        return;
                    }

                    if (input == "what is my favourite topic")
                    {
                        AddMessage("Bot: Your favourite cubesecurity topic is" + favouriteTopic);
                        return;
                    }

                    RespondToUser(input);
                }
                if (input.Contains("confused") || input.Contains("don't know"))
                {
                    AddMessage("Bot: Let me repeat that for you");
                    AddMessage(lastBotResponse);
                    return;
                }

                if (input.Contains("angry"))
                {
                    AddMessage("Bot: I understand you're frustrated. Let me try help.");
                    return;
                }

                if (input.Contains("sad"))
                {
                    AddMessage("Bot: I'm sorry you're feeling sad :( , Stay safe online and take things step by step.");
                    return;
                }

                if (input.Contains("happy"))
                {
                    AddMessage("Bot: I'm glad you're feeling happy:)");
                    return;
                }

                if (input.Contains("confused"))
                {
                    AddMessage("Bot: No worries. Let me explain it differently.");
                    return;
                }

                txtUserInput.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
        private void RespondToUser(string input)
        {
            bool found = false;

            foreach (var keyword in responses.Keys)
            {
                if (input.Contains(keyword))
                {
                    List<string> possibleResponses = responses[keyword];

                    int index = random.Next(possibleResponses.Count);

                    AddMessage("Bot: " + possibleResponses[index]);

                    found = true;
                    break;
                }
            }

            // Sentiment Detection
            if (input.Contains("worried & stressed.") || input.Contains("scared"))
            {
                AddMessage("Bot: It's okay to feel concerned. Cybersecurity awareness helps keep you safe online.");
                found = true;
            }

            if (found)
            {
                AddMessage("Bot: I'm not sure I understand. Can you rephrase?");
            }
        }

        private void rtbChat_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
        

