using System.Reflection;

namespace IoCContainer;

internal class CtorNestedInfo(ConstructorInfo baseCtor, ParameterInfo[] paramsToSearch, int totalRequiredResolves, Type returnType)
{
    public ConstructorInfo BaseCtor { get; set; } = baseCtor;
    public ParameterInfo[] ParamsToSearch { get; set; } = paramsToSearch;
    public int TotalRequiredResolves { get; set; } = totalRequiredResolves;
    public Type ReturnType { get; set; } = returnType;
}
