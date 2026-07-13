using LogicBuilder.App.Utils.Rules.Interfaces;
using LogicBuilder.Workflow.Activities.Rules;
using LogicBuilder.Workflow.ComponentModel.Serialization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml;

namespace LogicBuilder.App.Utils.Rules
{
    public class RulesSerializer : IRulesSerializer
    {
        public RuleSet? DeserializeRuleSet(string ruleSetXmlDefinition)
        {
            WorkflowMarkupSerializer serializer = new();
            if (!string.IsNullOrEmpty(ruleSetXmlDefinition))
            {
                using StringReader stringReader = new(ruleSetXmlDefinition);
                using XmlTextReader reader = new(stringReader);
                return serializer.Deserialize(reader) as RuleSet;
            }
            else
            {
                return null;
            }
        }

        public RuleSet? DeserializeRuleSetFile(RulesModule module)
        {
            using StreamReader inStream = new(new MemoryStream(module.RuleSetFile));
            return DeserializeRuleSet(inStream.ReadToEnd());
        }

        public RuleValidation GetValidation(RuleSet ruleSet, RulesLoaderRequest rulesLoaderRequest)
        {
            RuleValidation ruleValidation = new(rulesLoaderRequest.FlowActivityType, rulesLoaderRequest.ReferenceAssemblies);
            if (!ruleSet.Validate(ruleValidation))
            {
                List<string> errors = ruleValidation.Errors.Aggregate
                (
                    new List<string>
                    {
                        string.Format
                        (
                            CultureInfo.CurrentCulture,
                            Properties.Resources.invalidRulesetFormat,
                            ruleSet.Name
                        )
                    },
                    (list, next) =>
                    {
                        list.Add(next.ErrorText);
                        return list;
                    }
                );

                throw new ArgumentException(string.Join(Environment.NewLine, errors));
            }

            return ruleValidation;
        }
    }
}
