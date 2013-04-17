using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestWPF
{
    static class Bootstrapper
    {
        private static CompositionContainer _container;

        static Bootstrapper()
        {
            var catalog = new AggregateCatalog();

            catalog.Catalogs.Add(new ApplicationCatalog());

            _container = new CompositionContainer(catalog);
        }

        public static void ComposeParts<T>(T part)
        {
            try
            {
                _container.ComposeParts(part);
            }
            catch (CompositionException ex)
            {
                
                throw ex;
            }
        }
    }
}
