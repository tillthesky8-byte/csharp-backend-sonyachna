using Microsoft.AspNetCore.Mvc;

// the controller works and ready for use
// remember to remove the temporary endpoint for creating questions in the database after testing, as it can cause issues if accidentally called multiple times and creates duplicate questions.
[ApiController]
[Route("api/[controller]")]
public class SurveyController : ControllerBase
{
    private readonly ILogger<SurveyController> logger;
    private readonly SurveyService service;
    private readonly IRepository<Question> repo;

    public SurveyController(SurveyService service, IRepository<Question> repo, ILogger<SurveyController> logger)
    {
        this.service = service;
        this.logger = logger;
        this.repo = repo;
    }


    // add check whether such questioncode exists in the database when submitting a survey, if not return an error response. This is to prevent foreign key errors when adding answers to the database.
    [HttpPost("submit")]
     // add check for today's date, if the survey session for today's date already exists, return an error response. This is to prevent multiple survey sessions for the same date and the foreign key errors that occur when adding answers to the database.
    public IActionResult SubmitSurvey(DTOs.CreateSurveyRequest request)
    {
      
        var answers = request.Answers;
        var serviceResponse = service.SubmitSurvey(answers);
        if (serviceResponse)
        {
            logger.LogInformation("Survey submitted successfully");
            return Ok(new DTOs.CreateSurveyResponse { Success = serviceResponse, Message = "Survey submitted successfully" });
        }
        else
        {
            logger.LogError("Failed to submit survey");
            return BadRequest(new DTOs.CreateSurveyResponse { Success = false, Message = "Failed to submit survey" });
        }
    
    }

    [HttpPost("check-existence")]
    public IActionResult CheckSurveyExistence(DTOs.CheckSurveyExistenceRequest request)
    {
        var exists = service.SurveyExistsForDate(request.Date);
        logger.LogInformation($"Survey existence check for date {request.Date}: {exists}");
        return Ok(new DTOs.CheckSurveyExistenceResponse { Exists = exists });
    }

    //temporary enpoint to create question entities in the database, will be removed later.

    // [HttpPost("create-questions")]
    // public IActionResult CreateQuestions()
    // {
    //     var questions = new List<Question>
    //     {
    //         new Question { Text = "How are you feeling today?", Code = "FEELING" },
    //         new Question { Text = "Did you sleep well last night?", Code = "SLEEP" },
    //         new Question { Text = "What is your energy level right now?", Code = "ENERGY" }
    //     };

    //     logger.LogInformation("question objects are created");

    //     foreach (var question in questions)
    //     {
    //         var response = repo.Add(question);
    //         if (!response.Success)
    //         {
    //             logger.LogError($"Failed to create question: {response.Message}");
    //             return BadRequest($"Failed to create question: {response.Message}");
    //         }
    //     }

    //     logger.LogInformation("Questions created successfully");
    //     return Ok("Questions created successfully");
    // }

}

//the curl requests to test all the endpoints of the survey controller for port 5155.
//change the body of the second request according to the model, which as List<Answer> Answers and DateTime Timestamp. The answers should have questionId and answerText fields. The date should be in the format "2024-06-01T00:00:00Z" for example.
/*
1. Check survey existence for a specific date:
curl -X POST "http://localhost:5155/api/survey/check-existence" -H "Content-Type: application/json" -d '{"date":"2024-06-01"}'
2. Submit a survey:
curl -X POST "http://localhost:5155/api/survey/submit" -H "Content-Type: application/json" -d '{"answers":[{"questionId":1,"Response":"Sample answer", "Remark":"Sample remark"}],"timestamp":"2024-06-01T00:00:00Z"}' 
// // survey session is created, but question aren't added, unknown EF error occursd. The error has something to do with foreign key.
3. For debugging: Create questions in the database:
curl -X POST "http://localhost:5155/api/survey/create-questions"

*/