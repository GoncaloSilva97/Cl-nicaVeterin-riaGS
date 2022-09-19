using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace VeterinaryClinicGS.Helperes
{
    public interface IImageHelper
    {
        Task<string> UploadImageAsync(IFormFile imageFile, string folder);
    }
}
