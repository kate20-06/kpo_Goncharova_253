public class QualityInspectionRule : IInspectionRule
{
    public bool IsSatisfied(ITransport transport)
    {
        return transport.Quality >= 9;
    }
}