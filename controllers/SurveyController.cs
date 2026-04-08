using Microsoft.AspNetCore.Mvc;
// The controller is tested and ready for use
// NOTES are set
[ApiController]
[Route("api/[controller]")]
public class SurveyController : ControllerBase
{
    // CONFIGURATION
    private readonly ILogger<SurveyController> logger;
    private readonly SurveyService service;
    private readonly IRepository<Question> repo;

    public SurveyController(SurveyService service, IRepository<Question> repo, ILogger<SurveyController> logger)
    {
        this.service = service;
        this.logger = logger;
        this.repo = repo;
    }

    // ENDPOINTS

    [HttpPost("submit")]
    public IActionResult SubmitSurvey(DTOs.CreateSurveyRequest request)
    {
      
        var answers = request.Answers;
        var serviceResponse = service.SubmitSurvey(answers);
        if (serviceResponse.Success)
        {
            logger.LogInformation("Survey submitted successfully");
            return Ok(new DTOs.CreateSurveyResponse { Success = true, Message = "Survey submitted successfully" });
        }
        else
        {
            logger.LogError("Failed to submit survey");
            return BadRequest(new DTOs.CreateSurveyResponse { Success = false, Message = "Failed to submit survey" });
        }
    
    }
    // curl -X POST "http://localhost:5155/api/survey/submit" -H "Content-Type: application/json" -d '{"answers":[{"questionId":1,"Response":"Sample answer", "Remark":"Sample remark"}],"timestamp":"2024-06-01T00:00:00Z"}'
    // Endpoint for submitting a survey.
    // Add more validations (detailes in version 1.2 description)

    [HttpPost("check-existence")]
    public IActionResult CheckSurveyExistence(DTOs.CheckSurveyExistenceRequest request)
    {
        var serviceResponse = service.SurveyExistsForDate(request.Date);
        logger.LogInformation($"Survey existence check for date {request.Date}: {serviceResponse.Data}");
        return Ok(new DTOs.CheckSurveyExistenceResponse { Exists = serviceResponse.Data });
    }
    // curl -X POST "http://localhost:5155/api/survey/check-existence" -H "Content-Type: application/json" -d '{"date":"2024-06-01"}'
    // Endpoint for checking if a survey exists for a specific date.

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