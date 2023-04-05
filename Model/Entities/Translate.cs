using System.Collections.Generic;

namespace Memorizer.DbModel
{
    public class Translate : BaseEntity
    {
        public string Text { get; set; }

        public override string ToString()
        {
            return this.Text;
        }

    }
}
