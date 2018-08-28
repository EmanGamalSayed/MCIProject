using ITZone.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITZone.API.DataProvider
{
    public interface IUserDataProvider
    {
        Task<TaskModel> GetTaskById(int id);
    }
}
