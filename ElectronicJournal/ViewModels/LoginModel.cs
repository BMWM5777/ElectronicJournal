using ElectronicJournal.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ElectronicJournal.ViewModels
{
    public class LoginModel
    {
        public string userName { get; set; }
        public string password { get; set; }
    }
}
