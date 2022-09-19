using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace VeterinaryClinicGS.Helperes
{
    public interface IBlobHelper
    {
        Task<Guid> UploadBlobAsync(IFormFile file, String containerName);

        Task<Guid> UploadBlobAsync(byte[] file, String containerName);

        Task<Guid> UploadBlobAsync(string image, String containerName);
    }
}
