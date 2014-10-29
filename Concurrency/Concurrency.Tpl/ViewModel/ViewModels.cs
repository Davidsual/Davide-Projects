using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Concurrency.Tpl.ViewModel
{
    public class ViewModels
    {
        private static object mainViewModel = new MainViewModel();

        public static object MainViewModel
        {
            get { return ViewModels.mainViewModel; }
        }
    }
}
