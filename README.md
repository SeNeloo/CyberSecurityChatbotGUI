Cybersecurity Awareness Bot POE 3

# Project Description
The Cybersecurity Awareness Bot is a Windows Presentation Foundation (WPF) desktop application developed in C#. The purpose of the application is to educate users about cybersecurity concepts through an interactive chatbot. The application includes a chatbot, sentiment detection, memory features, task management, quiz functionality, activity logging, and MySQL database integration.

# Features
The chatbot allows users to ask cybersecurity-related questions and receive educational responses. The chatbot recognises keywords and provides information about various cybersecurity topics.

# User Memory
The chatbot remembers:
The user's name
The user's favourite cybersecurity topic
Example:
What is my name?
What is my favourite topic?

# Sentiment Detection
The chatbot detects emotions expressed by users and responds appropriately.
Supported sentiments:
-Happy
-Sad
-Angry
-Scared
-Confused

# Cybersecurity Topics
The chatbot can provide information on:
-Phishing
-Password Safety
-Malware
-VPNs
-Social Engineering
-Privacy and Online Safety

# Task Management
Users can:
-Add tasks
-Delete tasks
-View tasks
-Manage cybersecurity learning activities

# MySQL Database Integration
The application uses MySQL to store task information. Task records are saved in the CyberSecurityDB database for persistence.

# Activity Log
The application records activities such as:
-Task creation
-Task deletion
-Task completion

# Cybersecurity Quiz
The quiz feature allows users to:
-View cybersecurity questions
-Submit answers
-Test their cybersecurity knowledge

# Technologies Used
-C#
-WPF (Windows Presentation Foundation)
-.NET
-MySQL
-MySQL Workbench
-Visual Studio 2026
-GitHub
 
# Database Information
Database Name : CyberSecurityDB
Table Name : Tasks
Example SQL Commands
-CREATE DATABASE CyberSecurityDB;
-USE CyberSecurityDB;
-CREATE TABLE Tasks
(
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Title VARCHAR(255),
    Description VARCHAR(255),
    ReminderDate DATETIME
);

# Project Structure
Main Files
-MainWindow.xaml
-MainWindow.xaml.cs
-TaskItem.cs
-QuizQuestion.cs
-README.md

# Installation Instructions
Prerequisites
Before running the project, install:
-Visual Studio 2026
-MySQL Server
-MySQL Workbench
-.NET SDK

# Required NuGet Package
Install the following package:
-MySql.Data

# Running the Application
1. Clone the repository from GitHub.
2. Open the solution in Visual Studio.
3. Restore NuGet packages.
4. Ensure MySQL Server is running.
5. Create the CyberSecurityDB database.
6. Build and run the application.

# GitHub Version Control
The following Git tags were created during development:
-Part2Complete
-v1.0
-v2.0
-finalsubmission
These tags represent major development milestones and project versions.

# Future Improvements
-Potential future enhancements include:
-More cybersecurity topics
-Additional quiz questions
-Improved Natural Language Processing (NLP)
-User authentication and login system
-Enhanced user interface design
-More advanced reporting and analytics

# References
Cisco Networking Academy. 2024. Introduction to Cybersecurity. Available at: https://www.netacad.com

Microsoft. 2026. C# Documentation. Available at: https://learn.microsoft.com/dotnet/csharp/

Microsoft. 2026. Windows Presentation Foundation (WPF) Documentation. Available at: https://learn.microsoft.com/dotnet/desktop/wpf/

Oracle Corporation. 2026. MySQL Documentation. Available at: https://dev.mysql.com/doc/

Oracle Corporation. 2026. MySQL Workbench Documentation. Available at: https://dev.mysql.com/doc/workbench/en/

GitHub. 2026. GitHub Documentation. Available at: https://docs.github.com/
