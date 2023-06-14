using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Exceptions
{
    public class PromotionNeededException : Exception
    {
        public PromotionNeededException() : base() { }
        public PromotionNeededException(string message) : base(message) { }
        public PromotionNeededException(string message, Exception inner) : base(message, inner) { }
    }
}
