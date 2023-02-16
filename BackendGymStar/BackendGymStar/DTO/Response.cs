namespace BackendGymStar.DTO
{
    public class Response<T>
    {
        public bool ok { get; set; }
        public string? msg { get; set; }
        public T? data { get; set; }
    }



    public class GeneralResp
    {
        public string? Details { get; set; }
    }


}
