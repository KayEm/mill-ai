using System.Collections.Generic;

namespace Mills.Models
{
    public class MillModel
    {
        public MillModel(List<List<PointModel>> mills)
        {
            Mills = mills;
        }
        
        public List<List<PointModel>> Mills { get; private set; }

        public bool IsMill { get; set; }
    }
}
