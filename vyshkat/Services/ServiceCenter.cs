public class ServiceCenter : IServiceCenter
{
    private readonly IEnumerable<IInspectionRule> _rules;

    public ServiceCenter(IEnumerable<IInspectionRule> rules)
    {
        _rules = rules;
    }

    public bool Inspect(ITransport transport)
    {
        return _rules.All(rule => rule.IsSatisfied(transport));
    }
}