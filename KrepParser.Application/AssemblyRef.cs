using System.Reflection;

namespace KrepParser.Application
{
    /// <summary>
    /// Класс ссылки на сборку.
    /// </summary>
    public static class AssemblyRef
    {
        public static readonly Assembly Assembly = typeof(AssemblyRef).Assembly;
    }
}
