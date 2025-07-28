

namespace Application.Features.GeneralPropose.DTOs
{
    public class CheckedIds
    {
        public List<int> Ids { get; set; }
        public bool AllExist { get; set; }
        public int? FirstMissedId { get; set; }
    }
}


