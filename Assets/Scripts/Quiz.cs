using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class QuizManager : MonoBehaviour
{
    public TextMeshProUGUI questionText;
    public Button[] answerButtons; // Array for the 4 answer buttons
    public TextMeshProUGUI feedbackText;
    public Button nextButton;

    private List<QuizQuestion> questions = new List<QuizQuestion>();
    private QuizQuestion currentQuestion;
    private int currentQuestionIndex;

    [System.Serializable]
    public class QuizQuestion
    {
        public string question;
        public string[] answers; // 4 answers
        public int correctAnswerIndex; // Index of the correct answer (0-3)

        public QuizQuestion(string q, string[] a, int correct)
        {
            question = q;
            answers = a;
            correctAnswerIndex = correct;
        }
    }

    void Start()
    {
        // Populate the list of animal-related quiz questions
        InitializeQuestions();

        // Set up button listeners
        for (int i = 0; i < answerButtons.Length; i++)
        {
            int index = i; // Capture the index for the lambda
            answerButtons[i].onClick.AddListener(() => CheckAnswer(index));
        }

        // Set up the next button
        nextButton.onClick.AddListener(ShowNextQuestion);
        nextButton.interactable = false;

        // Show the first question
        ShowNextQuestion();
    }

    void InitializeQuestions()
    {
        questions.Add(new QuizQuestion(
            "Which animal has the longest tongue?",
            new string[] { "Chameleon", "Giraffe", "Anteater", "Frog" },
            2 // Anteater
        ));
        questions.Add(new QuizQuestion(
            "Which animal can sleep standing up?",
            new string[] { "Elephant", "Horse", "Penguin", "Bear" },
            1 // Horse
        ));
        questions.Add(new QuizQuestion(
            "Which animal has the most powerful kick?",
            new string[] { "Kangaroo", "Zebra", "Giraffe", "Ostrich" },
            3 // Ostrich
        ));
        questions.Add(new QuizQuestion(
            "Which animal can hold its breath longest?",
            new string[] { "Dolphin", "Sea Turtle", "Crocodile", "Whale" },
            1 // Sea Turtle
        ));
        questions.Add(new QuizQuestion(
            "Which animal has the best sense of smell?",
            new string[] { "Dog", "Bear", "Shark", "Elephant" },
            2 // Shark
        ));
        questions.Add(new QuizQuestion(
            "Which animal can change its gender?",
            new string[] { "Clownfish", "Penguin", "Seahorse", "Shark" },
            0 // Clownfish
        ));
        questions.Add(new QuizQuestion(
            "Which animal has the longest hibernation?",
            new string[] { "Bear", "Squirrel", "Bat", "Hedgehog" },
            0 // Bear
        ));
        questions.Add(new QuizQuestion(
            "Which animal can regenerate its heart?",
            new string[] { "Zebrafish", "Starfish", "Octopus", "Squid" },
            0 // Zebrafish
        ));
        questions.Add(new QuizQuestion(
            "Which animal can see ultraviolet light?",
            new string[] { "Eagle", "Owl", "Reindeer", "Cat" },
            2 // Reindeer
        ));
        questions.Add(new QuizQuestion(
            "Which animal has the strongest grip strength?",
            new string[] { "Gorilla", "Lion", "Tiger", "Bear" },
            0 // Gorilla
        ));
    }

    void ShowNextQuestion()
    {
        // Pick a random question
        currentQuestionIndex = Random.Range(0, questions.Count);
        currentQuestion = questions[currentQuestionIndex];

        // Display the question
        questionText.text = currentQuestion.question;

        // Display the answer options
        for (int i = 0; i < answerButtons.Length; i++)
        {
            answerButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = currentQuestion.answers[i];
            answerButtons[i].interactable = true;
        }

        // Clear feedback and disable the next button
        feedbackText.text = "";
        nextButton.interactable = false;
    }

    void CheckAnswer(int selectedIndex)
    {
        // Disable answer buttons to prevent multiple clicks
        foreach (Button button in answerButtons)
        {
            button.interactable = false;
        }

        // Check if the answer is correct
        if (selectedIndex == currentQuestion.correctAnswerIndex)
        {
            feedbackText.text = "Correct!";
            feedbackText.color = Color.green;
        }
        else
        {
            feedbackText.text = "Incorrect! The correct answer was: " + currentQuestion.answers[currentQuestion.correctAnswerIndex];
            feedbackText.color = Color.red;
        }

        // Enable the next button
        nextButton.interactable = true;
    }
}