using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.AddDbContext<AppDbContext>(options =>
	options.UseSqlite("Data Source=database/app.db"));

builder.Services.AddScoped<IRepository<SurveySession>, SurveySessionsRepository>();
builder.Services.AddScoped<IRepository<Question>, QuestionsRepository>();
builder.Services.AddScoped<IRepository<DreamEntry>, DreamEntriesRepository>();
builder.Services.AddScoped<IRepository<Todo>, TododsRepository>();

builder.Services.AddScoped<SurveyService>();
builder.Services.AddScoped<TodoService>();
builder.Services.AddScoped<DreamService>();

var app = builder.Build();

using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;

var db = services.GetRequiredService<AppDbContext>();
db.Database.EnsureCreated();

db.Answers.RemoveRange(db.Answers);
db.DreamEntries.RemoveRange(db.DreamEntries);
db.Todos.RemoveRange(db.Todos);
db.Questions.RemoveRange(db.Questions);
db.SurveySessions.RemoveRange(db.SurveySessions);
db.SaveChanges();

var surveyService = services.GetRequiredService<SurveyService>();
var todoService = services.GetRequiredService<TodoService>();
var dreamService = services.GetRequiredService<DreamService>();

var surveyRepo = services.GetRequiredService<IRepository<SurveySession>>();
var questionRepo = services.GetRequiredService<IRepository<Question>>();
var dreamRepo = services.GetRequiredService<IRepository<DreamEntry>>();
var todoRepo = services.GetRequiredService<IRepository<Todo>>();

var today = DateOnly.FromDateTime(DateTime.UtcNow);
var now = (int)DateTimeOffset.UtcNow.ToUnixTimeSeconds();

Console.WriteLine("=== Temporary service smoke test ===");

var questionResult = questionRepo.Add(new Question
{
	Code = "Q1",
	Text = "How focused did you feel today?"
});
Console.WriteLine($"Question add: {questionResult.Success}");

var surveySubmitResult = surveyService.SubmitSurvey(new List<Answer>
{
	new Answer
	{
		QuestionId = questionResult.Data!.Id,
		Response = "8",
		Remark = "Good concentration"
	}
});
Console.WriteLine($"Survey submit: {surveySubmitResult}");

var surveyExists = surveyService.SurveyExistsForDate(today);
Console.WriteLine($"Survey exists for {today}: {surveyExists}");

var surveySessionResult = surveyRepo.GetAll();
var session = surveySessionResult.Data!.First();

var dreamAddResult = dreamRepo.Add(new DreamEntry
{
	SurveySessionId = session.Id,
	SurveySession = session,
	Content = "Dreamed of walking through a familiar house and becoming lucid.",
	IsLucid = true
});
Console.WriteLine($"Dream add: {dreamAddResult.Success}");

var dreamEntries = dreamService.RetriveDreamEntriesByDate(today);
Console.WriteLine($"Dream entries for {today}: {dreamEntries.Count}");

var todoAddResult = todoRepo.Add(new Todo
{
	Description = "Do reality checks",
	Status = TodoStatus.NotStarted,
	Scope = TodoScope.Daily,
	CreatedAt = DateTime.UtcNow,
	UpdatedAt = DateTime.UtcNow,
	CompletedAt = null,
	DueAt = DateTime.UtcNow.AddHours(1)
});
Console.WriteLine($"Todo add: {todoAddResult.Success}");

var todoId = todoAddResult.Data!.Id;
var markCompleted = todoService.MarkTodoCompleted(todoId);
Console.WriteLine($"Todo mark completed: {markCompleted}");

var markFailed = todoService.MarkTodoFailed(todoId);
Console.WriteLine($"Todo mark failed: {markFailed}");

Console.WriteLine("=== Smoke test finished ===");
