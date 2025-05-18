using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class QuizManager : MonoBehaviour
{
    public TextMeshProUGUI questionText;
    public Button[] answerButtons;
    public TextMeshProUGUI feedbackText;
    public Button nextButton;

    private List<QuizQuestion> questions = new List<QuizQuestion>();
    private QuizQuestion currentQuestion;
    private int currentQuestionIndex;

    [System.Serializable]
    public class QuizQuestion
    {
        public string question;
        public string[] answers;
        public int correctAnswerIndex;

        public QuizQuestion(string q, string[] a, int correct)
        {
            question = q;
            answers = a;
            correctAnswerIndex = correct;
        }
    }

    void Start()
    {
        InitializeQuestions();

        for (int i = 0; i < answerButtons.Length; i++)
        {
            int index = i;
            answerButtons[i].onClick.AddListener(() => CheckAnswer(index));
        }

        nextButton.onClick.AddListener(ShowNextQuestion);
        nextButton.interactable = false;

        ShowNextQuestion();
    }

    void InitializeQuestions()
    {
        questions.Add(new QuizQuestion(
            "Which animal has the longest tongue?",
            new string[] { "Chameleon", "Giraffe", "Anteater", "Frog" },
            2
        ));
        questions.Add(new QuizQuestion(
            "Which animal can sleep standing up?",
            new string[] { "Elephant", "Horse", "Penguin", "Bear" },
            1
        ));
        questions.Add(new QuizQuestion(
            "Which animal has the most powerful kick?",
            new string[] { "Kangaroo", "Zebra", "Giraffe", "Ostrich" },
            3
        ));
        questions.Add(new QuizQuestion(
            "Which animal can hold its breath longest?",
            new string[] { "Dolphin", "Sea Turtle", "Crocodile", "Whale" },
            1
        ));
        questions.Add(new QuizQuestion(
            "Which animal has the best sense of smell?",
            new string[] { "Dog", "Bear", "Shark", "Elephant" },
            2
        ));
        questions.Add(new QuizQuestion(
            "Which animal can change its gender?",
            new string[] { "Clownfish", "Penguin", "Seahorse", "Shark" },
            0
        ));
        questions.Add(new QuizQuestion(
            "Which animal has the longest hibernation?",
            new string[] { "Bear", "Squirrel", "Bat", "Hedgehog" },
            0
        ));
        questions.Add(new QuizQuestion(
            "Which animal can regenerate its heart?",
            new string[] { "Zebrafish", "Starfish", "Octopus", "Squid" },
            0
        ));
        questions.Add(new QuizQuestion(
            "Which animal can see ultraviolet light?",
            new string[] { "Eagle", "Owl", "Reindeer", "Cat" },
            2
        ));
        questions.Add(new QuizQuestion(
            "Which animal has the strongest grip strength?",
            new string[] { "Gorilla", "Lion", "Tiger", "Bear" },
            0
        ));
    }

    void ShowNextQuestion()
    {
        currentQuestionIndex = Random.Range(0, questions.Count);
        currentQuestion = questions[currentQuestionIndex];

        questionText.text = currentQuestion.question;

        for (int i = 0; i < answerButtons.Length; i++)
        {
            answerButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = currentQuestion.answers[i];
            answerButtons[i].interactable = true;
        }

        feedbackText.text = "";
        nextButton.interactable = false;
    }

    void CheckAnswer(int selectedIndex)
    {
        foreach (Button button in answerButtons)
        {
            button.interactable = false;
        }

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

        nextButton.interactable = true;
    }
}