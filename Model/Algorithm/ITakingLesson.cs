using Memorizer.Controller;
using ReversoApi.Models;
using System.Threading.Tasks;

namespace Memorizer.Algorithm
{
    public interface ITakingLesson
    {
        public Lesson GetNextLesson(WebAppContext context);
        public Task ReturnFinishedLesson(Lesson lesson, WebAppContext context);
    }
}
