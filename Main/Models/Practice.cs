namespace Main.Models;

class Practice : Exam
{
    public Practice() : this("", "", 0) { }

    public Practice(string subjectId, string type, int duration)
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

        Console.WriteLine("\n=============================");
        Console.WriteLine("         EXAM RESULTS        ");
        Console.WriteLine("=============================");

        int i = 1;

        foreach (var pair in AnswerKey)
        {
            Question     question    = pair.Key;
            List<Answer> correctList = pair.Value;
            List<string> givenIds    = studentAnswers[question];

            List<string> correctIds = new List<string>();
            foreach (Answer answer in correctList)
                correctIds.Add(answer.Id);

            bool isCorrect = givenIds.Count == correctIds.Count;

            if (isCorrect)
            {
                foreach (string id in givenIds)
                {
                    bool match = false;
                    foreach (string cid in correctIds)
                    {
                        if (id == cid) { match = true; break; }
                    }
                    if (!match) { isCorrect = false; break; }
                }
            }

            List<string> givenTexts = new List<string>();
            foreach (string id in givenIds)
            {
                Answer found = null;
                foreach (Answer a in question.Answers)
                {
                    if (a.Id == id) { found = a; break; }
                }
                givenTexts.Add(found != null ? found.Text : id);
            }

            List<string> correctTexts = new List<string>();
            foreach (Answer answer in correctList)
                correctTexts.Add(answer.Text);

            Console.WriteLine($"\n[{i}]- {question.Header}");
            Console.WriteLine($"Your answer   : {string.Join(", ", givenTexts)}");
            Console.WriteLine($"Correct answer: {string.Join(", ", correctTexts)}");
            Console.WriteLine(isCorrect ? "✓ Correct" : "✗ Wrong");

            i++;
        }

        return studentAnswers;
    }
}