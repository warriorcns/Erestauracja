using System;

namespace Erestauracja.Helpers
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class AllowAnnonymous : Attribute
    {

    }
}