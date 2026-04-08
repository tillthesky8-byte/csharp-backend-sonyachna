using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 
builder.Services.AddControllers();
//db context registration
var connectionString = builder.Configuration.GetConnectionString("AppDb")
	?? "Data Source=database/app.db";
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlite(connectionString));


//services registration
builder.Services.AddScoped<DreamService>();
builder.Services.AddScoped<TodoService>();
builder.Services.AddScoped<SurveyService>();

//repositories registration
builder.Services.AddScoped<IRepository<SurveySession>, SurveySessionsRepository>();
builder.Services.AddScoped<IRepository<Question>, QuestionsRepository>();
builder.Services.AddScoped<IRepository<Answer>, AnswerRepository>();
builder.Services.AddScoped<IRepository<DreamEntry>, DreamEntriesRepository>();
builder.Services.AddScoped<IRepository<Todo>, TododsRepository>(); // typo in repository name is intentional to match the class name. Future reminder to fix

var app = builder.Build();
app.MapControllers();

app.Run();
