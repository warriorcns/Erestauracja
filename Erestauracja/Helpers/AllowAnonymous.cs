using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Erestauracja.Helpers
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class AllowAnnonymous : Attribute
    {

    }
}