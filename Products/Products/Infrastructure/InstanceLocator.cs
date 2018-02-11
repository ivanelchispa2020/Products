using System;
using System.Collections.Generic;
using System.Text;
using Products.ViewModels;

namespace Products.Infrastructure
{
    public  class InstanceLocator
    {

        public MainViewModel Main { get; set; }

        public InstanceLocator()
        {
            Main = new MainViewModel();
        }


    }
}
