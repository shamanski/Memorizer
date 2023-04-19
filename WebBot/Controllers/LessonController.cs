using Memorizer.Algorithm;
using Memorizer.DbModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model.Entities;
using Model.Services;

namespace WebBot.Controllers
{
    [Route("api/lesson")]
    [ApiController]
    public class LessonController : ControllerBase
    {
        private readonly StandardLesson _standardLesson;
        private readonly WebAppContext _context;

        public LessonController(StandardLesson standardLesson, WebAppContext context)
        {
            _standardLesson = standardLesson;
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<Lesson>> GetNextLesson()
        {
            Lesson nextLesson = await _standardLesson.GetNextLesson();
            if (nextLesson == null)
            {
                return NotFound();
            }
            return nextLesson;
        }

        [HttpPost]
        public async Task<ActionResult<Lesson>> ReturnFinishedLesson(Lesson lesson)
        {
            await _standardLesson.ReturnFinishedLesson(lesson);
            return CreatedAtAction(nameof(GetNextLesson), new { id = lesson.Id }, lesson);
        }
    }
}
