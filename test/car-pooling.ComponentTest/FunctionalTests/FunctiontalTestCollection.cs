using System;
using System.Collections.Generic;
using System.Linq;

namespace car_pooling.ComponentTest.FunctionalTests
{
    public static class FunctionalTestCollection {
        public static IEnumerable<IFunctionalTest> GetAllFunctionalTest() {
            var type = typeof(IFunctionalTest);
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => type.IsAssignableFrom(p) && !p.IsInterface);
            
            return types.Select(t => (IFunctionalTest)Activator.CreateInstance(t));
        }
    }
}