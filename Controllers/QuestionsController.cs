using backend.Data;
using backend.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionsController : ControllerBase
    {
        private readonly IDataRepository _dataRepository;

        public QuestionsController(IDataRepository dataRepository)
        {
            _dataRepository = dataRepository;
        }
        
        [HttpGet]
        public IEnumerable<QuestionGetManyResponse> GetQuestions(string search)
        {
            //TODO -get questions from data repository
            if (string.IsNullOrEmpty(search))
            {
                return _dataRepository.GetQuestions();
            }
            else
            {
                return _dataRepository.GetQuestionsBySearch(search);
            }
        }

        [HttpGet("unsanswered")]
        public IEnumerable<QuestionGetManyResponse> GetUnansweredQuestions()
        {
            return _dataRepository.GetUnansweredQuestions();
        }

        [HttpGet("{questionId}")]
        public ActionResult<QuestionGetSingleResponse> GetQuestion(int questionId)
        {
            //TODO - call the data repository to get the question 
            var question = _dataRepository.GetQuestion(questionId);
            //TODO - return HTTP status code $)$ if the question isn't found
            if (question == null)
            {
                return NotFound();
            }
            //TODO - return question in  response with status code 200
            return question;
        }

        [HttpPost]
        public ActionResult<QuestionGetSingleResponse> PostQuestion(QuestionPostRequest questionPostRequest)
        {
            //TODO - call the data repository to save the question
            var savedQuestion = _dataRepository.PostQuestion(new QuestionPostFullRequest { Title = questionPostRequest.Title , Content = questionPostRequest.Content, UserId = "1" ,UserName = "bob.test@test.com" , Created = DateTime.UtcNow });
            //TODO - return HTTP status code 201
            return CreatedAtAction(nameof(GetQuestion), new { questionId = savedQuestion.QuestionId }, savedQuestion);
        }
        
        [HttpPut("{questionId}")]
        public ActionResult<QuestionGetSingleResponse> PutQuestion(int questionId, QuestionPutRequest questionPutRequest)
        {
            // TODO - get the question from the data

            // repository
            var question = _dataRepository.GetQuestion(questionId);

            // TODO - return HTTP status code 404 if the

            // question isn't found
            if (question == null)
            {
                return NotFound();
            }

            // TODO - update the question model
            questionPutRequest.Title = string.IsNullOrEmpty(questionPutRequest.Title) ? question.Title : questionPutRequest.Title;
            questionPutRequest.Content = string.IsNullOrEmpty(questionPutRequest.Content) ? question.Content : questionPutRequest.Content;
            // TODO - call the data repository with the

            var savedQuestion = _dataRepository.PutQuestion(questionId, questionPutRequest);
            // updated question model to update the question

            // in the database
            return savedQuestion;
            // TODO - return the saved question
        }

        [HttpDelete("{questionId}")]
        public ActionResult DeleteQuestion(int questionId)
        {
            var question = _dataRepository.GetQuestion(questionId);
            if (question == null)
            {
                return NotFound();
            }
            _dataRepository.DeleteQuestion(questionId);
            return NoContent();
        }

        [HttpPost("answer")] 
        public ActionResult<AnswerGetResponse> PostAnswer(AnswerPostRequest answerPostRequest)
        {
            var questionExists = _dataRepository.QuestionExists(answerPostRequest.QuestionId.Value);
            if (!questionExists)
            {
                return NotFound();
            }

            var savedAnswer = _dataRepository.PostAnswer(new AnswerPostFullRequest {QuestionId = answerPostRequest.QuestionId.Value, Content = answerPostRequest.Content, UserId = "1" , UserName = "bob.test@test.com" , Created = DateTime.UtcNow});
            return savedAnswer;
        }
    }
}
