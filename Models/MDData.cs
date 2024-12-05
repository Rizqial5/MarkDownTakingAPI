

namespace MarkDownTaking.API.Model
{
    public class MDData
    {
        public int Id {set;get;}
        public string? Title {set;get;}
        public string? ContentType{set;get;}
        public long FileSize {set;get;}
        public byte[]? MDFile {set; get;}
        public string? MDString{get;set;}

    }
}