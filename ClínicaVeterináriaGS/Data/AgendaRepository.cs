

using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VeterinaryClinicGS.Data.Entity;

namespace VeterinaryClinicGS.Data
{
    public class AgendaRepository : GenericRepository<Agenda>, IAgendaRepository
    {

        private readonly DataContext _dataContext;

        public AgendaRepository(DataContext dataContext) : base(dataContext)
        {
            _dataContext = dataContext;
        }

       

        //public async Task SearchDaysAsync(int days)
        //{
        //    DateTime initialDate;

        //    if (!_dataContext.Agendas.Any())
        //    {
        //        initialDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 8, 0, 0);
        //    }
        //    else
        //    {
        //        var agenda = _dataContext.Agendas.LastOrDefault();
        //        initialDate = new DateTime(agenda.Date.Year, agenda.Date.Month, agenda.Date.AddDays(1).Day, 8, 0, 0);
        //    }

        //    var finalDate = initialDate.AddDays(days);
        //    while (initialDate < finalDate)
        //    {
        //        if (initialDate.DayOfWeek != DayOfWeek.Sunday)
        //        {
        //            var finalDate2 = initialDate.AddHours(10);
        //            while (initialDate < finalDate2)
        //            {
        //                _dataContext.Agendas.Add(new Agenda
        //                {
        //                    Date = initialDate.ToUniversalTime(),
        //                    IsAvailable = true
        //                });

        //                initialDate = initialDate.AddMinutes(30);
        //            }

        //            initialDate = initialDate.AddHours(8);
        //        }
        //        else
        //        {
        //            initialDate = initialDate.AddDays(1);
        //        }
        //    }

        //    await _dataContext.SaveChangesAsync();
        //}
















        public async Task AddDaysAsync(int days)
        {
            DateTime initialDate;

            if (!_dataContext.Agendas.Any())
            {
                initialDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, (DateTime.Now.Hour + 2), 0, 0);
            }
            else
            {
                var agenda = _dataContext.Agendas.OrderByDescending(o => o.Date).LastOrDefault();
                initialDate = new DateTime(agenda.Date.Year, agenda.Date.Month, agenda.Date.AddDays(1).Day, 10, 0, 0);
            }


            var nRooms = _dataContext.Rooms.Count();
            
                var finalDate = initialDate.AddDays(days);
                while (initialDate < finalDate)
                {
                    if (initialDate.DayOfWeek != DayOfWeek.Sunday && initialDate.DayOfWeek != DayOfWeek.Saturday)
                    {

                   
                            
                            var finalDate2 = initialDate.AddHours(12);
                            while (initialDate < finalDate2 && initialDate.Hour >= 10 && initialDate.Hour <= 16 )
                            {
                                var room = 0;
                                while (nRooms > room)
                                {
                                    _dataContext.Agendas.Add(new Agenda
                                    {
                                        Date = initialDate.ToUniversalTime(),
                                        IsAvailable = true
                                    });
                                    room++;
                                }
                                
                                initialDate = initialDate.AddMinutes(30);
                            }

                            initialDate = initialDate.AddHours(1);
                       


                    }
                    else
                    {
                        initialDate = initialDate.AddDays(1);
                    }
                }
            
            await _dataContext.SaveChangesAsync();
        }















































        //public IEnumerable<SelectListItem> GetComboServiceTypes()
        //{
        //    var list = _context.ServiceTypes.Select(p => new SelectListItem
        //    {
        //        Text = p.Name,
        //        Value = p.Id.ToString(),

        //    }).ToList();

        //    list.Insert(0, new SelectListItem
        //    {
        //        Text = "(Select an option...)",
        //        Value = "0",
        //    });

        //    return list;
        //}

        //public IEnumerable<SelectListItem> GetComboRooms()
        //{
        //    var list = _context.Rooms.Select(p => new SelectListItem
        //    {
        //        Text = p.RoomNumber.ToString(),
        //        Value = p.Id.ToString(),

        //    }).ToList();

        //    list.Insert(0, new SelectListItem
        //    {
        //        Text = "(Select an option...)",
        //        Value = "0",
        //    });

        //    return list;
        //}

        //public IEnumerable<SelectListItem> GetComboDoctors()
        //{
        //    var list = _context.Doctors.Select(p => new SelectListItem
        //    {
        //        Text = p.FullName,
        //        Value = p.Id.ToString(),

        //    }).ToList();

        //    list.Insert(0, new SelectListItem
        //    {
        //        Text = "(Select an option...)",
        //        Value = "0",
        //    });

        //    return list;
        //}

        //public IEnumerable<SelectListItem> GetComboAnimals()
        //{
        //    var list = _context.Animals.Select(p => new SelectListItem
        //    {
        //        Text = p.Name,
        //        Value = p.Id.ToString(),

        //    }).ToList();

        //    list.Insert(0, new SelectListItem
        //    {
        //        Text = "(Select an option...)",
        //        Value = "0",
        //    });

        //    return list;
        //}

        //public IEnumerable<SelectListItem> GetComboOwners()
        //{
        //    var list = _context.Owners.Select(p => new SelectListItem
        //    {
        //        Text = p.FullName,
        //        Value = p.Id.ToString(),

        //    }).ToList();

        //    list.Insert(0, new SelectListItem
        //    {
        //        Text = "(Select an option...)",
        //        Value = "0",
        //    });

        //    return list;
        //}
    }
}
