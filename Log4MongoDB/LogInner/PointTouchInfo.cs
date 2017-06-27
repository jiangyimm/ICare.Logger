using MongoDB.Bson;

namespace Log4MongoDB.LogInner
{
    public class PointTouchInfo:InfoBase
    {
       
        public string ViewName { get; set; }
        public string ButtonContent { get; set; }

        public double PointX { get; set; }
        public double PointY { get; set; }
    }
}