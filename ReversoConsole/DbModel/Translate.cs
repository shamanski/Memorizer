using System.Collections.Generic;

namespace ReversoConsole.DbModel
{
    public class Translate : LearningModelBase
    {
        public  override int Id { get; set; }
        public string Text { get; set; }

        public override string ToString()
        {
            return this.Text;
        }

    }
}
