using System.Threading.Tasks;

namespace VeterinaryClinicGS.Helpers
{
    public interface IAgendaHelper
    {
        Task AddDaysAsync(int days);
    }
}
