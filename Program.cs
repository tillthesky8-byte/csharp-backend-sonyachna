
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.AddDbContext<AppDbContext>(options =>
	options.UseSqlite("Data Source=database/app.db"));

builder.Services.AddScoped<SurveySessionsRepository>();
builder.Services.AddScoped<QuestionsRepository>();
builder.Services.AddScoped<AnswerRepository>();
builder.Services.AddScoped<TododsRepository>();
builder.Services.AddScoped<DreamEntriesRepository>();

var app = builder.Build();

using var scope = app.Services.CreateScope();
var provider = scope.ServiceProvider;

var db = provider.GetRequiredService<AppDbContext>();
db.Database.EnsureCreated();

// Keep test runs deterministic.
db.Answers.RemoveRange(db.Answers);
db.DreamEntries.RemoveRange(db.DreamEntries);
db.Todos.RemoveRange(db.Todos);
db.Questions.RemoveRange(db.Questions);
db.SurveySessions.RemoveRange(db.SurveySessions);
db.SaveChanges();

var sessionsRepo = provider.GetRequiredService<SurveySessionsRepository>();
var questionsRepo = provider.GetRequiredService<QuestionsRepository>();
var answersRepo = provider.GetRequiredService<AnswerRepository>();
var todosRepo = provider.GetRequiredService<TododsRepository>();
var dreamsRepo = provider.GetRequiredService<DreamEntriesRepository>();

var now = DateTime.UtcNow;

Console.WriteLine("=== Temporary CRUD repository tests ===");

var createdSession = sessionsRepo.Add(new SurveySession
{
	Date = DateOnly.FromDateTime(DateTime.UtcNow),
	CreatedAt = now
});
Console.WriteLine($"SurveySession Add: {createdSession.Success}, id={createdSession.Data?.Id}");

var createdQuestion = questionsRepo.Add(new Question
{
	Code = "Q1",
	Text = "How energetic did you feel today?"
});
Console.WriteLine($"Question Add: {createdQuestion.Success}, id={createdQuestion.Data?.Id}");

var createdAnswer = answersRepo.Add(new Answer
{
	QuestionId = createdQuestion.Data!.Id,
	Response = "7",
	Remark = "Average focus",
	Question = createdQuestion.Data,
	SurveySession = createdSession.Data
});
Console.WriteLine($"Answer Add: {createdAnswer.Success}, id={createdAnswer.Data?.Id}");

var createdTodo = todosRepo.Add(new Todo
{
	Description = "Reality checks every 2 hours",
	Status = TodoStatus.NotStarted,
	Scope = TodoScope.Daily,
	CreatedAt = now,
	UpdatedAt = now,
	DueAt = now.AddDays(1)
});
Console.WriteLine($"Todo Add: {createdTodo.Success}, id={createdTodo.Data?.Id}");

var createdDream = dreamsRepo.Add(new DreamEntry
{
	SurveySessionId = createdSession.Data!.Id,
	Content = "Short fragment about flying over a city.",
	IsLucid = false
});
Console.WriteLine($"DreamEntry Add: {createdDream.Success}, id={createdDream.Data?.Id}");

var loadedSession = sessionsRepo.GetById(createdSession.Data!.Id);
Console.WriteLine($"SurveySession GetById: {loadedSession.Success}");

var loadedQuestion = questionsRepo.GetById(createdQuestion.Data!.Id);
Console.WriteLine($"Question GetById: {loadedQuestion.Success}");

var loadedAnswer = answersRepo.GetById(createdAnswer.Data!.Id);
Console.WriteLine($"Answer GetById: {loadedAnswer.Success}");

var loadedTodo = todosRepo.GetById(createdTodo.Data!.Id);
Console.WriteLine($"Todo GetById: {loadedTodo.Success}");

var loadedDream = dreamsRepo.GetById(createdDream.Data!.Id);
Console.WriteLine($"DreamEntry GetById: {loadedDream.Success}");

createdTodo.Data.Description = "Reality checks every 90 minutes";
createdTodo.Data.Status = TodoStatus.InProgress;
createdTodo.Data.UpdatedAt = DateTime.UtcNow;
var updatedTodo = todosRepo.Update(createdTodo.Data);
Console.WriteLine($"Todo Update: {updatedTodo.Success}");

createdDream.Data.Content = "Updated: I noticed dream signs and stabilized awareness.";
createdDream.Data.IsLucid = true;
var updatedDream = dreamsRepo.Update(createdDream.Data);
Console.WriteLine($"DreamEntry Update: {updatedDream.Success}");

var allSessions = sessionsRepo.GetAll();
var allQuestions = questionsRepo.GetAll();
var allAnswers = answersRepo.GetAll();
var allTodos = todosRepo.GetAll();
var allDreams = dreamsRepo.GetAll();

Console.WriteLine($"SurveySessions count: {allSessions.Data?.Count}");
Console.WriteLine($"Questions count: {allQuestions.Data?.Count}");
Console.WriteLine($"Answers count: {allAnswers.Data?.Count}");
Console.WriteLine($"Todos count: {allTodos.Data?.Count}");
Console.WriteLine($"DreamEntries count: {allDreams.Data?.Count}");

var deleteAnswer = answersRepo.Delete(createdAnswer.Data!.Id);
var deleteDream = dreamsRepo.Delete(createdDream.Data!.Id);
var deleteTodo = todosRepo.Delete(createdTodo.Data!.Id);
var deleteQuestion = questionsRepo.Delete(createdQuestion.Data!.Id);
var deleteSession = sessionsRepo.Delete(createdSession.Data!.Id);

Console.WriteLine($"Answer Delete: {deleteAnswer.Success}");
Console.WriteLine($"DreamEntry Delete: {deleteDream.Success}");
Console.WriteLine($"Todo Delete: {deleteTodo.Success}");
Console.WriteLine($"Question Delete: {deleteQuestion.Success}");
Console.WriteLine($"SurveySession Delete: {deleteSession.Success}");

Console.WriteLine("=== CRUD test run finished ===");
