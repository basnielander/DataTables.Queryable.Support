using System.Reflection;

namespace DataTables.Queryable.Support
{
    internal static class GetProperty<T>
    {
        internal static BindingFlags BindingFlags => BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty;

        internal static PropertyInfo ByName(string name)
        {
            return typeof(T).GetProperty(name, BindingFlags);
        }
    }
}
