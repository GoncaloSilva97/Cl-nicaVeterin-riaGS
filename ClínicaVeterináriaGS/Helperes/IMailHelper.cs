using Azure;

namespace VeterinaryClinicGS.Helperes
{
    public interface IMailHelper
    {
        Response SendEmail(string to, string subject, string body);
    }
}
