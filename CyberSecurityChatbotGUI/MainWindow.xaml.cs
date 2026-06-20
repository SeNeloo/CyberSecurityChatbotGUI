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
using MySql.Data.MySqlClient;
using System.Data;



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

        private List<TaskItem> tasks = new List<TaskItem>();
        private string connectionString =
                  "server=localhost;database=CyberSecurityDB;uid=root;pwd=@Labs2026!;";

        private List<QuizQuestion> quizQuestions =
            new List<QuizQuestion>();

        private int currentQuestion = 0;
        private int score = 0;

        //convo stages
        bool nameCaptured = false;
        bool topicCaptured = false;

        public MainWindow()

        {
            InitializeComponent();

            LoadTasks();
            CheckReminders();
            LoadQuizQuestions();
            DisplayQuestion();
        


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
                    AddMessage("Bot: I'm sorry you're feeling sad :( , Stay safe online and take everthing step by step.");
                    return;
                }

                if (input.Contains("happy"))
                {
                    AddMessage("Bot: I'm glad you're feeling happy:)");
                    return;
                }

                if (input.Contains("confused"))
                {
                    AddMessage("Bot: Dont stress. Let me explain it differently.");
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



        //add task button
        private void btnAddTask_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                TaskItem task = new TaskItem()
                {
                    Title = txtTaskTitle.Text,
                    Description = txtTaskDescription.Text,
                    ReminderDate = dpReminder.SelectedDate ?? DateTime.Now,
                    Status = "Pending"
                };

                tasks.Add(task);
                SaveTaskToDatabase(task);

                dgTasks.ItemsSource = null;
                dgTasks.ItemsSource = tasks;

                lstActivityLog.Items.Add(
                    $"Task Added: {task.Title} ({DateTime.Now})");

                MessageBox.Show("Task added successfully!");

                txtTaskTitle.Clear();
                txtTaskDescription.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        // delete task button
        private void btnDeleteTask_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgTasks.SelectedItem is TaskItem selectedTask)
                {
                    tasks.Remove(selectedTask);

                    dgTasks.ItemsSource = null;
                    dgTasks.ItemsSource = tasks;

                    lstActivityLog.Items.Add(
                        $"Task Deleted: {selectedTask.Title}");

                    MessageBox.Show("Task deleted.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //save task to database 
        private void SaveTaskToDatabase(TaskItem task)
        {
            try
            {
                using (MySqlConnection conn =
                    new MySqlConnection(connectionString))
                {
                    conn.Open();

                    string query =
                    @"INSERT INTO Tasks
              (Title, Description, ReminderDate, Status)
              VALUES
              (@Title, @Description, @ReminderDate, @Status)";

                    MySqlCommand cmd =
                        new MySqlCommand(query, conn);

                    cmd.Parameters.AddWithValue("@Title", task.Title);
                    cmd.Parameters.AddWithValue("@Description", task.Description);
                    cmd.Parameters.AddWithValue("@ReminderDate", task.ReminderDate);
                    cmd.Parameters.AddWithValue("@Status", task.Status);

                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //laod task
        private void LoadTasks()
        {
            try
            {
                tasks.Clear();

                using (MySqlConnection conn =
                    new MySqlConnection(connectionString))
                {
                    conn.Open();

                    string query = "SELECT * FROM Tasks";

                    MySqlCommand cmd =
                        new MySqlCommand(query, conn);

                    MySqlDataReader reader =
                        cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        TaskItem task = new TaskItem()
                        {
                            TaskID = Convert.ToInt32(reader["TaskID"]),
                            Title = reader["Title"].ToString(),
                            Description = reader["Description"].ToString(),
                            ReminderDate =
                                Convert.ToDateTime(reader["ReminderDate"]),
                            Status = reader["Status"].ToString()
                        };

                        tasks.Add(task);
                    }
                }

                dgTasks.ItemsSource = null;
                dgTasks.ItemsSource = tasks;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //reminder checker 
        private void CheckReminders()
        {
            try
            {
                foreach (TaskItem task in tasks)
                {
                    if (task.Status == "Pending" &&
                        task.ReminderDate.Date <= DateTime.Today)
                    {
                        MessageBox.Show(
                            $"Reminder: {task.Title}",
                            "Task Reminder");

                        lstActivityLog.Items.Add(
                            $"Reminder Triggered: {task.Title}");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        //complete buttom
        private void btnCompleteTask_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgTasks.SelectedItem is TaskItem selectedTask)
                {
                    selectedTask.Status = "Completed";

                    dgTasks.ItemsSource = null;
                    dgTasks.ItemsSource = tasks;

                    lstActivityLog.Items.Add(
                        $"Task Completed: {selectedTask.Title}");

                    MessageBox.Show("Task marked complete.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void LoadQuizQuestions()
        {
            quizQuestions.Add(new QuizQuestion()
            {
                Question = "What is phishing?",
                CorrectAnswer = "A scam to steal information"
            });

            quizQuestions.Add(new QuizQuestion()
            {
                Question = "Should you share passwords?",
                CorrectAnswer = "No"
            });

            quizQuestions.Add(new QuizQuestion()
            {
                Question = "What does VPN stand for?",
                CorrectAnswer = "Virtual Private Network"
            });
        }
 
    private void DisplayQuestion()
        {
            if (currentQuestion < quizQuestions.Count)
            {
                lblQuestion.Content =
                    quizQuestions[currentQuestion].Question;
            }
        }
        //answer button 
        private void btnSubmitAnswer_Click(
            object sender,
            RoutedEventArgs e)
        {
            string answer = txtQuizAnswer.Text.Trim();

            if (answer.Equals(
                quizQuestions[currentQuestion].CorrectAnswer,
                StringComparison.OrdinalIgnoreCase))
            {
                score++;

                MessageBox.Show("Correct!");
            }
            else
            {
                MessageBox.Show(
                    "Incorrect. Correct answer: " +
                    quizQuestions[currentQuestion].CorrectAnswer);
            }

            currentQuestion++;

            txtQuizAnswer.Clear();

            if (currentQuestion < quizQuestions.Count)
            {
                DisplayQuestion();
            }
            else
            {
                lblQuestion.Content = "Quiz Complete!";

                lblScore.Content =
                    $"Final Score: {score}/{quizQuestions.Count}";

                lstActivityLog.Items.Add(
                    $"Quiz Completed: {score}/{quizQuestions.Count}");

                if (score >= 8)
                {
                    MessageBox.Show(
                        "Excellent cybersecurity knowledge!");
                }
                else if (score >= 5)
                {
                    MessageBox.Show(
                        "Good effort! Keep learning.");
                }
                else
                {
                    MessageBox.Show(
                        "You should review cybersecurity basics.");
                }
            }
        }
    }
}




        

