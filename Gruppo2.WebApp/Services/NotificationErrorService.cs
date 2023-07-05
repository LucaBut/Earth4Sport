using Gruppo2.WebApp.Entities;
using Gruppo2.WebApp.Models.Dtos;
using InfluxDB.Client;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Security.Policy;

namespace Gruppo2.WebApp.Services
{
    public class NotificationErrorService
    {
        private readonly DBAdminContext _adminContext;
        public NotificationErrorService(DBAdminContext adminContext)
        {
            _adminContext = adminContext;
        }

        public async Task<ActionResult<bool>> PostNotificationError(NotificationError error)
        {
            _adminContext.NotificationError.Add(error);
            await _adminContext.SaveChangesAsync();
            return true;
        }
    }
}
