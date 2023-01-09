using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Samples.Domain
{
    public class Trivia
    {
        public int Id { get; set; }

        public int MovieId { get; set; }

        public string Description { get; set; }
    }
}
