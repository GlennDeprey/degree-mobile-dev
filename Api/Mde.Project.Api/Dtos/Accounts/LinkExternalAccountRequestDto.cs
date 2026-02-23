namespace Mde.Project.Api.Dtos.Accounts
{
    public class LinkExternalAccountRequestDto
    {
        public string IdToken { get; set; }
        public string Provider { get; set; }
    }
}
