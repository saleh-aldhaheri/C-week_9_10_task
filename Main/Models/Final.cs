namespace Main.Models;

class Final : Exam
{
    public Final() : this("", "", 0) { }

    public Final(string subjectId, string type, int duration)
        : base(subjectId, type, duration) { }

    public override Dictionary<Question, List<string>> Display()
    {
        Dictionary<Question, List<string>> studentAnswers = new Dictionary<Question, List<string>>();

        int order = 1;

        foreach (var pair in AnswerKey)
        {
            Question question = pair.Key;
            question.Display(order);

            studentAnswers[question] = question.AskAnswer();
            order++;
        }

        return studentAnswers;
    }
}