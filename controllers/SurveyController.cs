using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi;

[ApiController]
[Route("api/[controller]")]
public class SurveyController : ControllerBase
{
    private readonly ILogger<SurveyController> logger;
    private readonly SurveyService service;
    
    public SurveyController(SurveyService service, ILogger<SurveyController> logger)
    {
        this.service = service;
        this.logger = logger;
    }

    [HttpPost("submit")]
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

    public IActionResult CheckSurveyExistence(DTOs.CheckSurveyExistenceRequest request)
    {
        var exists = service.SurveyExistsForDate(request.Date);
        logger.LogInformation($"Survey existence check for date {request.Date}: {exists}");
        return Ok(new DTOs.CheckSurveyExistenceResponse { Exists = exists });
    }

}