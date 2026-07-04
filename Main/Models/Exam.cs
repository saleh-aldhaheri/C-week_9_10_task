namespace Main.Models;

using Main.Enums;
using Main.DataAccess;

abstract class Exam : Model, IComparable<Exam>, ICloneable
{
    public Exam() : this("", "", 0) { }

    public Exam(string subjectId, string type, int duration)
    {
        SubjectId       = subjectId;
        Type            = type;
        DurationMinutes = duration;
    }

    public string? SubjectId { get; set; }
    public string? Type { get; set; }
    public int DurationMinutes { get; set; }
    public int NumberOfQuestions { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public List<string> QuestionIds { get; set; } = new List<string>();
    public Dictionary<Question, List<Answer>> AnswerKey { get; set; } = new Dictionary<Question, List<Answer>>();

    public ExamMode Mode
    {
        get
        {
            if (DateTime.Now < StartTime)
                return ExamMode.Queued;

            if (DateTime.Now >= StartTime && DateTime.Now <= EndTime)
                return ExamMode.Starting;

            return ExamMode.Finished;
        }
    }

    public void TriggerExamNotification()
    {
        if (DateTime.Now < StartTime)
            ExamQueued?.Invoke(this);
        else if (DateTime.Now >= StartTime && DateTime.Now <= EndTime)
            ExamStarted?.Invoke(this);
        else if (DateTime.Now > EndTime)
            ExamFinished?.Invoke(this);
    }

    public event Action<Exam>? ExamStarted;
    public event Action<Exam>? ExamFinished;
    public event Action<Exam>? ExamQueued;

    /// <summary>
    ///  Create Dictionary For Question And Answers
    /// </summary>
    public void LoadAnswerKey(QuestionList questions)
    {
        foreach (string questionId in QuestionIds)
        {
            Question question = null;

            foreach (Question q in questions)
            {
                if (q.Id == questionId)
                {
                    question = q;
                    break;
                }
            }

            if (question == null) continue;

            if (question.Answers == null || question.Answers.Count == 0) continue;

            List<Answer> correctAnswers = new List<Answer>();

            foreach (int index in question.IndexOfCorrectAnswer)
            {
                if (index >= 0 && index < question.Answers.Count)
                    correctAnswers.Add(question.Answers[index]);
            }

            AnswerKey[question] = correctAnswers;
        }
    }
    
    public abstract Dictionary<Question, List<string>> Display();
  
    public int CompareTo(Exam? other)
    {
        if (other == null) return 1;
        return this.StartTime.CompareTo(other.StartTime);
    }

    public override string ToString()
    {
        return $"Exam | Subject: {SubjectId} | Duration: {DurationMinutes} mins | Mode: {Mode} | Starts: {StartTime}";
    }
}