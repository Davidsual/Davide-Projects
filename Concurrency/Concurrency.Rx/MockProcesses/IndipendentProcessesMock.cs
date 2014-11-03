using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.ObjectBuilder2;

namespace Concurrency.Rx.MockProcesses
{
    public interface IIndipendentProcessesMock
    {
        Task<IEnumerable<string>> GetData(string param);
    }

    public class IndipendentProcessesMock : IIndipendentProcessesMock
    {
        public async Task<IEnumerable<string>> GetData(string param)
        {
            var res = new[]
            {
                "Lea Lincoln",
                "Catharine Vigo",
                "Wonda Deering",  
                "Clemencia Woll",  
                "Nobuko Rote",  
                "Shanon Macek",  
                "Dusty Valcourt",  
                "Garnet Mcadoo",  
                "Nikole Hentz",  
                "Jospeh Blankinship",  
                "Lorie Strahan",  
                "Venetta Defoor",  
                "Fernando Silliman",  
                "Joshua Mckinstry",  
                "Tena Walcott",  
                "Donette Loud",  
                "Lorinda Mcgonigal",  
                "Viva Adair",  
                "Raven Gammons",  
                "Zelma Santo",  
                "Dylan Demasi",  
                "Lise Cortright",  
                "Brian Severin",  
                "Gladis Sprung",  
                "Hazel Titus",  
                "Olin Leeman",  
                "Jack Schurg",  
                "April Weedon",  
                "Jamee Hieb",  
                "Ha Salzman",  
                "Pamella Diehl",  
                "Mana Dials",  
                "Etta Rinaldi",  
                "Felisha Frame",  
                "Jesenia Rollo",  
                "Terrell Focht",  
                "Georgeann Patchell",  
                "Lourie Vermillion",  
                "Giovanna Canizales",  
                "Chong Geoghegan",  
                "Domitila Olden",  
                "Renita Smythe",  
                "Teresa Thoman",  
                "Foster Olszewski",  
                "Jenifer Montoya",  
                "Toccara Martineau",  
                "Garrett Tibbitts",  
                "Joye Hughley",  
                "Malorie Otts",  
                "Shoshana Heintzelman"  
            };

            return (string.IsNullOrEmpty(param)) ? res : res.Where(c => c.ToLowerInvariant().Contains(param.ToLowerInvariant())).AsEnumerable();
        }
    }
}
