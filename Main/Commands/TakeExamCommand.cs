namespace Main.Commands;

using Main.DataAccess;
using Main.Models;
using Main.Utilities;

class TakeExamCommand
{
    public static void Execute(Student student, SubjectDictionary subjects)
    {
        Exam selectedExam       = null;
        Subject selectedSubject = null;

        try
        {
            (selectedExam, selectedSubject) = SelectSubject.SelectExam(student, subjects);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return;
        }

        Dictionary<Question, List<string>> studentAnswers = selectedExam.Display();

        int rawScore   = 0;
        int totalMarks = 0;

        List<StudentAnswer> answerList = new List<StudentAnswer>();

        foreach (var pair in selectedExam.AnswerKey)
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

            if (isCorrect)
                rawScore += question.Marks;

            totalMarks += question.Marks;

            answerList.Add(new StudentAnswer
            {
                QuestionId     = question.Id,
                GivenAnswerIds = givenIds
            });
        }

        int scoreOutOf100 = totalMarks == 0 ? 0 : (int)Math.Round((double)rawScore / totalMarks * 100);

        Console.WriteLine("\n=============================");
        Console.WriteLine($"Your Score: {scoreOutOf100} / 100");
        Console.WriteLine("=============================");

        ResultDictionary results = new ResultDictionary(selectedSubject, selectedExam);

        ExamResult result = new ExamResult
        {
            Id        = results.GenerateId(),
            ExamId    = selectedExam.Id,
            StudentId = student.Id,
            Score     = scoreOutOf100,
            TotalMarks = 100,
            TakenAt   = DateTime.Now,
            Answers   = answerList
        };

        results.Add(result.Id, result);
    }
}