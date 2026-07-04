namespace Main;

using Main.Models;
using Main.DataAccess;

class EventMapper
{
    public static void RegisterToExamEvents(Student student, SubjectDictionary subjects)
    {
        foreach (string subjectId in student.SubjectIds)
        {
            if (!subjects.ContainsKey(subjectId)) continue;

            Subject subject      = subjects[subjectId];
            ExamDictionary exams = new ExamDictionary(subject);

            foreach (var pair in exams)
            {
                Exam exam = pair.Value;

                exam.ExamStarted  += student.OnExamStarted;
                exam.ExamQueued   += student.OnExamQueued;
                exam.ExamFinished += student.OnExamFinished;

                exam.TriggerExamNotification();
            }
        }
    }
}