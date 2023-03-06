using Model.Services;
using ReversoApi.Models;
using System.Threading.Tasks;

namespace Memorizer.Algorithm
{
    public interface ILessonService<T>
    {
        public Task<T> GetNextLesson(WebAppContext context);
        public Task ReturnFinishedLesson(T lesson, WebAppContext context);
    }
}
