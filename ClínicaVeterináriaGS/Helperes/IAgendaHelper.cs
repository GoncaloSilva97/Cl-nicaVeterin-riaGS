using System.Threading.Tasks;

namespace VeterinaryClinicGS.Helperes
{
    public interface IAgendaHelper
    {
        Task AddDaysAsync(int days);
    }
}
