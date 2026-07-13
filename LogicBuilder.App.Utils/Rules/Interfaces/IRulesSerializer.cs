using LogicBuilder.Workflow.Activities.Rules;

namespace LogicBuilder.App.Utils.Rules.Interfaces
{
    public interface IRulesSerializer
    {
        RuleSet? DeserializeRuleSet(string ruleSetXmlDefinition);
        RuleSet? DeserializeRuleSetFile(RulesModule module);
        RuleValidation GetValidation(RuleSet ruleSet, RulesLoaderRequest rulesLoaderRequest);
    }
}
