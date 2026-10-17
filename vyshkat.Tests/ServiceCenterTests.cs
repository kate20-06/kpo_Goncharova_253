public class ServiceCenterTests
{
    [Fact]
    public void Inspect_UsesRealQualityRuleAtBoundary()
    {
        var center = new ServiceCenter(new IInspectionRule[]
        {
            new QualityInspectionRule()
        });

        var accepted = new Scooter { Quality = 9 };
        var rejected = new Scooter { Quality = 8.99 };

        Assert.True(center.Inspect(accepted));
        Assert.False(center.Inspect(rejected));
    }

    [Fact]
    public void Inspect_FailsWhenAnyRuleFails()
    {
        var center = new ServiceCenter(new IInspectionRule[]
        {
            new ConstantInspectionRule(true),
            new ConstantInspectionRule(false),
            new ConstantInspectionRule(true)
        });

        Assert.False(center.Inspect(new Scooter()));
    }

    private sealed class ConstantInspectionRule : IInspectionRule
    {
        private readonly bool _result;

        public ConstantInspectionRule(bool result)
        {
            _result = result;
        }

        public bool IsSatisfied(ITransport transport)
        {
            return _result;
        }
    }
}
