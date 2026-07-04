# Exam System

A console-based exam management system written in C# (.NET 10). Teachers create
questions and schedule exams; students take those exams, get scored automatically,
and review their results. Data is persisted to plain-text files.

## Features

- **Role-based access** — register/login as a **Teacher** or **Student** (passwords hashed with BCrypt).
- **Question bank** — teachers author three question types:
  - `TrueOrFalse`
  - `ChooseOne` (single correct answer)
  - `ChooseAll` (multiple correct answers)
- **Exams** — two kinds:
  - `Practice` — shows per-question feedback (your answer vs. correct answer) after submission.
  - `Final` — no feedback, score only.
- **Auto-generated exams** — pick a difficulty level and a count; questions are drawn
  at random, with the option to fill in from other levels when a level runs short.
- **Scheduling & state** — each exam is `Queued`, `Starting`, or `Finished` based on
  its start time and duration. Students are notified of exam state on login.
- **Scoring & results** — answers are graded automatically and results are saved and
  viewable per student.

## Requirements

- [.NET SDK 10.0](https://dotnet.microsoft.com/download) or later.
- NuGet dependency `BCrypt.Net-Next` (restored automatically on build — no manual install).

## Getting started

```bash
# from the repository root
dotnet run --project Main
```

Then follow the on-screen menus:

1. Choose a role (Teacher / Student) or exit.
2. Register a new account or log in.
3. Use the role menu to create/take exams.

A typical first run:
- Register a **teacher** → create a few questions for a subject → create an exam.
- Register a **student** who studies that subject → take the exam → view results.

## Project structure

```
Main/
├── Program.cs            # entry point
├── AppBoot.cs            # top-level role menu loop
├── Commands/             # one class per user action (Register, Login, CreateExam, TakeExam, ...)
├── Services/             # AuthService, TeacherService, StudentService (menu flows)
├── Models/               # domain types (User, Teacher, Student, Exam, Question, ...)
├── DataAccess/           # text-file persistence (dictionaries & lists)
├── Utilities/            # SelectSubject, GenerateQuestion helpers
├── Enums/                # ExamMode, QuestionLevel
└── data/                 # persisted data (see below)
```

## Data storage

The app reads and writes plain-text files under `Main/data/`:

| File | Contents |
|------|----------|
| `subjects.txt` | Seed list of subjects (**committed** to the repo) |
| `users.txt` | Registered users (generated at runtime) |
| `<Subject>_questions.txt` | Question bank per subject (generated) |
| `<Subject>_exams.txt` | Exams per subject (generated) |
| `<Subject>_exam_<id>_results.txt` | Student results per exam (generated) |

Only `subjects.txt` is version-controlled; the rest are runtime state and are
git-ignored. Delete any of the generated files to reset that part of the app
(keep `subjects.txt`).

> **⚠️ Setup note:** the data directory path is currently **hardcoded** as an
> absolute path in `DataAccess/BaseDictionary.cs` and `DataAccess/BaseList.cs`
> (`BasePath`). To run this project on a different machine or location, update
> those two `BasePath` values to point at your `Main/data/` folder.

