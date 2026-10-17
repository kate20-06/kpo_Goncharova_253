public interface IInspectionRule
{
    bool IsSatisfied(ITransport transport);
}